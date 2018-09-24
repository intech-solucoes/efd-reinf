import React, { Component } from 'react';
import { CampoTexto, Botao, Box, Row, Col } from '../../components';

import { UsuarioService, ContribuinteService } from "@intechprev/efdreinf-service";

import "./index.css";

export default class MinhaConta extends Component {

    constructor(props) {
        super(props);

        this.listaCampos = [];
        this.erros = [];

        this.state = {
            usuario: {},
            listaContribuintes: [],
            erros: []
        }
    }

    async componentDidMount() {
        var result = await UsuarioService.Buscar();
        await this.setState({
            usuario: result.data
        });

        result = await ContribuinteService.Listar();
        await this.setState({
            listaContribuintes: result.data
        });
    }

    limparErros = async () => {
        this.erros = [];
        await this.setState({
            erros: this.erros
        });
    }

    adicionarErro = async (mensagem) => {
        this.erros.push(mensagem);
        await this.setState({
            erros: this.erros
        });
    }

    alterarDados = async () => { 
        await this.limparErros();

        for(var i = 0; i < this.listaCampos.length; i++) {
            var campo = this.listaCampos[i];
            campo.validar();

            if(campo.possuiErros)
                await this.adicionarErro(campo.erros);
        }

        if(this.state.erros.length === 0) {
            try {
                await UsuarioService.Atualizar(this.state.usuario);
                alert("Dados atualizados com sucesso!");
            } catch(err) {
                if(err.response)
                    await this.adicionarErro(err.response.data);
                else
                    await this.adicionarErro(err);
            }
        }
    }

    render() {
        return (
            <div>
                <Box>
                    <CampoTexto contexto={this} ref={ (input) => this.listaCampos[0] = input }
                                label={"E-mail"} nome={"TXT_EMAIL"} tipo={"email"} placeholder={"E-mail"}
                                valor={this.state.usuario.TXT_EMAIL} desabilitado={true} parent={"usuario"} />

                    <CampoTexto contexto={this} ref={ (input) => this.listaCampos[1] = input }
                                label={"Nome"} nome={"NOM_USUARIO"} tipo={"text"} placeholder={"Nome"}
                                valor={this.state.usuario.NOM_USUARIO} obrigatorio={true} parent={"usuario"} />
                                
                    <CampoTexto contexto={this} ref={ (input) => this.listaCampos[2] = input }
                                label={"CPF"} nome={"cpf"} tipo={"text"} placeholder={"CPF"}
                                valor={this.state.usuario.COD_CPF} obrigatorio={true} mascara={"999.999.999-99"} parent={"usuario"} />
                    
                    <CampoTexto contexto={this} ref={ (input) => this.listaCampos[3] = input }
                                label={"Telefone Fixo"} nome={"telefoneFixo"} tipo={"text"} placeholder={"Telefone Fixo"}
                                valor={this.state.usuario.COD_TELEFONE_FIXO} obrigatorio={true} mascara={"(99) 9999-9999"} parent={"usuario"} />
                    
                    <CampoTexto contexto={this} ref={ (input) => this.listaCampos[4] = input }
                                label={"Telefone Celular"} nome={"telefoneCelular"} tipo={"text"} placeholder={"Telefone Celular"}
                                valor={this.state.usuario.COD_TELEFONE_CEL} obrigatorio={true} mascara={"(99) 99999-9999"} parent={"usuario"} />

                    <Row>
                        <Col>
                            {this.state.erros.length > 0 &&
                                <div className="alert alert-danger" role="alert" 
                                    dangerouslySetInnerHTML={{__html: this.state.erros.join("<br/>") }}>
                                </div>
                            }
                            
                            <Botao titulo={"Salvar"} tipo={"primary"} clicar={this.alterarDados}
                                usaLoading={true} />
                            
                            <a href="/alterarSenha" className="btn btn-light ml-3">Alterar Senha</a>
                        </Col>
                    </Row>
                </Box>

                <Box titulo={"Contribuintes Vinculados"}>
                    {this.state.listaContribuintes.map((contribuinte, index) => (

                        <div className="media" key={index}>
                            <div className="media-body">
                                <h5 className="mt-0 text-primary">{contribuinte.NOM_RAZAO_SOCIAL}</h5>
                                {contribuinte.COD_CNPJ_CPF}
                            </div>
                        </div>
                        
                    ))}
                </Box>
            </div>
        );
    }
}