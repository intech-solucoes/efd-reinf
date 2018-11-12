import React, { Component } from "react";

import { UsuarioService } from "@intechprev/efdreinf-service";

import { Botao, CampoTexto, PainelErros } from "../../components";
import { PageClean } from "../";

export default class EsqueciSenha extends Component {

    constructor(props) {
        super(props);

        this.listaCampos = [];
        this.erros = [];

        this.state = {
            email: "",
            erros: []
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

    recuperarSenha = async () => {
        await this.limparErros();

        for(var i = 0; i < this.listaCampos.length; i++) {
            var campo = this.listaCampos[i];
            campo.validar();

            if(campo.possuiErros)
                await this.adicionarErro(campo.erros);
        }

        if(this.state.erros.length === 0) {
            try {
                await UsuarioService.RecuperarSenha(this.state.email);
                alert("Uma nova senha foi gerada e enviada para o seu e-mail.");
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

    render() {
        return (
            <PageClean {...this.props}>
                <h4>Esqueci a Senha</h4>
				<br/>

                <p>Uma nova senha será gerada e enviada para o seu e-mail.</p>

                <CampoTexto contexto={this} ref={ (input) => this.listaCampos[0] = input }
                            placeholder={"E-mail"} valor={this.state.email} nome={"email"} tipo={"email"} max={60}
                            obrigatorio={true} />

                <div className="form-group">
                    <Botao titulo="Enviar" clicar={this.recuperarSenha} tipo={"primary"} block={true} usaLoading={true} />
                </div>

                <PainelErros erros={this.state.erros} />
            </PageClean>
        )
    }

}