import React, { Component } from "react";

import Botao from "../../components/Botao";
import CampoTexto from "../../components/CampoTexto";

import { UsuarioService } from "@intechprev/efdreinf-service";

export default class Cadastro extends Component {

    constructor(props) {
        super(props);

        this.listaCampos = [];
        this.erros = [];

        this.state = {
            nome: "",
            email: "",
            cpf: "",
            telefoneFixo: "",
            telefoneCelular: "",
            senha: "",
            senhaConfirma: "",
            erros: []
        }
    }

    criar = async () => {
        await this.limparErros();

        if(this.state.senha !== this.state.senhaConfirma)
            this.adicionarErro("Confirmação de senha inválida.");
        
        for(var i = 0; i < this.listaCampos.length; i++) {
            var campo = this.listaCampos[i];
            campo.validar();

            if(campo.possuiErros)
                await this.adicionarErro(campo.erros);
        }

        if(this.state.erros.length === 0) {
            try {
                await UsuarioService.Criar(this.state.email, this.state.senha, this.state.nome, this.state.cpf, this.state.telefoneFixo, this.state.telefoneCelular);
                alert("Um e-mail foi enviado para a confirmação do seu cadastro.");
                this.props.history.push("/");
            } catch(erro) {
                if(erro.response) {
                    await this.adicionarErro(erro.response.data);
                } else {
                    await this.adicionarErro(erro);
                }
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
        })
    }

    render() {
        return (
            <div>
                <h4>Novo Usuário</h4>
				<br/>

                <CampoTexto contexto={this} ref={ (input) => this.listaCampos[0] = input }
                            label={"Nome"} valor={this.state.nome} nome={"nome"} tipo={"text"} 
                            obrigatorio={true} />

                <CampoTexto contexto={this} ref={ (input) => this.listaCampos[1] = input }
                            label={"E-mail"} valor={this.state.email} nome={"email"} tipo={"email"}
                            obrigatorio={true} />

                <CampoTexto contexto={this} ref={ (input) => this.listaCampos[2] = input } 
                            label={"CPF"} valor={this.state.cpf} nome={"cpf"} tipo={"text"} mascara="999.999.999-99" 
                            obrigatorio={true} />

                <CampoTexto contexto={this} ref={ (input) => this.listaCampos[3] = input }
                            label={"Telefone Fixo"} valor={this.state.telefoneFixo} nome={"telefoneFixo"} tipo={"text"} 
                            obrigatorio={true} mascara="(99) 9999-9999" />

                <CampoTexto contexto={this} ref={ (input) => this.listaCampos[4] = input }
                            label={"Telefone Celular"} valor={this.state.telefoneCelular} nome={"telefoneCelular"} tipo={"text"} 
                            obrigatorio={true} mascara="(99) 99999-9999" />

                <CampoTexto contexto={this} ref={ (input) => this.listaCampos[5] = input }
                            label={"Senha"} valor={this.state.senha} nome={"senha"} tipo={"password"} 
                            obrigatorio={true} />

                <CampoTexto contexto={this} ref={ (input) => this.listaCampos[6] = input }
                            label={"Confirmar Senha"} valor={this.state.senhaConfirma} nome={"senhaConfirma"} tipo={"password"} 
                            obrigatorio={true} />
                
				{this.state.erros.length > 0 &&
                    <div className="alert alert-danger" role="alert" 
                         dangerouslySetInnerHTML={{__html: this.state.erros.join("<br/>") }}>
					</div>
                }
                
				<div className="form-group">
					<Botao titulo="Entrar" clicar={this.criar} tipo={"primary"} block={true} />
				</div>
			</div>
        );
    }

}