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

		var valorSemMascara = this.props.valor.split("_").join("");
		if(this.props.min && valorSemMascara.length < this.props.min)
			this.erros.push(`Campo "${this.props.label || this.props.placeholder}" inválido.`);

		this.possuiErros = this.erros.length > 0;
	}

	render() {
		var col = "col-lg-2";

		if(this.props.col)
			col = this.props.col;

		return (
			<div className="form-group row">
				
				{this.props.label &&
					<div className={col + " col-md-12 text-lg-right col-form-label"}>
						<b><label htmlFor={this.props.nome}>
							{this.props.label}
							{this.props.obrigatorio && " *"}
						</label></b>
					</div>
				}

				<div className="col">
					<InputMask mask={this.props.mascara} name={this.props.nome} value={this.props.valor} maxLength={this.props.max} className="form-control"
							   type={this.props.tipo} placeholder={this.props.placeholder} id={this.props.nome} disabled={this.props.desabilitado}
							   onChange={(e) => handleFieldChange(this.props.contexto, e, this.props.parent)} />
				</div>

				{this.props.botaoAjuda && 
					<BotaoAjuda textoModal={this.props.botaoAjuda} titulo={this.props.label} />
				}

				{this.props.label && this.props.placeholder && !this.props.botaoAjuda &&
					<div className="col-1">
					</div>
				}

			</div>
		);
	}

}