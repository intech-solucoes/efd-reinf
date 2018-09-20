import React from 'react';
import Botao from './Botao';

/**
 * @returns Componente com o botão de ajuda.
 * @description Classe responsável pela renderização e comportamentos do botão de ajuda, que abre uma modal com informações.
 */
export default class BotaoAjuda extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            modalVisivel: false
        }
    }

    /**
     * @description Método que renderiza a Modal com o texto enviado pela props 'textoModal'. A Modal é renderizada apenas se o state 'modalVisivel' for true,
     * alterado pelo método toggleModal() chamado no onClick do Botao de Ajuda.
     */
    renderModal() {
        if (this.state.modalVisivel) {
            return (
                <div className="modal" role="dialog">
                    <div className="modal-dialog" role="document">
                        <div className="modal-content">
                            <div className="modal-header">
                                <h5 className="modal-title">{this.props.titulo}</h5>
                                <button type="button" className="close" data-dismiss="modal" aria-label="Close" onClick={() => this.toggleModal()}>
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div className="modal-body">
                                {this.props.textoModal}
                            </div>
                            <div className="modal-footer">
                                <button type="button" className="btn btn-primary" onClick={() => this.toggleModal()}>Ok, entendi!</button>
                            </div>
                        </div>
                    </div>
                </div>
            );
        }
        else
        {
            return <div></div>
        }
    }

    toggleModal = () => {
        this.setState({ modalVisivel: !this.state.modalVisivel });
    }

    render() {
        return (
            <div className="col-1">
                <Botao titulo="" clicar={() => this.toggleModal()} tipo={"outline-dark rounded-circle bg-dark text-white"} block={false} usaLoading={false} >
                    <i className="fa fa-question"></i>
                </Botao>

                {this.renderModal()}
            </div>
        );
    }

}
