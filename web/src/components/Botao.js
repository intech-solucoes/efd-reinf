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
        var sm = this.props.pequeno ? " btn-sm" : "";
        var type = this.props.submit ? "submit" : "button";

        return (
            <button type={type} 
                className={"btn btn-" + this.props.tipo + block + sm + " " + this.props.className} 
                onClick={this.onClick} disabled={this.props.desativado || this.state.carregando}
            >
                {!this.state.carregando && 
                    this.props.titulo}

                {this.props.children}

                {this.state.carregando &&
                    <i className="fas fa-spinner fa-pulse"></i>}
            </button>
        );
    }

}