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
} PokemonEnMapa;
// LISTA FIJA DE POKEMON PARA EL MAPA DE LA PARTIDA
PokemonEnMapa pokemonesDelMapa[] = {
	{1, 2, 800, 800},   // Un Charmander (pokedex_id=2) en (800, 800)
	{2, 3, 1500, 3500}, // Un Squirtle (pokedex_id=3) en (1500, 3500)
	{3, 1, 3000, 1200}  // Un Pikachu (pokedex_id=1) en (3000, 1200)
};
int numPokemonesEnMapa = 3;

void* HiloDeMovimiento(void* arg)
{
	printf("Hilo de movimiento iniciado.\n");
	
	while(1)
	{
		// Pausa de 5 segundos antes de mover los Pokemon de nuevo
		sleep(5);
		
		pthread_mutex_lock(&mutex);
		
		// Recorremos todos los Pokemon del mapa
		for (int k = 0; k < numPokemonesEnMapa; k++)
		{
			// Calculamos una nueva posicion aleatoria en un radio de 200 pi­xeles
			int radioMovimiento = 200;
			pokemonesDelMapa[k].x += (rand() % (2 * radioMovimiento)) - radioMovimiento;
			pokemonesDelMapa[k].y += (rand() % (2 * radioMovimiento)) - radioMovimiento;
			
			char mensajeMovimiento[200];
			sprintf(mensajeMovimiento, "97~$%d/%.0f/%.0f", 
					pokemonesDelMapa[k].instancia_id,
					pokemonesDelMapa[k].x,
					pokemonesDelMapa[k].y);
			printf("Enviando actualizacion de movimiento: %s\n", mensajeMovimiento);
			for (int j = 0; j < i; j++)
			{
				write(sockets[j], mensajeMovimiento, strlen(mensajeMovimiento));
			}
		}
		pthread_mutex_unlock(&mutex);
	}
	
	return NULL;
}
	


void *AtenderCliente(void *socket)
{
	
	int socket_conn;
	char user[80];
	int userId;
	int *s;
	int ret;
	s =(int *) socket;
	socket_conn = *s;
	char peticion[512];
	char buff2[16384];
	int idPartida;
	
	
	
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
		ret = read(socket_conn, peticion, sizeof(peticion));
		printf("Recibido\n");
		
		//Tenemos que aï¿½adirle la marca de fin de string
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
		else if(codigo == 90) // El cliente notifica que se ha unido a una partida
		{
			printf("Recibida notificacion de  union a partida.\n");
			char mensajeLista[2048] = "90~$";			
			for (int k = 0; k < numPokemonesEnMapa; k++) {
				char bufferPokemon[100];
				sprintf(bufferPokemon, "%d/%d/%.0f/%.0f#",
						pokemonesDelMapa[k].instancia_id,
						pokemonesDelMapa[k].pokedex_id,
						pokemonesDelMapa[k].x,
						pokemonesDelMapa[k].y);
				strcat(mensajeLista, bufferPokemon);
			}
			printf("Enviando lista de Pokemon por union: %s\n", mensajeLista);
			write(socket_conn, mensajeLista, strlen(mensajeLista));
		}
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
			sprintf(mensaje, "91~$%d", idGenerado);
			
			write(socket_conn, mensaje, strlen(mensaje));
			
			
			//Enviamos la lista de pokemons
			char mensajeLista[2048] = "90~$"; 
			
			for (int k = 0; k < numPokemonesEnMapa; k++) {
				char bufferPokemon[100];
				// Formato: <instancia_id>/<pokedex_id>/<x>/<y>#
				sprintf(bufferPokemon, "%d/%d/%.0f/%.0f#",
						pokemonesDelMapa[k].instancia_id,
						pokemonesDelMapa[k].pokedex_id,
						pokemonesDelMapa[k].x,
						pokemonesDelMapa[k].y);
				strcat(mensajeLista, bufferPokemon);
			}
			printf("Enviando a socket %d la lista de Pokemon: %s\n", socket_conn, mensajeLista);
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
			
			for (int k = 0; k < numPokemonesEnMapa; k++) {
				char bufferPokemon[100];
				// Formato: <instancia_id>/<pokedex_id>/<x>/<y>#
				sprintf(bufferPokemon, "%d/%d/%.0f/%.0f#",
						pokemonesDelMapa[k].instancia_id,
						pokemonesDelMapa[k].pokedex_id,
						pokemonesDelMapa[k].x,
						pokemonesDelMapa[k].y);
				strcat(mensajeLista, bufferPokemon);
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
	}	
	//Cerramos la conexion con el servidor
close(socket_conn); 
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
		

	
	
	// INICIAMOS EL HILO DE MOVIMIENTO
	pthread_t thread_movimiento;
	srand(time(NULL)); // Inicializamos la semilla para numeros aleatorios
	
	if (pthread_create(&thread_movimiento, NULL, HiloDeMovimiento, NULL) != 0) {
		perror("No se pudo crear el hilo de movimiento");
		return 1;
	}

	printf("DEBUG: Intentando crear el hilo de movimiento...\n");
	fflush(stdout); 
	
	if (pthread_create(&thread_movimiento, NULL, HiloDeMovimiento, NULL) != 0) {
		perror("ERROR: No se pudo crear el hilo de movimiento");
		return 1;
	}
	printf("DEBUG: ¡Hilo de movimiento creado con Exito!\n");
	fflush(stdout);
	
	//Aqui iniciariamos la listas de conectados
	
	pthread_t thread;
	for(;;){
		printf ("Servidor 2 Escuchando\n");
		
		sock_conn = accept(sock_listen, NULL, NULL);
		printf ("He recibido conexi?n\n");
		
		//sockets[i] es el socket que usaremos para este cliente
		sockets[i] = sock_conn;
		
		//Creatr thead y decirle lo que tiene que hacer
		pthread_create(&thread, NULL, AtenderCliente, &sockets[i]); //Le passas el socket por referencia. Para no passar le una copia 
		
		i++;
	}
	
	
	

	//Cerramos la conexion MYSQL
	mysql_close(conn);

	return 0;
}
