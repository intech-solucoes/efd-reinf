import React, { Component } from "react";
import { CampoTexto, Combo, Botao } from '../../components';

import { ContribuinteService } from "@intechprev/efdreinf-service";

const textosAjuda = require('../../Textos');

export default class NovoContribuinte extends Component {
    constructor(props) {
        super(props);
        
        this.listaCampos = [];
        this.erros = [];

        this.state = {
            tipoInscricao: "1",
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
            cnpjEfr: "",
            nomeContato: "",
            cpfContato: "",
            telefoneFixoContato: "",
            telefoneCelularContato: "",
            emailContato: "",

            cnpjEfrObrigatorio: false,
            tamanhoMinimoTelefoneFixo: 0,
            tamanhoMinimoTelefoneCelular: 0,
            erros: []
        }
        
        // Este objeto armazena as opções dos combos do formulário. Deverá ser apagado quando houver uma rota para TBG_DOMINIO.
        this.opcoes = {
            tipoInscricao: [
                {
                    valor: '1',
                    nome: "PESSOA JURÍDICA"
                },
                {
                    valor: '2',
                    nome: "PESSOA FÍSICA"
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
                    valor: '06',
                    nome: "AGROINDÚSTRIA"
                },
                {
                    valor: '07',
                    nome: "PRODUTOR RURAL PESSOA JURÍDICA"
                },
                {
                    valor: '08',
                    nome: "CONSÓRCIO SIMPLIFICADO DE PRODUTORES RURAIS"
                },
                {
                    valor: '09',
                    nome: "ÓRGÃO GESTOR DE MÃO DE OBRA"
                },
                {
                    valor: '10',
                    nome: "ENTIDADE SINDICAL - LEI 12.023/2009"
                },
                {
                    valor: '11',
                    nome: "ASSOC. DESPORT. QUE MANTÉM CLUBE DE FUTEBOL PROF."
                },
                {
                    valor: '13',
                    nome: "BANCO, CAIXA ECONÔMICA, SOC.CRÉDITO, FINANC. INV."
                },
                {
                    valor: '14',
                    nome: "SINDICATOS EM GERAL, EXCETO CÓDIGO [10]"
                },
                {
                    valor: '21',
                    nome: "PESSOA FÍSICA, EXCETO SEGURADO ESPECIAL"
                },
                {
                    valor: '22',
                    nome: "SEGURADO ESPECIAL"
                },
                {
                    valor: '60',
                    nome: "MISSÃO DIPLOMÁTICA / REPARTIÇÃO CONSULAR"
                },
                {
                    valor: '70',
                    nome: "EMPRESA DE QUE TRATA O DECRETO 5.436/2005"
                },
                {
                    valor: '80',
                    nome: "ENTIDADE IMUNE OU ISENTA"
                },                {
                    valor: '85',
                    nome: "ENTE FEDERATIVO, ÓRGÃOS UNIÃO, AUTARQ. FUND."
                },
                {
                    valor: '99',
                    nome: "PESSOAS JURÍDICAS EM GERAL"
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
                {
                    valor: '4',
                    nome: "INCORPORAÇÃO"
                }
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
        });
    }

    novo = async () => {
        // Validação dos campos de telefone: ao menos um telefone do contato deve ser fornecido, e estes não devem estar inválidos.
        await this.setState({ tamanhoMinimoTelefoneCelular: 0, tamanhoMinimoTelefoneFixo: 0 });

        if(this.state.telefoneFixoContato && !this.state.telefoneCelularContato)
            await this.setState({ tamanhoMinimoTelefoneFixo: 14 });
        else if(this.state.telefoneCelularContato && !this.state.telefoneFixoContato)
            await this.setState({ tamanhoMinimoTelefoneCelular: 15 });
        else if(this.state.telefoneCelularContato && this.state.telefoneFixoContato)
            await this.setState({ tamanhoMinimoTelefoneFixo: 14, tamanhoMinimoTelefoneCelular: 15 });

        // Validações de todos os campos pelo método 'validar' de cada componente.            
        await this.limparErros();
        for(var i = 0; i < this.listaCampos.length; i++) {
            var campo = this.listaCampos[i];

            // Não deve ser feita a validação do campo CNPJ EFR, esta é feita na API e mostra uma mensagem de erro específica.
            if(campo.props.nome !== 'cnpjEfr')
                campo.validar();

            if(campo.possuiErros)
                await this.adicionarErro(campo.erros);
        }
        
        if(!this.state.telefoneFixoContato && !this.state.telefoneCelularContato)
            await this.adicionarErro("Pelo menos um telefone do contato deve ser fornecido");

        if(this.state.erros.length === 0) {
            // Criar contribuinte.
            try {
                await ContribuinteService.Criar(this.state.razaoSocial, this.state.tipoInscricao, this.state.cnpj, this.state.inicioValidade, 
                      this.state.terminoValidade, this.state.classificacaoTributaria, this.state.obrigatoriedadeECD, this.state.desoneracaoFolhaCPRB, 
                      this.state.isencaoMulta, this.state.situacaoPJ, this.state.enteFederativoResponsavel, this.state.cnpjEfr, this.state.nomeContato, 
                      this.state.cpfContato, this.state.telefoneFixoContato, this.state.telefoneCelularContato, this.state.emailContato, this.state.emailContato);

                alert("Contribuinte inserido com sucesso! Aguarde confirmação da Intech para iniciar a utilização do Intech EFD-Reinf!");
                window.location.reload();
                
            } catch(err) {
				if(err.response) {
					await this.adicionarErro(err.response.data);
				} else {
					await this.adicionarErro(err);
				}
            }
        }

    }

    /**
     * @description Método que altera o state de obrigatoriedade do campo cnpjEfr, cujo a regra é: caso o combo 'Ente Federativo Responsável' possua o valor 
     * 'S', cnpjEfr não deve ser obrigatório; caso possua o valor 'N', cnpjEfr deve ser obrigatório.
     */
    handleEfrChange = async () => {
        if(this.state.enteFederativoResponsavel !== 'N')
            await this.setState({ cnpjEfrObrigatorio: false });
        else
            await this.setState({ cnpjEfrObrigatorio: true });
    }

    render() {
        return (
            <div>
                <div className="row">
                    <div className="col">
                        <h4>Novo Contribuinte</h4>
                        <br/>

                        <Combo contexto={this} label={"Tipo de inscrição"} ref={ (input) => this.listaCampos[0] = input } 
                               nome="tipoInscricao" valor={this.state.tipoInscricao} obrigatorio={true} padrao={"1"}
                               opcoes={this.opcoes.tipoInscricao} desabilitado={true} col="col-lg-5" />

                        <CampoTexto contexto={this} ref={ (input) => this.listaCampos[1] = input }
                                    label={"Razão social"} nome={"razaoSocial"} tipo={"text"} max={115}
                                    placeholder={"Razão Social"} valor={this.state.razaoSocial} obrigatorio={true} col="col-lg-5" />

                        <CampoTexto contexto={this} ref={ (input) => this.listaCampos[2] = input }
                                    label={"CNPJ"} nome={"cnpj"} tipo={"text"} placeholder={"CNPJ"}
                                    obrigatorio={true} valor={this.state.cnpj} mascara={"99.999.999/9999-99"} col="col-lg-5" />

                        <CampoTexto contexto={this} ref={ (input) => this.listaCampos[3] = input }
                                    label={"Início Validade"} nome={"inicioValidade"} tipo={"text"} 
                                    placeholder={"Início Validade"} valor={this.state.inicioValidade} 
                                    obrigatorio={true} mascara={"99/99/9999"} botaoAjuda={textosAjuda.inicioValidade} col="col-lg-5" />

                        <CampoTexto contexto={this} ref={ (input) => this.listaCampos[4] = input }
                                    label={"Término Validade"} nome={"terminoValidade"} tipo={"text"} 
                                    placeholder={"Término Validade"} valor={this.state.terminoValidade}
                                    obrigatorio={false} mascara={"99/99/9999"} botaoAjuda={textosAjuda.terminoValidade} col="col-lg-5" />

                        <Combo contexto={this} label={"Classificação Tributária"} ref={ (input) => this.listaCampos[5] = input } 
                               nome="classificacaoTributaria" valor={this.state.classificacaoTributaria} obrigatorio={true}
                               opcoes={this.opcoes.classificacaoTributaria} botaoAjuda={textosAjuda.classificacaoTributaria} col="col-lg-5"  />

                        <Combo contexto={this} label={"Obrigatoriedade ECD"} ref={ (input) => this.listaCampos[6] = input } 
                               nome="obrigatoriedadeECD" valor={this.state.obrigatoriedadeECD} obrigatorio={true}
                               opcoes={this.opcoes.obrigatoriedadeECD} botaoAjuda={textosAjuda.obrigatoriedadeECD} col="col-lg-5" />

                        <Combo contexto={this} label={"Desoneração Folha CPRB"} ref={ (input) => this.listaCampos[7] = input } 
                               nome="desoneracaoFolhaCPRB" valor={this.state.desoneracaoFolhaCPRB} obrigatorio={true} 
                               opcoes={this.opcoes.desoneracaoFolhaCPRB} botaoAjuda={textosAjuda.desoneracaoFolhaCPRB} col="col-lg-5" />

                        <Combo contexto={this} label={"Isenção Multa"} ref={ (input) => this.listaCampos[8] = input } 
                               nome="isencaoMulta" valor={this.state.isencaoMulta} obrigatorio={true}
                               opcoes={this.opcoes.isencaoMulta} botaoAjuda={textosAjuda.isencaoMulta} col="col-lg-5" />

                        <Combo contexto={this} label={"Situação PJ"} ref={ (input) => this.listaCampos[9] = input } 
                               nome="situacaoPJ" valor={this.state.situacaoPJ} obrigatorio={true}
                               opcoes={this.opcoes.situacaoPJ} botaoAjuda={textosAjuda.situacaoPJ} col="col-lg-5" />

                        <Combo contexto={this} label={"Ente Federativo Responsável (EFR)"} ref={ (input) => this.listaCampos[10] = input } 
                               nome="enteFederativoResponsavel" valor={this.state.enteFederativoResponsavel} obrigatorio={true}
                               opcoes={this.opcoes.enteFederativoResponsavel} botaoAjuda={textosAjuda.enteFederativoResponsavel} 
                               onChange={this.handleEfrChange} col="col-lg-5" />

                        <CampoTexto contexto={this} ref={ (input) => this.listaCampos[11] = input } 
                                    placeholder={"CNPJ EFR"} valor={this.state.cnpjEfr} label={"CNPJ EFR"} 
                                    nome={"cnpjEfr"} tipo={"text"} obrigatorio={this.state.cnpjEfrObrigatorio} 
                                    mascara={"99.999.999/9999-99"} botaoAjuda={textosAjuda.cnpjEfr} col="col-lg-5" />

                        <CampoTexto contexto={this} ref={ (input) => this.listaCampos[12] = input }
                                    label={"Nome do Contato"} nome={"nomeContato"} tipo={"text"} 
                                    placeholder={"Nome do Contato"} obrigatorio={true} valor={this.state.nomeContato} 
                                    terminoValidadeobrigatorio={false} botaoAjuda={textosAjuda.nomeContato} col="col-lg-5" />

                        <CampoTexto contexto={this} ref={ (input) => this.listaCampos[13] = input } placeholder={"CPF do Contato"}
                                    label={"CPF do Contato"} nome={"cpfContato"} tipo={"text"} 
                                    valor={this.state.cpfContato} obrigatorio={true} 
                                    mascara={"999.999.999-99"} botaoAjuda={textosAjuda.cpfContato} col="col-lg-5" />

                        <CampoTexto contexto={this} ref={ (input) => this.listaCampos[14] = input }
                                    label={"Telefone Fixo do Contato"} nome={"telefoneFixoContato"} tipo={"text"} 
                                    placeholder={"Telefone Fixo do Contato"} valor={this.state.telefoneFixoContato} obrigatorio={false} 
                                    mascara={"(99) 9999-9999"} botaoAjuda={textosAjuda.telefoneFixoContato} col="col-lg-5" min={this.state.tamanhoMinimoTelefoneFixo} />

                        <CampoTexto contexto={this} ref={ (input) => this.listaCampos[15] = input }
                                    label={"Telefone Celular do Contato"} nome={"telefoneCelularContato"} tipo={"text"} 
                                    placeholder={"Telefone Celular do Contato"} valor={this.state.telefoneCelularContato} obrigatorio={false} 
                                    mascara={"(99) 99999-9999"} botaoAjuda={textosAjuda.telefoneCelularContato} col="col-lg-5" min={this.state.tamanhoMinimoTelefoneCelular} />

                        <CampoTexto contexto={this} ref={ (input) => this.listaCampos[16] = input }
                                    label={"E-mail do Contato"} nome={"emailContato"} tipo={"text"} 
                                    placeholder={"E-mail do Contato"} valor={this.state.emailContato} 
                                    obrigatorio={false} botaoAjuda={textosAjuda.emailContato} col="col-lg-5" />
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
                                <Botao titulo={"Incluir Novo Contribuinte"} tipo={"primary"} clicar={this.novo}
                                    block={true} usaLoading={true} />

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }

}