import React, { Component } from 'react';
import { Link } from "react-router-dom";
import { Botao, CampoTexto, Dialog, PainelErros } from "../../components";
import { PageClean } from "../";

import packageJson from '../../../package.json';

import { UsuarioService } from "@intechprev/efdreinf-service";

export default class Login extends Component {

	constructor(props) {
		super(props);

        this.listaCampos = [];
        this.erros = [];

		this.state = {
			email: "",
			senha: "",
			erros: []
		};

        this.dialog = React.createRef();
	}

	entrar = async (e) => {
		await this.limparErros();

		for(var i = 0; i < this.listaCampos.length; i++) {
			var campo = this.listaCampos[i];
			campo.validar();

			if(campo.possuiErros)
				await this.adicionarErro(campo.erros);
		}

		if(this.state.erros.length === 0) {
			try {
				var resultado = await UsuarioService.Login(this.state.email, this.state.senha);
				console.log(resultado.data);
				//await localStorage.setItem("token", resultado.data.AccessToken);
				//this.props.history.push("/selecionarContribuinte");
			} catch(erro) {
				if(erro.response) {
					if(erro.response.data === "IND_EMAIL_VERIFICADO")
						this.dialog.current.toggle();
					else
						await this.adicionarErro(erro.response.data);
				} else {
					await this.adicionarErro(erro);
				}
			}
		}
	}

    limparErros = async () => {
        this.erros = [];
        await this.setState({ erros: this.erros });
    }

    adicionarErro = async (mensagem) => {
        this.erros.push(mensagem);
        await this.setState({ erros: this.erros });
	}
	
	confirmarReenvio = async () => {
		try {
			await UsuarioService.ReenviarConfirmacao(this.state.email, this.state.senha);
		} catch(erro) {
			if(erro.response) {
				await this.adicionarErro(erro.response.data);
			} else {
				await this.adicionarErro(erro);
			}
		}
	}

	render() {
		return (
			<PageClean {...this.props}>

                <h4>Bem-Vindo ao Intech EFD-Reinf</h4>
				<br/>

				<form>
					<CampoTexto contexto={this} ref={ (input) => this.listaCampos[0] = input }
								placeholder={"E-mail"} valor={this.state.email} nome={"email"} tipo={"email"}
								obrigatorio={true} />

					<CampoTexto contexto={this} ref={ (input) => this.listaCampos[1] = input }
								placeholder={"Senha"} valor={this.state.senha} nome={"senha"} tipo={"password"} 
								obrigatorio={true} />

					<div className="form-group">
						<Botao titulo="Entrar" clicar={this.entrar} tipo={"primary"} block={true} usaLoading={true} submit={true} />
					</div>
				</form>

                <PainelErros erros={this.state.erros} />

				<div className="form-group row">
					<Link className="col-sm-6" to="/esqueciSenha">Esqueci Minha Senha</Link>
					<Link className="col-sm-6 text-right" to="/cadastro">Cadastre-se</Link>
				</div>
				<br/>

				<div className="form-group">
					Dúvidas? <a target="_blank" rel="noopener noreferrer" href="http://www.intech.com.br/contato/">Fale conosco</a>
				</div>
				<br/>
				<br/>

                <div className="form-group text-center">
                    Versão {packageJson.version}
                </div>

				<Dialog ref={this.dialog} confirmar={this.confirmarReenvio}
						textoConfirmar={"Enviar"} mensagem={"E-mail não confirmado. Deseja enviar novamente a confirmação?"} />
			</PageClean>
		);
	}
}