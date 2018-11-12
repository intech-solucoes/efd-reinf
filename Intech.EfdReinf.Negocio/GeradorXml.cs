#region Usings
using ICSharpCode.SharpZipLib.Zip;
using Intech.EfdReinf.Entidades;
using Intech.EfdReinf.Negocio.Proxy;
using Intech.Lib.Dominios;
using Intech.Lib.Util.Date;
using Intech.Lib.Util.Validacoes;
using Scriban;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
#endregion

namespace Intech.EfdReinf.Negocio
{
    public class GeradorXml
    {
        #region R-1000

        public void GerarR1000(decimal oidUsuario, decimal oidContribuinte, string tipoOperacao, string tipoAmbiente, string baseCaminhoArquivo)
        {
            // Busca contribuinte
            var contribuinte = new ContribuinteProxy().BuscarPorChave(oidContribuinte);
            var usuarioContribuinte = new UsuarioContribuinteProxy().BuscarPorOidUsuarioOidContribuinte(oidUsuario, oidContribuinte);
                       
            // Monta nome do arquivo
            var nomeArquivoZip = "XML_R1000_" + Guid.NewGuid().ToString() + ".zip";
            var arquivoUploadProxy = new ArquivoUploadProxy();

            var oidArquivoUpload = arquivoUploadProxy.Inserir(new ArquivoUploadEntidade
            {
                DTA_UPLOAD = DateTime.Now,
                IND_STATUS = DMN_STATUS_EFD_UPLOAD.NAO_PROCESSADO,
                NOM_ARQUIVO_LOCAL = "Upload/" + nomeArquivoZip,
                NOM_EXT_ARQUIVO = ".zip",
                NOM_ARQUIVO_ORIGINAL = nomeArquivoZip,
                NOM_DIRETORIO_LOCAL = "Upload",
                OID_USUARIO_CONTRIBUINTE = usuarioContribuinte.OID_USUARIO_CONTRIBUINTE
            });

            // Cria novo ContribuinteEnvio
            var contribuinteEnvioProxy = new ContribuinteEnvioProxy();
            contribuinteEnvioProxy.Inserir(new ContribuinteEnvioEntidade
            {
                OID_CONTRIBUINTE = oidContribuinte,
                OID_USUARIO_ENVIO = oidUsuario,
                IND_TIPO_AMBIENTE = tipoAmbiente,
                OID_ARQUIVO_UPLOAD = oidArquivoUpload
            });

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
                id =  $"ID{contribuinte.IND_TIPO_INSCRICAO}{contribuinte.COD_CNPJ_CPF.Substring(0, 8).PadLeft(14, '0')}{DateTime.Now:yyyyMMddHHmmss}{oidArquivoUpload.ToString().PadLeft(5, '0')}",
                tipoAmbiente,
                abertura_tag_operacao = tipoOperacao == DMN_OPERACAO_REGISTRO.INCLUSAO ? "<inclusao>" : tipoOperacao == DMN_OPERACAO_REGISTRO.ALTERACAO ? "<alteracao>" : "<exclusao>",
                versao = Assembly.GetExecutingAssembly().GetName().Version.ToString(3),
                ind_tipo_inscricao = contribuinte.IND_TIPO_INSCRICAO,
                cod_cnpj_cpf = contribuinte.COD_CNPJ_CPF.Substring(0, 8),
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
                cod_cnpj_efr = contribuinte.COD_CNPJ_EFR,
                fechamento_tag_operacao = tipoOperacao == DMN_OPERACAO_REGISTRO.INCLUSAO ? "</inclusao>" : tipoOperacao == DMN_OPERACAO_REGISTRO.ALTERACAO ? "</alteracao>" : "</exclusao>"
            });

            var caminhoArquivo = GerarArquivo("R1000_", baseCaminhoArquivo, xmlR1000);

