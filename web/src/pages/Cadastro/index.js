import React, { Component } from "react";

import { Botao, CampoTexto, PainelErros } from "../../components";

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
                            placeholder={"Nome"} valor={this.state.nome} nome={"nome"} tipo={"text"} max={60}
                            obrigatorio={true} />

                <CampoTexto contexto={this} ref={ (input) => this.listaCampos[1] = input }
                            placeholder={"E-mail"} valor={this.state.email} nome={"email"} tipo={"email"} max={60}
                            obrigatorio={true} />

                <CampoTexto contexto={this} ref={ (input) => this.listaCampos[2] = input } 
                            placeholder={"CPF"} valor={this.state.cpf} nome={"cpf"} tipo={"text"} mascara="999.999.999-99" 
                            obrigatorio={true} />

                <CampoTexto contexto={this} ref={ (input) => this.listaCampos[3] = input }
                            placeholder={"Telefone Fixo"} valor={this.state.telefoneFixo} nome={"telefoneFixo"} tipo={"text"} 
                            obrigatorio={true} mascara="(99) 9999-9999" />

                <CampoTexto contexto={this} ref={ (input) => this.listaCampos[4] = input }
                            placeholder={"Telefone Celular"} valor={this.state.telefoneCelular} nome={"telefoneCelular"} tipo={"text"} 
                            obrigatorio={true} mascara="(99) 99999-9999" />

                <CampoTexto contexto={this} ref={ (input) => this.listaCampos[5] = input }
                            placeholder={"Senha"} valor={this.state.senha} nome={"senha"} tipo={"password"} 
                            obrigatorio={true} />

                <CampoTexto contexto={this} ref={ (input) => this.listaCampos[6] = input }
                            placeholder={"Confirmar Senha"} valor={this.state.senhaConfirma} nome={"senhaConfirma"} tipo={"password"} 
                            obrigatorio={true} />
                
                <div className="form-group">
                    <Botao titulo="Incluir Usuário" clicar={this.criar} tipo={"primary"} block={true} usaLoading={true} />
                </div>
                
                <PainelErros erros={this.state.erros} />
			</div>
        );
    }

}