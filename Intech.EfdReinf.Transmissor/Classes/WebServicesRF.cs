using System.Xml.Linq;

namespace Intech.EfdReinf.Transmissor.Classes
{
    public class WebServicesRF
    {
        WSConsulta.ConsultasReinfClient wsConsulta;
        WSRecepcao.RecepcaoLoteReinfClient wsRecepcao;

        public WebServicesRF()
        {
            wsRecepcao = new WSRecepcao.RecepcaoLoteReinfClient();
            wsConsulta = new WSConsulta.ConsultasReinfClient();

            if (Global.AMBIENTE_TESTES)
            {
                wsRecepcao.Endpoint.Address = new System.ServiceModel.EndpointAddress(Global.ENDERECO_BASE_WEBSERVICE_RF_PREPROD + "RecepcaoLoteReinf.svc");
                wsConsulta.Endpoint.Address = new System.ServiceModel.EndpointAddress(Global.ENDERECO_BASE_WEBSERVICE_RF_PREPROD + "ConsultasReinf.svc");
            }
            else
            {
                wsRecepcao.Endpoint.Address = new System.ServiceModel.EndpointAddress(Global.ENDERECO_BASE_WEBSERVICE_RF + "RecepcaoLoteReinf.svc");
                wsConsulta.Endpoint.Address = new System.ServiceModel.EndpointAddress(Global.ENDERECO_BASE_WEBSERVICE_RF + "ConsultasReinf.svc");
            }

            wsRecepcao.ClientCredentials.ClientCertificate.Certificate = Global.Certificado;
            wsConsulta.ClientCredentials.ClientCertificate.Certificate = Global.Certificado;
        }

        public XElement ReceberLoteEvento(XElement loteEventos)
            => wsRecepcao.ReceberLoteEventos(loteEventos);

        public XElement ConsultarInformacoesCadastrais(byte tipoInscricaoContribuinte, string inscricaoContribuinte, string numeroProtocoloFechamento)
            => wsConsulta.ConsultaInformacoesConsolidadas(tipoInscricaoContribuinte, inscricaoContribuinte, numeroProtocoloFechamento);
    }
}