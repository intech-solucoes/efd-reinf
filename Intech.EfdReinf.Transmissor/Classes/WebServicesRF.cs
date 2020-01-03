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
                wsRecepcao.Endpoint.Address = new System.ServiceModel.EndpointAddress(Global.ENDERECO_BASE_WEBSERVICE_RF_ENVIO_PREPROD + "RecepcaoLoteReinf.svc");
                wsConsulta.Endpoint.Address = new System.ServiceModel.EndpointAddress(Global.ENDERECO_BASE_WEBSERVICE_RF_CONSULTA_PREPROD + "ConsultasReinf.svc");
            }
            else
            {
                wsRecepcao.Endpoint.Address = new System.ServiceModel.EndpointAddress(Global.ENDERECO_BASE_WEBSERVICE_RF_ENVIO + "RecepcaoLoteReinf.svc");
                wsConsulta.Endpoint.Address = new System.ServiceModel.EndpointAddress(Global.ENDERECO_BASE_WEBSERVICE_RF_CONSULTA + "ConsultasReinf.svc");
            }

            wsRecepcao.ClientCredentials.ClientCertificate.Certificate = Global.Certificado;
            wsConsulta.ClientCredentials.ClientCertificate.Certificate = Global.Certificado;
        }

        public XElement ReceberLoteEvento(XElement loteEventos)
            => wsRecepcao.ReceberLoteEventos(loteEventos);

        public XElement ConsultarInformacoesCadastrais(byte tipoInscricaoContribuinte, string inscricaoContribuinte, string numeroProtocoloFechamento)
            => wsConsulta.ConsultaInformacoesConsolidadas(tipoInscricaoContribuinte, inscricaoContribuinte, numeroProtocoloFechamento);

        public XElement ConsultaReciboEvento1000(byte tipoInscricaoContribuinte, string inscricaoContribuinte)
            => wsConsulta.ConsultaReciboEvento1000(1000, tipoInscricaoContribuinte, inscricaoContribuinte);

        public XElement ConsultaReciboEvento1070(byte tipoInscricaoContribuinte, string inscricaoContribuinte)
            => wsConsulta.ConsultaReciboEvento1070(1070, tipoInscricaoContribuinte, inscricaoContribuinte);

        public XElement ConsultaReciboEvento2010(byte tipoInscricaoContribuinte, string inscricaoContribuinte, string perApur, byte tpInscEstab, string nrInscEstab, string cnpjPrestador)
            => wsConsulta.ConsultaReciboEvento2010(2010, tipoInscricaoContribuinte, inscricaoContribuinte, perApur, tpInscEstab, nrInscEstab, cnpjPrestador);

        //public XElement ConsultaReciboEvento2020(byte tipoInscricaoContribuinte, string inscricaoContribuinte, string numeroProtocoloFechamento)
        //    => wsConsulta.ConsultaReciboEvento2020(tipoInscricaoContribuinte, inscricaoContribuinte, numeroProtocoloFechamento);

        //public XElement ConsultaReciboEvento2030(byte tipoInscricaoContribuinte, string inscricaoContribuinte, string numeroProtocoloFechamento)
        //    => wsConsulta.ConsultaReciboEvento2030(tipoInscricaoContribuinte, inscricaoContribuinte, numeroProtocoloFechamento);

        //public XElement ConsultaReciboEvento2040(byte tipoInscricaoContribuinte, string inscricaoContribuinte, string numeroProtocoloFechamento)
        //    => wsConsulta.ConsultaReciboEvento2040(tipoInscricaoContribuinte, inscricaoContribuinte, numeroProtocoloFechamento);

        //public XElement ConsultaReciboEvento2050(byte tipoInscricaoContribuinte, string inscricaoContribuinte, string numeroProtocoloFechamento)
        //    => wsConsulta.ConsultaReciboEvento2050(tipoInscricaoContribuinte, inscricaoContribuinte, numeroProtocoloFechamento);

        //public XElement ConsultaReciboEvento2060(byte tipoInscricaoContribuinte, string inscricaoContribuinte, string numeroProtocoloFechamento)
        //    => wsConsulta.ConsultaReciboEvento2060(tipoInscricaoContribuinte, inscricaoContribuinte, numeroProtocoloFechamento);

        public XElement ConsultaReciboEvento2098(byte tipoInscricaoContribuinte, string inscricaoContribuinte, string perApur)
            => wsConsulta.ConsultaReciboEvento2098(2098, tipoInscricaoContribuinte, inscricaoContribuinte, perApur);

        public XElement ConsultaReciboEvento2099(byte tipoInscricaoContribuinte, string inscricaoContribuinte, string perApur)
            => wsConsulta.ConsultaReciboEvento2099(2099, tipoInscricaoContribuinte, inscricaoContribuinte, perApur);

        //public XElement ConsultaReciboEvento3010(byte tipoInscricaoContribuinte, string inscricaoContribuinte, string numeroProtocoloFechamento)
        //    => wsConsulta.ConsultaReciboEvento3010(tipoInscricaoContribuinte, inscricaoContribuinte, numeroProtocoloFechamento);
    }
}