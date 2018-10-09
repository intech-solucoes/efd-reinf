import React, { Component } from 'react';
import { Row, Col, Box, Combo, Botao, InputFile, PainelErros } from '../../components';
import { DominioService } from "@intechprev/efdreinf-service";

export default class ImportacaoArquivos extends Component {
    constructor(props) {
        super(props);

        this.listaCampos = [];
        this.erros = [];

        this.state = {
            filtrarSituacao: "",
            filtrarSituacaoCombo: [],
            arquivo: "",
            arquivosImportacao: [
                {
                    arquivoOriginal: "Envio2010.csv",
                    dataUpload: "29/09/2018 17:31:41",
                    status: "Não Processado",
                    usuario: "CLEBER"
                },
                {
                    arquivoOriginal: "Envio1118.csv",
                    dataUpload: "02/10/2018 09:12:03",
                    status: "Processado",
                    usuario: "CLEBER"
                }
            ],
            erros: []
        }
    }

    async componentDidMount() {
        var comboSituacao = await DominioService.BuscarPorCodigo("DMN_STATUS_IMPORTACAO");
        await this.setState({ filtrarSituacaoCombo: comboSituacao });
        
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

    enviar = async () => { 
        await this.limparErros();

        var campo = this.listaCampos[0];
        await campo.validar();
        if(campo.possuiErros)
            await this.adicionarErro(campo.erros);

        // Rota para upload.
    }

    filtrarSituacao = async () => { 
        var contribuinte = localStorage.getItem("contribuinte");
        // Atualizar state arquivosImportação com os dados buscados na tabela EFD_ARQUIVO_UPLOAD filtrando pelo contribuinte logado e filtro selecionado.
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
                    <a href="layouts/LayoutR1070.xlsx" download="Layout.xlsx"><h5>-Leiaute Importação - Registro R-1070</h5></a>
                    <a href="" download><h5>-Leiaute Importação - Registro R-2010</h5></a>
                    <br />
                    <h5>Manual de preenchimento:</h5>
                    <a href="" download><h5>-Manual de preenchimento - Registro R-1070.pdf</h5></a>
                    <a href="" download><h5>-Manual de preenchimento - Registro R-2010.pdf</h5></a>
                </Box>

                <Box titulo="Arquivo para Upload">
                    <Row>
                        <Col tamanho={"4"}>
                            <InputFile contexto={this} ref={ (input) => this.listaCampos[0] = input } label={"Arquivo para upload"}
                                       nome={"arquivo"} aceita={".csv"} obrigatorio={true} valor={this.state.arquivo} />
                        </Col>
                        <Col>
                            <Botao tipo={"primary btn-sm"} titulo={"Enviar"} clicar={this.enviar}/>
                        </Col>
                    </Row>
                    <Row>
                        <Col tamanho="5">
                            <PainelErros erros={this.state.erros} />
                        </Col>
                    </Row>
                </Box>

                <Box titulo={"Arquivo para Importação"}>
                    <Combo contexto={this} label={"Filtrar situação do arquivo"} ref={ (input) => this.listaCampos[1] = input } 
                           nome="filtrarSituacao" valor={this.state.filtrarSituacao} obrigatorio={true} col={"col-lg-4"}
                           opcoes={this.state.filtrarSituacaoCombo.data} onChange={this.filtrarSituacao} />

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
                                                <Botao tipo={"light btn-sm"} clicar={() => console.log("Sem ação definida")}>
                                                    <i className="fas fa-angle-right"></i>
                                                    <i className="fas fa-angle-right"></i>
                                                </Botao>
                                            </td>
                                            <td>{arquivo.arquivoOriginal}</td>
                                            <td>{arquivo.dataUpload}</td>
                                            <td>{arquivo.status}</td>
                                            <td>{arquivo.usuario}</td>
                                            <td>
                                                <Botao tipo={"light btn-sm"} clicar={() => console.log("Sem ação definida")}>
                                                    <i className="fas fa-clipboard"></i>
                                                </Botao>
                                            </td>
                                            <td>
                                                <Botao tipo={"light btn-sm"} clicar={() => console.log("Sem ação definida")}>
                                                    <i className="fas fa-trash-alt"></i>
                                                </Botao>
                                            </td>
                                        </tr>
                                    );
                                })
                            }
                        </tbody>
                    </table>
                </Box>
            </div>
        );
    }
}
