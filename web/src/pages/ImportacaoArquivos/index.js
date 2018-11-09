import React, { Component } from 'react';
import axios from "axios";
import { Link } from "react-router-dom";
import { Row, Col, Box, Combo, Botao, InputFile, PainelErros } from '../../components';
import { DominioService, UploadService, ImportacaoCsvService } from "@intechprev/efdreinf-service";

export default class ImportacaoArquivos extends Component {
    constructor(props) {
        super(props);

        this.listaCampos = [];
        this.erros = [];

        this.state = {
            situacao: "",
            filtrarSituacaoCombo: [],
            arquivo: "",
            formData: "",
            arquivosImportacao: [],
            visibilidadeInput: true,
            erros: []
        }

        this.oidUsuarioContribuinte = localStorage.getItem("oidUsuarioContribuinte");
    }

    async componentDidMount() {
        window.scrollTo(0, 0);
        var comboSituacao = await DominioService.BuscarPorCodigo("DMN_STATUS_IMPORTACAO");
        this.setState({ filtrarSituacaoCombo: comboSituacao.data });
        this.buscarArquivosImportados();
    }

    limparErros = async () => {
        this.erros = [];
        await this.setState({
            erros: this.erros
        });
    }

    adicionarErro = async (mensagem) => {
        this.erros.push(mensagem);
        await this.setState({
            erros: this.erros
        });
    }

    importarCsv = async (oidArquivoUpload) => {
        try {
            var oidContribuinte = localStorage.getItem("contribuinte");
            await ImportacaoCsvService.ImportarCsv(oidArquivoUpload, oidContribuinte);
            alert("Arquivo processado com sucesso.");
            this.buscarArquivosImportados();
        } catch(err) {
            if(err.response) 
                alert(err.response.data);
            else
                console.error(err);
        }
    }

    uploadFile = async (e) => {
        const formData = new FormData()
        var arquivoUpload = e.target.files[0];
        formData.append("File", arquivoUpload, arquivoUpload.name);
        await this.setState({ 
            formData: formData,
            arquivo: ""
        });
    }

    enviar = async () => { 
        await this.limparErros();

        if(!this.state.formData)
            await this.adicionarErro("Campo \"Arquivo para Upload\" obrigatório.");

        var apiUrl = require("../../config").apiUrl;
        // Rota para upload.
        var oidUsuarioContribuinte = localStorage.getItem("oidUsuarioContribuinte");
        if(this.state.erros.length === 0) {
            try { 
                var token = localStorage.getItem("token");

                await axios.post(apiUrl + `/upload/${oidUsuarioContribuinte}`, this.state.formData, {
                    headers: {
                        'Content-Type': 'multipart/form-data',
                        "Authorization": "Bearer " + token
                    },
                    onUploadProgress: progressEvent => {
                    },
                });

                alert("Arquivo enviado com sucesso!");

                // Quando o arquivo é enviado com sucesso, esconde-se o campo de enviar arquivos e limpa o state formData;
                await this.setState({ 
                    visibilidadeInput: false,
                    formData: null
                });
                this.buscarArquivosImportados();
            } catch (err) {
                if(err.response)
                    this.adicionarErro(err.response);
                else 
                    this.adicionarErro(err);
            }
        }
    }

    buscarArquivosImportados = async () => { 
        try {
            var situacao = this.state.situacao
            if(situacao === "")
                situacao = null;
            
            var arquivosImportacao = await UploadService.BuscarCsvPorOidUsuarioContribuinteStatus(this.oidUsuarioContribuinte, situacao);
            await this.setState({ arquivosImportacao: arquivosImportacao.data });
        } catch(err) {
            console.error(err);
        }
    }

    deletar = async (oidArquivoUpload) => {
        try {
            await UploadService.Deletar(oidArquivoUpload);
            alert("Registro excluído com sucesso!");
            this.buscarArquivosImportados();
        } catch(err) {
            if(err.response.data)
                alert(err.response.data);
            else 
                console.error(err);
        }
    }

    gerarRelatorio = async (oidArquivoUpload) => {
        try {
            var relatorio = await UploadService.Relatorio(oidArquivoUpload);
            const url = window.URL.createObjectURL(new Blob([relatorio.data]));
            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', 'Relatório.pdf');
            document.body.appendChild(link);
            link.click();
        } catch(err) {
            alert("Nenhuma ocorrência encontrada.");
        }
    }

    download = async (oidArquivoUpload) => { 
        var apiUrl = require("../../config").apiUrl;
        try {
            var caminhoArquivo = await UploadService.BuscarPorOidArquivoUpload(oidArquivoUpload);
            caminhoArquivo =  caminhoArquivo.data.NOM_ARQUIVO_LOCAL;
            apiUrl = apiUrl.substring(0, apiUrl.length - 4);
            apiUrl = apiUrl + "/" + caminhoArquivo;

            const link = document.createElement('a');
            link.href = apiUrl;
            document.body.appendChild(link);
            link.click();
        } catch(err) {
            if(err.response) 
                alert(err.response.data);
            else 
                console.error(err);
        }
    }

