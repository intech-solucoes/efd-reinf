using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Intech.EfdReinf.Transmissor
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();

            LabelVersao.Text = $"{Assembly.GetExecutingAssembly().GetName().Version.Major}.{Assembly.GetExecutingAssembly().GetName().Version.Revision}";
        }

        private void ButtonInicio_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            panel1.Controls.Add(new Home());
        }

        private void ButtonTransmitir_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            panel1.Controls.Add(new Transmissor());
        }

        private void ButtonConsultar_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            panel1.Controls.Add(new Home());
        }

        private void ButtonUtilitarios_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            panel1.Controls.Add(new Home());
        }
    }
}
