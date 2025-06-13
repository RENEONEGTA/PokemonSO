#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <stdio.h>
#include <mysql.h>
#include <pthread.h>
#include <time.h> 

char ubicacion[30];

//Esttructura necessaria para acesso excluyente
pthread_mutex_t mutex = PTHREAD_MUTEX_INITIALIZER;
int i = 0;
int sockets[100];
int Puerto = 9040; // Puertos disponibles 50081 - 50085



typedef struct {
	int instancia_id;
	int pokedex_id;
	float x;
	float y;
	time_t tiempo_despawn; 
	int activo;
} PokemonEnMapa;

typedef struct {
	int socket;
	int indice;
} ThreadData;

#define MAX_POKEMON_EN_MAPA 45
PokemonEnMapa pokemonesDelMapa[MAX_POKEMON_EN_MAPA];
int numPokemonesActivos = 0; // Para llevar la cuenta de cuantos hay vivos
int proximo_instancia_id = 1; // Para asegurar que cada ID es unico

void* HiloDeMovimiento(void* arg)
{
	printf("Motor del mundo iniciado (movimiento, spawn y despawn).\n");	
	while(1)
	{
		// Pausa de 5 segundos antes de mover los Pokemon de nuevo
		sleep(5);
		
		pthread_mutex_lock(&mutex);
		
		// ========================
		// SECCION 1: DESPAWN Y MOVIMIENTO
		// ========================
		
		// Recorremos todos los Pokemon del mapa
		for (int k = 0; k < MAX_POKEMON_EN_MAPA; k++)
		{
			// Solo procesamos los Pokémon activos
			if (pokemonesDelMapa[k].activo == 1)
			{
				// Comprobamos si el Pokémon ha expirado
				if (time(NULL) > pokemonesDelMapa[k].tiempo_despawn)
				{
					printf("Despawneando Pokemon con instancia_id %d\n", pokemonesDelMapa[k].instancia_id);
					pokemonesDelMapa[k].activo = 0; // Lo marcamos como inactivo
					numPokemonesActivos--;
					
					char mensajeDespawn[2048];
					sprintf(mensajeDespawn, "96~$%d<EOM>", pokemonesDelMapa[k].instancia_id);
					
					for (int j = 0; j < 100; j++) // Recorremos todo el array
					{
						// Si el socket es valido (no es -1), enviamos el mensaje
						if (sockets[j] != -1)
						{
							write(sockets[j], mensajeDespawn, strlen(mensajeDespawn));
						}
					}
				}
				else // Si no ha expirado, lo movemos
				{
					int radioMovimiento = 200;
					pokemonesDelMapa[k].x += (rand() % (2 * radioMovimiento)) - radioMovimiento;
					pokemonesDelMapa[k].y += (rand() % (2 * radioMovimiento)) - radioMovimiento;
					
					char mensajeMovimiento[2048];
					printf("Moviendo pokemon con Id %d a la posicion %.0f/%.0f\n", pokemonesDelMapa[k].instancia_id, pokemonesDelMapa[k].x, pokemonesDelMapa[k].y);
					sprintf(mensajeMovimiento, "97~$%d/%.0f/%.0f<EOM>", 
							pokemonesDelMapa[k].instancia_id, pokemonesDelMapa[k].x, pokemonesDelMapa[k].y);
					
					
					for (int j = 0; j < 100; j++) // Recorremos todo el array
					{
						// Si el socket es valido (no es -1), enviamos el mensaje
						if (sockets[j] != -1)
						{
							write(sockets[j], mensajeMovimiento, strlen(mensajeMovimiento));
						}
					}
				}
			}
		}
		
		// ========================
		// SECCIoN 2: SPAWN
		// ========================
		// Si hay hueco y con un 30% de probabilidad, creamos un nuevo Pokemon
		if (numPokemonesActivos < MAX_POKEMON_EN_MAPA && (rand() % 100) < 30)
		{
			// Buscamos un hueco libre en el array
			for (int k = 0; k < MAX_POKEMON_EN_MAPA; k++)
			{
				if (pokemonesDelMapa[k].activo == 0)
				{
					// Encontramos un hueco, creamos el nuevo Pokemon aqui
					pokemonesDelMapa[k].activo = 1;
					pokemonesDelMapa[k].instancia_id = proximo_instancia_id++;
					pokemonesDelMapa[k].pokedex_id = (rand() % 3) + 1; // Pikachu, Charmander o Squirtle
					pokemonesDelMapa[k].x = rand() % 4000; // Coordenada X aleatoria
					pokemonesDelMapa[k].y = rand() % 4000; // Coordenada Y aleatoria
					pokemonesDelMapa[k].tiempo_despawn = time(NULL) + 120; // Durara 2 minutos
					
					numPokemonesActivos++;
					
					printf("Spawneando Pokemon ID %d (Pokedex %d) en (%f, %f)\n",
						   pokemonesDelMapa[k].instancia_id, pokemonesDelMapa[k].pokedex_id,
						   pokemonesDelMapa[k].x, pokemonesDelMapa[k].y);
					
					char mensajeSpawn[2048];
					sprintf(mensajeSpawn, "95~$%d/%d/%.0f/%.0f<EOM>",
							pokemonesDelMapa[k].instancia_id, pokemonesDelMapa[k].pokedex_id,
							pokemonesDelMapa[k].x, pokemonesDelMapa[k].y);
					
					for (int j = 0; j < 100; j++) // Recorremos todo el array
					{
						// Si el socket es valido (no es -1), enviamos el mensaje
						if (sockets[j] != -1)
						{
							write(sockets[j], mensajeSpawn, strlen(mensajeSpawn));
						}
					}
					break; // Salimos del bucle for para no crear mas de uno a la vez
				}
			}
		}		
		pthread_mutex_unlock(&mutex);
	}
	
	return NULL;
}

