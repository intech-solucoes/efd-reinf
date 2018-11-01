import React, { Component } from 'react';

export default class Radio extends Component {

    onChange = async (e) => {
        var context = this.props.contexto;
        await context.setState({
            opcaoSelecionada: e.target.value
        });
    
        if(this.props.onChange) {
            await this.props.onChange(e);
        }
    }

    render() {
        return (
            <div className="form-check">
                <input className={"form-check-input"} name={this.props.nome} id={this.props.nome} type="radio" 
                       value={this.props.valor} checked={this.props.marcado} onChange={this.onChange} />
                <label className="form-check-label" htmlFor={this.props.nome} >
                    {this.props.label}
                </label>
            </div>
        );
    }

}

