using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Intech.EfdReinf.Entidades;
using Intech.EfdReinf.Negocio.Proxy;
using Intech.Lib.Dominios;
using Microsoft.AspNetCore.Mvc;

namespace Intech.EfdReinf.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportacaoController : BaseController
    {
        private List<ImportacaoCriticaEntidade> _listImportacaoCritica;
        private decimal _oidArquivoUpload;
        private decimal _oidContribuinte;
        private int _numLinha = 0;
        CultureInfo _culturePtBR = new CultureInfo("pt-BR");
        private bool _linhaNaoContemErro = true;

        [HttpPost("importarCsv2010/{oidArquivoUpload}/{oidContribuinte}")]
        public ActionResult ImportarCsv2010(decimal oidArquivoUpload, decimal oidContribuinte)
        {
            try
            {
                _oidArquivoUpload = oidArquivoUpload;
                _oidContribuinte = oidContribuinte;

                var proxyArquivoUpload = new ArquivoUploadProxy();
                var rowArquivoUpload = proxyArquivoUpload.BuscarPorChave(_oidArquivoUpload);

                if (rowArquivoUpload == null)
                    throw new Exception("Arquivo não encontrado.");

                if (rowArquivoUpload.IND_STATUS == DMN_STATUS_EFD_UPLOAD.PROCESSADO)
                    throw new Exception("O arquivo já foi processado.");

                var linhas = System.IO.File.ReadAllLines(rowArquivoUpload.NOM_ARQUIVO_LOCAL, Encoding.Default);
                int quantidadeLinhas = linhas.Length - 1;

                if (quantidadeLinhas == 0)
                    throw new Exception("O arquivo está em branco.");

                _listImportacaoCritica = new List<ImportacaoCriticaEntidade>();

                //Lendo e validando as linhas do arquivo.
                for (int i = 0; i <= quantidadeLinhas; i++)
                {
                    _numLinha = i;

                    if (string.IsNullOrEmpty(linhas[_numLinha].Replace(";", string.Empty)))
                        continue;

                    if(_numLinha == 0)
                    {
                        ValidarCabecalho(linhas[0]);
                        continue;
                    }

                    R2010Entidade linhaImportacao = LerLinha(linhas[_numLinha]);

                    if (_linhaNaoContemErro)
                    {
                        decimal oidR2010 = new R2010Proxy().Inserir(linhaImportacao);
                    } 
                }

                //Gravando as críticas na EFD_IMPORTACAO_CRITICA.
                GravarImportacaoCritica();

                //Atualizando o status do arquivo na EFD_ARQUIVO_UPLOAD.
                if (rowArquivoUpload.IND_STATUS == DMN_STATUS_EFD_UPLOAD.NAO_PROCESSADO)
                {
                    //rowArquivoUpload.IND_STATUS = DMN_STATUS_EFD_UPLOAD.PROCESSADO;
                    //proxyArquivoUpload.Atualizar(rowArquivoUpload);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private R2010Entidade LerLinha(string linha)
        {
            R2010Entidade linhaImportacao = new R2010Entidade();
            var colunas = linha.Split(';');

            try
            { 


                linhaImportacao.DTA_APURACAO = ValidarDateTime("Data de Apuração", colunas[1], true);                
            }
            catch (Exception ex)
            {
                InserirNaListaDeCriticas(ex.Message);
            }

            return linhaImportacao;
        }

        private string ValidarString(string stringParaValidar, decimal tamanhoMaximo, bool ehObrigatorio)
        {
            string retorno = string.Empty;




            return retorno;
        }

        private decimal ValidarDecimal()
        {
            decimal retorno = 0;

            return retorno;
        }

        private DateTime ValidarDateTime(string nomeDoCampo, string dataParaValidar, bool ehObrigatorio)
        {
            DateTime retorno = new DateTime();

            if (ehObrigatorio && string.IsNullOrEmpty(dataParaValidar))
            {
                _linhaNaoContemErro = false;
                throw new Exception(string.Format("{0} não foi informada", nomeDoCampo));
            }
            
            if (!DateTime.TryParse(dataParaValidar.Trim(), _culturePtBR, DateTimeStyles.None, out retorno))
                throw new Exception(string.Format("{0} inválida", nomeDoCampo));

            return retorno;
        }

        private void ValidarCabecalho(string linha)
        {
            var colunas = linha.Split(';');

            if (colunas.Length != 37 ||
                colunas[0] != "IndRegistro" ||
                colunas[1] != "DataApuração" ||
                colunas[2] != "TipoInscriçãoEstab" ||
                colunas[3] != "NúmeroInscriçãoEstab" ||
                colunas[4] != "indPrestaçãoServiçoObra" ||
                colunas[5] != "cnpjPrestador" ||
                colunas[6] != "vlrTotalBruto" ||
                colunas[7] != "vlrTotalBaseRet" ||
                colunas[8] != "vlrTotalRetPrinc" ||
                colunas[9] != "vlrTotalRetAdic" ||
                colunas[10] != "vlrTotalNRetPrinc" ||
                colunas[11] != "vlrTotalNRetAdic" ||
                colunas[12] != "indCPRB" ||
                colunas[13] != "serie" ||
                colunas[14] != "numDocto" ||
                colunas[15] != "dtEmissaoNF" ||
                colunas[16] != "vlrBruto" ||
                colunas[17] != "obs" ||
                colunas[18] != "tpServico" ||
                colunas[19] != "vlrBaseRet" ||
                colunas[20] != "vlrRetencao" ||
                colunas[21] != "vlrRetSub" ||
                colunas[22] != "vlrNRetPrinc" ||
                colunas[23] != "vlrServicos15" ||
                colunas[24] != "vlrServicos20" ||
                colunas[25] != "vlrServicos25" ||
                colunas[26] != "vlrAdicional" ||
                colunas[27] != "vlrNRetAdic" ||
                colunas[28] != "tpProcRetPrinc" ||
                colunas[29] != "nrProcRetPrinc" ||
                colunas[30] != "codSuspPrinc" ||
                colunas[31] != "valorPrinc" ||
                colunas[32] != "tpProcRetAdic" ||
                colunas[33] != "nrProcRetAdic" ||
                colunas[34] != "codSuspAdic" ||
                colunas[35] != "valorAdic" ||
                colunas[36] != "nrReciboRetificado")
                throw new Exception("Leiaute incorreto. Favor corrigir para realizar a importação.");                
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
            var proxyImportacaoCritica = new ImportacaoCriticaProxy();

            foreach (var item in _listImportacaoCritica)
            {
                proxyImportacaoCritica.Inserir(item);
            }
        }
    }
}