void *AtenderCliente(void *data)
{

	char user[80];
	int userId;
	
	char peticion[512];
	char buff2[16384];
	int idPartida;
	
	ThreadData* threadData = (ThreadData*)data;
	int socket_conn = threadData->socket;
	int mi_indice = threadData->indice;
	free(threadData); // Liberamos la memoria de la estructura, ya no la necesitamos
	


	printf("Cliente atendido en socket %d (i�ndice %d)\n", socket_conn, mi_indice);
	
	strcpy(ubicacion, "localhost");
	printf("Socket del cliente: %d\n", socket_conn);

	//Inicio el MYSQL
	MYSQL *conn;
	int err;
	
	// Estrucutra especial para almecenar resultados de consultas
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	//Creamos una conexion al servidor MYSQL
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexion: %u %s\n", mysql_errno(conn), mysql_error(conn));
		exit(1);
	}
	
    // Conectar con el servidor MySQL
    if (mysql_real_connect(conn, ubicacion, "root", "mysql", "T8_JuegoPokemon", 0, NULL, 0) == NULL) 
	{
        printf("Error al conectar con el servidor MySQL\n");
        mysql_close(conn);
        close(socket_conn);
        pthread_exit(NULL);
    }
	else 
	{
		printf("Servidor 2 Se ha conectado a la base de datos con exito\n");
	}

	int terminar = 0;
	//Entramos en un bucle para atender todas las peticiones de este cliente
	//hasta que se desconecte
	while(terminar == 0)
	{
		//Ahora recibimos la peticion
		int ret = read(socket_conn, peticion, sizeof(peticion));
		printf("Recibido\n");
		
		//Tenemos que a�adirle la marca de fin de string
		//Para que no escriba lo que hay despues en el buffer
		peticion[ret] = '\0';
		
		printf ("Peticion: %s\n", peticion);
		
		
		char *p = strtok(peticion, "/"); //Separamos los campos de las consultas por /
		if (p == NULL) {
			printf("Error: Codigo no especificado.\n");
			break;
		}
		int codigo =  atoi (p);

		//Formato de la consulta: 0/nombreUsuario/userId
		if(codigo == 0)
		{
			char userIdChar[80];
			p = strtok(NULL, "/");
			if (p != NULL) {
				strcpy(user, p);
			}
			else {
				strcpy(buff2, "Error: Formato de nombre incorrecto");
				write(socket_conn, buff2, strlen(buff2));
				break;
			}
			p = strtok(NULL, "/");
			if (p != NULL) {
				strcpy(userIdChar, p);
			}
			else {
				strcpy(buff2, "Error: Formato de nombre incorrecto");
				write(socket_conn, buff2, strlen(buff2));
				break;
			}
			userId=atoi(userIdChar);
			
			printf("Usuario %s con Id %d conectado al servidor de partidas", user, userId);
		}
		else if (codigo == 1) {
			printf("Cliente %s desconectado del servidor de partidas", user);
			terminar == 1;
		}
/*		else if(codigo == 90) */// El cliente notifica que se ha unido a una partida
/*		{*/
/*			printf("Recibida notificacion de  union a partida.\n");*/
/*			char mensajeLista[2048] = "90~$";			*/
			// Recorremos el array y enviamos solo los Pokemon activos
/*			for (int k = 0; k < MAX_POKEMON_EN_MAPA; k++) {*/
/*				if (pokemonesDelMapa[k].activo == 1) { */// Solo si esta activo
/*					char bufferPokemon[100];*/
/*					sprintf(bufferPokemon, "%d/%d/%.0f/%.0f#",*/
/*							pokemonesDelMapa[k].instancia_id,*/
/*							pokemonesDelMapa[k].pokedex_id,*/
/*							pokemonesDelMapa[k].x,*/
/*							pokemonesDelMapa[k].y);*/
/*					strcat(mensajeLista, bufferPokemon);*/
/*				}*/
							
/*			}				*/
/*			printf("Enviando lista de Pokemon por union: %s\n", mensajeLista);*/
/*			write(socket_conn, mensajeLista, strlen(mensajeLista));*/
/*		}*/
		else if(codigo == 91) // Crear Partida
		{
			char query[512];
			time_t t = time(NULL);
			struct tm tm = *localtime(&t);
			char fecha[11];
			char jugador1[80];
			char mensaje[80];
			int idGenerado ;
			strcpy(jugador1, user);
			snprintf(fecha, sizeof(fecha), "%04d%02d%02d", tm.tm_year + 1900, tm.tm_mon + 1, tm.tm_mday);
			snprintf(query, sizeof(query),
					 "INSERT INTO Partidas (fecha, jugador1) VALUES ('%s', '%s')",
					 fecha, jugador1);
			if (mysql_query(conn, query)) {
				fprintf(stderr, "Error al crear partida: %s\n", mysql_error(conn));
			} else {
				printf("Partida creada con exito por %s\n", jugador1);
				idGenerado = (int)mysql_insert_id(conn);
			}
			sprintf(mensaje, "91~$%d<EOM>", idGenerado);
			
			write(socket_conn, mensaje, strlen(mensaje));
			
			
			//Enviamos la lista de pokemons
			char mensajeLista[2048] = "90~$"; 
			
			for (int k = 0; k < MAX_POKEMON_EN_MAPA; k++) {
				// Solo procesamos los Pokémon activos
				if (pokemonesDelMapa[k].activo == 1)
				{
					char bufferPokemon[100];
					// Formato: <instancia_id>/<pokedex_id>/<x>/<y>#
					sprintf(bufferPokemon, "%d/%d/%.0f/%.0f#",
							pokemonesDelMapa[k].instancia_id,
							pokemonesDelMapa[k].pokedex_id,
							pokemonesDelMapa[k].x,
							pokemonesDelMapa[k].y);
					strcat(mensajeLista, bufferPokemon);
				}
			}
			printf("Enviando a socket %d la lista de Pokemon: %s\n", socket_conn, mensajeLista);
			strcat(mensajeLista, "<EOM>");
			// Enviamos el mensaje completo al cliente que crea la partida
			write(socket_conn, mensajeLista, strlen(mensajeLista));
			
		}
		else if(codigo == 92) // Unirse a partida
		{
			MYSQL_RES *res;
			MYSQL_ROW row;
			char query[256];
			char mensaje[100];
			char jugador[80];
			strcpy(jugador, user);
			int idPartida;
			char id[80];
			
			p = strtok(NULL, "/");
			if(p != NULL)
			{
				strcpy(id, p);
			}
			idPartida = atoi(id);
			
			
			// Buscar partidas con huecos
			snprintf(query, sizeof(query),
					 "SELECT jugador2, jugador3, jugador4 FROM Partidas WHERE id = %d",
					 idPartida);
			
			
			if (mysql_query(conn, query)) {
				fprintf(stderr, "Error al consultar partida: %s\n", mysql_error(conn));
				return;
			}
			res = mysql_store_result(conn);
			if (res == NULL || mysql_num_rows(res) == 0) {
				fprintf(stderr, "No existe la partida con id %d\n", idPartida);
				if (res) mysql_free_result(res);
				return;
			}
			row = mysql_fetch_row(res);

			
			
			char campoLibre[20]="";
			if (row[1] == NULL || strlen(row[1]) == 0)
				strcpy(campoLibre, "jugador2");
			else if (row[2] == NULL || strlen(row[2]) == 0)
				strcpy(campoLibre, "jugador3");
			else if (row[3] == NULL || strlen(row[3]) == 0)
				strcpy(campoLibre, "jugador4");
			
			if (strlen(campoLibre) == 0) {
				printf("La partida con id %d esta llena\n", idPartida);
				mysql_free_result(res);
				return;
			}
			
			mysql_free_result(res);
			
			// Unirse a la partida actualizando el campo libre
			char update[256];
			snprintf(update, sizeof(update),
					 "UPDATE Partidas SET %s = '%s' WHERE id = %d",
					 campoLibre, jugador, idPartida);
			
			
			if (mysql_query(conn, update)) {
				fprintf(stderr, "Error al unirse a la partida: %s\n", mysql_error(conn));
			} else {
				printf("%s se ha unido a la partida %d como %s\n", jugador, idPartida, campoLibre);
			}
			
			//Enviamos la lista de pokemons
			char mensajeLista[2048] = "90~$"; 
			
			for (int k = 0; k < MAX_POKEMON_EN_MAPA; k++) {
				// Solo procesamos los Pokémon activos
				if (pokemonesDelMapa[k].activo == 1)
				{
					char bufferPokemon[100];
					// Formato: <instancia_id>/<pokedex_id>/<x>/<y>#
					sprintf(bufferPokemon, "%d/%d/%.0f/%.0f#",
							pokemonesDelMapa[k].instancia_id,
							pokemonesDelMapa[k].pokedex_id,
							pokemonesDelMapa[k].x,
							pokemonesDelMapa[k].y);
					strcat(mensajeLista, bufferPokemon);
				}
			}
			printf("Enviando a socket %d la lista de Pokemon: %s\n", socket_conn, mensajeLista);
			// Enviamos el mensaje completo al cliente que crea la partida
			write(socket_conn, mensajeLista, strlen(mensajeLista));
			
		}
		else if (codigo == 93) // Actualizar coordenadas jugador
		{
			int idJugador, idPartida, posX, posY;
			
			char *strIdPartida = strtok(NULL, "/");
			char *strPosX = strtok(NULL, "/");
			char *strPosY = strtok(NULL, "/");
			
			if (!strIdPartida || !strPosX || !strPosY) {
				printf("Faltan datos en el mensaje de coordenadas\n");
				return;
			}
			idJugador = userId;
			idPartida = atoi(strIdPartida);
			posX = atoi(strPosX);
			posY = atoi(strPosY);
			
			char query[512];
			snprintf(query, sizeof(query),
					 "INSERT INTO JugadoresPartidas (idJugador, idPartida, posX, posY) "
					 "VALUES (%d, %d, %d, %d) "
					 "ON DUPLICATE KEY UPDATE posX = VALUES(posX), posY = VALUES(posY)",
					 idJugador, idPartida, posX, posY);
			
			if (mysql_query(conn, query)) {
				fprintf(stderr, "Error al insertar/actualizar coordenadas: %s\n", mysql_error(conn));
			} else {
				printf("Coordenadas registradas: Jugador %d en Partida %d -> (%d, %d)\n",
					   idJugador, idPartida, posX, posY);
			}
			
			// Paso 1: Obtener todas las coordenadas de jugadores en la misma partida
			char queryCoordenadas[256];
			snprintf(queryCoordenadas, sizeof(queryCoordenadas),
					 "SELECT idJugador, posX, posY FROM JugadoresPartidas WHERE idPartida = %d",
					 idPartida);
			
			if (mysql_query(conn, queryCoordenadas)) {
				fprintf(stderr, "Error al obtener coordenadas de la partida: %s\n", mysql_error(conn));
				return;
			}
			MYSQL_RES *resCoords = mysql_store_result(conn);
			MYSQL_ROW rowCoords;
			
			char mensajeCoordenadas[1024] = "94~$"; // codigo de coordenadas
			
			while ((rowCoords = mysql_fetch_row(resCoords)) != NULL) {
				int jugador = atoi(rowCoords[0]);
				int x = atoi(rowCoords[1]);
				int y = atoi(rowCoords[2]);
				
				char linea[64];
				snprintf(linea, sizeof(linea), "%d:%d:%d/", jugador, x, y); // formato id:x:y/
				strcat(mensajeCoordenadas, linea);
			}
			strcat(mensajeCoordenadas, "<EOM>");
			mysql_free_result(resCoords);
			
			// Paso 2: Obtener sockets de todos los jugadores de esta partida
			char querySockets[256];
			snprintf(querySockets, sizeof(querySockets),
					 "SELECT idJugador FROM JugadoresPartidas WHERE idPartida = %d", idPartida);
			
			if (mysql_query(conn, querySockets)) {
				fprintf(stderr, "Error al obtener lista de jugadores: %s\n", mysql_error(conn));
				return;
			}
			MYSQL_RES *resSockets = mysql_store_result(conn);
			MYSQL_ROW rowSocket;
			
			while ((rowSocket = mysql_fetch_row(resSockets)) != NULL) {
				int jugadorId = atoi(rowSocket[0]);
				
				// Consultamos el socket de este jugador
				char querySock[128];
				snprintf(querySock, sizeof(querySock),
						 "SELECT socket FROM Conectados WHERE idJ = %d", jugadorId);
				if (mysql_query(conn, querySock) == 0) {
					MYSQL_RES *resSock = mysql_store_result(conn);
					MYSQL_ROW filaSock = mysql_fetch_row(resSock);
					
					if (filaSock != NULL) {
						int socketJugador = atoi(filaSock[0]);
						write(socketJugador, mensajeCoordenadas, strlen(mensajeCoordenadas));
					}
					
					mysql_free_result(resSock);
				}
			}
			
			mysql_free_result(resSockets);			
		}
		else if (codigo == 98) // El cliente quiere iniciar un combate
		{
			p = strtok(NULL, "/");
			if (p != NULL)
			{
				int instancia_id_combatir = atoi(p);
				printf("Cliente en socket %d quiere combatir con Pokemon de instancia %d\n", socket_conn, instancia_id_combatir);
				
				pthread_mutex_lock(&mutex);
				// Buscamos el Pokemon en nuestro array
				for (int k = 0; k < MAX_POKEMON_EN_MAPA; k++)
				{
					if (pokemonesDelMapa[k].activo == 1 && pokemonesDelMapa[k].instancia_id == instancia_id_combatir)
					{
						// Lo "consumimos" del mapa
						pokemonesDelMapa[k].activo = 0;
						numPokemonesActivos--;
						// Notificamos a TODOS los jugadores que este Pokemon ha desaparecido
						char mensajeDespawn[50];
						sprintf(mensajeDespawn, "96~$%d<EOM>", instancia_id_combatir);
						for (int j = 0; j < 100; j++)
							if (sockets[j] != -1)
								write(sockets[j], mensajeDespawn, strlen(mensajeDespawn));
						// Notificamos al jugador que inicia el combate que la batalla empieza
						
						 char mensajeCombate[50];
						 sprintf(mensajeCombate, "103~$%d/%d<EOM>", pokemonesDelMapa[k].instancia_id, pokemonesDelMapa[k].pokedex_id);						
						 write(socket_conn, mensajeCombate, strlen(mensajeCombate));
						
						break;
					}
				}
				pthread_mutex_unlock(&mutex);
			}
		}	
		else if (codigo == 99) // El cliente intenta una captura
		{
			// Formato esperado: 99/instancia_id/vida_actual
			char *idInstanciaStr = strtok(NULL, "/");
			char *vidaActualStr = strtok(NULL, "/");
			
			if (idInstanciaStr != NULL && vidaActualStr != NULL)
			{
				int instancia_id = atoi(idInstanciaStr);
				int vida_actual = atoi(vidaActualStr);
				int pokedex_id = -1;
				int vida_maxima = -1;
				// Buscamos el Pokemon en el array para obtener su pokedex_id
				for (int k = 0; k < MAX_POKEMON_EN_MAPA; k++)
				{
					if (pokemonesDelMapa[k].instancia_id == instancia_id)
					{
						pokedex_id = pokemonesDelMapa[k].pokedex_id;
						break;
					}
				}
				// Ahora buscamos la vida maxima en la base de datos
				char query[256];
				sprintf(query, "SELECT hp FROM Pokedex WHERE id = %d", pokedex_id);
				mysql_query(conn, query);
				MYSQL_RES *res = mysql_store_result(conn);
				MYSQL_ROW row = mysql_fetch_row(res);
				if (row != NULL) vida_maxima = atoi(row[0]);
				mysql_free_result(res);
				// en funcion de la vida se decide la captura
				int exito = 0;
				if (vida_maxima > 0)
				{
					float probabilidad = (float)(vida_maxima - vida_actual) / vida_maxima; // 0.0 a 1.0
					float tirada = (float)(rand() % 100) / 100.0f; // num aleatorio de 0.0 a 0.99
					
					if (tirada < probabilidad) exito = 1;
				}// Si la captura es exitosa, lo anadimos a la BBDD
				if (exito == 1)
				{
					printf("�Captura exitosa! Anadiendo Pokemon %d al jugador %d\n", pokedex_id, userId);
					sprintf(query, "INSERT INTO Relacio (IdJ, IdP, Nivell) VALUES (%d, %d, 1)", userId, pokedex_id);
					mysql_query(conn, query);
					// También actualizamos el contador de Pokémon del jugador
					sprintf(query, "UPDATE Jugadores SET numeroPokemons = numeroPokemons + 1 WHERE id = %d", userId);
					mysql_query(conn, query);
				}
				// Enviamos el resultado al cliente (1 = exito, 0 = fallo)
				char respuesta[50];
				sprintf(respuesta, "104~$%d<EOM>", exito);
				write(socket_conn, respuesta, strlen(respuesta));
			}
				
				
		}		
	}
	
					
			
	
	terminar = 0;
	while(terminar == 0)
	{
		char peticion[512];
		int ret = read(socket_conn, peticion, sizeof(peticion));
		
		if (ret <= 0) // Si read devuelve 0 o -1, el cliente se desconecta
		{
			printf("Cliente en i�ndice %d (socket %d) se ha desconectado.\n", mi_indice, socket_conn);
			terminar = 1; // Salimos del bucle
		}
		else
		{
			peticion[ret] = '\0';
			printf("Recibido del cliente en i�ndice %d: %s\n", mi_indice, peticion);
			
		}
	}
	// --- LOGICA DE LIMPIEZA ---
	printf("Limpiando puesto del i�ndice %d.\n", mi_indice);
	sockets[mi_indice] = -1; // Marcamos el hueco como libre
	close(socket_conn);      // Cerramos el socket
	return NULL;
}


