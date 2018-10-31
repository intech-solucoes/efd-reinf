using System;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace Intech.EfdReinf.Transmissor.Classes
{
    public class CertificadoHelper
    {
        public static X509Certificate2 SelecionaCertificado()
        {
            X509Certificate2 vRetorna;
            X509Certificate2 oX509Cert = new X509Certificate2();
            X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            X509Certificate2Collection collection = store.Certificates;
            X509Certificate2Collection collection1 = collection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
            X509Certificate2Collection collection2 = collection.Find(X509FindType.FindByKeyUsage, X509KeyUsageFlags.DigitalSignature, false);
            X509Certificate2Collection scollection = X509Certificate2UI.SelectFromCollection(collection2, "Certificado(s) Digital(is) disponível(is)", "Selecione o certificado digital para uso no aplicativo", X509SelectionFlag.SingleSelection);

            if (scollection.Count == 0)
            {
                string msgResultado = "Nenhum certificado digital foi selecionado ou o certificado selecionado está com problemas.";
                MessageBox.Show(msgResultado, "Advertência", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                vRetorna = null;
            }
            else
            {
                oX509Cert = scollection[0];
                vRetorna = oX509Cert;
            }

            return vRetorna;
        }
    }
}
