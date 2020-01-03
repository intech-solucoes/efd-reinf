using Intech.EfdReinf.Entidades;
using Intech.EfdReinf.Transmissor.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Intech.EfdReinf.Transmissor.Controles
{
    public partial class Consultar : UserControl
    {
        WebServicesRF webServices = new WebServicesRF();

        public Consultar()
        {
            InitializeComponent();
        }

        private void ButtonConsultar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxCNPJ.Text))
            {
                MessageBox.Show("É necessário preencher o campo \"CNPJ\"");
                return;
            }

            if(TextBoxCNPJ.Text.Length < 14)
            {
                MessageBox.Show("O campo \"CNPJ\" possúi um tamanho inválido");
                return;
            }

            byte tipoInsc = 1;
            var cnpj = TextBoxCNPJ.Text.LimparMascara();
            var numInsc = cnpj.Substring(0, 8);

            var periodo = "";
            if(new string[] { "R-2010", "R-2098", "R-2099" }.Any(ComboBoxTipoConsulta.Text.Contains))
            {
                if (ComboBoxPeriodo.SelectedValue != null)
                    periodo = ((MesR2010)ComboBoxPeriodo.SelectedValue).DataInvertida;
                else
                {
                    var periodoDigitado = ComboBoxPeriodo.Text.Split('/');
                    periodo = $"{periodoDigitado[1]}-{periodoDigitado[0]}";
                }
            }

            var cnpjPrestador = "";
            if (ComboBoxCNPJPrestador.SelectedValue != null)
                cnpjPrestador = ((string)ComboBoxCNPJPrestador.SelectedValue).LimparMascara();
            else
                cnpjPrestador = ComboBoxCNPJPrestador.Text;

            XElement xmlRetorno;

            switch(ComboBoxTipoConsulta.Text)
            {
                case "R-1000":
                    xmlRetorno = webServices.ConsultaReciboEvento1000(tipoInsc, numInsc);
                    break;
                case "R-1070":
                    xmlRetorno = webServices.ConsultaReciboEvento1070(tipoInsc, numInsc);
                    break;
                case "R-2010":
                    xmlRetorno = webServices.ConsultaReciboEvento2010(tipoInsc, numInsc, periodo, tipoInsc, cnpj, cnpjPrestador);
                    ProcessarR2010(xmlRetorno.ToXmlNode());
                    break;
                case "R-2098":
                    xmlRetorno = webServices.ConsultaReciboEvento2098(tipoInsc, numInsc, periodo);
                    break;
                case "R-2099":
                    xmlRetorno = webServices.ConsultaReciboEvento2099(tipoInsc, numInsc, periodo);
                    ToggleCampos(true, false);
                    break;
            }
        }

        private void Consultar_Load(object sender, EventArgs e)
        {
            ComboBoxTipoConsulta.SelectedIndex = 0;

            if (Global.Contribuinte != null)
            {
                TextBoxCNPJ.Text = Global.Contribuinte.COD_CNPJ_CPF.AplicarMascara(Mascaras.CNPJ);
                TextBoxCNPJ.Enabled = false;
                LabelSubCNPJ.Visible = false;

                // Preenche períodos
                var datas = ServiceEfdReinf.BuscarDatasProcessadasEnviadasR2010(Global.Contribuinte.OID_CONTRIBUINTE);
                var listaDatas = new List<MesR2010>();

                foreach (var data in datas)
                    listaDatas.AddRange(data.Meses);

                ComboBoxPeriodo.DataSource = listaDatas;
                ComboBoxPeriodo.DisplayMember = "Data";
                //ComboBoxPeriodo.DropDownStyle = ComboBoxStyle.DropDownList;

                // Preenche prestadores
                var prestadores = ServiceEfdReinf.BuscarPrestadores(Global.Contribuinte.OID_CONTRIBUINTE);

                ComboBoxCNPJPrestador.DataSource = prestadores;
                ComboBoxCNPJPrestador.DropDownStyle = ComboBoxStyle.DropDownList;
                LabelSubPrestador.Visible = false;
            }

            ComboBoxTipoConsulta_SelectedIndexChanged(this, null);
        }

        private void ComboBoxTipoConsulta_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(ComboBoxTipoConsulta.Text)
            {
                case "R-1000":
                case "R-1070":
                    ToggleCampos(false, false);
                    break;
                case "R-2010":
                    ToggleCampos(true, true);
                    break;
                case "R-2098":
                case "R-2099":
                    ToggleCampos(true, false);
                    break;
            }
        }

        private void ToggleCampos(bool mostrarPeriodo, bool mostrarPrestador)
        {
            ComboBoxPeriodo.Visible = mostrarPeriodo;
            LabelPeriodo.Visible = mostrarPeriodo;
            LabelSubPeriodo.Visible = mostrarPeriodo;

            ComboBoxCNPJPrestador.Visible = mostrarPrestador;
            LabelPrestador.Visible = mostrarPrestador;
            LabelSubPrestador.Visible = mostrarPrestador;
        }

        private void ProcessarR2010(XmlNode xml)
        {
            var nav = xml.CreateNavigator();
            var nsmgr = new XmlNamespaceManager(nav.NameTable);
            nsmgr.AddNamespace("a", "http://www.reinf.esocial.gov.br/schemas/retornoRecibosChaveEvento/v1_04_00");

            var eventos = xml.SelectNodes("//a:Reinf/a:retornoEventos/a:evento", nsmgr);

            for(var i = 0; i < eventos.Count; i++)
            {
                var id = eventos[i].Attributes["id"].Value;
                var oidR2010 = Convert.ToDecimal(id.Substring(31));
                var numRecibo = eventos[i]["nrRecibo"].InnerText;

                ServiceEfdReinf.UpdateRecibo(oidR2010, numRecibo);
            }
        }
    }
}