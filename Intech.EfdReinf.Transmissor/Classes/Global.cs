using System.Security.Cryptography.X509Certificates;
using Intech.EfdReinf.Entidades;

namespace Intech.EfdReinf.Transmissor.Classes
{
    public static class Global
    {
        public const bool AMBIENTE_TESTES = false;
        public const bool DEV = false;

        public const string ENDERECO_BASE_WEBSERVICE_RF_ENVIO = "https://reinf.receita.fazenda.gov.br/WsREINF/";
        public const string ENDERECO_BASE_WEBSERVICE_RF_ENVIO_PREPROD = "https://preprodefdreinf.receita.fazenda.gov.br/WsREINF/";

        public const string ENDERECO_BASE_WEBSERVICE_RF_CONSULTA = "https://reinf.receita.fazenda.gov.br/WsREINFConsultas/";
        public const string ENDERECO_BASE_WEBSERVICE_RF_CONSULTA_PREPROD = "https://preprodefdreinf.receita.fazenda.gov.br/WsREINFConsultas/";

        public const string ENDERECO_BASE_API_EFD = 
            DEV ? 
            "http://localhost/EfdReinfAPI/api/" : 
            "http://10.10.170.11/EfdReinfAPI/api/";

        public static X509Certificate2 Certificado { get; set; }

        public static string Token { get; set; }
        public static ContribuinteEntidade Contribuinte { get; set; }
        public static UsuarioEntidade Usuario { get; set; }
    }
}