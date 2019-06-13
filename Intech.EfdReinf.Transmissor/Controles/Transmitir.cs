#region Usings
using Intech.EfdReinf.Transmissor.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
#endregion

namespace Intech.EfdReinf.Transmissor.Controles
{
    public partial class Transmitir : UserControl
    {
        private const string NamespaceRetornoLoteEventos = "http://www.reinf.esocial.gov.br/schemas/retornoLoteEventos/v1_04_00";
        private const string NamespaceEvtTotal = "http://www.reinf.esocial.gov.br/schemas/evtTotal/v1_04_00";

        DispatcherHelper dispatcher = new DispatcherHelper();
        WebServicesRF webServices = new WebServicesRF();
        XmlDocument xmlRetornoEvento = new XmlDocument();
        StringBuilder logEventos = new StringBuilder();

        private bool salvar;
        private string nomeArquivoLog;

        public Transmitir()
        {
            InitializeComponent();
            
            LabelProgressBarPrimaria.Text = string.Empty;
            LabelProgressBarSecundaria.Text = string.Empty;
        }

        private void ButtonProcurar_Click(object sender, EventArgs e)
        {
            // Seleciona o arquivo gerado pelo BrPrev Reinf
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Arquivos Zip (*.zip)|*.zip";

            if (fileDialog.ShowDialog() == DialogResult.OK)
                TextBoxArquivo.Text = fileDialog.FileName;
        }

