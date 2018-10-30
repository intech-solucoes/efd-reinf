using System;
using System.Xml;
using System.Xml.Serialization;

namespace Intech.EfdReinf.Transmissor.Classes
{
    [Serializable]
    [XmlRoot(ElementName = "eFinanceira", Namespace = "http://www.eFinanceira.gov.br/schemas/envioLoteCriptografado/v1_2_0")]
    public class XmlEventoCriptografado
    {
        [XmlElement]
        public LoteCriptografado loteCriptografado { get; set; }
    }

    [Serializable]
    public class LoteCriptografado
    {
        [XmlElement]
        public string id { get; set; }

        [XmlElement]
        public string idCertificado { get; set; }

        [XmlElement]
        public string chave { get; set; }

        [XmlElement]
        public string lote { get; set; }
    }
}
