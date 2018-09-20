import React, { Component } from 'react';
import { CampoTexto, Botao, Modal } from '../../components';

import { validarEmail } from "@intechprev/react-lib";
import ListaContribuintes from './ListaContribuintes';

export default class MinhaConta extends Component {

    constructor(props) {
        super(props);

        this.listaCampos = [];
        this.erros = [];

        this.state = {
            email: "",
            nome: "",
            cpf: "",
            telefoneFixo: "",
            telefoneCelular: "",

            contribuintes: [
                {
                    nome: "INTECH SOLUÇÕES EM TECNOLOGIA",
                    cnpj: "12.123.456/0001-45"
                },
                {
                    nome: "FUNDAÇÃO XPTO DE PREVIDÊNCIA",
                    cnpj: "23.985.943/0001-94"
                },
            ],
            desvincularVisivel: false,
            erros: []
        }

        this.modal = React.createRef();
    }

    componentDidMount() {
        // Busca dados do usuário
        // Busca lista de contribuintes
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

    alterarDados = async () => { 
        console.log(this.state);
        
        this.validarFormulario();
    }

    validarFormulario = async () => {
        // Validações de campos obrigatórios.
        await this.limparErros();

        for(var i = 0; i < this.listaCampos.length; i++) {
            var campo = this.listaCampos[i];
            campo.validar();

            if(campo.possuiErros) {
                await this.adicionarErro(campo.erros);
            }
        }

        // Validação do campo de e-mail, caso esteja preenchido.
        if(this.state.email.length > 0) {
            if(validarEmail(this.state.email))
                await this.adicionarErro("Campo e-mail inválido!");
        }
    }

    render() { 
        return ( 

            <div className="row">
                <div className="col">
                    <CampoTexto contexto={this} ref={ (input) => this.listaCampos[0] = input }
                                label={"E-mail"} nome={"email"} tipo={"text"} valor={this.state.email}
                                placeholder={"E-mail"} />

                    <CampoTexto contexto={this} ref={ (input) => this.listaCampos[1] = input }
                                label={"Nome"} nome={"nome"} tipo={"text"} placeholder={"Nome"}
                                valor={this.state.nome} obrigatorio={true} />
                                
                    <CampoTexto contexto={this} ref={ (input) => this.listaCampos[2] = input }
                                label={"CPF"} nome={"cpf"} tipo={"text"} placeholder={"CPF"}
                                valor={this.state.cpf} obrigatorio={true} mascara={"999.999.999-99"} />
                    
                    <CampoTexto contexto={this} ref={ (input) => this.listaCampos[3] = input }
                                label={"Telefone Fixo"} nome={"telefoneFixo"} tipo={"text"} placeholder={"Telefone Fixo"}
                                valor={this.state.telefoneFixo} obrigatorio={true} mascara={"(99) 9999-9999"} />
                    
                    <CampoTexto contexto={this} ref={ (input) => this.listaCampos[4] = input }
                                label={"Telefone Celular"} nome={"telefoneCelular"} tipo={"text"} placeholder={"Telefone Celular"}
                                valor={this.state.telefoneCelular} obrigatorio={true} mascara={"(99) 99999-9999"} />
                    
                    <br />

                    <div className="row">
                        <div className="col">
                            <div align="center">
                                <div className="col-5">

                                    {this.state.erros.length > 0 &&
                                        <div className="alert alert-danger" role="alert" 
                                            dangerouslySetInnerHTML={{__html: this.state.erros.join("<br/>") }}>
                                        </div>
                                    }

                                    <br />
                                    <Botao titulo={"Alterar Dados"} tipo={"primary"} clicar={this.alterarDados}
                                           block={true} usaLoading={true} />
                                    <br />

                                    <a href="/alterarSenha">Alterar Senha</a>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    
                    <ListaContribuintes />
                </div>
            </div>
        );
    }

}
