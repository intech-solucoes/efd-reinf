import React, { Component } from "react";

export default class Page extends Component {

    render() {
        return (
            <div className="panel-login middle-box">
                <div className="logo">
                    <img src="./imagens/Intech.png" alt="Intech" />
                </div>

                {this.props.children}

                <br/>
                <br/>
            </div>
        );
    }

}