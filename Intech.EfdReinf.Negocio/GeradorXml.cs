#region Usings
using ICSharpCode.SharpZipLib.Zip;
using Intech.EfdReinf.Entidades;
using Intech.EfdReinf.Negocio.Proxy;
using Intech.Lib.Dominios;
using Intech.Lib.Util.Validacoes;
using Scriban;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
#endregion

namespace Intech.EfdReinf.Negocio
{
    public class GeradorXml
    {
        #region R-1000

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
            var templateFile = Path.Combine(baseCaminhoArquivo, "../TemplatesXml", "R1000.liquid");
            var template = Template.Parse(File.OpenText(templateFile).ReadToEnd());
            var xmlR1000 = template.Render(new
            {
                id,
                tipoAmbiente,
                versao = Assembly.GetExecutingAssembly().GetName().Version.ToString(3),
                ind_tipo_inscricao = contribuinte.IND_TIPO_INSCRICAO,
                cod_cnpj_cpf = contribuinte.COD_CNPJ_CPF,
                dta_inicio_validade = contribuinte.DTA_INICIO_VALIDADE.ToString("yyyy-MM"),
                dta_fim_validade = contribuinte.DTA_FIM_VALIDADE?.ToString("yyyy-MM"),
                ind_classif_tribut = contribuinte.IND_CLASSIF_TRIBUT,
                ind_obrigada_ecd = contribuinte.IND_OBRIGADA_ECD,
                ind_desoneracao_cprb = contribuinte.IND_DESONERACAO_CPRB,
                ind_isencao_multa = contribuinte.IND_ISENCAO_MULTA,
                ind_situacao_pj = contribuinte.IND_SITUACAO_PJ,
                nom_contato = contribuinte.NOM_CONTATO,
                cod_cpf_contato = contribuinte.COD_CPF_CONTATO,
                cod_fone_fixo_contato = contribuinte.COD_FONE_FIXO_CONTATO,
                txt_email_contato = contribuinte.TXT_EMAIL_CONTATO,
                ind_efr = contribuinte.IND_EFR,
                cod_cnpj_efr = contribuinte.COD_CNPJ_EFR
            });

            var caminhoArquivo = GerarArquivo(baseCaminhoArquivo, xmlR1000);

            CompactarArquivo(caminhoArquivo, baseCaminhoArquivo, nomeArquivoZip);
        }

        #endregion

        #region R-2010



        #endregion

        #region Métodos Auxiliares

        private string GerarArquivo(string baseCaminhoArquivo, string conteudoArquivo)
        {
            var nomeArquivo = "R1000_" + Guid.NewGuid().ToString() + ".xml";
            var caminhoArquivo = Path.Combine(baseCaminhoArquivo, nomeArquivo);

            using (TextWriter writer = new StreamWriter(caminhoArquivo))
                writer.Write(conteudoArquivo);

            return caminhoArquivo;
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

        #endregion
    }
}