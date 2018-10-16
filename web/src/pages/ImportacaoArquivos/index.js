import React, { Component } from 'react';
import axios from "axios";
import { Row, Col, Box, Combo, Botao, InputFile, PainelErros } from '../../components';
import { DominioService, UploadService } from "@intechprev/efdreinf-service";

const apiUrl = process.env.API_URL;

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
    }

    async componentDidMount() {
        window.scrollTo(0, 0);
        var comboSituacao = await DominioService.BuscarPorCodigo("DMN_STATUS_IMPORTACAO");
        this.setState({ filtrarSituacaoCombo: comboSituacao, });
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

    buscarArquivosImportados = async () => {
        var oidUsuarioContribuinte = localStorage.getItem("oidUsuarioContribuinte");
        try { 
            var arquivosImportacao = await UploadService.BuscarPorOidUsuarioContribuinteStatus(oidUsuarioContribuinte, this.state.situacao);
            await this.setState({ arquivosImportacao: arquivosImportacao.data });
        } catch(err) {
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

        // Rota para upload.
        var oidUsuarioContribuinte = localStorage.getItem("oidUsuarioContribuinte");
        if(this.state.erros.length === 0) {
            try { 
                await axios.post(apiUrl + `/Upload/${oidUsuarioContribuinte}`, this.state.formData, {
                    headers: {'Content-Type': 'multipart/form-data'},
                    onUploadProgress: progressEvent => {
                    },
                });
                alert("Arquivo enviado com sucesso!");
                await this.setState({ visibilidadeInput: false });
                this.buscarArquivosImportados();
            } catch (err) {
                if(err.response)
                    this.adicionarErro(err.response);
                else 
                    this.adicionarErro(err);
            }
        }
    }

    filtrarSituacao = async () => { 
        var oidUsuarioContribuinte = localStorage.getItem("oidUsuarioContribuinte");
        var arquivosImportacao = await UploadService.BuscarPorOidUsuarioContribuinteStatus(oidUsuarioContribuinte, this.state.situacao);
        await this.setState({ arquivosImportacao: arquivosImportacao.data });
    }

    deletar = async (oidArquivoUpload) => {
        try {
            await UploadService.Deletar(oidArquivoUpload);
            alert("Registro excluído com sucesso!");
            this.buscarArquivosImportados();
        } catch(err) {
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
            console.error(err)
        }
    }

    render() {
        return (
            <div>
                <Box titulo="Instruções">
                    <h5>Para realizar uma importação de arquivo externo (formato CSV):</h5>
                    <br />
                    <h5>1. Selecione um arquivo para importação (botão "Selecione um arquivo..." e faça o upload no botão "Enviar");</h5>
                    <h5>2. Selecione da lista o arquivo desejado para importação, se necessário filtre a situação do arquivo;</h5>
                    <h5>3. Clique no botão "Importar";</h5>
                    <h5>4. Caso ocorra erros durante a importação, no final do processo será impresso um relatório com os erros;</h5>
                    <br />
                    <h5>Links do Leiaute:</h5>
                    <a href="layouts/LayoutImportacao-R1070 - Processos ADM-JUD.xlsx" download="LayoutImportacao-R1070 - Processos ADM-JUD.xlsx"><h5>-Leiaute Importação - Registro R-1070</h5></a>
                    <a href="layouts/LayoutImportacao-R2010 - Retenção PREVIDENCIARIA.xlsx" download="LayoutImportacao-R2010 - Retenção PREVIDENCIARIA.xlsx"><h5>-Leiaute Importação - Registro R-2010</h5></a>
                    <br />
                    <h5>Manual de preenchimento:</h5>
                    <a href="" download><h5>-Manual de preenchimento - Registro R-1070.pdf</h5></a>
                    <a href="" download><h5>-Manual de preenchimento - Registro R-2010.pdf</h5></a>
                </Box>

                <Box titulo="Arquivo para Upload">
                    <Row>
                        <Col tamanho={"4"}>
                            {this.state.visibilidadeInput && 
                                <div>
                                    <InputFile contexto={this} ref={ (input) => this.listaCampos[0] = input } label={"Arquivo para upload"}
                                            nome={"arquivo"} aceita={".csv"} obrigatorio={true} valor={this.state.arquivo} onChange={this.uploadFile} />
                                    <Botao tipo={"primary btn-sm"} titulo={"Enviar"} clicar={this.enviar} />
                                </div>
                            }
                            {!this.state.visibilidadeInput && 
                                <Botao titulo={"Enviar outro arquivo"} tipo={"primary"} clicar={async () => await this.setState({ visibilidadeInput: true })} />
                            }
                        </Col>
                    </Row>
                    <br />
                    <Row>
                        <Col tamanho="5">
                            <PainelErros erros={this.state.erros} />
                        </Col>
                    </Row>
                </Box>

                <Box titulo={"Arquivo para Importação"}>

                    <Combo contexto={this} label={"Filtrar situação do arquivo"} ref={ (input) => this.listaCampos[1] = input } 
                        nome="situacao" valor={this.state.situacao} col={"col-lg-6"} comboCol={"col-lg-4"}
                        opcoes={this.state.filtrarSituacaoCombo.data} textoVazio={"Todos"} onChange={this.filtrarSituacao} />

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
                                    <th></th>
                                </tr>
                            </thead>
                            <tbody>
                                {
                                    this.state.arquivosImportacao.map((arquivo, index) => {
                                        return (
                                            <tr key={index}>
                                                <td>
                                                    <Botao tipo={"light btn-sm"} clicar={() => console.log("")}>
                                                        <i className="fas fa-angle-right"></i>
                                                        <i className="fas fa-angle-right"></i>
                                                    </Botao>
                                                </td>
                                                <td>{arquivo.NOM_ARQUIVO_ORIGINAL}</td>
                                                <td>{arquivo.DTA_UPLOAD}</td>
                                                <td>{arquivo.IND_STATUS}</td>
                                                <td>{arquivo.NOM_USUARIO}</td>
                                                <td>
                                                    <Botao tipo={"light btn-sm"} clicar={() => this.gerarRelatorio(arquivo.OID_ARQUIVO_UPLOAD)}>
                                                        <i className="fas fa-clipboard"></i>
                                                    </Botao>
                                                </td>
                                                <td>
                                                    <Botao tipo={"light btn-sm"} clicar={() => this.deletar(arquivo.OID_ARQUIVO_UPLOAD)}>
                                                        <i className="fas fa-trash-alt"></i>
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
