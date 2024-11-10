using System;
using System.Windows.Forms;

namespace AutoBackup {
    class Program {

        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Form1 f = new Form1();
            //f.Show();
            //Application.Run();
            new Principal().iniciar();
        }
    }
}

