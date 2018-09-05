import React, { Component } from "react";

export default class Botao extends Component {

    constructor(props) {
        super(props);

        this.state = {
            carregando: false
        }
    }

    onClick = async (e) => {
        e.preventDefault();

        if(this.props.usaLoading)
            await this.setState({ carregando: true });

        await this.props.clicar();

        if(this.props.usaLoading)
            await this.setState({ carregando: false });
    }

    render() {
        var block = this.props.block ? " btn-block" : "";

        return (
            <button className={"btn btn-" + this.props.tipo + block} onClick={this.onClick} disabled={this.state.carregando}>
                {!this.state.carregando && 
                    this.props.titulo}

                {this.state.carregando &&
                    <i className="fas fa-spinner fa-pulse"></i>}
            </button>
        );
    }

}