using Intech.EfdReinf.Transmissor.Classes;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Intech.EfdReinf.Transmissor.Controles
{
    public partial class Home : UserControl
    {
        private Point DefaultLocation = new Point(7, 20);

        public Home()
        {
            InitializeComponent();
        }

        private void Home_Load(object sender, EventArgs e)
        {
            var friendlyName = Global.Certificado.FriendlyName.Split(':');
            LabelCNPJ.Text = friendlyName[1];
            LabelContribuinte.Text = friendlyName[0];
            LabelSituacao.Text = Global.Certificado.NotAfter > DateTime.Now ? "Ativo" : "Expirado";
            LabelValidade.Text = Global.Certificado.NotAfter.ToString("dd/MM/yyyy");

            if (Global.Token == null)
            {
                GroupBoxLogin.Controls.Clear();
                var loginForm = new LoginForm(this)
                {
                    Location = DefaultLocation
                };
                GroupBoxLogin.Controls.Add(loginForm);
            }
            else
            {
                Logar();
            }
        }

        public void Logar()
        {
            if (Global.Token != null)
            {
                GroupBoxLogin.Controls.Clear();
                var contribuintenForm = new Contribuinte(this)
                {
                    Location = DefaultLocation
                };
                GroupBoxLogin.Controls.Add(contribuintenForm);
            }
        }

        public void Deslogar()
        {
            Global.Usuario = null;
            Global.Token = null;
            Global.Contribuinte = null;

            GroupBoxLogin.Controls.Clear();
            var loginForm = new LoginForm(this)
            {
                Location = DefaultLocation
            };
            GroupBoxLogin.Controls.Add(loginForm);
        }
    }
}