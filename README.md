Primeros proyectos en C# .net Framework 4

Aplicación de consola para realizar backups.

Para utilizarla debes adjuntar la ruta origen y destino del backup en el archivo de texto llamado "archivo", ubicado en la base del proyecto.
Las rutas deben estar codificadas en hexadecimal tal como esta en el archivo.
Yo la utilizo asociada a una tarea programada que se ejecuta a diario.

Al finalizar la copia de los archivos la aplicación genera una carperta donde colocará dos reportes con fecha x cada ejecución de los backups, 
en uno de ellos habrá un historial del los archivos copiados, el pesp de la copia, tiempo empleado, etc. y en el otro las excepciones.

Para codificar en hexadecimal el texto de los paths de origen y destino del backup 
puedes utilizar esta aplicación en mi otro repo https://github.com/ariel-fer-dev/Txt_to_Hex.git
