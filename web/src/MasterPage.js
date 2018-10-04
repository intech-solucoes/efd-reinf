import React from "react";
import { BrowserRouter as Router, Route, Link } from "react-router-dom";

import { VersaoService } from "@intechprev/efdreinf-service";

import { Row, Col } from "./components";

import GetRotas from './Rotas';

const rotas = GetRotas();

var nomeUsuario = "";
var nomeContribuinte = "";

export default class MasterPage extends React.Component {

    componentWillMount() {
        nomeUsuario = localStorage.getItem("nomeUsuario");
        nomeContribuinte = localStorage.getItem("nomeContribuinte");

        if(localStorage.getItem("token")) {
            VersaoService.ValidarToken()
                .then(() => {})
                .catch((err) => {
                    if(err.message.indexOf("401") > -1)
                    {
                        localStorage.removeItem("token");
                        document.location = "/";
                    }
                });
        } else {
            localStorage.removeItem("token");
            document.location = "/";
        }
        
    }

    getRota() {
        var rota = window.location.pathname;
        for(var i = 0; i < rotas.length; i++) {
            if(rota === rotas[i].caminho)
                return rotas[i].componente();
        }
    }

    logout() {
        localStorage.removeItem("token");
        localStorage.removeItem("contribuinte");
        document.location = "/";
    }

	render() {
        var Title = () => {
            var rota = window.location.pathname;
            for(var i = 0; i < rotas.length; i++) {
                if(rota === rotas[i].caminho) {
                    return(<h2>{rotas[i].titulo}</h2>);
                }
            }
        };

		return (
            <Router basename={process.env.PUBLIC_URL}>
                <div className="wrapper">
                    <nav className="navbar-default nav-open">
                        <ul>
                            <li className="navbar-header">
                                <img src="./imagens/intech.png" alt="Intech" />
                            </li>
                            {
                                rotas.map((rota, index) => {
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
                                        {nomeUsuario}<br/>
                                        <small className={"text-primary"}>{nomeContribuinte}</small>
                                    </Col>
                                    <Col tamanho={"2"}>
                                        <img className="icon" src="./imagens/UserImage.jpg" alt="user" />
                                    </Col>
                                </Row>
                            </Col>
                        </Row>

                        <div className="wrapper-content">
                            <div id="route">
                                { rotas.map((rota, index) => <Route key={index} exact={rota.exact} path={rota.caminho} component={rota.componente} />) }
                            </div>
                        </div>
                    </div>
                </div>
            </Router>
		);
	}
}