        private void ButtonTransmitir_Click(object sender, EventArgs e)
        {
            try
            {
                ButtonProcurar.Enabled = false;
                ButtonTransmitir.Enabled = false;
                TextBoxLog.Clear();

                ProgressBarSecundaria.Value = 0;
                ProgressBarPrimaria.Value = 0;

                dispatcher.DoEvents();

                // Instancia a classe que irá assinar o arquivo
                var assinador = new AssinadorXML(dispatcher, ProgressBarSecundaria);

                var dirZipDescompactado = Guid.NewGuid().ToString();

                // Descompacta o arquivo .intech
                if (!Directory.Exists(dirZipDescompactado))
                    Directory.CreateDirectory(dirZipDescompactado);

                Zip.Descompacta(TextBoxArquivo.Text, dirZipDescompactado);

                var arquivos = Directory.GetFiles(dirZipDescompactado);
                var contadorLote = 0;
                var totalLotes = arquivos.Length;

                ProgressBarPrimaria.Maximum = arquivos.Length;

                // Processa cada xml contido no arquivo .intech
                foreach (var arquivo in arquivos)
                {
                    contadorLote++;

                    LabelProgressBarPrimaria.Text = $"Processando lote {contadorLote} de {totalLotes}...";

                    // Assina o xml do lote utilizando o certificado digital selecionado
                    LabelProgressBarSecundaria.Text = "Assinando lote...";
                    ProgressBarSecundaria.Value = 0;
                    dispatcher.DoEvents();
                    var xml = assinador.AssinarEventosDoArquivo(Global.Certificado, arquivo);

                    LabelProgressBarSecundaria.Text = "Enviando lote...";
                    ProgressBarSecundaria.Value = 0;
                    ProgressBarSecundaria.Maximum = 1;
                    dispatcher.DoEvents();

                    var retorno = webServices.ReceberLoteEvento(XElement.Parse(xml.InnerXml)).ToXmlNode();

                    ProgressBarSecundaria.Value = 1;
                    dispatcher.DoEvents();

                    string tipoEvento = assinador.BuscarElementoAssinar(xml);

                    if (tipoEvento == "evtServTom")
                    {
                        var sucesso = true;
                        var eventos = BuscaEventosRetorno(retorno);

                        LabelProgressBarSecundaria.Text = "Atualizando status dos registros...";
                        ProgressBarSecundaria.Value = 0;
                        ProgressBarSecundaria.Maximum = eventos.Count;
                        dispatcher.DoEvents();

                        if (eventos.Count == 0)
                            throw new Exception("Nenhum evento encontrado.");

                        for (int i = 0; i < eventos.Count; i++)
                        {
                            // Busca o OID_OPER_FINANCEIRA na tag, onde os 3 primeiros caracteres são ignorados,
                            // E o resto da string é o OID.
                            //var idEvento = BuscaIDEvento(eventos[i]);
                            //var oidMovimento = Convert.ToDecimal(idEvento.Replace("ID9", ""));

                            var resultado = BuscaResultadoEvento(eventos[i]);

                            if (resultado == "ERRO")
                            {
                                // Mostra um dialogo para salvar o retorno
                                //SalvarArquivoRetorno(eventos[i]);

                                try
                                {
                                    var mensagem = BuscaMensagemEvento(eventos[i]);

                                    // Atualiza a ocorrência no banco utilizando o webservice da EFIWeb/WS/WSReinf.asmx
                                    //wsReinf.AtualizarOcorrenciaMovimento(oidMovimento, mensagem);
                                    sucesso = false;
                                    dispatcher.DoEvents();
                                }
                                catch (Exception ex)
                                {
                                    AdicionarLog("Erro ao atualizar ocorrência: " + BuscaMensagemEvento(eventos[i]));
                                }
                            }
                            else
                            {
                                try
                                {
                                    // Atualiza o status e o nº do recibo no banco utilizando 
                                    // o webservice da EFIWeb/WS/WSReinf.asmx
                                    var numRecibo = BuscaRecibo(eventos[i]);
                                    //wsReinf.AtualizarStatusMovimento(oidMovimento, numRecibo);
                                }
                                catch (Exception ex)
                                {
                                    // Mostra um dialogo para salvar o retorno
                                    //SalvarArquivoRetorno(eventos[i]);

                                    AdicionarLog("Erro ao atualizar movimento: " + ex.Message);
                                }
                            }

                            ProgressBarSecundaria.Increment(1);
                            dispatcher.DoEvents();
                        }

                        LabelProgressBarPrimaria.Text = string.Empty;

                        if (sucesso)
                            LabelProgressBarSecundaria.Text = "Arquivo enviado com sucesso!";
                        else
                            LabelProgressBarSecundaria.Text = "Arquivo enviado com erros.";
                    }
                    else
                    {
                        var resultado = BuscaResultado(retorno);

                        if (resultado == "ERRO")
                        {
                            // Mostra um dialogo para salvar o retorno
                            //SalvarArquivoRetorno(retorno);

                            AdicionarLog("Erro: " + BuscaMensagem(retorno));
                        }
                        else
                        {
                            LabelProgressBarPrimaria.Text = string.Empty;
                            LabelProgressBarSecundaria.Text = "Arquivo enviado com sucesso!";
                            var numRecibo = BuscaRecibo(retorno);
                            MessageBox.Show("Arquivo enviado com sucesso!\nNúmero do recibo: " + numRecibo);

                            // Atualiza o status e o nº do recibo no banco utilizando 
                            // o webservice da EFIWeb/WS/WSReinf.asmx
                            //wsReinf.AtualizarStatusArquivo(TextBoxArquivo.Text, numRecibo);
                        }
                    }

                    ProgressBarPrimaria.Increment(1);
                    dispatcher.DoEvents();
                }

                Directory.Delete(dirZipDescompactado, true);
            }
            catch (Exception ex)
            {
                AdicionarLog("Erro: " + ex.Message);
                VerificarInnerException(ex);
            }
            finally
            {
                ButtonProcurar.Enabled = true;
                ButtonTransmitir.Enabled = true;

                if (salvar)
                {
                    TextWriter writer = new StreamWriter(nomeArquivoLog);
                    writer.Write(logEventos);
                    writer.Flush();
                    writer.Close();
                }
            }
        }

        #region Parse XML Retorno
        
        private XmlNodeList BuscaEventosRetorno(XmlNode xml)
        {
            try
            {
                XPathNavigator nav = xml.CreateNavigator();
                var nsmgr = new XmlNamespaceManager(nav.NameTable);

                if (xml.GetType() == typeof(XmlDocument))
                    nsmgr = new XmlNamespaceManager(((XmlDocument)xml).NameTable);
                else
                    nsmgr = new XmlNamespaceManager(xml.OwnerDocument.NameTable);

                nsmgr.AddNamespace("a", NamespaceRetornoLoteEventos);
                nsmgr.AddNamespace("b", NamespaceEvtTotal);

                return xml.SelectNodes("//a:retornoLoteEventos/a:retornoEventos/a:evento", nsmgr);
            }
            catch (Exception ex)
            {
                SalvarArquivoRetorno(xml);
                throw new Exception("Erro no BuscaEventosRetorno()");
            }
        }

