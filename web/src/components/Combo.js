import React, { Component } from "react";
import { BotaoAjuda, Row, Col } from "../components";

import { handleFieldChange } from "@intechprev/react-lib";

export default class Combo extends Component {

	constructor(props) {
		super(props);

		this.erros = [];
		this.possuiErros = false;
	}

	static defaultProps = {
		padrao: "",
		opcoes: [],
		textoVazio: "Selecione uma opção",
		nomeMembro: "NOM_DOMINIO",
		valorMembro: "SIG_DOMINIO"
	}

	async componentDidMount() {
		var nome = this.props.nome;

		// Atualiza o state do combo para o valor padrão selecionado via props.
		await this.props.contexto.setState({
			[nome]: this.props.padrao
		});
	}

	validar = () => {
		this.possuiErros = false;
		this.erros = [];

		if(this.props.obrigatorio)
		{
			if(this.props.valor === "")
				this.erros.push(`Campo "${this.props.label}" obrigatório.`);
		}

		this.possuiErros = this.erros.length > 0;
	}

	onChange = async (e) => {
		await handleFieldChange(this.props.contexto, e);
		
		if(this.props.onChange) {
			await this.props.onChange(e);
		}
	}

    render() {
		var labelCol = "col-lg-2";
		var comboCol = "col";

		if(this.props.labelCol)
			labelCol = this.props.labelCol;

		if(this.props.comboCol)
			comboCol = this.props.comboCol;

        return (
			<Row className="form-group row">
				{this.props.label && 
					<Col className={labelCol + " col-md-12 text-lg-right col-form-label"}>
						<b><label htmlFor={this.props.nome}>
							{this.props.label} {this.props.obrigatorio && " *"}
						</label></b>
					</Col>
				}

				<Col className={comboCol}>
					<select id={this.props.nome} name={this.props.nome} className="form-control" onChange={this.onChange} 
						    value={this.props.valor} disabled={this.props.desabilitado}>

						{this.props.textoVazio &&
							<option value="">{this.props.textoVazio}</option>
						}

						{
							this.props.opcoes.map((opcao, index) => {
								return (
									<option key={index} value={opcao[this.props.valorMembro]}>{opcao[this.props.nomeMembro]}</option>
								)
							})
						}
						
                    </select>
				</Col>

				{this.props.botaoAjuda && 
					<BotaoAjuda textoModal={this.props.botaoAjuda} titulo={this.props.label} />
				}

				{this.props.label && !this.props.botaoAjuda &&
					<div className="col-1">
					</div>
				}
			</Row>
        )
    }

}