int main(int argc, char *argv[])
{
	strcpy(ubicacion, "localhost");
	//Inicio conexion con MYSQL
	MYSQL *conn;
	int err;
	

	//Estructura especial para almacenar resultados de consultas
	MYSQL_RES *resultado;
	MYSQL_ROW row;

	//Creamos una conexion al servidor MYSQL
	conn = mysql_init(NULL);
	if(conn == NULL){
		printf("Error al crear la conexion: %u %S\n", mysql_errno(conn), mysql_error(conn));
		exit(1);
	}
	// Conectar con el servidor MySQL
	if (mysql_real_connect(conn, ubicacion, "root", "mysql", "T8_JuegoPokemon", 0, NULL, 0) == NULL) 
	{
		printf("Error al conectar con el servidor MySQL\n");
		mysql_close(conn);
		
	}
	else 
	{
		printf("Se ha conectado a la base de datos con exito\n");
	}
	
	char query[512] = "DELETE FROM Conectados";
	if (mysql_query(conn, query) == 0) {
		printf("La tabla de conectados se ha limpiado correctamente\n");
	}
	else{
		printf("La tabla de conectados no se ha limpiado correctamente %s \n", mysql_error(conn));
	}
	
	int sock_conn, sock_listen, ret;
	struct sockaddr_in serv_adr;
	pthread_t threads[100]; // Usamos un array de threads
	// Inicializamos todos los sockets a -1 para saber que están libres
	for(int k = 0; k < 100; k++) sockets[k] = -1;

	// INICIALITZACIONS
	// Obrim el socket
	if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0)
		printf("Error creando socket\n");
	// Fem el bind al port
	memset(&serv_adr, 0, sizeof(serv_adr));// inicialitza a zero serv_addr
	serv_adr.sin_family = AF_INET;
	// asocia el socket a cualquiera de las IP de la m?quina. 
	//htonl formatea el numero que recibe al formato necesario
	serv_adr.sin_addr.s_addr = htonl(INADDR_ANY);
	// escucharemos en el port 9050
	serv_adr.sin_port = htons(Puerto);

	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0){
		printf ("Error al bind\n");
	}
	//La cola de peticiones pendientes no podr? ser superior a 4
	if (listen(sock_listen, 2) < 0){
		printf("Error en el Listen\n");
	}
		

	// Inicializamos nuestro array de Pokémon
	for (int k = 0; k < MAX_POKEMON_EN_MAPA; k++) {
		pokemonesDelMapa[k].activo = 0;
	}
	
	
	// INICIAMOS EL HILO DE MOVIMIENTO
	pthread_t thread_movimiento;
	srand(time(NULL)); // Inicializamos la semilla para numeros aleatorios
	
	if (pthread_create(&thread_movimiento, NULL, HiloDeMovimiento, NULL) != 0) {
		perror("No se pudo crear el hilo de movimiento");
		return 1;
	}

	printf("DEBUG: Intentando crear el hilo de movimiento...\n");

	
	if (pthread_create(&thread_movimiento, NULL, HiloDeMovimiento, NULL) != 0) {
		perror("ERROR: No se pudo crear el hilo de movimiento");
		return 1;
	}
	printf(" �Hilo de movimiento creado con Exito!\n");

	



	
	// BUCLE PARA ACEPTAR CLIENTES
	while(1)
	{
		printf("Servidor 2 esperando conexiones...\n");
		sock_conn = accept(sock_listen, NULL, NULL);
		
		int i = -1;
		// Buscamos el primer hueco libre (-1) en el array de sockets
		for (int j = 0; j < 100; j++) {
			if (sockets[j] == -1) {
				i = j;
				break;
			}
		}
		
		if (i != -1) {
			sockets[i] = sock_conn;
			// Creamos la estructura para pasar el socket y el índice al hilo
			ThreadData* data = (ThreadData*)malloc(sizeof(ThreadData));
			data->socket = sock_conn;
			data->indice = i;
			
			pthread_create(&threads[i], NULL, AtenderCliente, (void*)data);
			printf("Conexión aceptada en el índice %d.\n", i);
		} else {
			printf("Servidor lleno. Conexión rechazada.\n");
			close(sock_conn);
		}
	}
	return 0;
}