        private string BuscaIDEvento(XmlNode xml)
        {
            try
            {
                XPathNavigator nav = xml.CreateNavigator();
                var nsmgr = new XmlNamespaceManager(nav.NameTable);
                nsmgr.AddNamespace("a", NamespaceRetornoLoteEventos);
                nsmgr.AddNamespace("b", NamespaceEvtTotal);

                return xml.SelectSingleNode(".//b:Reinf/b:evtTotal", nsmgr).Attributes["id"].Value;
            }
            catch (Exception ex)
            {
                SalvarArquivoRetorno(xml);
                throw new Exception("Erro no BuscaIDEvento()");
            }
        }

        private string BuscaResultadoEvento(XmlNode xml)
        {
            try
            {
                XPathNavigator nav = xml.CreateNavigator();
                var nsmgr = new XmlNamespaceManager(nav.NameTable);
                nsmgr.AddNamespace("a", NamespaceRetornoLoteEventos);
                nsmgr.AddNamespace("b", NamespaceEvtTotal);

                return xml.SelectSingleNode(".//b:Reinf/b:evtTotal/b:ideRecRetorno/b:ideStatus/b:descRetorno", nsmgr).InnerText;
            }
            catch (Exception ex)
            {
                SalvarArquivoRetorno(xml);
                throw new Exception("Erro no BuscaResultadoEvento()");
            }
        }

        private string BuscaMensagemEvento(XmlNode xml)
        {
            try
            {
                XPathNavigator nav = xml.CreateNavigator();
                var nsmgr = new XmlNamespaceManager(nav.NameTable);
                nsmgr.AddNamespace("a", NamespaceRetornoLoteEventos);
                nsmgr.AddNamespace("b", NamespaceEvtTotal);

                return xml.SelectSingleNode(".//b:Reinf/b:evtTotal/b:ideRecRetorno/b:ideStatus/b:regOcorrs/b:dscResp", nsmgr).InnerText;
            }
            catch (Exception ex)
            {
                SalvarArquivoRetorno(xml);
                throw new Exception("Erro no BuscaMensagemEvento()");
            }
        }

        private string BuscaRecibo(XmlNode xml)
        {
            try
            {
                XPathNavigator nav = xml.CreateNavigator();
                var nsmgr = new XmlNamespaceManager(nav.NameTable);
                nsmgr.AddNamespace("a", NamespaceRetornoLoteEventos);
                nsmgr.AddNamespace("b", NamespaceEvtTotal);

                return xml.SelectSingleNode(".//b:Reinf/b:retornoEvento/b:dadosReciboEntrega/b:numeroRecibo", nsmgr).InnerText;
            }
            catch (Exception ex)
            {
                SalvarArquivoRetorno(xml);
                throw new Exception("Erro no BuscaRecibo()");
            }
        }

        private string BuscaResultado(XmlNode xml)
        {
            try
            {
                XPathNavigator nav = xml.CreateNavigator();
                var nsmgr = new XmlNamespaceManager(nav.NameTable);
                nsmgr.AddNamespace("a", NamespaceRetornoLoteEventos);
                nsmgr.AddNamespace("b", NamespaceEvtTotal);

                return xml.SelectSingleNode("//a:retornoLoteEventos/a:retornoEventos/a:evento/b:Reinf/b:evtTotal/b:ideRecRetorno/b:ideStatus/b:descRetorno", nsmgr).InnerText;
            }
            catch (Exception ex)
            {
                SalvarArquivoRetorno(xml);
                throw new Exception("Erro no BuscaResultado()");
            }
        }

        private string BuscaMensagem(XmlNode xml)
        {
            try
            {
                XPathNavigator nav = xml.CreateNavigator();
                var nsmgr = new XmlNamespaceManager(nav.NameTable);
                nsmgr.AddNamespace("a", NamespaceRetornoLoteEventos);
                nsmgr.AddNamespace("b", NamespaceEvtTotal);

                return xml.SelectSingleNode("//a:retornoLoteEventos/a:retornoEventos/a:evento/b:Reinf/b:evtTotal/b:ideRecRetorno/b:ideStatus/b:regOcorrs/b:dscResp", nsmgr).InnerText;
            }
            catch (Exception ex)
            {
                SalvarArquivoRetorno(xml);
                throw new Exception("Erro no BuscaMensagem()");
            }
        }

