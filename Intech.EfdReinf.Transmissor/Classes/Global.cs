using System.Security.Cryptography.X509Certificates;

namespace Intech.EfdReinf.Transmissor.Classes
{
    public static class Global
    {
        public const bool AMBIENTE_TESTES = false;
        //public const string ENDERECO_BASE_WEBSERVICE_RF = "https://efinanc.receita.fazenda.gov.br/";
        //public const string ENDERECO_BASE_WEBSERVICE_RF_PREPROD = "https://preprod-efinanc.receita.fazenda.gov.br/";
        public static X509Certificate2 Certificado { get; set; }
    }
}
