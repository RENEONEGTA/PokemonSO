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
int Puerto = 9020; // Puertos disponibles 50081 - 50085

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
	int *s;
	int ret;
	s =(int *) socket;
	socket_conn = *s;
	char peticion[512];
	char buff2[1000];
	
	
	
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
		printf("Se ha conectado a la base de datos con exito\n");
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

		else if (codigo ==2) //piden iniciar sesion en un usuario con nombre y contrase�a, si se encuientra en la base de datos manda un Ok
		{
			strcpy(buff2,"");
			pthread_mutex_lock(&mutex);
			char nombre[50] = "";
			char contrasena[50] = "";
			
			// Separar la consulta en nombre y contraseña
			p = strtok(NULL, "/");
			if (p != NULL) {
				strcpy(nombre, p);//Copimaos el usuario
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

					strcpy(buff2, "2~$1\n"); // Usuario encontrado
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

		else if(codigo == 8)
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
		else if(codigo == 9){
			
		}
		else if(codigo == 10) // Enviar 3 pokémons aleatoris de la Pokedex
		{
			strcpy(buff2, "");

			char query[512];
			sprintf(query, "SELECT * FROM Pokedex ORDER BY RAND() LIMIT 3;");

			if (mysql_query(conn, query) == 0) {
				MYSQL_RES *res = mysql_store_result(conn);
				MYSQL_ROW row;

				if (res != NULL) {
					strcpy(buff2, "10~$");
					while ((row = mysql_fetch_row(res)) != NULL) {
						// Format: id/nombre/ataque/hp/elemento/debilidad/fortaleza/fase/descripcion#
						for (int i = 0; i < mysql_num_fields(res); i++) {
							strcat(buff2, row[i]);
							strcat(buff2, "/");
						}
						strcat(buff2, "#");
					}
					mysql_free_result(res);
				} else {
					strcpy(buff2, "11~$Error: No s'han pogut obtenir pokémons");
				}
			} else {
				sprintf(buff2, "11~$Error SQL: %s", mysql_error(conn));
			}

			write(socket_conn, buff2, strlen(buff2));
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
		

	//Aqui iniciariamos la listas de conectados
	
	pthread_t thread;
	for(;;){
		printf ("Escuchando\n");
		
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
