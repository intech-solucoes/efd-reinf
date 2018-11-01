using System.Security.Cryptography.X509Certificates;

namespace Intech.EfdReinf.Transmissor.Classes
{
    public static class Global
    {
        public const bool AMBIENTE_TESTES = true;
        public const string ENDERECO_BASE_WEBSERVICE_RF = "https://reinf.receita.fazenda.gov.br/WsREINF/";
        public const string ENDERECO_BASE_WEBSERVICE_RF_PREPROD = "https://preprodefdreinf.receita.fazenda.gov.br/WsREINF/";
        public static X509Certificate2 Certificado { get; set; }
    }
}
