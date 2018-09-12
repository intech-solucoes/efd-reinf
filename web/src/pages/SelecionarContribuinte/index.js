import React, { Component } from "react";
import NovoContribuinte from "./Novo";
import Modal from "../../components/Modal";

export default class SelecionarContribuinte extends Component {

    constructor(props) {
        super(props);

        this.state = {
            novoContribuinteVisivel: false,
            contribuintes: []
        };

        this.modal = React.createRef();
    }

    render() {
        return (
            <div>
                <h4>Selecione um contribuinte</h4>
				<br/>
				<br/>

                {this.state.contribuintes.map((contribuinte, index) => {
                    return (
                        <div key={index} className="row">
                            <div className="col">

                            </div>
                        </div>
                    );
                })}

                {this.state.contribuintes.length === 0 && 
                    <div className="alert alert-info">
                        Nenhum contribuinte cadastrado
                    </div>}
                <br/>
				<br/>
                
                <div className="row">
                    <div className="col">
                        <button className="btn btn-primary btn-block" onClick={() => this.modal.current.toggle()}>Novo Contribuinte</button>
                    </div>
                </div>
                
                <Modal ref={this.modal} visivel={this.state.novoContribuinteVisivel}>
                    <NovoContribuinte />
                </Modal>
            </div>
        );
    }

}