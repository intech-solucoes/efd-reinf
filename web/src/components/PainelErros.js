import React, { Component } from "react";

export default class PainelErros extends Component {

    render() {
        if(this.props.erros.length > 0)
            return (    
                <div className="alert alert-danger" role="alert" 
                    dangerouslySetInnerHTML={{__html: this.props.erros.join("<br/>") }}>
                </div>
            );
        else
            return "";
    }

}