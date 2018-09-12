import React, { Component } from "react";
import { CampoTexto, Combo, Botao } from '../../components';

const textosAjuda = require('../../Textos');

export default class NovoContribuinte extends Component {
    constructor(props) {
        super(props);
        
        this.listaCampos = [];
        this.erros = [];

    }

    componentDidMount() {
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
        
    }


    render() {
        return (
            <div>
                <h4>Novo Contribuinte</h4>
				<br/>

                <Combo label={"Tipo de inscrição"} nome="tipoInscricao"
                       valor="1" opcao={"PESSOA JURÍDICA"} />

                <CampoTexto contexto={this} placeholder={"Razão Social"}
                            label={"Razão social"} nome={"razaoSocial"} tipo={"text"} 
                            obrigatorio={true} />

                <CampoTexto contexto={this} placeholder={"CNPJ"} id={"cnpj"} 
                            label={"CNPJ"} nome={"cnpj"} tipo={"text"} 
                            obrigatorio={true} mascara={"99.999.999/9999-99"} />

                <CampoTexto contexto={this} placeholder={"Início Validade"}
                            label={"Início Validade"} nome={"inicioValidade"} tipo={"text"} 
                            obrigatorio={true} mascara={"99/99/9999"} botaoAjuda={textosAjuda.inicioValidade} />

                <CampoTexto contexto={this} placeholder={"Término Validade"}
                            label={"Término Validade"} nome={"terminoValidade"} tipo={"text"} 
                            obrigatorio={false} mascara={"99/99/9999"} botaoAjuda={textosAjuda.terminoValidade} />

                <Combo label={"Classificação Tributária"} nome="classificacaoTributaria" 
                       valor="1" opcao={"PESSOAS JURÍDICAS EM GERAL"} botaoAjuda={textosAjuda.classificacaoTributaria} />

                <Combo label={"Obrigatoriedade ECD"} nome="obrigatoriedadeECD" 
                       valor="1" opcao={"EMPRESA NÃO OBRIGADA"} botaoAjuda={textosAjuda.obrigatoriedadeECD} />

                <Combo label={"Desoneração Folha CPRB"} nome="desoneracaoFolhaCPRB"
                       valor="1" opcao={"NÃO APLICÁVEL"} botaoAjuda={textosAjuda.desoneracaoFolhaCPRB} />

                <Combo label={"Isenção Multa"} nome="isencaoMulta"
                       valor="1" opcao={"SEM ACORDO"} botaoAjuda={textosAjuda.isencaoMulta} />

                <Combo label={"Situação PJ"} nome="situacaoPJ"
                       valor="1" opcao={"SITUAÇÃO NORMAL"} botaoAjuda={textosAjuda.situacaoPJ} />

                <Combo label={"Ente Federativo Responsável"} nome="enteFederativoResponsavel"
                       valor="1" opcao={"NÃO É EFR"} botaoAjuda={textosAjuda.enteFederativoResponsavel} />

                <CampoTexto contexto={this} placeholder={"CNPJ ERF"}
                            label={"CNPJ ERF"} nome={"cnpjERF"} tipo={"text"} 
                            obrigatorio={true} mascara={"99.999.999/9999-99"} botaoAjuda={textosAjuda.cnpjERF} />

                <CampoTexto contexto={this} placeholder={"Nome do Contato"}
                            label={"Nome do Contato"} nome={"nomeContato"} tipo={"text"} 
                            obrigatorio={false} botaoAjuda={textosAjuda.nomeContato} />

                <CampoTexto contexto={this} placeholder={"CPF do Contato"}
                            label={"CPF do Contato"} nome={"cpfContato"} tipo={"text"} 
                            obrigatorio={true} mascara={"999.999.999-99"} botaoAjuda={textosAjuda.cpfContato} />

                <CampoTexto contexto={this} placeholder={"Telefone Fixo do Contato"}
                            label={"Telefone Fixo do Contato"} nome={"telefoneFixoContato"} tipo={"text"} 
                            obrigatorio={false} mascara={"(99) 9999-9999"} botaoAjuda={textosAjuda.telefoneFixoContato} />

                <CampoTexto contexto={this} placeholder={"Telefone Celular do Contato"}
                            label={"Telefone Celular do Contato"} nome={"telefoneCelularContato"} tipo={"text"} 
                            obrigatorio={false} mascara={"(99) 99999-9999"} botaoAjuda={textosAjuda.telefoneCelularContato} />

                <CampoTexto contexto={this} placeholder={"E-mail do Contato"}
                            label={"E-mail do Contato"} nome={"emailContato"} tipo={"text"} 
                            obrigatorio={false} botaoAjuda={textosAjuda.emailContato} />

                <Botao titulo={"Alterar dados do Contribuinte"} tipo={"primary"} clicar={this.novo}
                       block={true} usaLoading={true} />

            </div>
        );
    }

}