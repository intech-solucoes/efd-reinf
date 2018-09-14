import React, { Component } from "react";
import { handleFieldChange } from "@intechprev/react-lib";
import { validarEmail } from "@intechprev/react-lib";
import { BotaoAjuda } from "../components";

var InputMask = require('react-input-mask');

export default class CampoTexto extends Component {

	constructor(props) {
		super(props);

		this.erros = [];
		this.possuiErros = false;
	}

	validar = () => {
		this.possuiErros = false;
		this.erros = [];

		if(this.props.obrigatorio)
		{
			if(this.props.valor === "")
				this.erros.push(`Campo "${this.props.label || this.props.placeholder}" obrigatório.`);
		}

		else if(this.props.tipo === "email" && validarEmail(this.props.valor))
			this.erros.push("E-mail inválido.");

		this.possuiErros = this.erros.length > 0;
	}

	render() {
		return (
			<div className="form-group row">
				
				{this.props.label &&
					<div className="col-lg-5 col-md-12 text-lg-right col-form-label">
						<b><label htmlFor={this.props.nome}>{this.props.label}</label></b>
					</div>
				}

				<div className="col">
					<InputMask mask={this.props.mascara} name={this.props.nome} value={this.props.valor} maxlength={this.props.max} className="form-control"
							   type={this.props.tipo} placeholder={this.props.placeholder} id={this.props.nome}
							   onChange={(e) => handleFieldChange(this.props.contexto, e)} />
				</div>

				{this.props.botaoAjuda && 
					<BotaoAjuda textoModal={this.props.botaoAjuda} titulo={this.props.label} />
				}
			</div>
		);
	}

}