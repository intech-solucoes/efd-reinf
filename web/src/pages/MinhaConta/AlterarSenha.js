import React, { Component } from 'react';
import { Link  } from 'react-router-dom'
import { CampoTexto, Botao, Box, PainelErros, Row, Col } from '../../components';
import { Page } from "../";

import { UsuarioService } from "@intechprev/efdreinf-service";

export default class AlterarSenha extends Component {
    constructor(props) {
        super(props);

        this.listaCampos = [];
        this.erros = [];

        this.state = {
            senhaAtual: "",
            novaSenha: "",
            confirmarSenha: "",

            erros: []
        }

        this.page = React.createRef();
    }
    
    async componentDidMount() {
        await this.page.current.loading(false);
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

    alterarSenha = async () => { 
        await this.validar();

        if(this.state.erros.length === 0) {
            try {
                await UsuarioService.AlterarSenha(this.state.senhaAtual, this.state.novaSenha);
                alert("Senha alterada com sucesso!");
                this.props.history.push('/minhaConta');
            } catch(err) {
				if(err.response) {
					await this.adicionarErro(err.response.data);
				} else {
					await this.adicionarErro(err);
				}
            }
        }
    }

    validar = async () => { 
        // Validação de campos obrigatórios.
        await this.limparErros();

        for(var i = 0; i < this.listaCampos.length; i++) {
            var campo = this.listaCampos[i];
            campo.validar();

            if(campo.possuiErros) {
                await this.adicionarErro(campo.erros);
            }
        }

        // Validação dos campos senha e confirmação de senha.
        if(this.state.novaSenha.length < 6)
            await this.adicionarErro("A senha deve possuir ao menos 6 caracteres.");

        else if(this.state.novaSenha !== this.state.confirmarSenha)
            await this.adicionarErro("As senhas devem coincidir.");
    }

    render() {
        return (            
            <Page {...this.props} ref={this.page}>                
                <Box>
                    <CampoTexto contexto={this} ref={ (input) => this.listaCampos[0] = input }
                                label={"Senha Atual"} nome={"senhaAtual"} tipo={"password"} valor={this.state.senhaAtual}
                                placeholder={"Informe sua senha atual"} obrigatorio={true} />

                    <CampoTexto contexto={this} ref={ (input) => this.listaCampos[1] = input }
                                label={"Nova Senha"} nome={"novaSenha"} tipo={"password"} valor={this.state.novaSenha}
                                placeholder={"Informe sua senha nova"} obrigatorio={true} />
                                
                    <CampoTexto contexto={this} ref={ (input) => this.listaCampos[2] = input }
                                label={"Confirmar Senha"} nome={"confirmarSenha"} tipo={"password"} valor={this.state.confirmarSenha}
                                placeholder={"Confirme sua nova senha"} obrigatorio={true} />

                    <Row>
                        <Col>
                            <PainelErros erros={this.state.erros} />

                            <Botao titulo={"Alterar Senha"} tipo={"primary"} clicar={this.alterarSenha} usaLoading={false} />
                            
                            <Link to="/minhaConta" className="btn btn-light ml-3">Cancelar</Link>
                        </Col>
                    </Row>

                </Box>
            </Page>
        );
    }
}