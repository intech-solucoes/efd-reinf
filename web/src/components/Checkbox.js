import React, { Component } from 'react';

export default class Checkbox extends Component {

    onChange = async (e) => {
        var context = this.props.contexto;
        var valor = !this.props.valor;
        var nome = e.target.name;

        await context.setState({ [nome]: valor });

        if(this.props.onChange)
            await this.props.onChange(e);
    }

    render() {
        return (
            <div className="form-check" hidden={this.props.invisivel}>
                <input className="form-check-input" type="checkbox" name={this.props.nome} id={this.props.nome}
                       checked={this.props.valor} onChange={this.onChange} disabled={this.props.desabilitado} />
                <label className="form-check-label" htmlFor={this.props.nome}>{this.props.label}</label>
            </div>
        );
    }

}

