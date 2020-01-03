using Intech.EfdReinf.Entidades;
using Intech.EfdReinf.Transmissor.Classes;
using System;
using System.Windows.Forms;

namespace Intech.EfdReinf.Transmissor.Controles
{
    public partial class Contribuinte : UserControl
    {
        public Home HomeForm { get; }
        public bool EditandoContribuinte { get; set; }

        public Contribuinte(Home homeForm)
        {
            InitializeComponent();

            HomeForm = homeForm;
        }

        private void Contribuinte_Load(object sender, EventArgs e)
        {
            Global.Usuario = ServiceEfdReinf.BuscarUsuario();
            LabelUsuario.Text = Global.Usuario.NOM_USUARIO;

            var contribuintes = ServiceEfdReinf.BuscarContribuintesAtivos();
            ComboBoxContribuinte.DataSource = contribuintes;
            ComboBoxContribuinte.DisplayMember = "NOM_RAZAO_SOCIAL";
            ComboBoxContribuinte.SelectedIndex = 0;
            ComboBoxContribuinte.Enabled = false;
            Global.Contribuinte = (ContribuinteEntidade)ComboBoxContribuinte.SelectedItem;
        }

        private void ButtonTrocar_Click(object sender, EventArgs e)
        {
            if (!EditandoContribuinte)
            {
                ComboBoxContribuinte.Enabled = true;
                ButtonTrocar.Text = "Selecionar";
                EditandoContribuinte = true;
            }
            else
            {
                ComboBoxContribuinte.Enabled = false;
                Global.Contribuinte = (ContribuinteEntidade)ComboBoxContribuinte.SelectedItem;
                ButtonTrocar.Text = "Trocar";
                EditandoContribuinte = false;
            }
        }

        private void ButtonSair_Click(object sender, EventArgs e)
        {
            HomeForm.Deslogar();
        }
    }
}