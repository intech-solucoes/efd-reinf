#region Usings
using ICSharpCode.SharpZipLib.Zip;
using Intech.EfdReinf.Entidades;
using Intech.EfdReinf.Entidades.ArquivosXml;
using Intech.EfdReinf.Negocio.Proxy;
using Intech.Lib.Dominios;
using Intech.Lib.Util.Validacoes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization; 
#endregion

namespace Intech.EfdReinf.Negocio
{
    public class GeradorXml
    {
        public void GerarR1000(decimal oidUsuario, decimal oidContribuinte, string tipoAmbiente, string baseCaminhoArquivo)
        {
            // Busca contribuinte
            var contribuinte = new ContribuinteProxy().BuscarPorChave(oidContribuinte);
            var usuarioContribuinte = new UsuarioContribuinteProxy().BuscarPorOidUsuarioOidContribuinte(oidUsuario, oidContribuinte);

            // Cria novo ContribuinteEnvio
            var contribuinteEnvioProxy = new ContribuinteEnvioProxy();
            contribuinteEnvioProxy.Inserir(new ContribuinteEnvioEntidade
            {
                OID_CONTRIBUINTE = oidContribuinte,
                OID_USUARIO_ENVIO = oidUsuario,
                IND_TIPO_AMBIENTE = tipoAmbiente
            });

            // Monta nome do arquivo
            var nomeArquivoZip = Guid.NewGuid().ToString() + ".intech";
            var arquivoUploadProxy = new ArquivoUploadProxy();

            var oidArquivoUpload = arquivoUploadProxy.Inserir(new ArquivoUploadEntidade
            {
                DTA_UPLOAD = DateTime.Now,
                IND_STATUS = DMN_STATUS_EFD_UPLOAD.NAO_PROCESSADO,
                NOM_ARQUIVO_LOCAL = "Upload/" + nomeArquivoZip,
                NOM_EXT_ARQUIVO = ".intech",
                NOM_ARQUIVO_ORIGINAL = nomeArquivoZip,
                NOM_DIRETORIO_LOCAL = "Upload",
                OID_USUARIO_CONTRIBUINTE = usuarioContribuinte.OID_USUARIO_CONTRIBUINTE
            });

            var id = "ID" + oidArquivoUpload.ToString().PadLeft(18, '0');

            if (contribuinte.IND_TIPO_INSCRICAO == DMN_TIPO_INSCRICAO_EFD.PESSOA_FISICA)
            {
                if (!Validador.ValidarCPF(contribuinte.COD_CNPJ_CPF))
                    throw new Exception("CPF inválido");
            }
            else
            {
                if (!Validador.ValidarCNPJ(contribuinte.COD_CNPJ_CPF))
                    throw new Exception("CNPJ inválido");
            }

            var inscricao = contribuinte.COD_CNPJ_CPF.LimparMascara();

            // Monta XML
            var xmlR1000 = new XmlR1000
            {
                loteEventos = new LoteEventos
                {
                    evento = new Evento
                    {
                        id = id,
                        Reinf = new Reinf
                        {
                            evtInfoContri = new evtInfoContri
                            {
                                id = id,
                                ideEvento = new ideEvento
                                {
                                    procEmi = "1",
                                    tpAmb = tipoAmbiente,
                                    verProc = Assembly.GetExecutingAssembly().GetName().Version.ToString(3)
                                },
                                ideContri = new ideContri
                                {
                                    tpInsc = contribuinte.IND_TIPO_INSCRICAO,
                                    nrInsc = inscricao
                                },
                                infoContri = new infoContri
                                {
                                    inclusao = new inclusao
                                    {
                                        idePeriodo = new idePeriodo
                                        {
                                            iniValid = contribuinte.DTA_INICIO_VALIDADE.ToString("yyyy-MM"),
                                            fimValid = contribuinte.DTA_FIM_VALIDADE?.ToString("yyyy-MM")
                                        },
                                        infoCadastro = new infoCadastro
                                        {
                                            classTrib = contribuinte.IND_CLASSIF_TRIBUT,
                                            indEscrituracao = contribuinte.IND_OBRIGADA_ECD,
                                            indDesoneracao = contribuinte.IND_DESONERACAO_CPRB,
                                            indAcordoIsenMulta = contribuinte.IND_ISENCAO_MULTA,
                                            indSitPJ = contribuinte.IND_SITUACAO_PJ,
                                            contato = new contato
                                            {
                                                nmCtt = contribuinte.NOM_CONTATO,
                                                cpfCtt = contribuinte.COD_CPF_CONTATO,
                                                foneFixo = contribuinte.COD_FONE_FIXO_CONTATO,
                                                email = contribuinte.TXT_EMAIL_CONTATO
                                            },
                                            softHouse = new softHouse
                                            {
                                                cnpjSoftHouse = "07669168000133",
                                                nmRazao = "INTECH SOLUÇÕES EM TECNOLOGIA DA INFORMAÇÃO LTDA",
                                                nmCont = "GUSTAVO HENRIQUE PERSIANO DE ALMEIDA",
                                                telefone = "6135332400",
                                                email = "gustavo@intech.com.br"
                                            },
                                            infoEFR = new infoEFR
                                            {
                                                ideEFD = contribuinte.IND_EFR,
                                                cnpjEFD = contribuinte.COD_CNPJ_EFR
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var serializer = new XmlSerializer(typeof(XmlR1000));

            var nomeArquivo = "R1000_" + Guid.NewGuid().ToString() + ".xml";
            var caminhoArquivo = Path.Combine(baseCaminhoArquivo, nomeArquivo);

            using (TextWriter writer = new StreamWriter(caminhoArquivo))
            {
                serializer.Serialize(writer, xmlR1000);
            }

            CompactarArquivo(caminhoArquivo, baseCaminhoArquivo, nomeArquivoZip);
        }

        private void CompactarArquivo(string arquivo, string caminhoArquivoDestino, string nomeArquivoDestino)
        {
            var caminho = Path.Combine(caminhoArquivoDestino, nomeArquivoDestino);
            var zipStream = new ZipOutputStream(File.Create(caminho));
            zipStream.SetLevel(5);

            adicionaArquivoAoZip(arquivo, zipStream);
            File.Delete(arquivo);

            zipStream.Finish();
            zipStream.Close();
        }

        private void CompactarArquivos(List<string> listaArquivos, string caminhoArquivoDestino, string nomeArquivoDestino)
        {
            var caminho = Path.Combine(caminhoArquivoDestino, nomeArquivoDestino);
            var zipStream = new ZipOutputStream(File.Create(caminho));
            zipStream.SetLevel(5);

            foreach (var arquivo in listaArquivos)
            {
                adicionaArquivoAoZip(arquivo, zipStream);
                File.Delete(arquivo);
            }

            zipStream.Finish();
            zipStream.Close();
        }

        private void adicionaArquivoAoZip(string arquivo, ZipOutputStream zipStream)
        {
            string caminhoRelativoArquivo = Path.GetFileName(arquivo);

            var entry = new ZipEntry(caminhoRelativoArquivo);
            entry.DateTime = DateTime.Now;
            zipStream.PutNextEntry(entry);

            using (FileStream fs = File.OpenRead(arquivo))
            {
                int sourceBytes;
                var buffer = new byte[4096];
                do
                {
                    sourceBytes = fs.Read(buffer, 0, buffer.Length);
                    zipStream.Write(buffer, 0, sourceBytes);
                } while (sourceBytes > 0);
            }
        }
    }
}