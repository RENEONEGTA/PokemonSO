#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <stdio.h>
#include <mysql.h>
#include <pthread.h>

char ubicacion[30];

//Esttructura necessaria para acesso excluyente
pthread_mutex_t mutex = PTHREAD_MUTEX_INITIALIZER;
int i = 0;
int sockets[100];
int Puerto = 50082; // Puertos disponibles 50081 - 50085

//Anade nuevo conectado
int Anade(MYSQL *conn, int IdJ, char nombre[20], int socket) {
   

	printf("Anadiendo jugador: '%s' con id %d y socket %d a la lista de conectados\n", nombre, IdJ, socket);
    char query[512];
	sprintf(query,
			"INSERT INTO Conectados (IdJ, nombre, socket) VALUES (%d, '%s', %d)"
			"ON DUPLICATE KEY UPDATE socket = VALUES(socket);",
			IdJ, nombre, socket);
	

    int result = -1;

	if (mysql_query(conn, query) == 0) {
		result = 0;  // Exito aunque no se modificara nada
		printf("Insercion a la lista de conectados exitosa\n");
	}
   
    return result;
}


//Retorna 0 si elimina y -1 si el usuario no esta en la Base de datos de conectados
int Elimina(MYSQL *conn, char nombre[20]) {
    pthread_mutex_lock(&mutex);

    char query[256];
    sprintf(query, "DELETE FROM Conectados WHERE nombre = '%s';", nombre);

    int result = -1;
    if (mysql_query(conn, query) == 0 && mysql_affected_rows(conn) > 0) {
        result = 0;  // Eliminació exitosa
    }

    pthread_mutex_unlock(&mutex);
    return result;
}


