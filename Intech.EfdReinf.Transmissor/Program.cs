using Intech.EfdReinf.Transmissor.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Intech.EfdReinf.Transmissor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Seleciona o certificado
            Global.Certificado = CertificadoHelper.SelecionaCertificado();

            if (Global.Certificado != null)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                var main = new Main();

                Application.Run(main);
            }
        }
    }
}