    render() {
        return (
            <div>
                <Box titulo="Instruções">
                    <h6>Para realizar uma importação de arquivo externo (formato CSV):</h6>
                    <br />
                    <h6>1. Selecione um arquivo para importação (botão "Selecione um arquivo..." e faça o upload no botão "Enviar");</h6>
                    <h6>2. Selecione da lista o arquivo desejado para importação, se necessário filtre a situação do arquivo;</h6>
                    <h6>3. Clique no botão "Processar";</h6>
                    <h6>4. Caso ocorra erros durante a importação, no final do processo será impresso um relatório com os erros;</h6>
                    <h6>5. Após o processamento  o(s) arquivo(s) XML já podem ser gerados <Link to={"/geracaoXml"}>aqui</Link>.</h6>
                    <br />

                    Leiautes de Importação de Arquivos CSV:
                    <br />

                    <a href="layouts/LayoutImportacao-R1070 - Processos ADM-JUD.xlsx" 
                       download="LayoutImportacao-R1070 - Processos ADM-JUD.xlsx">
                       -Leiaute Importação - Registro R-1070
                    </a>
                    <br />

                    <a href="layouts/LayoutImportacao-R2010 - Retenção PREVIDENCIARIA.xlsx" 
                       download="LayoutImportacao-R2010 - Retenção PREVIDENCIARIA.xlsx">
                       -Leiaute Importação - Registro R-2010
                    </a>
                    <br /><br />

                    Manual de preenchimento:
                    <br />
                    <a href="">-Manual de preenchimento - Registro R-1070.pdf</a><br />
                    <a href="">-Manual de preenchimento - Registro R-2010.pdf</a>
                </Box>

                <Box titulo="Arquivo para Upload">
                    <Row>
                        <Col tamanho={"4"}>
                            {this.state.visibilidadeInput && 
                                <div>
                                    <InputFile contexto={this} ref={ (input) => this.listaCampos[0] = input } label={"Arquivo para upload"}
                                               nome={"arquivo"} aceita={".csv"} obrigatorio={true} onChange={this.uploadFile} />
                                    <Botao tipo={"primary btn-sm"} titulo={"Enviar"} clicar={this.enviar} />
                                </div>
                            }
                            {!this.state.visibilidadeInput && 
                                <Botao titulo={"Enviar outro arquivo"} tipo={"primary"} clicar={async () => await this.setState({ visibilidadeInput: true })} />
                            }
                        </Col>
                    </Row>
                    <Row>
                        <Col tamanho="5">
                            {this.state.erros.length > 0 && <br />}
                            <PainelErros erros={this.state.erros} />
                        </Col>
                    </Row>
                </Box>

                <Box titulo={"Arquivo para Importação"}>

                    <Combo contexto={this} label={"Filtrar situação do arquivo"} ref={ (input) => this.listaCampos[1] = input } 
                           nome="situacao" valor={this.state.situacao} col={"col-lg-6"} comboCol={"col-lg-4"} padrao={"NAO"}
                           opcoes={this.state.filtrarSituacaoCombo} textoVazio={"Todos"} onChange={this.buscarArquivosImportados} />

                    {this.state.arquivosImportacao.length > 0 &&
                        <table className="table table-striped">
                            <thead>
                                <tr>
                                    <th></th>
                                    <th>Arquivo Original</th>
                                    <th>Data de Upload</th>
                                    <th>Status</th>
                                    <th>Usuário</th>
                                    <th></th>
                                </tr>
                            </thead>
                            <tbody>
                                {
                                    this.state.arquivosImportacao.map((arquivo, index) => {
                                        var status = arquivo.IND_STATUS === 'PRO' ? "Processado" : "Não Processado";
                                        var corStatus = arquivo.IND_STATUS === 'PRO' ? "#14B449" : "blue";
                                        var processarDesativado = arquivo.IND_STATUS === 'PRO' ? true : false;
                                        var ocorrenciaDesativado = arquivo.IND_STATUS === 'PRO' ? false : true;

                                        return (
                                            <tr key={index}>
                                                <td width="90">
                                                    <Botao tipo={"light btn-sm"} clicar={() => this.importarCsv(arquivo.OID_ARQUIVO_UPLOAD)} 
                                                           desativado={processarDesativado} usaLoading={true} >
                                                        Processar
                                                    </Botao>
                                                </td>

                                                <td>{arquivo.NOM_ARQUIVO_ORIGINAL}</td>

                                                <td width="140">{arquivo.DTA_UPLOAD}</td>

                                                <td width="120"><font color={corStatus}>{status}</font></td>

                                                <td>{arquivo.NOM_USUARIO}</td>

                                                <td width="240">
                                                    <Botao tipo={"light btn-sm"} clicar={() => this.gerarRelatorio(arquivo.OID_ARQUIVO_UPLOAD)}
                                                           desativado={ocorrenciaDesativado} usaLoading={true} >
                                                        Ocorrências
                                                    </Botao>&nbsp;

                                                    <Botao tipo={"light btn-sm"} clicar={() => this.deletar(arquivo.OID_ARQUIVO_UPLOAD)} usaLoading={true}>
                                                        Excluir
                                                    </Botao>&nbsp;

                                                    <Botao tipo={"light btn-sm"} clicar={() => this.download(arquivo.OID_ARQUIVO_UPLOAD)} usaLoading={true}>
                                                        Download
                                                    </Botao>
                                                </td>
                                            </tr>
                                        );
                                    })
                                }
                            </tbody>
                        </table>
                    }
                    {this.state.arquivosImportacao.length === 0 && 
                        <h4>Não há arquivos importados</h4>
                    }
                </Box>
            </div>
        );
    }
}
