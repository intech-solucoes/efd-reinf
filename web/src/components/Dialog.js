import React, { Component } from "react";

export default class Dialog extends Component {

    constructor(props) {
        super(props);

        this.state = {
            visivel: false
        }
    }

    toggle = () => {
        this.setState({ visivel: !this.state.visivel });
    }

    aceitar = () => {
        this.toggle();

        this.props.confirmar();
    }

    render() {
        if (this.state.visivel) {
            return (
                <div className={"dialog"}>
                    <div className="dialog-inner">
                        <p>
                            {this.props.mensagem}
                        </p>

                        <div className="actions">
                            <button className={"btn btn-light"} onClick={() => this.toggle()}>Cancelar</button>
                            <button className={"btn btn-primary"} onClick={() => this.aceitar()}>{this.props.textoConfirmar || "Confirmar"}</button>
                        </div>
                    </div>
                </div>
            );
        } else {
            return "";
        }
    }

}