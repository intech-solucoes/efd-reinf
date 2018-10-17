import React, { Component } from "react";
import NovoContribuinte from "./Novo";
import { Modal, Col, Row, PainelErros } from "../../components";

import "./index.css";

import { ContribuinteService, UsuarioService } from "@intechprev/efdreinf-service";

export default class SelecionarContribuinte extends Component {

    constructor(props) {
        super(props);

        this.erros = [];

        this.state = {
            contribuintes: [],
            erros: []
        };

        this.modal = React.createRef();
    }

    async componentDidMount() {
        try {
            var result = await ContribuinteService.Listar();
            this.setState({
                contribuintes: result.data
            });
        } catch(err) {
            if(err.response) {
                await this.adicionarErro(err.response.data);
            } else {
                await this.adicionarErro(err);
            }
        }
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

    selecionar = async (contribuinte) => {
        await localStorage.setItem("contribuinte", contribuinte.OID_CONTRIBUINTE);
        await localStorage.setItem("nomeContribuinte", contribuinte.NOM_RAZAO_SOCIAL);
        await localStorage.setItem("oidUsuarioContribuinte", contribuinte.OID_USUARIO_CONTRIBUINTE);

        var result = await UsuarioService.Buscar();
        await localStorage.setItem("nomeUsuario", result.data.NOM_USUARIO);

        this.props.history.push("/");
        document.location.reload();
    }

    render() {
        return (
            <div>
                <h4>Selecione um contribuinte</h4>
				<br/>
				<br/>

                {this.state.contribuintes.map((contribuinte, index) => {
                    return (
                        <Row key={index}>
                            <Col>
                                <div className="contrib-card" onClick={() => this.selecionar(contribuinte)}>
                                    <Row>
                                        <Col>
                                            {contribuinte.NOM_RAZAO_SOCIAL}
                                        </Col>

                                        <Col tamanho={"2"} className={"text-right"}>
                                            <i className="fas fa-angle-right"></i>
                                        </Col>
                                    </Row>
                                </div>
                            </Col>
                        </Row>
                    );
                })}

                {this.state.contribuintes.length === 0 && 
                    <div className="alert alert-info">
                        Nenhum contribuinte cadastrado
                    </div>}
                <br/>
				<br/>

                <PainelErros erros={this.state.erros} />
                
                <Row>
                    <Col>
                        <button className="btn btn-primary btn-block" onClick={() => this.modal.current.toggle()}>Novo Contribuinte</button>
                    </Col>
                </Row>
                
                <Modal ref={this.modal}>
                    <NovoContribuinte />
                </Modal>
            </div>
        );
    }

}