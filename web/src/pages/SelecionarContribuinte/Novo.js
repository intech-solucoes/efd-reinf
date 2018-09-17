import React, { Component } from "react";
import { CampoTexto, Combo, Botao } from '../../components';

import { validarEmail } from "@intechprev/react-lib";
import DataInvalida from '../../utils/Data';

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

            erros: [],
        }
        
        this.opcoesCT = [
            {
                nome: "Teste",
                valor: 1
            },
            {
                nome: "Teste2",
                valor: 2
            }
        ]
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

        await this.validarFormulario();

        var dados = {}
        if(this.state.erros.length === 0) {
            dados = this.state;
            // Criar contribuinte.
        } else {
            // Exibir erros da API no vetor.
        }

    }

    validarFormulario = async () => {
        // Validações de campos obrigatórios.
        await this.limparErros();

        for(var i = 0; i < this.listaCampos.length; i++) {
            var campo = this.listaCampos[i];
            campo.validar();

            if(campo.possuiErros) {
                await this.adicionarErro(campo.erros);
            }
        }

        // Validações por comprimento da string.
        this.validarTamanho("CPF do Contato", this.state.cpfContato, 14);
        this.validarTamanho("Telefone Fixo do Contato", this.state.telefoneFixoContato, 14);
        this.validarTamanho("Telefone Celular do Contato", this.state.telefoneCelularContato, 15);

        // Validação dos campos de data.
        await this.validarDatas();

        // Validação do campo de e-mail, caso esteja preenchido.
        if(this.state.emailContato.length > 0) {
            if(validarEmail(this.state.emailContato))
                await this.adicionarErro("Campo e-mail inválido!");
        }

    }

    /**
     * @param {string} campo String com o nome do campo que será mostrado na mensagem de erro para o usuário.
     * @param {string} valor String com o valor do campo que terá o tamanho validado.
     * @param {comprimento} number Número inteiro com o valor do tamanho que a string deve ter.
     * @description Método que valida o campo por comprimento do valor. Caso o valor do campo não atinja o comprimento estipulado, uma mensagem de erro 
     * é adicionada ao vetor de erros.
     */
    validarTamanho = (campo, valor, comprimento) => {
        valor = valor.split("_").join("");

        if(valor.length < comprimento && valor.length !== 0) {
            console.log("IF");
            this.adicionarErro(`Campo ${campo} inválido!`);
        }
    }

    /**
     * @description Método que valida os campos de data do formulário (campos 'Início Validade' e 'Término Validade').
     */
    validarDatas = async () => {
        var dataInicioObjeto = this.converteData(this.state.inicioValidade);
        var dataInicioInvalida = DataInvalida(dataInicioObjeto, this.state.inicioValidade);
        
        if(dataInicioInvalida)
            await this.adicionarErro("Campo \"Início Validade\" inválido!");
        
        // var dataImplantacao = new Date(2018, 10, 15);
        // console.log(dataImplantacao);
        // if(dataInicioObjeto < dataImplantacao)
        //     await this.adicionarErro("Data \"Início Validade\" deve ser posterior à data de implantação");

        var dataTerminoObjeto = this.converteData(this.state.terminoValidade);
        var dataTerminoInvalida = DataInvalida(dataTerminoObjeto, this.state.terminoValidade);

        if(dataTerminoInvalida)
            await this.adicionarErro("Campo \"Término Validade\" inválido!")

        // O campo 'Término Validade' deve conter uma data posterior à data do campo 'Início Validade'.
        if(dataTerminoObjeto < dataInicioObjeto)
            await this.adicionarErro("Data \"Término Validade\" deve ser posterior à data \"Início Validade\"");
    }

    /**
     * @param {string} dataString Data a ser convertida para Date().
     * @description Método responsável por converter a data recebida (no formato 'dd/mm/aaaa') para date (Objeto).
     */
    converteData = (dataString) => {
        var dataPartes = dataString.split("/");
        return new Date(dataPartes[2], dataPartes[1] - 1, dataPartes[0]);
    }

    render() {
        return (
            <div>
                <div className="row">
                    <div className="col">
                        <h4>Novo Contribuinte</h4>
                        <br/>

                        <Combo label={"Tipo de inscrição"} ref={ (input) => this.listaCampos[0] = input } 
                               nome="tipoInscricao" opcoes={this.opcoesCT} />

                        <CampoTexto contexto={this} ref={ (input) => this.listaCampos[1] = input }
                                    label={"Razão social"} nome={"razaoSocial"} tipo={"text"} max={115}
                                    placeholder={"Razão Social"} valor={this.state.razaoSocial} obrigatorio={true} />

                        <CampoTexto contexto={this} ref={ (input) => this.listaCampos[2] = input }
                                    label={"CNPJ"} nome={"cnpj"} tipo={"text"} placeholder={"CNPJ"}
                                    obrigatorio={true} valor={this.state.cnpj} mascara={"99.999.999/9999-99"} />

                        <CampoTexto contexto={this} ref={ (input) => this.listaCampos[3] = input }
                                    label={"Início Validade"} nome={"inicioValidade"} tipo={"text"} 
                                    placeholder={"Início Validade"} valor={this.state.inicioValidade} 
                                    obrigatorio={true} mascara={"99/99/9999"} botaoAjuda={textosAjuda.inicioValidade} />

                        <CampoTexto contexto={this} ref={ (input) => this.listaCampos[4] = input }
                                    label={"Término Validade"} nome={"terminoValidade"} tipo={"text"} 
                                    placeholder={"Término Validade"} valor={this.state.terminoValidade}
                                    obrigatorio={false} mascara={"99/99/9999"} botaoAjuda={textosAjuda.terminoValidade} />

                        <Combo label={"Classificação Tributária"} ref={ (input) => this.listaCampos[5] = input } 
                               nome="classificacaoTributaria" opcoes={this.opcoesCT} botaoAjuda={textosAjuda.classificacaoTributaria} />

                        <Combo label={"Obrigatoriedade ECD"} ref={ (input) => this.listaCampos[6] = input } 
                               nome="obrigatoriedadeECD" opcoes={this.opcoesCT} botaoAjuda={textosAjuda.obrigatoriedadeECD} />

                        <Combo label={"Desoneração Folha CPRB"} ref={ (input) => this.listaCampos[7] = input } 
                               nome="desoneracaoFolhaCPRB" opcoes={this.opcoesCT} botaoAjuda={textosAjuda.desoneracaoFolhaCPRB} />

                        <Combo label={"Isenção Multa"} ref={ (input) => this.listaCampos[8] = input } 
                               nome="isencaoMulta" opcoes={this.opcoesCT} botaoAjuda={textosAjuda.isencaoMulta} />

                        <Combo label={"Situação PJ"} ref={ (input) => this.listaCampos[9] = input } 
                               nome="situacaoPJ" opcoes={this.opcoesCT} botaoAjuda={textosAjuda.situacaoPJ} />

                        <Combo label={"Ente Federativo Responsável"} ref={ (input) => this.listaCampos[10] = input } 
                               nome="enteFederativoResponsavel" opcoes={this.opcoesCT} botaoAjuda={textosAjuda.enteFederativoResponsavel} />

                        <CampoTexto contexto={this} ref={ (input) => this.listaCampos[11] = input } 
                                    placeholder={"CNPJ ERF"} valor={this.state.cnpjERF} label={"CNPJ ERF"} 
                                    nome={"cnpjERF"} tipo={"text"} obrigatorio={true} mascara={"99.999.999/9999-99"} 
                                    botaoAjuda={textosAjuda.cnpjERF} />

                        <CampoTexto contexto={this} ref={ (input) => this.listaCampos[12] = input }
                                    label={"Nome do Contato"} nome={"nomeContato"} tipo={"text"} 
                                    placeholder={"Nome do Contato"} valor={this.state.nomeContato} 
                                    terminoValidadeobrigatorio={false} botaoAjuda={textosAjuda.nomeContato} />

                        <CampoTexto contexto={this} ref={ (input) => this.listaCampos[13] = input } placeholder={"CPF do Contato"}
                                    label={"CPF do Contato"} nome={"cpfContato"} tipo={"text"} 
                                    valor={this.state.cpfContato} obrigatorio={true} 
                                    mascara={"999.999.999-99"} botaoAjuda={textosAjuda.cpfContato} />

                        <CampoTexto contexto={this} ref={ (input) => this.listaCampos[14] = input }
                                    label={"Telefone Fixo do Contato"} nome={"telefoneFixoContato"} tipo={"text"} 
                                    placeholder={"Telefone Fixo do Contato"} valor={this.state.telefoneFixoContato} obrigatorio={false} 
                                    mascara={"(99) 9999-9999"} botaoAjuda={textosAjuda.telefoneFixoContato} />

                        <CampoTexto contexto={this} ref={ (input) => this.listaCampos[15] = input }
                                    label={"Telefone Celular do Contato"} nome={"telefoneCelularContato"} tipo={"text"} 
                                    placeholder={"Telefone Celular do Contato"} valor={this.state.telefoneCelularContato} obrigatorio={false} 
                                    mascara={"(99) 99999-9999"} botaoAjuda={textosAjuda.telefoneCelularContato} />

                        <CampoTexto contexto={this} ref={ (input) => this.listaCampos[16] = input }
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