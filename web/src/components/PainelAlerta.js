import React, { Component } from "react";

export default class PainelAlerta extends Component {

    render() {
        return (    
            <div className={"alert alert-" + this.props.tipo} role="alert">
                {this.props.children}
            </div>
        );
    }

}