            CompactarArquivo(caminhoArquivo, baseCaminhoArquivo, nomeArquivoZip);
        }

        #endregion

        #region R-1070

        public void GerarR1070(decimal oidUsuario, decimal oidContribuinte, string tipoAmbiente, string baseCaminhoArquivo)
        {
            // Busca contribuinte
            var contribuinte = new ContribuinteProxy().BuscarPorChave(oidContribuinte);
            var usuarioContribuinte = new UsuarioContribuinteProxy().BuscarPorOidUsuarioOidContribuinte(oidUsuario, oidContribuinte);

            // Monta nome do arquivo
            var nomeArquivoZip = "XML_R1070_" + Guid.NewGuid().ToString() + ".zip";
            var arquivoUploadProxy = new ArquivoUploadProxy();

            var oidArquivoUpload = arquivoUploadProxy.Inserir(new ArquivoUploadEntidade
            {
                DTA_UPLOAD = DateTime.Now,
                IND_STATUS = DMN_STATUS_EFD_UPLOAD.NAO_PROCESSADO,
                NOM_ARQUIVO_LOCAL = "Upload/" + nomeArquivoZip,
                NOM_EXT_ARQUIVO = ".zip",
                NOM_ARQUIVO_ORIGINAL = nomeArquivoZip,
                NOM_DIRETORIO_LOCAL = "Upload",
                OID_USUARIO_CONTRIBUINTE = usuarioContribuinte.OID_USUARIO_CONTRIBUINTE
            });

            R1070Proxy proxyR1070 = new R1070Proxy();
            IEnumerable<R1070Entidade> listRegistrosR1070;

            listRegistrosR1070 = proxyR1070.BuscarPorOidContribuinte(oidContribuinte);

            var eventos = from x in listRegistrosR1070
                          select new
                          {
                              id = string.Format("ID{0}{1}{2:yyyyMMddHHmmss}{3}", contribuinte.IND_TIPO_INSCRICAO, contribuinte.COD_CNPJ_CPF.PadLeft(14, '0'), DateTime.Now, oidArquivoUpload.ToString().PadLeft(5, '0')),
                              ind_ambiente_envio = tipoAmbiente,
                              versao = Assembly.GetExecutingAssembly().GetName().Version.ToString(3),
                              ind_tipo_inscricao = contribuinte.IND_TIPO_INSCRICAO,
                              cod_cnpj_cpf = contribuinte.COD_CNPJ_CPF,
                              abertura_tag_operacao = x.IND_OPERACAO_REGISTRO == DMN_OPERACAO_REGISTRO.INCLUSAO ? "<inclusao>" : x.IND_OPERACAO_REGISTRO == DMN_OPERACAO_REGISTRO.ALTERACAO ? "<alteracao>" : "<exclusao>",
                              ind_tipo_processo = x.IND_TIPO_PROCESSO,
                              num_processo = x.NUM_PROCESSO,
                              dta_inicio_validade = x.DTA_INICIO_VALIDADE.ToString("yyyy-MM"),
                              dta_fim_validade = x.DTA_FIM_VALIDADE == null ? string.Empty : Convert.ToDateTime(x.DTA_FIM_VALIDADE).ToString("yyyy-MM"),
                              ind_autoria_judicial = x.IND_AUTORIA_JUDICIAL,
                              cod_suspensao = x.COD_SUSPENSAO,
                              ind_suspensao = x.IND_SUSPENSAO,
                              dta_decisao = x.DTA_DECISAO == null ? string.Empty : Convert.ToDateTime(x.DTA_DECISAO).ToString("dd/MM/yyyy"),
                              ind_deposito_judicial = x.IND_DEPOSITO_JUDICIAL,
                              cod_uf_vara = x.COD_UF_VARA,
                              cod_municipio_vara = x.COD_MUNICIPIO_VARA,
                              cod_identificacao_vara = x.COD_IDENTIFICACAO_VARA,
                              fechamento_tag_operacao = x.IND_OPERACAO_REGISTRO == DMN_OPERACAO_REGISTRO.INCLUSAO ? "</inclusao>" : x.IND_OPERACAO_REGISTRO == DMN_OPERACAO_REGISTRO.ALTERACAO ? "</alteracao>" : "</exclusao>"
                          };


            // Monta XML
            var templateFile = Path.Combine(baseCaminhoArquivo, "../TemplatesXml", "R1070.liquid");
            var template = Template.Parse(File.OpenText(templateFile).ReadToEnd());
            var xmlR1070 = template.Render(new
            {
                eventos
            });

            var caminhoArquivo = GerarArquivo("R1070_", baseCaminhoArquivo, xmlR1070);

            CompactarArquivo(caminhoArquivo, baseCaminhoArquivo, nomeArquivoZip);

            //Atualizando a tabela R1070
            foreach (var rowR1070 in listRegistrosR1070)
            {
                rowR1070.OID_CONTRIBUINTE = oidContribuinte;
                rowR1070.OID_USUARIO_ENVIO = oidUsuario;
                rowR1070.IND_SITUACAO_PROCESSAMENTO = DMN_SITUACAO_PROCESSAMENTO.PROCESSADO;
                rowR1070.IND_AMBIENTE_ENVIO = tipoAmbiente;
                rowR1070.OID_ARQUIVO_UPLOAD = oidArquivoUpload;

                proxyR1070.Atualizar(rowR1070);
            }
        }

        #endregion

        #region R-2010

        public void GerarR2010(decimal oidUsuario, decimal oidContribuinte, string tipoOperacao, string tipoAmbiente, DateTime dtaInicial, DateTime dtaFinal, string baseCaminhoArquivo)
        {
            var mesesEntreDatas = dtaInicial.MesesEntreDatas(dtaFinal, true);

            //Intervalo intervaloDeDatas = new Intervalo(dtaFinal, dtaInicial, new CalculoAnosMesesDiasAlgoritmo2());

            if (mesesEntreDatas > 1)
                throw new Exception("Período inválido.");

            // Busca Contribuinte
            var contribuinte = new ContribuinteProxy().BuscarPorChave(oidContribuinte);
            var usuarioContribuinte = new UsuarioContribuinteProxy().BuscarPorOidUsuarioOidContribuinte(oidUsuario, oidContribuinte);

            // Monta nome do arquivo
            var nomeArquivoZip = "XML_R2010_" + Guid.NewGuid().ToString() + ".zip";
            var arquivoUploadProxy = new ArquivoUploadProxy();

            var oidArquivoUpload = arquivoUploadProxy.Inserir(new ArquivoUploadEntidade
            {
                DTA_UPLOAD = DateTime.Now,
                IND_STATUS = DMN_STATUS_EFD_UPLOAD.NAO_PROCESSADO,
                NOM_ARQUIVO_LOCAL = "Upload/" + nomeArquivoZip,
                NOM_EXT_ARQUIVO = ".zip",
                NOM_ARQUIVO_ORIGINAL = nomeArquivoZip,
                NOM_DIRETORIO_LOCAL = "Upload",
                OID_USUARIO_CONTRIBUINTE = usuarioContribuinte.OID_USUARIO_CONTRIBUINTE
            });

            R2010Proxy proxy2010 = new R2010Proxy();
            IEnumerable<R2010Entidade> listRegistrosR2010;

            switch (tipoOperacao)
            {
                case DMN_EFD_RETIFICADORA.ORIGINAL:
                    listRegistrosR2010 = proxy2010.BuscarPorOidContribuinteDtaInicioDtaFimIndSituacaoProcessamento(oidContribuinte, dtaInicial, dtaFinal, DMN_SITUACAO_PROCESSAMENTO.IMPORTADO);
                    break;
                case DMN_EFD_RETIFICADORA.RETIFICADORA:
                    listRegistrosR2010 = proxy2010.BuscarPorOidContribuinteMesEnvioAnoEnvio(oidContribuinte, dtaInicial.Month, dtaInicial.Year);
                    break;
                default:
                    throw new Exception("Tipo de operação inválido.");
            }

            if (listRegistrosR2010.Count() == 0)
                throw new Exception("Não existem registros para geração de arquivo XML.");

            var eventos = from x in listRegistrosR2010
                          group x by new { x.DTA_APURACAO.Month, x.DTA_APURACAO.Year, x.COD_CNPJ_PRESTADOR } into g
                          select new
                          {
                              id = string.Format("ID{0}{1}{2:yyyyMMddHHmmss}{3}", contribuinte.IND_TIPO_INSCRICAO, contribuinte.COD_CNPJ_CPF.PadLeft(14, '0'), DateTime.Now, oidArquivoUpload.ToString().PadLeft(5, '0')),
                              ind_retificacao = tipoOperacao,
                              dta_apuracao = string.Format("{0}-{1}", g.Key.Year, g.Key.Month),
                              ind_ambiente_envio = tipoAmbiente,
                              versao = Assembly.GetExecutingAssembly().GetName().Version.ToString(3),
                              ind_tipo_inscricao = contribuinte.IND_TIPO_INSCRICAO,
                              cod_cnpj_cpf = contribuinte.COD_CNPJ_CPF,
                              cod_insc_estabelecimento = g.First().COD_INSC_ESTABELECIMENTO,
                              cod_cnpj_cpf_obra = g.First().COD_CNPJ_CPF_OBRA,
                              ind_obra = g.First().IND_OBRA,
                              cod_cnpj_prestador = g.First().COD_CNPJ_PRESTADOR,
                              val_total_bruto = g.First().VAL_TOTAL_BRUTO.ToString().Replace('.', ','),
                              val_base_retencao = g.First().VAL_BASE_RETENCAO.ToString().Replace('.', ','),
                              val_total_retencao = g.First().VAL_TOTAL_RETENCAO.ToString().Replace('.', ','),
                              ind_cprb = g.First().IND_CPRB,
                              notas_fiscais = from y in listRegistrosR2010
                                              where y.DTA_APURACAO.Month == g.Key.Month &&
                                                    y.DTA_APURACAO.Year == g.Key.Year &&
                                                    y.COD_CNPJ_PRESTADOR == g.Key.COD_CNPJ_PRESTADOR
                                              group y by new { y.NUM_DOCUMENTO_NF } into z
                                              select new
                                              {
                                                  cod_serie_nf = z.First().COD_SERIE_NF,
                                                  num_documento_nf = z.Key.NUM_DOCUMENTO_NF,
                                                  dta_emissao_nf = z.First().DTA_EMISSAO_NF.ToString("yyyy-MM-dd"),
                                                  val_bruto_nf = z.First().VAL_BRUTO_NF.ToString().Replace('.', ','),
                                                  cod_tipo_servico = z.First().COD_TIPO_SERVICO,
                                                  val_base_ret_servico = z.First().VAL_BASE_RET_SERVICO.ToString().Replace('.', ','),
                                                  val_retencao_servico = z.First().VAL_RETENCAO_SERVICO.ToString().Replace('.', ',')
                                              }

                          };

            // Monta XML
            var templateFile = Path.Combine(baseCaminhoArquivo, "../TemplatesXml", "R2010.liquid");
            var template = Template.Parse(File.OpenText(templateFile).ReadToEnd());
            var xmlR2010 = template.Render(new
            {
                eventos
            });

            var caminhoArquivo = GerarArquivo("R2010_", baseCaminhoArquivo, xmlR2010);

            CompactarArquivo(caminhoArquivo, baseCaminhoArquivo, nomeArquivoZip);

            foreach (var item in listRegistrosR2010)
            {
                item.IND_AMBIENTE_ENVIO = tipoAmbiente;
                item.IND_RETIFICACAO = tipoOperacao;
                item.IND_SITUACAO_PROCESSAMENTO = DMN_SITUACAO_PROCESSAMENTO.PROCESSADO;
                item.OID_ARQUIVO_UPLOAD = oidArquivoUpload;
                item.OID_USUARIO_ENVIO = oidUsuario;

                proxy2010.Atualizar(item);
            }
        }

        #endregion

        #region R-2099

        public void GerarR2099(decimal oidUsuario, decimal oidContribuinte, R2099Entidade r2099, string baseCaminhoArquivo)
        {
            // Busca contribuinte
            var contribuinte = new ContribuinteProxy().BuscarPorChave(oidContribuinte);
            var usuarioContribuinte = new UsuarioContribuinteProxy().BuscarPorOidUsuarioOidContribuinte(oidUsuario, oidContribuinte);
            
            // Monta nome do arquivo
            var nomeArquivoZip = "XML_R2099_" + Guid.NewGuid().ToString() + ".zip";
            var arquivoUploadProxy = new ArquivoUploadProxy();

            var oidArquivoUpload = arquivoUploadProxy.Inserir(new ArquivoUploadEntidade
            {
                DTA_UPLOAD = DateTime.Now,
                IND_STATUS = DMN_STATUS_EFD_UPLOAD.NAO_PROCESSADO,
                NOM_ARQUIVO_LOCAL = "Upload/" + nomeArquivoZip,
                NOM_EXT_ARQUIVO = ".zip",
                NOM_ARQUIVO_ORIGINAL = nomeArquivoZip,
                NOM_DIRETORIO_LOCAL = "Upload",
                OID_USUARIO_CONTRIBUINTE = usuarioContribuinte.OID_USUARIO_CONTRIBUINTE
            });

            // Cria novo registro na R2099
            var R2099Proxy = new R2099Proxy();
            r2099.IND_SITUACAO_PROCESSAMENTO = DMN_SITUACAO_PROCESSAMENTO.PROCESSADO;
            r2099.IND_CPRB = contribuinte.IND_DESONERACAO_CPRB;
            r2099.OID_USUARIO_ENVIO = oidUsuario;
            r2099.OID_ARQUIVO_UPLOAD = oidArquivoUpload;
            var oidR2099 = R2099Proxy.Inserir(r2099);

            // Monta XML
            var templateFile = Path.Combine(baseCaminhoArquivo, "../TemplatesXml", "R2099.liquid");
            var template = Template.Parse(File.OpenText(templateFile).ReadToEnd());
            var xmlR2099 = template.Render(new
            {
                id = string.Format("ID{0}{1}{2:yyyyMMddHHmmss}{3}", contribuinte.IND_TIPO_INSCRICAO, contribuinte.COD_CNPJ_CPF.PadLeft(14, '0'), DateTime.Now, oidArquivoUpload.ToString().PadLeft(5, '0')),
                dta_periodo_Apuracao = r2099.DTA_PERIODO_APURACAO.ToString("yyyy-MM"),
                ind_ambiente_envio = r2099.IND_AMBIENTE_ENVIO,
                versao = Assembly.GetExecutingAssembly().GetName().Version.ToString(3),
                ind_tipo_inscricao = contribuinte.IND_TIPO_INSCRICAO,
                cod_cnpj_cpf = contribuinte.COD_CNPJ_CPF,
                nom_contato = contribuinte.NOM_CONTATO,
                cod_cpf_contato = contribuinte.COD_CPF_CONTATO,
                cod_fone_celular_contato = contribuinte.COD_FONE_CELULAR_CONTATO,
                txt_email_contato = contribuinte.TXT_EMAIL_CONTATO,
                ind_contratacao_serv = r2099.IND_CONTRATACAO_SERV,
                ind_prestacao_serv = r2099.IND_PRESTACAO_SERV,
                ind_associacao_desportiva = r2099.IND_ASSOCIACAO_DESPORTIVA,
                ind_repasse_assoc_desport = r2099.IND_REPASSE_ASSOC_DESPORT,
                ind_producao_rural = r2099.IND_PRODUCAO_RURAL,
                ind_desoneracao_cprb = contribuinte.IND_DESONERACAO_CPRB,
                ind_pagamentos_diversos = r2099.IND_PAGAMENTOS_DIVERSOS
            });

            var caminhoArquivo = GerarArquivo("R2099_", baseCaminhoArquivo, xmlR2099);

            CompactarArquivo(caminhoArquivo, baseCaminhoArquivo, nomeArquivoZip);
        }

        #endregion

        #region R-2098

        public void GerarR2098(decimal oidUsuario, decimal oidContribuinte, string tipoAmbiente, int ano, int mes, string baseCaminhoArquivo)
        {
            // Busca contribuinte
            var contribuinte = new ContribuinteProxy().BuscarPorChave(oidContribuinte);
            var usuarioContribuinte = new UsuarioContribuinteProxy().BuscarPorOidUsuarioOidContribuinte(oidUsuario, oidContribuinte);
            var dtaPeriodoApuracao = new DateTime(ano, mes, 1);                       

            // Monta nome do arquivo
            var nomeArquivoZip = "XML_R2098_" + Guid.NewGuid().ToString() + ".zip";
            var arquivoUploadProxy = new ArquivoUploadProxy();

            var oidArquivoUpload = arquivoUploadProxy.Inserir(new ArquivoUploadEntidade
            {
                DTA_UPLOAD = DateTime.Now,
                IND_STATUS = DMN_STATUS_EFD_UPLOAD.NAO_PROCESSADO,
                NOM_ARQUIVO_LOCAL = "Upload/" + nomeArquivoZip,
                NOM_EXT_ARQUIVO = ".zip",
                NOM_ARQUIVO_ORIGINAL = nomeArquivoZip,
                NOM_DIRETORIO_LOCAL = "Upload",
                OID_USUARIO_CONTRIBUINTE = usuarioContribuinte.OID_USUARIO_CONTRIBUINTE
            });

            //Inserinfo na R2098
            var r2098Proxy = new R2098Proxy();
            var oidR2098 = r2098Proxy.Inserir(new R2098Entidade
            {
                OID_CONTRIBUINTE = oidContribuinte,
                OID_USUARIO_ENVIO = oidUsuario,
                DTA_PERIODO_APURACAO = dtaPeriodoApuracao,
                IND_AMBIENTE_ENVIO = tipoAmbiente,
                NUM_RECIBO_ENVIO = null,
                DTA_ENVIO = null,
                IND_SITUACAO_PROCESSAMENTO_ = DMN_SITUACAO_PROCESSAMENTO.PROCESSADO,
                OID_ARQUIVO_UPLOAD = oidArquivoUpload
            });

            // Monta XML
            var templateFile = Path.Combine(baseCaminhoArquivo, "../TemplatesXml", "R2098.liquid");
            var template = Template.Parse(File.OpenText(templateFile).ReadToEnd());
            var xmlR2098 = template.Render(new
            {
                id = string.Format("ID{0}{1}{2:yyyyMMddHHmmss}{3}", contribuinte.IND_TIPO_INSCRICAO, contribuinte.COD_CNPJ_CPF.PadLeft(14, '0'), DateTime.Now, oidArquivoUpload.ToString().PadLeft(5, '0')),
                dta_periodo_Apuracao = dtaPeriodoApuracao.ToString("yyyy-MM"),
                ind_ambiente_envio = tipoAmbiente,
                versao = Assembly.GetExecutingAssembly().GetName().Version.ToString(3),
                ind_tipo_inscricao = contribuinte.IND_TIPO_INSCRICAO,
                cod_cnpj_cpf = contribuinte.COD_CNPJ_CPF
            });

            var caminhoArquivo = GerarArquivo("R2098_", baseCaminhoArquivo, xmlR2098);

            CompactarArquivo(caminhoArquivo, baseCaminhoArquivo, nomeArquivoZip);
        }

        #endregion

        #region Métodos Auxiliares

        private string GerarArquivo(string nomeInicioArquivo, string baseCaminhoArquivo, string conteudoArquivo)
        {
            var nomeArquivo = nomeInicioArquivo + Guid.NewGuid().ToString() + ".xml";
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

            AdicionaArquivoAoZip(arquivo, zipStream);
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
                AdicionaArquivoAoZip(arquivo, zipStream);
                File.Delete(arquivo);
            }

            zipStream.Finish();
            zipStream.Close();
        }

        private void AdicionaArquivoAoZip(string arquivo, ZipOutputStream zipStream)
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
