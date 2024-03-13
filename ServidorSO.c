#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <stdio.h>
#include <mysql.h>
int main(int argc, char *argv[])
{
	int sock_conn, sock_listen, ret;
	struct sockaddr_in serv_adr;
	char buff[512];
	char buff2[512];
	// INICIALITZACIONS
	// Obrim el socket
	if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0)
		printf("Error creant socket");
	// Fem el bind al port
	
	
	memset(&serv_adr, 0, sizeof(serv_adr));// inicialitza a zero serv_addr
	serv_adr.sin_family = AF_INET;
	
	// asocia el socket a cualquiera de las IP de la m?quina. 
	//htonl formatea el numero que recibe al formato necesario
	serv_adr.sin_addr.s_addr = htonl(INADDR_ANY);
	// escucharemos en el port 9050
	serv_adr.sin_port = htons(9080);
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0)
		printf ("Error al bind");
	//La cola de peticiones pendientes no podr? ser superior a 4
	if (listen(sock_listen, 2) < 0)
		printf("Error en el Listen");
	
	//Inicio el MYSQL
	MYSQL *conn;
	int err;
	// Estructura especial para almacenar resultados de consultas 
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	//Creamos una conexion al servidor MYSQL 
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexi\ufff3n: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	//inicializar la conexin
	conn = mysql_real_connect (conn, "localhost","root", "mysql", "parchis",0, NULL, 0);
	if (conn==NULL) {
		printf ("Error al inicializar la conexion: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	
	int i;
	// Atenderemos solo 5 peticione
	for(i=0;i<7;i++){
		printf ("Escuchando\n");
		
		sock_conn = accept(sock_listen, NULL, NULL);
		printf ("He recibido conexi?n\n");
		//sock_conn es el socket que usaremos para este cliente
		
		// Ahora recibimos su nombre, que dejamos en buff
		ret=read(sock_conn,buff, sizeof(buff));
		printf ("Recibido\n");
		
		// Tenemos que a?adirle la marca de fin de string 
		// para que no escriba lo que hay despues en el buffer
		buff[ret]='\0';
		
		//Escribimos el nombre en la consola
		
		printf ("Se ha conectado: %s\n",buff);
		
		
		
		
		
		char *p = strtok( buff, "/");
		int codigo =  atoi (p);
		
		
		if (codigo == 1) //piden la longitd del nombre
		{
			p = strtok( NULL, "/");
			char nombre[20];
			strcpy (nombre, p);
			printf ("Codigo: %d, Nombre: %s\n", codigo, nombre);
			sprintf (buff2,"%d,",strlen(nombre));
		}
		else if(codigo ==2)//Codigo de inicio de secion.
		{
			//Codigo de inicio de secion.
			char nomUsuario[20];
			char contra[20];
			
			p= strtok(NULL, "/");
			strcpy(nomUsuario, p);
			
			p= strtok(NULL, "/");
			strcpy(contra, p);
			
			//faig la consulta
			char consulta [1000];
			strcpy(consulta, "SELECT usuario.Nombre_Usuario FROM usuario WHERE usuario.Nombre_Usuario = '");
			strcat(consulta, nomUsuario);
			strcat(consulta, "' AND usuario.Contraseña = '");
			strcat(consulta, contra);
			strcat(consulta, ";");
			
			err = mysql_query (conn, consulta);
			if(err!=0){
				printf ("Error al consultar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
						
				exit (1);
			}
			//Recollim el resultat de la consulta.
			resultado = mysql_store_result(conn);
			//ahora busco la fila de del usuario.
			row = mysql_fetch_row (resultado);
			
			//si la contrasenya i el nom d'usuari son correctes no sera null.
			if(row == NULL){
				strcpy(buff2, "-1");
				printf("No hi es!!!");
			}
			else{
				strcpy(buff2, "0");
				printf("Trobat!!");
			}
			
			// Y lo enviamos
			write (sock_conn,buff2, strlen(buff2));
		}
		else if(codigo==3)
		{
			//Codigo de inicio de secion.
			char nomUsuario[20];
			int puntos;
			
			p= strtok(NULL, "/");
			strcpy(nomUsuario, p);
			
			
			//faig la consulta
			char consulta [1000];
			strcpy(consulta, "SELECT puntos FROM usuario WHERE usuario.Nombre_Usuario = '");
			strcat(consulta, nomUsuario);
			strcat(consulta, ";");
			
			err = mysql_query (conn, consulta);
			if(err!=0){
				printf ("Error al consultar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
				
				exit (1);
			}
			//Recollim el resultat de la consulta.
			resultado = mysql_store_result(conn);
			//ahora busco la fila de del usuario.
			row = mysql_fetch_row (resultado);
			
			strcpy(buff2, row);
			
			// Y lo enviamos
			write (sock_conn,buff2, strlen(buff2));
		}
		printf ("%s\n", buff2);
		
		
		// Se acabo el servicio para este cliente
		close(sock_conn); 
	}
}
