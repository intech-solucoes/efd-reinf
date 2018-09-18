import React, { Component } from "react";
import { CampoTexto, Combo, Botao } from '../../components';

import { ContribuinteService } from "@intechprev/efdreinf-service";
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
        
        // Este objeto armazena as opções dos combos do formulário. Deverá ser apagado quando houver uma rota para TBG_DOMINIO.
        this.opcoes = {
            tipoInscricao: [
                {
                    valor:'1',
                    nome: "PESSOA JURIDICA"
                },
                {
                    valor: '2',
                    nome: "PESSOA FISICA"
                }
            ],
            classificacaoTributaria: [
                {
                    valor: '01',
                    nome: "SIMPLES NAC. C/TRIBUTAÇÃO PREVID. SUBSTITUÍDA"
                },
                {
                    valor: '02',
                    nome: "SIMPLES NAC. C/TRIBUTAÇÃO PREVID. NÃO SUBSTITUÍDA"
                },
                {
                    valor: '03',
                    nome: "SIMPLES NAC. C/TRIBUTAÇÃO PREVID SUBSTITUÍDA/NÃO"
                },
                {
                    valor: '04',
                    nome: "MEI - MICRO EMPREENDEDOR INDIVIDUAL"
                },
                {
                    valor: '05',
                    nome: "AGROINDÚSTRIA"
                }
            ],
            obrigatoriedadeECD: [
                {
                    valor: '0',
                    nome: "EMPRESA NAO OBRIGADA"
                },
                {
                    valor: '1',
                    nome: "EMPRESA OBRIGADA À ECD"
                }
            ],
            desoneracaoFolhaCPRB: [
                {
                    valor: '0',
                    nome: "NÃO APLICÁVEL"
                },
                {
                    valor: '1',
                    nome: "EMPRESA ENQUADRADA NOS TERMOS DA LEI 12.546/2011"
                }
            ],
            isencaoMulta: [
                {
                    valor: '0',
                    nome: "SEM ACORDO"
                },
                {
                    valor: '1',
                    nome: "COM ACORDO"
                }
            ],
            situacaoPJ: [
                {
                    valor: '0',
                    nome: "NORMAL"
                },
                {
                    valor: '1',
                    nome: "EXTINÇÃO"
                },
                {
                    valor: '2',
                    nome: "FUSÃO"
                },
                {
                    valor: '3',
                    nome: "CISÃO"
                },
            ],
            enteFederativoResponsavel: [
                {
                    valor: 'S',
                    nome: "É EFR"
                },
                {
                    valor: 'N',
                    nome: "NÃO É EFR"
                }
            ]
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
        console.log(this.state);
        await this.validarFormulario();

        if(this.state.erros.length === 0) {
            // Criar contribuinte.
            try {
                await ContribuinteService.Criar(this.state.razaoSocial, this.state.tipoInscricao, this.state.cnpj, this.state.inicioValidade, 
                      this.state.terminoValidade, this.state.classificacaoTributaria, this.state.obrigatoriedadeECD, this.state.desoneracaoFolhaCPRB, 
                      this.state.isencaoMulta, this.state.situacaoPJ, this.state.enteFederativoResponsavel, this.state.cnpjERF, this.state.nomeContato, 
                      this.state.cpfContato, this.state.telefoneFixoContato, this.state.telefoneCelularContato, this.state.emailContato, this.state.emailContato);
            } catch(err) {
				if(err.response) {
					await this.adicionarErro(err.response.data);
				} else {
					await this.adicionarErro(err);
				}
            }
        } else {
            // Mostra os erros no state 'erros'.
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

                        <Combo contexto={this} label={"Tipo de inscrição"} ref={ (input) => this.listaCampos[0] = input } 
                               nome="tipoInscricao" valor={this.state.tipoInscricao} opcoes={this.opcoes.tipoInscricao} />

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

                        <Combo contexto={this} label={"Classificação Tributária"} ref={ (input) => this.listaCampos[5] = input } 
                               nome="classificacaoTributaria" valor={this.state.classificacaoTributaria} obrigatorio={true}
                               opcoes={this.opcoes.classificacaoTributaria} botaoAjuda={textosAjuda.classificacaoTributaria}  />

                        <Combo contexto={this} label={"Obrigatoriedade ECD"} ref={ (input) => this.listaCampos[6] = input } 
                               nome="obrigatoriedadeECD" valor={this.state.obrigatoriedadeECD} obrigatorio={true}
                               opcoes={this.opcoes.obrigatoriedadeECD} botaoAjuda={textosAjuda.obrigatoriedadeECD} />

                        <Combo contexto={this} label={"Desoneração Folha CPRB"} ref={ (input) => this.listaCampos[7] = input } 
                               nome="desoneracaoFolhaCPRB" valor={this.state.desoneracaoFolhaCPRB} obrigatorio={true} 
                               opcoes={this.opcoes.desoneracaoFolhaCPRB} botaoAjuda={textosAjuda.desoneracaoFolhaCPRB} />

                        <Combo contexto={this} label={"Isenção Multa"} ref={ (input) => this.listaCampos[8] = input } 
                               nome="isencaoMulta" valor={this.state.isencaoMulta} obrigatorio={true}
                               opcoes={this.opcoes.isencaoMulta} botaoAjuda={textosAjuda.isencaoMulta} />

                        <Combo contexto={this} label={"Situação PJ"} ref={ (input) => this.listaCampos[9] = input } 
                               nome="situacaoPJ" valor={this.state.situacaoPJ} obrigatorio={true}
                               opcoes={this.opcoes.situacaoPJ} botaoAjuda={textosAjuda.situacaoPJ} />

                        <Combo contexto={this} label={"Ente Federativo Responsável"} ref={ (input) => this.listaCampos[10] = input } 
                               nome="enteFederativoResponsavel" valor={this.state.enteFederativoResponsavel} obrigatorio={true}
                               opcoes={this.opcoes.enteFederativoResponsavel} botaoAjuda={textosAjuda.enteFederativoResponsavel} />

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