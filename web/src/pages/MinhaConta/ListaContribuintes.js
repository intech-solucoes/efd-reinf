import React, { Component } from 'react';
import { Modal, Botao } from "../../components";

export default class ListaContribuintes extends Component {
    constructor(props) {
        super(props);

        this.state = {
            contribuintes: [
                {
                    nome: "INTECH SOLUÇÕES EM TECNOLOGIA",
                    cnpj: "12.123.456/0001-45"
                },
                {
                    nome: "FUNDAÇÃO XPTO DE PREVIDÊNCIA",
                    cnpj: "23.985.943/0001-94"
                },
            ],
            desvincularVisivel: false,
        }

        //this.modal = React.createRef();
    }

    componentDidMount() {
        // Buscar contribuintes.
    }

    render() {
        return (
            <div className="row">
                <div className="col">
                    <h4>Contribuintes Vinculados</h4>
                    <hr />
                    
                    {
                        this.state.contribuintes.map((contribuinte, index) => {
                            return (
                                <div key={index} className="container">
                                    <div className="row">
                                        <div className="col-3">
                                            {contribuinte.cnpj}
                                        </div>
                                        <div className="col-sm">
                                            <div align="left">
                                                {contribuinte.nome}
                                            </div>
                                        </div>
                                        <div className="col-sm">
                                            <Botao titulo={"Desvincular"} tipo={"link"} clicar={() => this.modal.current.toggle()}
                                                block={true} usaLoading={false} />
                                        </div>
                                    </div>
                                </div>
                            )
                        })
                    }
                    
                    <Modal ref={this.modal} visivel={this.state.desvincularVisivel}>
                        <div className="row">
                            <div className="col">
                                <p align="justify">Ao desvincular um contribuinte você não poderá mais acessá-lo no sistema EFD-Reinf. Continuar?</p>
                            </div>
                        </div>
                        <div className="row">
                            <div className="col-1">
                                <Botao titulo={"Sim"} tipo={"primary"} clicar={() => {}}
                                    block={true} usaLoading={false} />
                            </div>
                            <div className="col-1">
                                <Botao titulo={"Não"} tipo={"danger"} clicar={() => {}}
                                    block={true} usaLoading={false} />
                            </div>
                        </div>
                    </Modal>
                </div>
            </div>
        )
    }
}
