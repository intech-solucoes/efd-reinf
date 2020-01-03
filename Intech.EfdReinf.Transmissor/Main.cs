#region Usings
using Intech.EfdReinf.Transmissor.Controles;
using System;
using System.Reflection;
using System.Windows.Forms; 
#endregion

namespace Intech.EfdReinf.Transmissor
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();

            LabelVersao.Text = $"{Assembly.GetExecutingAssembly().GetName().Version.Major}.{Assembly.GetExecutingAssembly().GetName().Version.Revision}";
        }

        private void Main_Load(object sender, EventArgs e)
        {
            TopMost = true;
            Focus();
            panel1.Controls.Clear();
            panel1.Controls.Add(new Home());
            TopMost = false;
        }

        private void ButtonInicio_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            panel1.Controls.Add(new Home());
        }

        private void ButtonTransmitir_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            panel1.Controls.Add(new Transmitir());
        }

        private void ButtonConsultar_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            panel1.Controls.Add(new Consultar());
        }

        private void ButtonUtilitarios_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            panel1.Controls.Add(new Utilitarios());
        }
    }
}