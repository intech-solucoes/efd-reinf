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
            <div className="radio">
                <label>
                    <input type="radio" value={this.props.valor} checked={this.props.marcado} onChange={this.onChange} />
                    {this.props.label}
                </label>
            </div>
        );
    }

}

