using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBackup
{
    class Principal
    {

        public static int i = 0, j = 0, k = 0;
        public static string linea, origen, destino, strFecha, tiempoTotal;
        public static StreamReader LeerRutas;

        public static ArrayList rutas = new ArrayList();
        static List<float> peso = new List<float>();

        static DateTime fecha = new DateTime();
        static DateTime fechaFinal = new DateTime();
        static readonly Stopwatch cronometro = new Stopwatch();

        private static string ruta = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6) + "/Rutas.txt";

        private static string rutaNueva = AppDomain.CurrentDomain.BaseDirectory;

        public void iniciar()
        {
            fecha = DateTime.Now;
            strFecha = Convert.ToDateTime(fecha).ToString("dd-MM-yyyy");
            tiempoTotal = Convert.ToDateTime(fecha).ToString("dd-MM-yyyy HH:mm:ss");

            LeerTXT();

            string logAutoBackup = destino + "\\Log AutoBackup";
            string txtExcepciones = destino + "\\Log AutoBackup\\" + strFecha + "__Excepciones.txt";
            string txtResumen = destino + "\\Log AutoBackup\\" + strFecha + "__Resumen.txt";

            if (!File.Exists(destino))
            {
                Directory.CreateDirectory(destino);
            }

            if (File.Exists(txtExcepciones))
            {
                File.Delete(txtExcepciones);
            }
            else
            {
                Directory.CreateDirectory(logAutoBackup);
            }

            if (File.Exists(txtResumen))
            {
                File.Delete(txtResumen);
            }
            else
            {
                Directory.CreateDirectory(logAutoBackup);
            }

            k = contarArchivos(origen);

            escribirTXT("Copia de archivos corriendo -----> " + tiempoTotal + Environment.NewLine + "Cantidad de archivos en el origen ---> " + k + Environment.NewLine + "Tamaño del origen: " + (peso.Sum() / 1024 / 1024).ToString("F") + " Mb" + Environment.NewLine, destino + "\\Log AutoBackup\\" + strFecha + "__Resumen.txt");

            cronometro.Start();
            Backup(origen, destino);

            escribirTXT(Environment.NewLine + "Copia de archivos finalizada con exito !!" + Environment.NewLine + i + " archivos respaldados" + Environment.NewLine + "Cantidad de excepciones capturadas -----> " + j, (destino + "\\Log AutoBackup\\" + strFecha + "__Excepciones.txt"));

            fechaFinal = DateTime.Now;
            string tiempoFinal = Convert.ToDateTime(fechaFinal).ToString("dd-MM-yyyy HH:mm:ss");

            strFecha = Convert.ToDateTime(fecha).ToString("dd-MM-yyyy");
            tiempoTotal = Convert.ToDateTime(fecha).ToString("dd-MM-yyyy HH:mm:ss");

            cronometro.Stop();
            TimeSpan ts = cronometro.Elapsed;
            string duracion = ts.ToString("hh' horas 'mm' minutos 'ss','fff' segundos'");

            escribirTXT(Environment.NewLine + "AutoBackup terminó ---> " + tiempoFinal + Environment.NewLine + "Copia finalizada en ---> " + duracion + Environment.NewLine + "Archivos copiados ---> " + i + Environment.NewLine + "Excepciones ---> " + j, (destino + "\\Log AutoBackup\\" + strFecha + "__Resumen.txt"));

            //Console.WriteLine("Peso copiado: " + peso.Sum());
            //System.Windows.MessageBox.Show("La copia de archivos finalizo satisfactoriamente !!" + Environment.NewLine + Environment.NewLine + "Path de Origen: " + origen + Environment.NewLine + "Path de destino: " + destino + Environment.NewLine + Environment.NewLine + "Archivos respaldados = " + i + Environment.NewLine + "Tamaño: " + (peso.Sum() / 1024 / 1024).ToString("F") + " Mb" + Environment.NewLine + "Tiempo empleado: " + duracion + Environment.NewLine + "Excepciones: " + j, "Mensaje de AutoBackup", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            Environment.Exit(0);
        }

        public static int contarArchivos(string origenDatos)
        {

            DirectoryInfo origen2 = new DirectoryInfo(origenDatos);

            string[] archivos = Directory.GetFiles(origen2.FullName, "*.*", SearchOption.AllDirectories);
            foreach (string archivo in archivos)
            {
                try
                {
                    Principal.rutas.Add(archivo);
                    FileInfo file = new FileInfo(archivo);
                    Principal.peso.Add(file.Length);
                }
                catch (IOException)
                {
                    //j++;
                }
                catch (UnauthorizedAccessException)
                {
                    //j++;
                }
            }
            return rutas.Count;

        }

        public static void Backup(string pathOrigen, string pathDestino)
        {

            DirectoryInfo origen = new DirectoryInfo(pathOrigen);
            DirectoryInfo destino = new DirectoryInfo(pathDestino);

            foreach (string dirPath in Directory.GetDirectories(pathOrigen, "*", SearchOption.AllDirectories))
            {
                try
                {
                    Directory.CreateDirectory(dirPath.Replace(pathOrigen, pathDestino));

                }
                catch (UnauthorizedAccessException exception)
                {
                    j++;
       
                        escribirTXT(tiempoTotal + " --> " + exception.Message, (pathDestino + "\\Log AutoBackup\\" + strFecha + "__Excepciones.txt"));
                }
                catch (IOException ex)
                {
                    j++;
                 
                        escribirTXT(tiempoTotal + " --> " + ex.Message, (pathDestino + "\\Log AutoBackup\\" + strFecha + "__Excepciones.txt"));
                }

            }
            foreach (string pathNuevo in Directory.GetFiles(pathOrigen, "*", SearchOption.AllDirectories))
            {
                try
                {
                    i++;
                    //Console.WriteLine(i);
                    File.Copy(pathNuevo, pathNuevo.Replace(pathOrigen, pathDestino), true);

                    DirectoryInfo orig = new DirectoryInfo(pathNuevo);

                    escribirTXT(i + "-> Copiado --> " + pathNuevo , pathDestino + "\\Log AutoBackup\\" + strFecha + "__Resumen.txt");


                }
                catch (UnauthorizedAccessException exception)
                {
                    j++;
                    escribirTXT(tiempoTotal + " --> " + exception.Message, (pathDestino + "\\Log AutoBackup\\" + strFecha + "__Excepciones.txt"));
                }
                catch (IOException ex)
                {
                    j++;
                    escribirTXT(tiempoTotal + " --> " + ex.Message, (pathDestino + "\\Log AutoBackup\\" + strFecha + "__Excepciones.txt"));
                }

            }
        }

        public static void escribirTXT(string excepcion, string path)
        {

            try
            {
                StreamWriter txt = new StreamWriter(path, true, Encoding.Default);

                txt.WriteLine(excepcion + Environment.NewLine);
                txt.Close();

            }
            catch (UnauthorizedAccessException exep)
            {
                System.Windows.Forms.MessageBox.Show("Imposible acceder a la ruta de destino destino" + Environment.NewLine + Environment.NewLine + exep.ToString(), "Error");
                Environment.Exit(0);
            }
        }

        public static void LeerTXT()
        {

            rutaNueva = rutaNueva + "archivo";
            //Console.WriteLine(rutaNueva);

            LeerRutas = new StreamReader(rutaNueva, Encoding.Default, true);

            while ((linea = LeerRutas.ReadLine()) != null)
            {

                if (linea.Contains("source"))
                {
                    string rutaOrigen = convertirAString(linea.Substring(8));
                    //Console.WriteLine(rutaOrigen);
                    origen = rutaOrigen;
                }
                if (linea.Contains("target"))
                {
                    string rutaDestino = convertirAString(linea.Substring(8));

                    //Console.WriteLine(rutaDestino);
                    destino = rutaDestino;
                }
            }
            LeerRutas.Close();
        }

        static string convertirAString(String texto)
        {

            byte[] raw = new byte[texto.Length / 2];

            try
            {
                for (int i = 0; i < raw.Length; i++)
                {
                    raw[i] = Convert.ToByte(texto.Substring(i * 2, 2), 16);
                }

            }
            catch (Exception ex)
            {
            }
            return Encoding.UTF8.GetString(raw);
        }
    }
}
