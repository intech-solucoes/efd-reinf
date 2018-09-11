import React, { Component } from "react";
import { handleFieldChange } from "@intechprev/react-lib";
import BotaoAjuda from "../components/BotaoAjuda";

export default class Combo extends Component {
    constructor(props) {
        super(props);

    }

    componentDidMount() {

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
                        <option value={this.props.valor}>{this.props.opcao}</option>
                    </select>
				</div>

				{this.props.botaoAjuda && 
					<BotaoAjuda textoModal={this.props.botaoAjuda} titulo={this.props.label} />
				}
			</div>
        )
    }

}