int VerListaConectados(MYSQL *conn, char *resultado) {
    MYSQL_RES *res;
    MYSQL_ROW row;
    char query[256] = "SELECT Jugadores.* FROM Conectados, Jugadores WHERE Jugadores.id = Conectados.IdJ";
    
    if (mysql_query(conn, query) != 0) {
        printf("Error en la consulta VerListaConectados: %s\n", mysql_error(conn));
        return -1;
    }

    res = mysql_store_result(conn);
    if (res == NULL) {
        printf("Error al almacenar resultado: %s\n", mysql_error(conn));
        return -1;
    }

    strcpy(resultado, "");  // Inicialitzar la cadena

    while ((row = mysql_fetch_row(res)) != NULL) {
        // Format: nombre1,socket1/nombre2,socket2/...
        strcat(resultado, row[0]); // id
        strcat(resultado, "/");
        strcat(resultado, row[1]); // nombre
		strcat(resultado, "/");
		strcat(resultado, row[2]); // password
		strcat(resultado, "/");
		strcat(resultado, row[3]); // numPokemons
		strcat(resultado, "/");
		strcat(resultado, row[4]); // Victorias
		strcat(resultado, "/");
		strcat(resultado, row[5]); // Derrotas
		strcat(resultado, "/");
		strcat(resultado, row[6]); // Posicion
        strcat(resultado, "#");
    }

    mysql_free_result(res);
    return 0;
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
	
	
	
	strcpy(ubicacion, "shiva2.upc.es");
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
		printf("Servidor 1 Se ha conectado a la base de datos con exito\n");
	}

	int terminar = 0;
	//Entramos en un bucle para atender todas las peticiones de este cliente
	//hasta que se desconecte
	while(terminar == 0)
	{
		//Ahora recibimos la peticion
		ret = read(socket_conn, peticion, sizeof(peticion));
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
		//Formato de la consulta: 0/nombreUsuario
		if(codigo == 0)
		{
			terminar = 1;
			//Eliminamos usuario de la lista de conectados
			p = strtok(NULL, "/");  // Obtenemos el nombre
            if (p != NULL) {
                char nombre[50];
                strcpy(nombre, p);  // Guardar el nombre
                int eli = Elimina(conn, nombre);
				if (eli ==-1){
					printf("No se ha podido eliminar a %s de la lista de conectados\n",nombre);
				}
				if (eli==0){
					printf("Usuario %s desconectado\n",nombre);
				}
				//notificar a tots els clients
				char listaConectados[1024];
				if (VerListaConectados(conn, listaConectados) == 0) {
					char notificacion[1100];
					sprintf(notificacion, "100~$%s", listaConectados);
					
					pthread_mutex_lock(&mutex); // Protegir l'accés a la llista de sockets
					for (int j = 0; j < i; j++) {
						write(sockets[j], notificacion, strlen(notificacion));
					}
					pthread_mutex_unlock(&mutex);
				}
					
            } else {
                strcpy(buff2, "Error: Formato de nombre incorrecto");
                write(socket_conn, buff2, strlen(buff2));
                break;
            }
            //Aqui le enviamos toda la lista de conectados a todos los clientes para que la actualizen
			char notificacion[200];
			

		}
		else if (codigo ==1) //piden registrar un usuario teniendo nombre y constrase�a si no esta ya en la base de datos manda un Ok
		{
			strcpy(buff2,"");
			pthread_mutex_lock(&mutex);
			char nombre[50];
			char contrasena[50];
				
			
			//Comprobamos que el mensaje recibido tienen un formato correcto
			p = strtok(NULL, "/");  // Obtenemos el usuario
			if (p != NULL) {
				strcpy(nombre, p);  // Guardar el nombre
				strcpy(user,p);
			} else {
				strcpy(buff2, "1~$Error: Formato de nombre incorrecto");
				write(socket_conn, buff2, strlen(buff2));
				break;
			}

			p = strtok(NULL, "/");  // Obtener la contraseña
			if (p != NULL) {
				strcpy(contrasena, p);  // Guardar la contraseña
			}else {
				strcpy(buff2, "1~$Error: Formato de contrase�a incorrecto");
				write(socket_conn, buff2, strlen(buff2));
				break;
			}
			
			printf("Se ha registrado el usuario: %s con contrae�a: %s", nombre, contrasena);


			/*Construir la consulta SQL para registrar usuario 
			y despues creamos comprobamos si el usuario existe*/
			char query[512];
			sprintf(query, "INSERT INTO Jugadores (nombre, pasword, numeroPokemons, victorias, derrotas, pos) "
					"SELECT * FROM (SELECT '%s' AS nombre, '%s' AS pasword, 0 AS numeroPokemons, "
					"0 AS victorias, 0 AS derrotas, '' AS pos) AS tmp "
					"WHERE NOT EXISTS (SELECT 1 FROM Jugadores WHERE nombre = '%s') LIMIT 1;",
					nombre, contrasena, nombre);
			
			// Ejecutar la consulta en MySQL
			if (mysql_query(conn, query) == 0 && mysql_affected_rows(conn) > 0) {
				// Si se insertó un usuario, enviar "Ok"
				strcpy(buff2, "1~$El registro ha sido exitoso");
				printf("Registro exitoso\n");
			} else {
				// Si no se pudo registrar (porque ya existe), enviar error
				strcpy(buff2, "1~$Error: Usuario ya existe");
				printf("Error: El usuario ya existe\n");
			}
			//Enviamos los resultados al cliente
			write(socket_conn, buff2, strlen(buff2));
			pthread_mutex_unlock(&mutex);
		}
		else if(codigo == 11)//Eliminar cuenta del usuario
		{
			char query[256];
			const char *campos[] = { "jugador1", "jugador2", "jugador3", "jugador4", "ganador" };
			int numCampos = 5;
			
			// 1. Actualizar todos los campos de Partidas		
			for (int i = 0; i < numCampos; i++) {
				snprintf(query, sizeof(query),
						 "UPDATE Partidas SET %s = NULL WHERE %s = '%s';",
						 campos[i], campos[i], user);
				
				if (mysql_query(conn, query)) {
					fprintf(stderr, "Error al actualizar %s: %s\n", campos[i], mysql_error(conn));
				}
			}
			// 2. Eliminar el jugador
			snprintf(query, sizeof(query),
					 "DELETE FROM Jugadores WHERE id = '%d';", userId);
			
			if (mysql_query(conn, query)) {
				fprintf(stderr, "Error al eliminar jugador: %s\n", mysql_error(conn));
			} else {
				printf("Jugador '%s' eliminado correctamente.\n", user);
			}
		}
		else if (codigo ==2) //piden iniciar sesion en un usuario con nombre y contrasena, si se encuientra en la base de datos manda un Ok
		{
			strcpy(buff2,"");
			pthread_mutex_lock(&mutex);
			char nombre[50] = "";
			char contrasena[50] = "";
			
			// Separar la consulta en nombre y contrasena
			p = strtok(NULL, "/");
			if (p != NULL) {
				strcpy(nombre, p);//Copimaos el usuario
				strcpy(user,p);
			} else {
				strcpy(buff2, "2~$Error: Formato de nombre incorrecto");
				write(socket_conn, buff2, strlen(buff2));
				break;
			}
			p = strtok(NULL, "/");
			if (p != NULL) {
				strcpy(contrasena, p); //Copiamos la contasena
			} else {
				strcpy(buff2, "2~$Error: Formato de contrasena incorrecto");
				write(socket_conn, buff2, strlen(buff2));
				break;
			}
			int idJugador = -1;  // Valor por defecto si no se encuentra el jugador
			// Consulta SQL para verificar si existe el usuario con la contraseña correcta
			printf("Buscando usuario en la base de datos\n");
			char query[512];
			sprintf(query, "SELECT id FROM Jugadores WHERE nombre = '%s' AND pasword = '%s';",
					nombre, contrasena);
			
			// Ejecutar consulta
			if (mysql_query(conn, query) == 0) {
				MYSQL_RES *res = mysql_store_result(conn);
				MYSQL_ROW row = mysql_fetch_row(res);
				printf("Haciendo cambios en la lista de conectados\n");
				
				if (row && atoi(row[0]) >0) 
				{
					if (row && row[0]) {
						idJugador = atoi(row[0]);  // Convertimos el string del id a entero
						userId = idJugador;
					}
					printf("Verificacion antes de anadir a la lista de conectados....\n");
					//Anadimos usuario a la lista de conectados
					//write(socket_conn, buff2, strlen(buff2));
                    int ana = Anade(conn, idJugador, nombre, socket_conn);
					
					printf("Intentando a�adir a la lista de conectados\n");

					if (ana ==-1)
					{
						printf("No se ha podido anadir a %s a la lista de conectados\n",nombre);
						sprintf(buff2,"2~$No te has podido conectar");
						write(socket_conn, buff2, strlen(buff2));
						pthread_mutex_unlock(&mutex);
						
						break;
					}
					else
					{
						printf("Usuario %s en lista de conenctados\n",nombre);
						

						//Aqui le enviamos toda la lista de conectados a todos los clientes para que la actualizen
						char listaConectados[1024] = "";
						if (VerListaConectados(conn, listaConectados) == 0) {
							char notificacion[1100] = "";
							sprintf(notificacion, "100~$%s\n", listaConectados);
							
							for (int j = 0; j < i; j++) {
								
								write(sockets[j], notificacion, strlen(notificacion));
							}
						}
					}

					sprintf(buff2, "2~$1/%d\n", userId); // Usuario encontrado
					
					printf("%s\n",buff2);
                    
				} 
				else 
				{
					strcpy(buff2, "2~$Error: Usuario o contrasena incorrectos");
					printf("%s\n",buff2);
				}
				
				mysql_free_result(res);
			} else {
				strcpy(buff2, "2~$Error: Problema con la base de datos");
				printf("%s",buff2);
			}
			
			// Enviar respuesta al cliente
			write(socket_conn, buff2, strlen(buff2));

			pthread_mutex_unlock(&mutex);
		}
		//Formato Consulta: 3/NombreJugador
		else if (codigo ==3) //Que pokemons tiene el jugador/ cliente
		{
			//Extraer el nombre del cliente
			strcpy(buff2,"");
			char nombrecliente[50] = "";
			// Extraer el nombre del Pokemon que en�a el cliente
			p = strtok(NULL, "/");
			if (p != NULL) {
				strcpy(nombrecliente, p);
			} else {
				strcpy(buff2, "3~$Error: Formato incorrecto");
				write(socket_conn, buff2, strlen(buff2));
				break;
			}
			
			printf("Buscando los pokemosn que tiene el jugador -> '%s'\n", nombrecliente);

			// Consulta SQL para obtener toda la informacion del Pokemon
			char query[512];
			sprintf(query, "SELECT Pokedex.*, Relacio.Nivell FROM Jugadores JOIN Relacio ON Jugadores.id = Relacio.IdJ JOIN Pokedex ON Relacio.IdP = Pokedex.id WHERE Jugadores.nombre = '%s';",nombrecliente);
			
			// Ejecutar consulta
			if (mysql_query(conn, query) == 0) {
				MYSQL_RES *res = mysql_store_result(conn);
				MYSQL_ROW row = mysql_fetch_row(res);

				if(row == NULL){
					printf("El jugador %s no tiene pokemons", nombrecliente);
					sprintf(buff2, "3~$El jugador %s no tiene pokemons", nombrecliente);
				}else{
					char listaPokemons[2048]="";
					while(row != NULL){
						sprintf(listaPokemons, "%s/%s/%s/%s/%s/%s/%s/%s/%s/%s/%s#", listaPokemons, row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8], row[9]);
						row = mysql_fetch_row(res);
					}
					printf("Pokemons: %s",listaPokemons);
					sprintf(buff2, "3~$%s",listaPokemons);
				}

				mysql_free_result(res);
			} else {
				strcpy(buff2, "3~$Error: Problema con la base de datos");
			}

			//Enviamos el mensaje
			write(socket_conn, buff2, strlen(buff2));

		}

		else if (codigo ==30) //Que pokemons tiene el jugador/ cliente
		{
			//Extraer el nombre del cliente
			strcpy(buff2,"");
			
			printf("Importando Pokedex al usuario %s",user);

			// Consulta SQL para obtener toda la informacion del Pokemon
			char query[512];
			sprintf(query, "SELECT * FROM Pokedex");
			
			// Ejecutar consulta
			if (mysql_query(conn, query) == 0) {
				MYSQL_RES *res = mysql_store_result(conn);
				MYSQL_ROW row = mysql_fetch_row(res);

				if(row == NULL){
					printf("No hay Pokemons en la Pokedex");
					sprintf(buff2, "30~$");
				}else{
					char listaPokemons[16384]="";
					while(row != NULL){
						sprintf(listaPokemons, "%s/%s/%s/%s/%s/%s/%s/%s/%s/%s/%s#", listaPokemons, row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8], row[9]);
						row = mysql_fetch_row(res);
					}
					printf("Pokemons: %s",listaPokemons);
					sprintf(buff2, "31~$%s",listaPokemons);
				}

				mysql_free_result(res);
			} else {
				strcpy(buff2, "30~$");
			}

			//Enviamos el mensaje
			write(socket_conn, buff2, strlen(buff2));

		}
		//Formato de la Consulta : 4/nombreJugador
		else if (codigo ==4) //Lista de partidas donde el juagador esta
		{
			char nombreCliente[50] = "";
			strcpy(buff2,"");
			
			// Extraer el nombre del cliente
			p = strtok(NULL, "/");
			if (p != NULL) {
				strcpy (nombreCliente, p);
			} else {
				strcpy(buff2, "4~$Error: Formato incorrecto");
				write(socket_conn, buff2, strlen(buff2));
				break;
			}
			
			printf("Buscando las partidas del usuario -> Nombre: '%s'\n", nombreCliente);
			// Consulta SQL para obtener toda la informacion del Pokemon
			char query[512];
			sprintf(query, "SELECT *FROM Partidas WHERE '%s' IN (jugador1, jugador2, jugador3, jugador4);", nombreCliente);
			
			// Ejecutar consulta
			if (mysql_query(conn, query) == 0) {
				MYSQL_RES *res = mysql_store_result(conn);
				MYSQL_ROW row = mysql_fetch_row(res);

				if(row != NULL){
					char listaPartidas[2048]="";
					while(row != NULL){
						sprintf(listaPartidas,"%s/%s/%s/%s/%s/%s/%s/%s#",listaPartidas, row[0], row[1], row[2], row[3], row[4], row[5], row[6]);
						row = mysql_fetch_row(res);
					}
					sprintf(buff2,"4~$%s", listaPartidas);
					printf("El jugador  %s tiene las siguientes partidas: %s", nombreCliente, listaPartidas);
				}else{
					printf("El jugador  %s no a participado en ninguna partida", nombreCliente);
					sprintf(buff2, "4~$El jugador %s no a participado en ninguna partida", nombreCliente);
				}
				mysql_free_result(res);
			} else {
				strcpy(buff2, "4~$Error: Problema con la base de datos");
			}
			
			// Enviar respuesta al cliente
			printf("Enviamos %s al cliente", buff2);
			write(socket_conn, buff2, strlen(buff2));
		}
		//Formato de la Consulta : 5/nombrePokemon
		else if (codigo ==5) //El servidor recibe el nombre de un pokemon y le manda al cliente todos los jugadores que tienen ese pokemon
		{
			char nombrePokemon[50] = "";
			char query[512];
			strcpy(buff2,"");
			
			
			// Extraer el nombre del Pokemon
			p = strtok(NULL, "/");
			if (p != NULL) {
				strncpy(nombrePokemon, p, sizeof(nombrePokemon) - 1);
				nombrePokemon[sizeof(nombrePokemon) - 1] = '\0'; // Evitar desbordamientos
			} else {
				strcpy(buff2, "5~$Error: Formato incorrecto");
				write(socket_conn, buff2, strlen(buff2));
				break;
			}
			printf("Buscando los jugadores que tienen el Pokemon %s\n", nombrePokemon);
			
			// Construccion de la consulta SQL con formato seguro
			snprintf(query, sizeof(query),
					 "SELECT Jugadores.nombre FROM Jugadores, Pokedex, Relacio "
					 "WHERE Pokedex.nombrePokemon = '%s' AND Pokedex.id = Relacio.IdP AND Relacio.IdJ = Jugadores.id;",
					 nombrePokemon);
			// Ejecutar la consulta SQL
			if (mysql_query(conn, query) == 0) {
				MYSQL_RES *res = mysql_store_result(conn);
				if (res) {
					MYSQL_ROW row;
					strcpy(buff2, "5~$");
					while ((row = mysql_fetch_row(res)) != NULL) {
						strcat(buff2, row[0]);
						strcat(buff2, "/");
					}
					mysql_free_result(res);
				}
				// Si no se encontraron jugadores, enviar mensaje adecuado
				if (strlen(buff2) == 0) {
					snprintf(buff2, sizeof(buff2), "5~$No hay jugadores con el Pokemon %s", nombrePokemon);
				}
			} else {
				strcpy(buff2, "5~$Error: Problema con la base de datos");
				printf("Error en la consulta SQL: %s\n", mysql_error(conn));
			}
			
			// Enviar la respuesta al cliente
			write(socket_conn, buff2, strlen(buff2));
			
		}
		// Formato Consulta: 6/
		else if (codigo == 6) // Mostrar informació completa dels jugadors connectats
		{
			strcpy(buff2, "");
			char query[512];

			// Consulta SQL per obtenir totes les columnes de Jugadores dels que estan en Conectados
			sprintf(query,
				"SELECT Jugadores.* FROM Jugadores "
				"INNER JOIN Conectados ON Jugadores.nombre = Conectados.nombre;");

			if (mysql_query(conn, query) == 0) {
				MYSQL_RES *res = mysql_store_result(conn);
				MYSQL_ROW row;

				if (res != NULL) {
					strcpy(buff2, "6~$");
					while ((row = mysql_fetch_row(res)) != NULL) {
						// Suposem que Jugadores té 7 columnes (ajusta si en té més!)
						for (int col = 0; col < mysql_num_fields(res); col++) {
							strcat(buff2, row[col]);
							strcat(buff2, "/");
						}
						strcat(buff2, "#"); // Separador de jugadors
					}
					mysql_free_result(res);
				} else {
					strcpy(buff2, "6~$Error: No s'han pogut obtenir dades");
				}
			} else {
				sprintf(buff2, "6~$Error SQL: %s", mysql_error(conn));
			}

			// Enviar resposta al client
			write(socket_conn, buff2, strlen(buff2));
		}
		//Formato de la Consulta: 7/Nombre Usuario/Texto a enviar
		else if (codigo == 7)
		{
			strcpy(buff2, "");
			char Notificacion [1820];
			char nombreCliente [20];
			char textoEnviar[1800];
			
			p = strtok(NULL, "/");
			if(p != NULL)
			{
				strcpy(nombreCliente, p);
			}
			else
			{
				strcpy(buff2, "7~$Error: Formato incorrecto");
				write(socket_conn, buff2, strlen(buff2));
				break;
			}
			
			p = strtok(NULL, "/");
			if(p != NULL)
			{
				strcpy(textoEnviar, p);
			}
			else
			{
				strcpy(buff2, "7~$Error: Di algo!!!!");
				write(socket_conn, buff2, strlen(buff2));
				break;
			}
			
			sprintf(Notificacion, "101~$%s: %s\n", nombreCliente, textoEnviar);
			
			for (int j=0; j<i; j++)
			{
				printf("socket: %d", sockets[j]);
				write(sockets[j], Notificacion, strlen(Notificacion));
				
			}
		}

		else if(codigo == 8)//Seleccionar pokemon inicial
		{
			strcpy(buff2, "");
			char pokemon [1820];
			char nombre[20];
			char query[512];
			int id = 0;

			p = strtok(NULL, "/");
			if(p != NULL)
			{
				strcpy(pokemon, p);
			}
			else
			{
				strcpy(buff2, "8~$Error: Pokemon no encontrado");
				write(socket_conn, buff2, strlen(buff2));
				break;
			}
			
			p = strtok(NULL, "/");
			if(p != NULL)
			{
				strcpy(nombre, p);
			}
			else
			{
				strcpy(buff2, "8~$Error: Jugador desconocido");
				write(socket_conn, buff2, strlen(buff2));
				break;
			}

			snprintf(query, sizeof(query),
					 "INSERT INTO Relacio (IdJ, IdP, Nivell) "
					 "SELECT j.id, p.id, 1 FROM Jugadores j, Pokedex p "
					 "WHERE j.nombre = '%s' AND p.nombrePokemon = '%s';",
					nombre, pokemon);
			
			// Ejecutar la consulta INSERT
			if (mysql_query(conn, query) != 0) {
				fprintf(stderr, "Error al insertar la relacion: %s\n", mysql_error(conn));
				strcpy(buff2, "8-$Error al insertar la relacion entre jugador y Pokemon");
				write(socket_conn, buff2, strlen(buff2));
				return;
			}
			
			// Crear segunda consulta: UPDATE Jugadores
			snprintf(query, sizeof(query),
					 "UPDATE Jugadores SET numeroPokemons = numeroPokemons + 1 "
					 "WHERE nombre = '%s';", nombre);
			
			// Ejecutar la consulta UPDATE
			if (mysql_query(conn, query) != 0) {
				fprintf(stderr, "Error al actualizar numero de Pokemon: %s\n", mysql_error(conn));
				strcpy(buff2, "8-$Error al actualizar el numero de Pokemon");
				write(socket_conn, buff2, strlen(buff2));
				return;
			}
		}
		else if(codigo == 9) // 9/IdJugadorParaInvitar
		{	
			char consulta[256];
			char id[80];
			char idP[80];
			int idJugadorObjetivo;
			char mensaje[100];
			
			p = strtok(NULL, "/");
			if(p != NULL)
			{
				strcpy(id, p);
			}
			idJugadorObjetivo = atoi(id);
			
			p = strtok(NULL, "/");
			if(p != NULL)
			{
				strcpy(idP, p);
			}
			idPartida = atoi(idP);
		
			sprintf(consulta, "SELECT socket FROM Conectados WHERE idJ = %d", idJugadorObjetivo);
			// Ejecutar consulta
			if (mysql_query(conn, consulta)) {
				fprintf(stderr, "Error al consultar el socket: %s\n", mysql_error(conn));
				return;
			}
			// Obtener resultado
			MYSQL_RES *resultado = mysql_store_result(conn);
			if (!resultado) {
				fprintf(stderr, "Error al obtener el resultado: %s\n", mysql_error(conn));
				return;
			}
			MYSQL_ROW fila = mysql_fetch_row(resultado);
			if (!fila) {
				printf("Jugador con ID %d no encontrado o no conectado.\n", idJugadorObjetivo);
				mysql_free_result(resultado);
				return;
			}
			int socketJugador = atoi(fila[0]);
			mysql_free_result(resultado);
			
			// Enviar mensaje de invitacion
			if (socketJugador < 0) {
				printf("Socket invalido para jugador con ID %d\n", idJugadorObjetivo);
				return;
			}
			sprintf(mensaje, "102~$%d/%s/%d",userId, user, idPartida);
			
			int bytes = write(socketJugador, mensaje, strlen(mensaje));
			if (bytes < 0) {
				perror("Error al enviar la invitacion");
			} else {
				printf("Invitacion enviada al jugador ID %d (socket %d)\n", idJugadorObjetivo, socketJugador);
			}
			
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
		else if(codigo == 10) // Enviar 1 pokemon aleatori de la Pokedex
		{
			strcpy(buff2, "");
			char query[512];
			sprintf(query, "SELECT * FROM Pokedex ORDER BY RAND() LIMIT 1;");
			
			if (mysql_query(conn, query) == 0) {
				MYSQL_RES *res = mysql_store_result(conn);
				MYSQL_ROW row;
				if (res != NULL) {
					strcpy(buff2, "10~$");
					while ((row = mysql_fetch_row(res)) != NULL) {
						// Format: id/nombre/ataque/hp/elemento/debilidad/fortaleza/fase/descripcion#
						for(int i; i<mysql_num_fields(res); i++){
							strcat(buff2, row[i]);
							strcat(buff2, "/");
						}	
						
						char idPChar[16];
						int idP;
						strcpy(idP,row[0]);
						idP = atoi(idPChar);
						strcat(buff2, "#");
									
						//Anadimos el pokemon a la tabla de Relacion
							char insertRelacio[256];
							sprintf(insertRelacio, "INSERT INTO Relacio (IdJ, IdP, Nivell) VALUES (%d, %d, 1);", userId, idP);
							if (mysql_query(conn, insertRelacio) != 0) {
								printf("Error afegint a Relacio: %s\n", mysql_error(conn));
								sprintf(buff2, "10~$Error SQL: %s", mysql_error(conn));
							}
							else{
								sprintf(buff2,"10~$%d",idP);
							}
					}
					mysql_free_result(res);
				}										
				else{
					sprintf(buff2, "10~$Error SQL: %s", mysql_error(conn));
				}
			} 				
		}						
	}	
	//Cerramos la conexion con el servidor
close(socket_conn); 
}


int main(int argc, char *argv[])
{
	strcpy(ubicacion, "shiva2.upc.es");
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
		

	//Aqui iniciariamos la listas de conectados
	
	pthread_t thread;
	for(;;){
		printf ("Servidor 1 Escuchando\n");
		
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
