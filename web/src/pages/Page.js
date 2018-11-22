import React, { Component } from "react";
import { Link } from "react-router-dom";

import { VersaoService } from "@intechprev/efdreinf-service";

import { Row, Col } from "../components";
import Rotas from '../Rotas';

export default class Page extends Component {
    
    constructor(props) {
        super(props);

        this.state = {
            nomeUsuario: "",
            nomeContribuinte: "",
            loading: false
        }

        this.page = React.createRef();
    }

    async componentWillMount() {
        try {
            await this.loading(true);

            var nomeUsuario = await localStorage.getItem("nomeUsuario");
            var nomeContribuinte = await localStorage.getItem("nomeContribuinte");

            await this.setState({
                nomeUsuario,
                nomeContribuinte
            })

            // Valida se o token é válido
            await VersaoService.ValidarToken();

            // Verifica se contribuinte foi selecionado
            var contribuinte = await localStorage.getItem("oidContribuinte");
            if(!contribuinte)
                this.props.history.push("/selecionarContribuinte");
        } catch(err) {
            if(err.message.indexOf("401") > -1)
            {
                localStorage.removeItem("token");
                localStorage.removeItem("oidContribuinte");
                localStorage.removeItem("nomeContribuinte");
                localStorage.removeItem("nomeUsuario");
                localStorage.removeItem("oidUsuarioContribuinte");
                this.props.history.push("/login");
            }
        }
        
    }

    loading = async (valor) => {
        await this.setState({
            loading: valor
        });
    }

    selecionarContribuinte = (e) => {
        e.preventDefault();

        localStorage.removeItem("nomeContribuinte");
        localStorage.removeItem("oidContribuinte");
        localStorage.removeItem("oidUsuarioContribuinte");
        this.props.history.push("/selecionarContribuinte");
    }

    logout = (e) => {
        e.preventDefault();

        localStorage.removeItem("token");
        localStorage.removeItem("oidContribuinte");
        localStorage.removeItem("oidUsuarioContribuinte");
        localStorage.removeItem("nomeContribuinte");
        localStorage.removeItem("nomeUsuario");
        this.props.history.push("/login");
    }

    render() {
        var Title = () => {
            var rota = this.props.history.location.pathname;
            
            var titulo = "";

            for(var i = 0; i < Rotas.length; i++) {
                if(rota === process.env.PUBLIC_URL + Rotas[i].caminho) {
                    titulo = <h2>{Rotas[i].titulo}</h2>;
                }
            }

            return titulo;
        };
        
        return (
            <div>
                <div className="loader" hidden={!this.state.loading}>
                    <img src="./imagens/loading.gif" alt="loading" />
                </div>

                <div className="wrapper">
                    <nav className="navbar-default nav-open">
                        <ul>
                            <li className="navbar-header">
                                <img src="./imagens/IntechSemSlogan.png" alt="IntechSemSlogan" />
                            </li>
                            {
                                Rotas.map((rota, index) => {
                                    var link = rota.caminhoLink ? rota.caminhoLink : rota.caminho;

                                    if(rota.mostrarMenu) {
                                        return (
                                            <li key={index}>
                                                <Link to={link}>
                                                    <i className={rota.icone}></i>
                                                    {rota.titulo}
                                                </Link>
                                            </li>
                                        );
                                    }
                                    else return "";
                                })
                            }
                            <li>
                                <a href="." onClick={this.selecionarContribuinte}>
                                    <i className="fas fa-exchange-alt"></i>
                                    Selecionar Contribuinte
                                </a>
                            </li>

                            <li>
                                <a href="." onClick={this.logout}>
                                    <i className="fas fa-sign-out-alt"></i>
                                    Sair
                                </a>
                            </li>
                        </ul>
                    </nav>

                    <div className="page-wrapper nav-open">
                        <Row className="page-heading">
                            <Col>
                                <button className="btn btn-primary btn-menu" onClick={this.toggleMenu}>
                                    <i className="fa fa-list"></i>
                                </button>

                                <Title />
                            </Col>
                            <Col tamanho={"sm-4"} className={"text-right user-icon"}>

                                <Row>
                                    <Col>
                                        {this.state.nomeUsuario}<br/>
                                        <Link to={"/editarContribuinte"}>
                                            <small className={"text-primary"}>{this.state.nomeContribuinte}</small>
                                        </Link>
                                    </Col>
                                    <Col tamanho={"2"}>
                                        <Link to={"/minhaConta"}>
                                            <img className="icon" src="./imagens/UserImage.jpg" alt="user" />
                                        </Link>
                                    </Col>
                                </Row>
                            </Col>
                        </Row>

                        <div className="wrapper-content">
                            <div id="route">
                                {this.props.children}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }

}