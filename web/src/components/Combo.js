import React, { Component } from "react";
import BotaoAjuda from "../components/BotaoAjuda";

export default class Combo extends Component {

	constructor(props) {
		super(props);

		this.erros = [];
		this.possuiErros = false;
	}

	componentDidMount() {

	}

	validar = () => {
		this.possuiErros = false;
		this.erros = [];

		if(this.props.obrigatorio && this.props.valor === "")
			this.erros.push(`Campo "${this.props.label}" obrigatório.`);

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
					<select id={this.props.nome} name={this.props.nome} className="form-control">
						{
							this.props.opcoes.map((opcao, index) => {
								return (
									<option key={index} value={opcao.valor}>{opcao.nome}</option>
								)
							})
						}
                    </select>
				</div>

				{this.props.botaoAjuda && 
					<BotaoAjuda textoModal={this.props.botaoAjuda} titulo={this.props.label} />
				}

				{this.props.label && !this.props.botaoAjuda &&
					<div className="col-1">
					</div>
				}
			</div>
        )
    }

}
