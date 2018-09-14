import React, { Component } from "react";
import { CampoTexto, Combo, Botao } from '../../components';

const textosAjuda = require('../../Textos');

export default class NovoContribuinte extends Component {
    constructor(props) {
        super(props);
        
        this.listaCampos = [];
        this.erros = [];

        this.state = {
            tipoInscricao: "",
            razaoSocial: "",
            cnpj: "",
            inicioValidade: "",
            terminoValidade: "",
            classificacaoTributaria: "",
            obrigatoriedadeECD: "",
            desoneracaoFolhaCPRB: "",
            isencaoMulta: "",
            situacaoPJ: "",
            enteFederativoResponsavel: "",
            cnpjERF: "",
            nomeContato: "",
            cpfContato: "",
            telefoneFixoContato: "",
            telefoneCelularContato: "",
            emailContato: "",

            erros: []
        }

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
        })
    }

    novo = async () => {
        await this.limparErros();
        
        for(var i = 0; i < this.listaCampos.length; i++) {
            var campo = this.listaCampos[i];
            campo.validar();

            if(campo.possuiErros) {
                await this.adicionarErro(campo.erros);
            }
        }

        if(this.state.erros.length === 0) {
            // Incluir contribuinte
        } else {
            // Exibir erros
        }

    }

    render() {
        return (
            <div>
                <div className="row">
                    <div className="col">
                        <h4>Novo Contribuinte</h4>
                        <br/>

                        <Combo label={"Tipo de inscrição"} nome="tipoInscricao"
                            valor="1" opcao={"PESSOA JURÍDICA"} />

                        <CampoTexto contexto={this} ref={ (input) => this.listaCampos[0] = input }
                                    label={"Razão social"} nome={"razaoSocial"} tipo={"text"}
                                    placeholder={"Razão Social"} valor={this.state.razaoSocial} obrigatorio={true} />

                        <CampoTexto contexto={this} ref={ (input) => this.listaCampos[1] = input }
                                    label={"CNPJ"} nome={"cnpj"} tipo={"text"} placeholder={"CNPJ"}
                                    obrigatorio={true} valor={this.state.cnpj} mascara={"99.999.999/9999-99"} />

                        <CampoTexto contexto={this} ref={ (input) => this.listaCampos[2] = input }
                                    label={"Início Validade"} nome={"inicioValidade"} tipo={"text"} 
                                    placeholder={"Início Validade"} valor={this.state.inicioValidade} 
                                    obrigatorio={true} mascara={"99/99/9999"} botaoAjuda={textosAjuda.inicioValidade} />

                        <CampoTexto contexto={this} ref={ (input) => this.listaCampos[3] = input }
                                    label={"Término Validade"} nome={"terminoValidade"} tipo={"text"} 
                                    placeholder={"Término Validade"} valor={this.state.terminoValidade}
                                    obrigatorio={false} mascara={"99/99/9999"} botaoAjuda={textosAjuda.terminoValidade} />

                        <Combo label={"Classificação Tributária"} nome="classificacaoTributaria" 
                            ref={ (input) => this.listaCampos[4] = input } valor="1" 
                            opcao={"PESSOAS JURÍDICAS EM GERAL"} botaoAjuda={textosAjuda.classificacaoTributaria} />

                        <Combo label={"Obrigatoriedade ECD"} nome="obrigatoriedadeECD" 
                            ref={ (input) => this.listaCampos[5] = input } valor="1"
                            opcao={"EMPRESA NÃO OBRIGADA"} botaoAjuda={textosAjuda.obrigatoriedadeECD} />

                        <Combo label={"Desoneração Folha CPRB"} nome="desoneracaoFolhaCPRB"
                            ref={ (input) => this.listaCampos[6] = input } valor="1" 
                            opcao={"NÃO APLICÁVEL"} botaoAjuda={textosAjuda.desoneracaoFolhaCPRB} />

                        <Combo label={"Isenção Multa"} nome="isencaoMulta"
                            ref={ (input) => this.listaCampos[7] = input } valor="1" 
                            opcao={"SEM ACORDO"} botaoAjuda={textosAjuda.isencaoMulta} />

                        <Combo label={"Situação PJ"} nome="situacaoPJ"
                            ref={ (input) => this.listaCampos[8] = input } valor="1" 
                            opcao={"SITUAÇÃO NORMAL"} botaoAjuda={textosAjuda.situacaoPJ} />

                        <Combo label={"Ente Federativo Responsável"} nome="enteFederativoResponsavel"
                            ref={ (input) => this.listaCampos[9] = input } valor="1" 
                            opcao={"NÃO É EFR"} botaoAjuda={textosAjuda.enteFederativoResponsavel} />

                        <CampoTexto contexto={this} ref={ (input) => this.listaCampos[10] = input } 
                                    placeholder={"CNPJ ERF"} valor={this.state.cnpjERF} label={"CNPJ ERF"} 
                                    nome={"cnpjERF"} tipo={"text"} obrigatorio={true} mascara={"99.999.999/9999-99"} 
                                    botaoAjuda={textosAjuda.cnpjERF} />

                        <CampoTexto contexto={this} ref={ (input) => this.listaCampos[11] = input }
                                    label={"Nome do Contato"} nome={"nomeContato"} tipo={"text"} 
                                    placeholder={"Nome do Contato"} valor={this.state.nomeContato} 
                                    terminoValidadeobrigatorio={false} botaoAjuda={textosAjuda.nomeContato} />

                        <CampoTexto contexto={this} ref={ (input) => this.listaCampos[12] = input } placeholder={"CPF do Contato"}
                                    label={"CPF do Contato"} nome={"cpfContato"} tipo={"text"} 
                                    placeholder={"CPF do Contato"} valor={this.state.cpfContato} obrigatorio={true} 
                                    mascara={"999.999.999-99"} botaoAjuda={textosAjuda.cpfContato} />

                        <CampoTexto contexto={this} ref={ (input) => this.listaCampos[13] = input }
                                    label={"Telefone Fixo do Contato"} nome={"telefoneFixoContato"} tipo={"text"} 
                                    placeholder={"Telefone Fixo do Contato"} valor={this.state.telefoneFixoContato} obrigatorio={false} 
                                    mascara={"(99) 9999-9999"} botaoAjuda={textosAjuda.telefoneFixoContato} />

                        <CampoTexto contexto={this} ref={ (input) => this.listaCampos[14] = input }
                                    label={"Telefone Celular do Contato"} nome={"telefoneCelularContato"} tipo={"text"} 
                                    placeholder={"Telefone Celular do Contato"} valor={this.state.telefoneCelularContato} obrigatorio={false} 
                                    mascara={"(99) 99999-9999"} botaoAjuda={textosAjuda.telefoneCelularContato} />

                        <CampoTexto contexto={this} ref={ (input) => this.listaCampos[15] = input }
                                    label={"E-mail do Contato"} nome={"emailContato"} tipo={"text"} 
                                    placeholder={"E-mail do Contato"} valor={this.state.emailContato} 
                                    obrigatorio={false} botaoAjuda={textosAjuda.emailContato} />
                    </div>
                </div>

                <div className="row">
                    <div className="col">
                        <div align="center">
                            <div className="col-5">

                                {this.state.erros.length > 0 &&
                                    <div className="alert alert-danger" role="alert" 
                                        dangerouslySetInnerHTML={{__html: this.state.erros.join("<br/>") }}>
                                    </div>
                                }

                                <br />
                                <Botao titulo={"Alterar dados do Contribuinte"} tipo={"primary"} clicar={this.novo}
                                    block={true} usaLoading={true} />

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }

}