using Intech.EfdReinf.Transmissor.Classes;
using System;
using System.Windows.Forms;

namespace Intech.EfdReinf.Transmissor.Controles
{
    public partial class LoginForm : UserControl
    {
        public Home HomeForm { get; }

        public LoginForm(Home homeForm)
        {
            InitializeComponent();

            HomeForm = homeForm;
        }

        private void ButtonEntrar_Click(object sender, EventArgs e)
        {
            try
            {
                Global.Token = ServiceEfdReinf.Login(TextBoxEmail.Text, TextBoxSenha.Text).AccessToken;
                HomeForm.Logar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}