import React, { Component } from 'react';
import { Link } from "react-router-dom";
import Botao from "../../components/Botao";
import CampoTexto from "../../components/CampoTexto";

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
	}

	entrar = async () => {
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
				localStorage.setItem("token", resultado.data.AccessToken);
				this.props.history.push("/selecionarContribuinte");
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
        await this.setState({ erros: this.erros });
    }

    adicionarErro = async (mensagem) => {
        this.erros.push(mensagem);
        await this.setState({ erros: this.erros });
    }

	render() {
		return (
			<div>

                <h4>Bem-Vindo ao Intech EFD-Reinf</h4>
				<br/>

				<CampoTexto contexto={this}  ref={ (input) => this.listaCampos[0] = input }
							placeholder={"E-mail"} valor={this.state.email} nome={"email"} tipo={"email"} 
							obrigatorio={true} />

				<CampoTexto contexto={this} ref={ (input) => this.listaCampos[1] = input }
							placeholder={"Senha"} valor={this.state.senha} nome={"senha"} tipo={"password"} 
							obrigatorio={true} />

				<div className="form-group">
					<Botao titulo="Entrar" clicar={this.entrar} tipo={"primary"} block={true} usaLoading={true} />
				</div>

				{this.state.erros.length > 0 &&
                    <div className="alert alert-danger" role="alert" 
                         dangerouslySetInnerHTML={{__html: this.state.erros.join("<br/>") }}>
					</div>
                }

				<div className="form-group row">
					<a className="col-sm-6" href="/esqueciSenha">Esqueci Minha Senha</a>
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
			</div>
		);
	}
}