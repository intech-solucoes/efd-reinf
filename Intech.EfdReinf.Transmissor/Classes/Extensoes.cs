using System.Xml;
using System.Xml.Linq;

namespace System
{
    public static class Extensoes
    {
        public static XmlNode ToXmlNode(this XElement element)
        {
            using (XmlReader xmlReader = element.CreateReader())
            {
                XmlDocument xmlDoc = new XmlDocument(xmlReader.NameTable);
                xmlDoc.Load(xmlReader);
                return xmlDoc;
            }
        }
    }
}
