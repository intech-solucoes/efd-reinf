#region Usings
using Intech.EfdReinf.Entidades;
using Intech.EfdReinf.Negocio.Proxy;
using Intech.Lib.Dominios;
using Intech.Lib.Dominios.EfdReinf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text; 
#endregion

namespace Intech.EfdReinf.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportacaoController : BaseController
    {
        private List<ImportacaoCriticaEntidade> _listImportacaoCritica;
        private List<R1070Entidade> _listR1070;
        private List<R2010Entidade> _listR2010;        
        private decimal _oidArquivoUpload = 0;
        private decimal _oidContribuinte = 0;
        private int _quantidadeLinhas = 0;
        private int _numLinha = 0;
        private CultureInfo _culturePtBR = new CultureInfo("pt-BR");
        private ArquivoUploadProxy _proxyArquivoUpload = new ArquivoUploadProxy();
        private ImportacaoCriticaProxy _proxyImportacaoCritica = new ImportacaoCriticaProxy();
        R2010Proxy _proxyR2010 = new R2010Proxy();

        [HttpPost("importarCsv/{oidArquivoUpload}/{oidContribuinte}")]
        [Authorize("Bearer")]
        public ActionResult ImportarCsv(decimal oidArquivoUpload, decimal oidContribuinte)
        {
            try
            {
                _oidArquivoUpload = oidArquivoUpload;
                _oidContribuinte = oidContribuinte;

                //Excluindo as críticas anteriores do arquivo na tabela EFD_IMPORTACAO_CRITICA.
                _proxyImportacaoCritica.ExcluirPorOidArquivoUpload(_oidArquivoUpload);

                var rowArquivoUpload = _proxyArquivoUpload.BuscarPorChave(_oidArquivoUpload);

                if (rowArquivoUpload == null)
                    throw new Exception("Arquivo não encontrado.");

                if (rowArquivoUpload.IND_STATUS == DMN_STATUS_EFD_UPLOAD.PROCESSADO)
                    throw new Exception("O arquivo já foi processado.");

                var linhas = System.IO.File.ReadAllLines(rowArquivoUpload.NOM_ARQUIVO_LOCAL, Encoding.Default);
                _quantidadeLinhas = linhas.Length - 1;

                if (_quantidadeLinhas == 0)
                    throw new Exception("O arquivo está em branco.");

                _listImportacaoCritica = new List<ImportacaoCriticaEntidade>();

                string indRegistro = linhas[1].Split(';')[0];

                //Chamando o Layout correspondente ao arquivo
                switch (indRegistro)
                {
                    case DMN_REGISTRO_EFD.R_1070:
                        ImportarCsv1070(linhas);
                        break;
                    case DMN_REGISTRO_EFD.R_2010:
                        ImportarCsv2010(linhas);
                        break;
                    default:
                        break;
                }

                //Gravando as críticas na tabela EFD_IMPORTACAO_CRITICA.
                GravarImportacaoCritica();

                if (_listImportacaoCritica.Count <= 0)
                {
                    //Atualizando o status do arquivo na tabela EFD_ARQUIVO_UPLOAD.
                    AtualizarArquivoUpload(rowArquivoUpload);
                }
                else
                {
                    return BadRequest("Arquivo não importado. Existe(m) erro(s) no layout do arquivo.");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #region Arquivo R-1070

        private void ImportarCsv1070(string[] linhas)
        {
            try
            {
                _listR1070 = new List<R1070Entidade>();

                //Lendo e validando as linhas do arquivo.
                for (int i = 0; i <= _quantidadeLinhas; i++)
                {
                    _numLinha = i;

                    if (string.IsNullOrEmpty(linhas[_numLinha].Replace(";", string.Empty)))
                        continue;

                    if (_numLinha == 0)
                    {
                        ValidarLayoutCabecalhoR1070(linhas[0]);
                        continue;
                    }

                    _listR1070.Add(LerLinhaR1070(linhas[_numLinha]));
                }                

                if (_listImportacaoCritica.Count <= 0)
                {
                    //Gravando as linhas do arquivo na tabela EFD_R1070.
                    GravarR1070();
                }
            }
            catch (Exception ex)
            {
                BadRequest(ex.Message);
            }
        }

        private void ValidarLayoutCabecalhoR1070(string linha)
        {
            var colunas = linha.Split(';');

            try
            {
                if (colunas.Length != 14 ||
                    colunas[0] != "IndRegistro" ||
                    colunas[1] != "TpProc" ||
                    colunas[2] != "NrProc" ||
                    colunas[3] != "IniValid" ||
                    colunas[4] != "FimValid" ||
                    colunas[5] != "IndAutoria" ||
                    colunas[6] != "CodSusp" ||
                    colunas[7] != "IndSusp" ||
                    colunas[8] != "DtDecisao" ||
                    colunas[9] != "IndDeposito" ||
                    colunas[10] != "UfVara" ||
                    colunas[11] != "CodMunic" ||
                    colunas[12] != "IdVara" ||
                    colunas[13] != "IndOperacao")
                    throw new Exception("Layout do cabeçalho incorreto. Favor corrigir para realizar a importação.");
            }
            catch (Exception ex)
            {
                InserirNaListaDeCriticas(ex.Message);
            }
        }

        private R1070Entidade LerLinhaR1070(string linha)
        {
            R1070Entidade linhaImportacao = new R1070Entidade();
            var colunas = linha.Split(';');

            try
            {
                ValidarLayoutDaLinha(colunas, 14);

                linhaImportacao.OID_CONTRIBUINTE = _oidContribuinte;
                linhaImportacao.IND_TIPO_PROCESSO = ValidarString("'TpProc'", colunas[1], 1, true);
                linhaImportacao.NUM_PROCESSO = ValidarString("'NrProc'", colunas[2], 21, true);
                linhaImportacao.DTA_INICIO_VALIDADE = ValidarDateTime("'IniValid'", colunas[3], true);
                linhaImportacao.DTA_FIM_VALIDADE = ValidarDateTime("'FimValid'", colunas[4], true);
                linhaImportacao.IND_AUTORIA_JUDICIAL = ValidarString("'IndAutoria'", colunas[5], 1, true);
                linhaImportacao.COD_SUSPENSAO = ValidarString("'CodSusp'", colunas[6], 14, false);
                linhaImportacao.IND_SUSPENSAO = ValidarString("'IndSusp'", colunas[7], 2, false);
                linhaImportacao.DTA_DECISAO = ValidarDateTime("'DtDecisao'", colunas[8], false);
                linhaImportacao.IND_DEPOSITO_JUDICIAL = ValidarString("'IndDeposito'", colunas[9], 1, false);
                linhaImportacao.COD_UF_VARA = ValidarString("'UfVara'", colunas[10], 2, false);
                linhaImportacao.COD_MUNICIPIO_VARA = ValidarString("'CodMunic'", colunas[11], 7, false);
                linhaImportacao.COD_IDENTIFICACAO_VARA = ValidarString("'IdVara'", colunas[12], 2, false);
                linhaImportacao.IND_OPERACAO_REGISTRO = ValidarString("'IndOperacao'", colunas[13], 3, true);
                linhaImportacao.IND_SITUACAO_PROCESSAMENTO = DMN_SITUACAO_PROCESSAMENTO.IMPORTADO;                
            }
            catch (Exception ex)
            {
                InserirNaListaDeCriticas(ex.Message);
            }

            return linhaImportacao;
        }

        private void GravarR1070()
        {
            R1070Proxy proxyR1070 = new R1070Proxy();

            foreach (var item in _listR1070)
            {
                decimal oidR1070 = proxyR1070.Inserir(item);
            }
        }

        #endregion

        #region Arquivo R-2010

        private void  ImportarCsv2010(string[] linhas)
        {
            try
            {                 
                _listR2010 = new List<R2010Entidade>();

                //Lendo e validando as linhas do arquivo.
                for (int i = 0; i <= _quantidadeLinhas; i++)
                {
                    _numLinha = i;

                    if (string.IsNullOrEmpty(linhas[_numLinha].Replace(";", string.Empty)))
                        continue;

                    if (_numLinha == 0)
                    {
                        ValidarLayoutCabecalhoR2010(linhas[0]);
                        continue;
                    }

                    _listR2010.Add(LerLinhaR2010(linhas[_numLinha]));
                }

                if (_listImportacaoCritica.Count <= 0)
                {
                    //Excluindo registros existentes na tabela EFD_R2010 para o "OID_CONTRIBUINTE" e "DTA_APURACAO".
                    ExcluirR2010();

                    //Gravando as linhas do arquivo na tabela EFD_R2010.
                    GravarR2010();
                }
            }
            catch (Exception ex)
            {
                BadRequest(ex.Message);
            }
        }       

        private R2010Entidade LerLinhaR2010(string linha)
        {
            R2010Entidade linhaImportacao = new R2010Entidade();
            var colunas = linha.Split(';');

            try
            {
                ValidarLayoutDaLinha(colunas, 37);

                linhaImportacao.OID_CONTRIBUINTE = _oidContribuinte;
                linhaImportacao.DTA_APURACAO = ValidarDateTime("'DataApuracao'", colunas[1], true);
                linhaImportacao.COD_INSC_ESTABELECIMENTO = ValidarString("'TipoInscricaoEstab'", colunas[2], 1, true);
                linhaImportacao.COD_CNPJ_CPF_OBRA = ValidarString("'NumeroInscricaoEstab'", colunas[3], 14, true);
                linhaImportacao.IND_OBRA = ValidarString("'IndPrestacaoServicoObra'", colunas[4], 1, true);
                linhaImportacao.COD_CNPJ_PRESTADOR = ValidarString("'CnpjPrestador'", colunas[5], 14, true);
                linhaImportacao.VAL_TOTAL_BRUTO = ValidarDecimal("'VlrTotalBruto'", colunas[6], 17, true);
                linhaImportacao.VAL_BASE_RETENCAO = ValidarDecimal("'VlrTotalBaseRet'", colunas[7], 17, true);
                linhaImportacao.VAL_TOTAL_RETENCAO = ValidarDecimal("'VlrTotalRetPrinc'", colunas[8], 17, true);
                linhaImportacao.VAL_RETENCAO_ADICIONAL = ValidarDecimal("'VlrTotalRetAdic'", colunas[9], 17, false);
                linhaImportacao.VAL_RETENCAO_JUD_PRINC = ValidarDecimal("'VlrTotalNRetPrinc'", colunas[10], 17, false);
                linhaImportacao.VAL_RETENCAO_JUD_ADIC = ValidarDecimal("'VlrTotalNRetAdic'", colunas[11], 17, false);
                linhaImportacao.IND_CPRB = ValidarDecimal("'IndCPRB'", colunas[12], 1, true);
                linhaImportacao.COD_SERIE_NF = ValidarString("'Serie'", colunas[13], 5, true);
                linhaImportacao.NUM_DOCUMENTO_NF = ValidarString("'NumDocto'", colunas[14], 15, true);
                linhaImportacao.DTA_EMISSAO_NF = ValidarDateTime("'DtEmissaoNF'", colunas[15], true);
                linhaImportacao.VAL_BRUTO_NF = ValidarDecimal("'VlrBruto'", colunas[16], 17, true);
                linhaImportacao.DES_OBSERVACAO_NF = ValidarString("'Obs'", colunas[17], 250, false);
                linhaImportacao.COD_TIPO_SERVICO = ValidarString("'TpServico'", colunas[18], 9, true);
                linhaImportacao.VAL_BASE_RET_SERVICO = ValidarDecimal("'VlrBaseRet'", colunas[19], 17, true);
                linhaImportacao.VAL_RETENCAO_SERVICO = ValidarDecimal("'VlrRetencao'", colunas[20], 17, true);
                linhaImportacao.VAL_RET_SUBCONTRATO = ValidarDecimal("'VlrRetSub'", colunas[21], 17, false);
                linhaImportacao.VAL_RET_SERV_JUIZO = ValidarDecimal("'VlrNRetPrinc'", colunas[22], 17, false);
                linhaImportacao.VAL_SERV_15_ANOS = ValidarDecimal("'VlrServicos15'", colunas[23], 17, false);
                linhaImportacao.VAL_SERV_20_ANOS = ValidarDecimal("'VlrServicos20'", colunas[24], 17, false);
                linhaImportacao.VAL_SERV_25_ANOS = ValidarDecimal("'VlrServicos25'", colunas[25], 17, false);
                linhaImportacao.VAL_RET_ADIC_ESPEC = ValidarDecimal("'VlrAdicional'", colunas[26], 17, false);
                linhaImportacao.VAL_RET_ADIC_JUIZO = ValidarDecimal("'VlrNRetAdic'", colunas[27], 17, false);
                linhaImportacao.COD_PROCESSO_JUD = ValidarString("'TpProcRetPrinc'", colunas[28], 1, false);
                linhaImportacao.NUM_PROCESSO_JUD = ValidarString("'NrProcRetPrinc'", colunas[29], 21, false);
                linhaImportacao.COD_SUSPENSAO_PROCESSO = ValidarString("'CodSuspPrinc'", colunas[30], 14, false);
                linhaImportacao.VAL_PRINCIPAL_PROCESSO = ValidarDecimal("'ValorPrinc'", colunas[31], 17, false);
                linhaImportacao.COD_PROCESSO_JUD_ADIC = ValidarString("'TpProcRetAdic'", colunas[32], 1, false);
                linhaImportacao.NUM_PROCESSO_JUD_ADIC = ValidarString("'NrProcRetAdic'", colunas[33], 21, false);
                linhaImportacao.COD_SUSPENSAO_PROC_ADIC = ValidarString("'CodSuspAdic'", colunas[34], 14, false);
                linhaImportacao.VAL_ADICIONAL_PROCESSO = ValidarDecimal("'ValorAdic'", colunas[35], 17, false);
                linhaImportacao.NUM_RECIBO_RETIFICADA = ValidarString("'NrReciboRetificado'", colunas[36], 52, false);
                linhaImportacao.IND_SITUACAO_PROCESSAMENTO = DMN_SITUACAO_PROCESSAMENTO.IMPORTADO;
                linhaImportacao.OID_ARQUIVO_UPLOAD = _oidArquivoUpload;
            }
            catch (Exception ex)
            {
                InserirNaListaDeCriticas(ex.Message);
            }

            return linhaImportacao;
        }

        private void ValidarLayoutCabecalhoR2010(string linha)
        {
            var colunas = linha.Split(';');

            try
            {
                if (colunas.Length != 37 ||
                    colunas[0] != "IndRegistro" ||
                    colunas[1] != "DataApuracao" ||
                    colunas[2] != "TipoInscricaoEstab" ||
                    colunas[3] != "NumeroInscricaoEstab" ||
                    colunas[4] != "IndPrestacaoServicoObra" ||
                    colunas[5] != "CnpjPrestador" ||
                    colunas[6] != "VlrTotalBruto" ||
                    colunas[7] != "VlrTotalBaseRet" ||
                    colunas[8] != "VlrTotalRetPrinc" ||
                    colunas[9] != "VlrTotalRetAdic" ||
                    colunas[10] != "VlrTotalNRetPrinc" ||
                    colunas[11] != "VlrTotalNRetAdic" ||
                    colunas[12] != "IndCPRB" ||
                    colunas[13] != "Serie" ||
                    colunas[14] != "NumDocto" ||
                    colunas[15] != "DtEmissaoNF" ||
                    colunas[16] != "VlrBruto" ||
                    colunas[17] != "Obs" ||
                    colunas[18] != "TpServico" ||
                    colunas[19] != "VlrBaseRet" ||
                    colunas[20] != "VlrRetencao" ||
                    colunas[21] != "VlrRetSub" ||
                    colunas[22] != "VlrNRetPrinc" ||
                    colunas[23] != "VlrServicos15" ||
                    colunas[24] != "VlrServicos20" ||
                    colunas[25] != "VlrServicos25" ||
                    colunas[26] != "VlrAdicional" ||
                    colunas[27] != "VlrNRetAdic" ||
                    colunas[28] != "TpProcRetPrinc" ||
                    colunas[29] != "NrProcRetPrinc" ||
                    colunas[30] != "CodSuspPrinc" ||
                    colunas[31] != "ValorPrinc" ||
                    colunas[32] != "TpProcRetAdic" ||
                    colunas[33] != "NrProcRetAdic" ||
                    colunas[34] != "CodSuspAdic" ||
                    colunas[35] != "ValorAdic" ||
                    colunas[36] != "NrReciboRetificado")
                    throw new Exception("Layout do cabeçalho incorreto. Favor corrigir para realizar a importação.");
            }
            catch (Exception ex)
            {
                InserirNaListaDeCriticas(ex.Message);
            }
        }

        private void ExcluirR2010()
        {
            foreach (var item in _listR2010)
            {
                _proxyR2010.ExcluirPorOidContribuinteDtaApuracao(item.OID_CONTRIBUINTE, item.DTA_APURACAO);        
            }
        }

        private void GravarR2010()
        { 
            foreach (var item in _listR2010)
            {                
                decimal oidR2010 = _proxyR2010.Inserir(item);
            }
        }

        #endregion

        #region Métodos Auxiliares

        private static void ValidarLayoutDaLinha(string[] colunas, int quantidadeDeColunas)
        {
            if (colunas.Length != quantidadeDeColunas)
                throw new Exception("Layout da linha incorreto. Favor corrigir para realizar a importação.");
        }

        private string ValidarString(string nomeDoCampo, string stringParaValidar, decimal tamanhoMaximo, bool ehObrigatorio)
        {
            if (ehObrigatorio && string.IsNullOrEmpty(stringParaValidar))
                throw new Exception(string.Format("O campo {0} não foi informado.", nomeDoCampo));

            if(stringParaValidar.Length > tamanhoMaximo)
                throw new Exception(string.Format("O campo {0} excede o tamanho máximo do banco de dados.", nomeDoCampo));

            stringParaValidar = string.IsNullOrEmpty(stringParaValidar) ? null : stringParaValidar;

            return stringParaValidar;
        }

        private decimal ValidarDecimal(string nomeDoCampo, string decimalParaValidar, decimal tamanhoMaximo, bool ehObrigatorio)
        {
            decimal retorno = 0;
            
            if (ehObrigatorio && string.IsNullOrEmpty(decimalParaValidar))
                throw new Exception(string.Format("O campo {0} não foi informado.", nomeDoCampo));

            decimalParaValidar = string.IsNullOrEmpty(decimalParaValidar) ? "0" : decimalParaValidar;

            if (!decimal.TryParse(decimalParaValidar, out retorno) || decimalParaValidar.Replace(".", string.Empty).Length > tamanhoMaximo)
                throw new Exception(string.Format("O campo {0} é inválido.", nomeDoCampo));

            return retorno;
        }

        private DateTime ValidarDateTime(string nomeDoCampo, string dataParaValidar, bool ehObrigatorio)
        {
            DateTime retorno = new DateTime();

            if (ehObrigatorio && string.IsNullOrEmpty(dataParaValidar))
                throw new Exception(string.Format("O campo {0} não foi informado.", nomeDoCampo));
            
            if (!DateTime.TryParse(dataParaValidar.Trim(), _culturePtBR, DateTimeStyles.None, out retorno))
                throw new Exception(string.Format("O campo {0} é inválido.", nomeDoCampo));

            return retorno;
        }        

        private void InserirNaListaDeCriticas(string desCritica)
        {
            ImportacaoCriticaEntidade importacaoCriticaEntidade = new ImportacaoCriticaEntidade();

            importacaoCriticaEntidade.OID_ARQUIVO_UPLOAD = _oidArquivoUpload;
            importacaoCriticaEntidade.NUM_LINHA_CRITICA = _numLinha;
            importacaoCriticaEntidade.DES_CRITICA = desCritica;

            _listImportacaoCritica.Add(importacaoCriticaEntidade);
        }

        private void GravarImportacaoCritica()
        {
            foreach (var item in _listImportacaoCritica)
            {
                _proxyImportacaoCritica.Inserir(item);
            }
        }        

        private void AtualizarArquivoUpload(ArquivoUploadEntidade rowArquivoUpload)
        {
            if (rowArquivoUpload.IND_STATUS == DMN_STATUS_EFD_UPLOAD.NAO_PROCESSADO)
            {
                rowArquivoUpload.IND_STATUS = DMN_STATUS_EFD_UPLOAD.PROCESSADO;
                _proxyArquivoUpload.Atualizar(rowArquivoUpload);
            }
        }

        #endregion
    }
}