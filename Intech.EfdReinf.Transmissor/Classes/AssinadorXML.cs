#region Usings
using System;
using System.Deployment.Internal.CodeSigning;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Windows.Forms;
using System.Xml; 
#endregion

namespace Intech.EfdReinf.Transmissor.Classes
{
    public class AssinadorXML
    {
        private string signatureMethod = @"http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";
        private string digestMethod = @"http://www.w3.org/2001/04/xmlenc#sha256";

        private ProgressBar progressBar;
        private DispatcherHelper dispatcher;

        public AssinadorXML(DispatcherHelper dispatcher, ProgressBar progressBar = null)
        {
            this.dispatcher = dispatcher;
            this.progressBar = progressBar;

            CryptoConfig.AddAlgorithm(typeof(RSAPKCS1SHA256SignatureDescription), signatureMethod);
        }

        /// <summary>
        /// Assina os eventos de um XML utilizando um certificado digital utilizando criptografia SHA-256.
        /// O webservice da Receita Federal não irá aceitar o certificado caso não possua uma chave privada.
        /// Até então não foi encontrada uma solução para buscar a chave privada em sistemas web,
        /// porém, funciona normalmente em sistemas desktop.
        /// Segundo pesquisas, esta é uma medida de segurança.
        /// </summary>
        /// <param name="certificadoAssinatura">Certificado a ser utilizado para a assinatura.</param>
        /// <param name="caminhoArquivo">Caminho do XML a ser assinado.</param>
        /// <returns></returns>
        public XmlDocument AssinarEventosDoArquivo(X509Certificate2 certificadoAssinatura, string caminhoArquivo)
        {
            // Tenta carregar o XML
            XmlDocument arquivoXML = new XmlDocument();
            try
            {
                arquivoXML.Load(caminhoArquivo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possivel carregar XML indicado : " + ex.Message);
                return null;
            }

            // Cria um XmlNamespaceManager baseado no XML selecionado
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(arquivoXML.NameTable);
            nsmgr.AddNamespace("Reinf", arquivoXML.DocumentElement.NamespaceURI);

            // Seleciona todos os eventos do lote
            XmlNodeList eventos = arquivoXML.SelectNodes("//Reinf:loteEventos/Reinf:evento", nsmgr);

            if (eventos.Count <= 0)
            {
                MessageBox.Show("Não encontrou eventos no arquivo de lotes selecionado");
                return null;
            }

            if (progressBar != null)
                progressBar.Maximum = eventos.Count;

            // Assina cada evento do arquivo            
            foreach (XmlNode node in eventos)
            {
                XmlDocument xmlDocEvento = new XmlDocument();
                xmlDocEvento.LoadXml(node.InnerXml);

                // Busca o tipo de evento a assinar
                string tipoEvento = this.BuscarElementoAssinar(xmlDocEvento);
                if (tipoEvento != null)
                {
                    XmlDocument xmlDocEventoAssinado = new XmlDocument();

                    // Assina o evento
                    xmlDocEventoAssinado = this.AssinarXML(xmlDocEvento, certificadoAssinatura, tipoEvento, "id");

                    if (xmlDocEventoAssinado == null)
                        break;

                    node.InnerXml = xmlDocEventoAssinado.InnerXml;
                }
                else
                {
                    MessageBox.Show("Processo de assinatura abortado. Existem eventos invalidos para a eFinanciera");
                    break;
                }


                if (progressBar != null)
                    progressBar.Increment(1);

                dispatcher.DoEvents();
            }

            return arquivoXML;
        }

        /// <summary>
        /// Busca o tipo de evento a ser assinado.
        /// </summary>
        /// <param name="arquivo">XML de origem do evento.</param>
        /// <returns></returns>
        public string BuscarElementoAssinar(XmlDocument arquivo)
        {
            if (arquivo.OuterXml.Contains("evtInfoContri")) return "evtInfoContri";
            else if (arquivo.OuterXml.Contains("evtTabProcesso")) return "evtTabProcesso";
            else if (arquivo.OuterXml.Contains("evtServTom")) return "evtServTom";
            else if (arquivo.OuterXml.Contains("evtReabreEvPer")) return "evtReabreEvPer";
            else if (arquivo.OuterXml.Contains("evtFechaEvPer")) return "evtFechaEvPer";
            else return null;
        }

        /// <summary>
        /// Assina o XML utilizando um certificado digital.
        /// </summary>
        /// <param name="documentoXML">XML a ser assinado.</param>
        /// <param name="certificadoX509">Certificado digital a ser utilizado para a assinatura.</param>
        /// <param name="tagAAssinar">Tag a assinar.</param>
        /// <param name="idAtributoTag">Atributo ID da tag a assinar.</param>
        /// <returns></returns>
        private XmlDocument AssinarXML(XmlDocument documentoXML, X509Certificate2 certificadoX509, string tagAAssinar, string idAtributoTag)
        {
            // Variáveis utilizadas na assinatura
            XmlNodeList nodeParaAssinatura = null;
            SignedXml signedXml = null;
            Reference reference = null;
            KeyInfo keyInfo = null;
            XmlElement sig = null;
            XmlDocument xmlAssinado = null;
            bool eValido = true;

            if (eValido)
            {
                // Verifica se o certificado passado por parâmetro possui chave privada. Lembrando que o webservice
                // da Receita Federal não irá aceitar o certificado caso não possua esta chave.
                // Até então não foi encontrada uma solução para buscar a chave privada em sistemas web,
                // porém, funciona normalmente em sistemas desktop.
                // Segundo pesquisas, esta é uma medida de segurança.
                if (certificadoX509.HasPrivateKey)
                {
                    if (!tagAAssinar.Equals(string.Empty))
                    {
                        if (!idAtributoTag.Equals(string.Empty))
                        {
                            try
                            {
                                // Informando qual a tag será assinada
                                nodeParaAssinatura = documentoXML.GetElementsByTagName(tagAAssinar);
                                signedXml = new SignedXml((XmlElement)nodeParaAssinatura[0]);
                                signedXml.SignedInfo.SignatureMethod = signatureMethod;

                                RSACryptoServiceProvider privateKey = (RSACryptoServiceProvider)certificadoX509.PrivateKey;

                                if (!privateKey.CspKeyContainerInfo.HardwareDevice)
                                {
                                    CspKeyContainerInfo enhCsp = new RSACryptoServiceProvider().CspKeyContainerInfo;
                                    CspParameters cspparams = new CspParameters(enhCsp.ProviderType, enhCsp.ProviderName, privateKey.CspKeyContainerInfo.KeyContainerName);
                                    if (privateKey.CspKeyContainerInfo.MachineKeyStore)
                                    {
                                        cspparams.Flags |= CspProviderFlags.UseMachineKeyStore;
                                    }
                                    privateKey = new RSACryptoServiceProvider(cspparams);
                                }

                                // Adicionando a chave privada para assinar o documento
                                signedXml.SigningKey = privateKey;

                                // Referenciando o identificador da tag que será assinada
                                reference = new Reference("#" + nodeParaAssinatura[0].Attributes[idAtributoTag].Value);
                                reference.AddTransform(new XmlDsigEnvelopedSignatureTransform(false));
                                reference.AddTransform(new XmlDsigC14NTransform(false));
                                reference.DigestMethod = digestMethod;

                                // Adicionando a referencia de qual tag será assinada
                                signedXml.AddReference(reference);

                                // Adicionando informações do certificado na assinatura
                                keyInfo = new KeyInfo();
                                keyInfo.AddClause(new KeyInfoX509Data(certificadoX509));
                                signedXml.KeyInfo = keyInfo;

                                // Calculando a assinatura
                                signedXml.ComputeSignature();

                                // Adicionando a tag de assinatura ao documento xml
                                sig = signedXml.GetXml();
                                documentoXML.GetElementsByTagName(tagAAssinar)[0].ParentNode.AppendChild(sig);
                                xmlAssinado = new XmlDocument();
                                xmlAssinado.PreserveWhitespace = true;
                                xmlAssinado.LoadXml(documentoXML.OuterXml);
                            }
                            catch (Exception ex)
                            {
                                // Falha ao assinar documento XML
                                MessageBox.Show("Falha ao assinar documento XML. " + ex.Message);
                            }
                        }
                        else
                        {
                            MessageBox.Show("String que informa o id da tag XML a ser assinada está vazia");
                        }
                    }
                    else
                    {
                        MessageBox.Show("String que informa a tag XML a ser assinada está vazia");
                    }
                }
                else
                {
                    MessageBox.Show("Certificado Digital informado não possui chave privada");
                }
            }

            return xmlAssinado;
        }
    }
}