        #endregion

        #region Métodos Privados

        private void SalvarArquivoRetorno(XmlNode evento)
        {
            logEventos.Append(evento.InnerXml);

            if (!salvar)
            {
                var msg = MessageBox.Show("Ocorreu um erro ao enviar o arquivo. Deseja salvar o arquivo de log?", "Erro", MessageBoxButtons.YesNo);

                if (msg == DialogResult.Yes)
                {
                    var dialogoSalvar = new SaveFileDialog();
                    dialogoSalvar.FileName = "log.xml";

                    if (dialogoSalvar.ShowDialog() == DialogResult.OK)
                    {
                        nomeArquivoLog = dialogoSalvar.FileName;
                        salvar = true;
                    }
                }
            }
        }

        private void VerificarInnerException(Exception ex)
        {
            if (ex.InnerException != null)
            {
                AdicionarLog(ex.InnerException.Message);
                VerificarInnerException(ex);
            }
        }

        /// <summary>
        /// Adiciona uma mensagem na caixa de log
        /// </summary>
        /// <param name="mensagem"></param>
        private void AdicionarLog(string mensagem) => 
            TextBoxLog.Text = TextBoxLog.Text + mensagem + "\r\n";

        /// <summary>
        /// Encripta o XML para envio
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="arquivo"></param>
        /// <returns></returns>
        private XmlDocument EncriptarXml(XmlDocument xml, string arquivo)
        {
            var random = new RNGCryptoServiceProvider();
            var key = new byte[16];
            random.GetBytes(key);

            var encriptado = EncryptStringToBytes_Aes(xml.OuterXml, key);

            var IV = encriptado.Key;
            var lote = Convert.ToBase64String(encriptado.Value);

            //X509Certificate2 cert;
            //string thumbprint;

            //if (Global.AMBIENTE_TESTES)
            //{
            //    cert = new X509Certificate2("cert_preprod.cer");
            //    thumbprint = "88edffa74bf7984197c1749ba96f56372dc02bac";
            //}
            //else
            //{
            //    cert = new X509Certificate2("cert_prod.cer");
            //    thumbprint = "4f96a2a59ef1248411e0ec4b3aed7f3c3e2d6727";
            //}

            //var tagChave = Convert.ToBase64String(EncriptarPrivateKeyToRSA(IV, cert));

            var eventoCriptografado = new XmlEventoCriptografado
            {
                loteCriptografado = new LoteCriptografado
                {
                    id = "ID0",
                    //idCertificado = thumbprint,
                    //chave = tagChave,
                    lote = lote
                }
            };

            var serializer = new XmlSerializer(typeof(XmlEventoCriptografado));
            using (TextWriter writer = new StreamWriter(arquivo))
            {
                serializer.Serialize(writer, eventoCriptografado);
            }

            var xmlRetorno = new XmlDocument();
            xmlRetorno.Load(arquivo);

            return xmlRetorno;
        }

        private KeyValuePair<byte[], byte[]> EncryptStringToBytes_Aes(string plainText, byte[] key)
        {
            byte[] encrypted;
            byte[] IV;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;

                aesAlg.GenerateIV();
                IV = aesAlg.IV;

                aesAlg.Mode = CipherMode.CBC;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption. 
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            var combinedKeyIv = new byte[key.Length + IV.Length];
            Array.Copy(key, 0, combinedKeyIv, 0, key.Length);
            Array.Copy(IV, 0, combinedKeyIv, key.Length, IV.Length);

            return new KeyValuePair<byte[], byte[]>(combinedKeyIv, encrypted);
        }

        private byte[] EncriptarPrivateKeyToRSA(byte[] privateKey, X509Certificate2 certificado)
        {
            using (var cryptoProvider = (RSACryptoServiceProvider)certificado.PublicKey.Key)
            {
                return cryptoProvider.Encrypt(privateKey, false);
            }
        }

        #endregion
    }
}