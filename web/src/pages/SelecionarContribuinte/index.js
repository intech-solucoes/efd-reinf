import React, { Component } from "react";
import NovoContribuinte from "./Novo";
import Modal from "../../components/Modal";

import "./index.css";

import { ContribuinteService } from "@intechprev/efdreinf-service";

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

    selecionar = async (oid) => {
        await localStorage.setItem("contribuinte", oid);
        document.location = "/";
    }

    render() {
        return (
            <div>
                <h4>Selecione um contribuinte</h4>
				<br/>
				<br/>

                {this.state.contribuintes.map((contribuinte, index) => {
                    return (
                        <div key={index} className="row box" onClick={() => this.selecionar(contribuinte.OID_CONTRIBUINTE)}>
                            <div className="col">

                                {contribuinte.NOM_RAZAO_SOCIAL}

                            </div>

                            <div className="col-1">
                                <i className="fas fa-angle-right"></i>
                            </div>
                        </div>
                    );
                })}

                {this.state.contribuintes.length === 0 && 
                    <div className="alert alert-info">
                        Nenhum contribuinte cadastrado
                    </div>}
                <br/>
				<br/>

                {this.state.erros.length > 0 &&
                    <div className="alert alert-danger" role="alert" 
                        dangerouslySetInnerHTML={{__html: this.state.erros.join("<br/>") }}>
                    </div>}
                
                <div className="row">
                    <div className="col">
                        <button className="btn btn-primary btn-block" onClick={() => this.modal.current.toggle()}>Novo Contribuinte</button>
                    </div>
                </div>
                
                <Modal ref={this.modal}>
                    <NovoContribuinte />
                </Modal>
            </div>
        );
    }

}