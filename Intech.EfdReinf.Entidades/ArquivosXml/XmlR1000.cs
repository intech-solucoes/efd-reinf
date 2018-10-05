using System;
using System.Xml.Serialization;

namespace Intech.EfdReinf.Entidades.ArquivosXml
{
    [Serializable]
    [XmlRoot(ElementName = "Reinf", Namespace = "http://www.reinf.esocial.gov.br/schemas/envioLoteEventos/v1_03_02")]
    public class XmlR1000
    {
        [XmlElement] public LoteEventos loteEventos { get; set; }
    }

    [Serializable]
    public class LoteEventos
    {
        [XmlElement] public Evento evento { get; set; }
    }

    [Serializable]
    public class Evento
    {
        [XmlAttribute(AttributeName = "id")] public string id { get; set; }

        [XmlElement(Namespace = "http://www.reinf.esocial.gov.br/schemas/evtInfoContribuinte/v1_03_02")]
        public Reinf Reinf { get; set; }
    }

    [Serializable]
    public class Reinf
    {
        public evtInfoContri evtInfoContri { get; set; }
    }

    [Serializable]
    public class evtInfoContri
    {
        [XmlAttribute(AttributeName = "id")] public string id { get; set; }

        [XmlElement] public ideEvento ideEvento { get; set; }
        [XmlElement] public ideContri ideContri { get; set; }
        [XmlElement] public infoContri infoContri { get; set; }
    }

    [Serializable]
    public class ideEvento
    {
        [XmlElement] public string tpAmb { get; set; }
        [XmlElement] public string procEmi { get; set; }
        [XmlElement] public string verProc { get; set; }
    }

    [Serializable]
    public class ideContri
    {
        [XmlElement] public string tpInsc { get; set; }
        [XmlElement] public string nrInsc { get; set; }
    }

    [Serializable]
    public class infoContri
    {
        [XmlElement] public inclusao inclusao { get; set; }
    }

    [Serializable]
    public class inclusao
    {
        [XmlElement] public idePeriodo idePeriodo { get; set; }
        [XmlElement] public infoCadastro infoCadastro { get; set; }
    }

    [Serializable]
    public class idePeriodo
    {
        [XmlElement] public string iniValid { get; set; }
        [XmlElement] public string fimValid { get; set; }
    }

    [Serializable]
    public class infoCadastro
    {
        [XmlElement] public string classTrib { get; set; }
        [XmlElement] public string indEscrituracao { get; set; }
        [XmlElement] public string indDesoneracao { get; set; }
        [XmlElement] public string indAcordoIsenMulta { get; set; }
        [XmlElement] public string indSitPJ { get; set; }
        [XmlElement] public contato contato { get; set; }
        [XmlElement] public softHouse softHouse { get; set; }
        [XmlElement] public infoEFR infoEFR { get; set; }
    }

    [Serializable]
    public class contato
    {
        [XmlElement] public string nmCtt { get; set; }
        [XmlElement] public string cpfCtt { get; set; }
        [XmlElement] public string foneFixo { get; set; }
        [XmlElement] public string email { get; set; }
    }

    [Serializable]
    public class softHouse
    {
        [XmlElement] public string cnpjSoftHouse { get; set; }
        [XmlElement] public string nmRazao { get; set; }
        [XmlElement] public string nmCont { get; set; }
        [XmlElement] public string telefone { get; set; }
        [XmlElement] public string email { get; set; }
    }

    public class infoEFR
    {
        [XmlElement] public string ideEFD { get; set; }
        [XmlElement] public string cnpjEFD { get; set; }
    }
}
