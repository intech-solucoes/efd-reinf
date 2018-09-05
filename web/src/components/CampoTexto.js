import React, { Component } from "react";
import { handleFieldChange } from "@intechprev/react-lib";
import { validarEmail } from "@intechprev/react-lib";

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

		if(this.props.obrigatorio && this.props.valor === "")
			this.erros.push(`Campo "${this.props.label}" obrigatório.`);

		else if(this.props.tipo === "email" && validarEmail(this.props.valor))
			this.erros.push("E-mail inválido.");

		this.possuiErros = this.erros.length > 0;
	}

	render() {
		return (
			<div className="form-group">
				<InputMask mask={this.props.mascara} name={this.props.nome} value={this.props.valor} className="form-control"
						   type={this.props.tipo} placeholder={this.props.label}
						   onChange={(e) => handleFieldChange(this.props.contexto, e)} />
			</div>
		);
	}

}