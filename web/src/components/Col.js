import React, { Component } from "react";

export default class Col extends Component {
    render() {
        var tamanho = this.props.tamanho;
    
        return (
            <div className={tamanho ? `col-${tamanho}` : "col"}>
                {this.props.children}
            </div>
        )
    }
}