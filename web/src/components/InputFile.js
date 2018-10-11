import React, { Component } from 'react';
import { handleFieldChange } from '@intechprev/react-lib';

export default class InputFile extends Component { 
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
        return (
            <div className="form-group">
                <input name={this.props.nome} id={this.props.nome} type="file" accept={this.props.aceita} 
                       onChange={this.onChange} value={this.props.valor} />
            </div>
        )
    }
}
