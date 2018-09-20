import React, { Component } from "react";

export default class Modal extends Component {

    constructor(props) {
        super(props);

        this.state = {
            visivel: false
        }
    }

    toggle = () => {
        this.setState({ visivel: !this.state.visivel });
    }

    render() {
        if (this.state.visivel) {
            return (
                <div className={"modal"}>
                    <div className="modal-inner">
                        {this.props.children}<br/>
                        <button className={"btn btn-light"} onClick={() => this.toggle()}>Fechar</button>
                    </div>
                </div>
            );
        } else {
            return "";
        }
    }

}