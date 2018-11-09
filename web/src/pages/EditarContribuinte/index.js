import React, { Component } from "react";
import { CampoTexto, Combo, Botao, PainelErros, Row, Col, Box } from '../../components';

import { ContribuinteService, DominioService } from "@intechprev/efdreinf-service";

const textosAjuda = require('../../Textos');

export default class EditarContribuinte extends Component {
    constructor(props) {
        super(props);
        
        this.listaCampos = [];
        this.erros = [];

        this.state = {
            // States dos valores dos campos do formulário:
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

            // States dos valores dos combos: 
            combos: {
                tipoInscricao: [],
                classificacaoTributaria: [],
                obrigatoriedadeECD: [],
                desoneracaoFolhaCPRB: [],
                isencaoMulta: [],
                situacaoPJ: [],
                enteFederativoResponsavel: []
            },

            // States de validação:
            cnpjEfrObrigatorio: false,
            tamanhoMinimoTelefoneFixo: 0,
            tamanhoMinimoTelefoneCelular: 0,
            erros: []
        }
        
        this.combos = this.state.combos;
        this.oidContribuinte = localStorage.getItem("contribuinte");
    }

    componentDidMount = async () => {
        await this.buscarDadosContribuinte();

        this.combos.tipoInscricao = await DominioService.BuscarPorCodigo("DMN_TIPO_INSCRICAO_EFD");
        this.combos.classificacaoTributaria = await DominioService.BuscarPorCodigo("DMN_CLASSIF_TRIBUT");
        this.combos.obrigatoriedadeECD = await DominioService.BuscarPorCodigo("DMN_OBRIGADO_EFD");
        this.combos.desoneracaoFolhaCPRB = await DominioService.BuscarPorCodigo("DMN_DESONERACAO_EFD");
        this.combos.isencaoMulta = await DominioService.BuscarPorCodigo("DMN_ISENC_MULTA_EFD");
        this.combos.situacaoPJ = await DominioService.BuscarPorCodigo("DMN_SITUACAO_PJ");
        this.combos.enteFederativoResponsavel = await DominioService.BuscarPorCodigo("DMN_EFR_EFD");
        
        await this.setState({ combos: this.combos });
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

    buscarDadosContribuinte = async () => {
        var contribuinte = await ContribuinteService.BuscarPorOidContribuinte(this.oidContribuinte);
        contribuinte = contribuinte.data;

        this.setState({ 
            tipoInscricao: contribuinte.IND_TIPO_INSCRICAO,
            razaoSocial: contribuinte.NOM_RAZAO_SOCIAL,
            cnpj: contribuinte.COD_CNPJ_CPF,
            inicioValidade: contribuinte.DTA_INICIO_VALIDADE,
            terminoValidade: contribuinte.DTA_VALIDADE,
            classificacaoTributaria: contribuinte.IND_CLASSIF_TRIBUT,
            obrigatoriedadeECD: contribuinte.IND_OBRIGADA_ECD,
            desoneracaoFolhaCPRB: contribuinte.IND_DESONERACAO_CPRB,
            isencaoMulta: contribuinte.IND_ISENCAO_MULTA,
            situacaoPJ: contribuinte.IND_SITUACAO_PJ,
            enteFederativoResponsavel: contribuinte.IND_EFR,
            cnpjEfr: contribuinte.COD_CNPJ_EFR,
            nomeContato: contribuinte.NOM_CONTATO,
            cpfContato: contribuinte.COD_CPF_CONTATO,
            telefoneFixoContato: contribuinte.COD_FONE_FIXO_CONTATO,
            telefoneCelularContato: contribuinte.COD_FONE_CELULAR_CONTATO,
            emailContato: contribuinte.TXT_EMAIL_CONTATO,
        });
    }

    alterar = async () => {
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
            var contribuinte = await ContribuinteService.BuscarPorOidContribuinte(this.oidContribuinte);
            contribuinte = contribuinte.data;

            contribuinte = {
                OID_CONTRIBUINTE: this.oidContribuinte,
                NOM_RAZAO_SOCIAL: this.state.razaoSocial,
                IND_TIPO_INSCRICAO: this.state.tipoInscricao,
                COD_CNPJ_CPF: this.state.cnpj,
                DTA_INICIO_VALIDADE: this.state.inicioValidade,
                DTA_FIM_VALIDADE: this.state.terminoValidade,
                IND_CLASSIF_TRIBUT: this.state.classificacaoTributaria,
                IND_OBRIGADA_ECD: this.state.obrigatoriedadeECD,
                IND_DESONERACAO_CPRB: this.state.desoneracaoFolhaCPRB,
                IND_ISENCAO_MULTA: this.state.desoneracaoFolhaCPRB,
                IND_SITUACAO_PJ: this.state.situacaoPJ,
                IND_EFR: this.state.enteFederativoResponsavel,
                COD_CNPJ_EFR: this.state.cnpjEfr,
                NOM_CONTATO: this.state.nomeContato,
                COD_CPF_CONTATO: this.state.cpfContato,
                COD_FONE_FIXO_CONTATO: this.state.telefoneFixoContato,
                COD_FONE_CELULAR_CONTATO: this.state.telefoneCelularContato,
                TXT_EMAIL_CONTATO: this.state.emailContato,
                IND_APROVADO: "SIM",
                DTA_VALIDADE: contribuinte.DTA_VALIDADE,
                IND_TIPO_AMBIENTE: contribuinte.IND_TIPO_AMBIENTE
            }
            try {
                await ContribuinteService.Atualizar(contribuinte);

                alert("Dados do Contribuinte atualizados com sucesso!");
                
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
        if(this.state.enteFederativoResponsavel === 'S')
            await this.setState({ cnpjEfrObrigatorio: true });
        else if(this.state.enteFederativoResponsavel === 'N')
            await this.setState({ cnpjEfrObrigatorio: false });
    }

    render() {
        return (
            <Box>
                <Combo contexto={this} label={"Tipo de inscrição"} ref={ (input) => this.listaCampos[0] = input } 
                        nome="tipoInscricao" valor={this.state.tipoInscricao} obrigatorio={true} padrao={"1"}
                        opcoes={this.state.combos.tipoInscricao.data} desabilitado={true} textoVazio="Selecione uma opção" labelCol="col-lg-4" />

                <CampoTexto contexto={this} ref={ (input) => this.listaCampos[1] = input }
                            label={"Razão social"} nome={"razaoSocial"} tipo={"text"} max={115}
                            placeholder={"Razão Social"} valor={this.state.razaoSocial} obrigatorio={true} col="col-lg-4" />

                <CampoTexto contexto={this} ref={ (input) => this.listaCampos[2] = input }
                            label={"CNPJ"} nome={"cnpj"} tipo={"text"} placeholder={"CNPJ"}
                            obrigatorio={true} valor={this.state.cnpj} mascara={"99.999.999/9999-99"} col="col-lg-4" />

                <CampoTexto contexto={this} ref={ (input) => this.listaCampos[3] = input }
                            label={"Início Validade"} nome={"inicioValidade"} tipo={"text"} 
                            placeholder={"Início Validade"} valor={this.state.inicioValidade} 
                            obrigatorio={true} mascara={"99/99/9999"} botaoAjuda={textosAjuda.inicioValidade} col="col-lg-4" />

                <CampoTexto contexto={this} ref={ (input) => this.listaCampos[4] = input }
                            label={"Término Validade"} nome={"terminoValidade"} tipo={"text"} 
                            placeholder={"Término Validade"} valor={this.state.terminoValidade}
                            obrigatorio={false} mascara={"99/99/9999"} botaoAjuda={textosAjuda.terminoValidade} col="col-lg-4" />

                <Combo contexto={this} label={"Classificação Tributária"} ref={ (input) => this.listaCampos[5] = input } 
                        nome="classificacaoTributaria" valor={this.state.classificacaoTributaria} obrigatorio={true} textoVazio="Selecione uma opção"
                        opcoes={this.state.combos.classificacaoTributaria.data} botaoAjuda={textosAjuda.classificacaoTributaria} labelCol="col-lg-4"  />

                <Combo contexto={this} label={"Obrigatoriedade ECD"} ref={ (input) => this.listaCampos[6] = input } 
                        nome="obrigatoriedadeECD" valor={this.state.obrigatoriedadeECD} obrigatorio={true} textoVazio="Selecione uma opção"
                        opcoes={this.state.combos.obrigatoriedadeECD.data} botaoAjuda={textosAjuda.obrigatoriedadeECD} labelCol="col-lg-4" />

                <Combo contexto={this} label={"Desoneração Folha CPRB"} ref={ (input) => this.listaCampos[7] = input } 
                        nome="desoneracaoFolhaCPRB" valor={this.state.desoneracaoFolhaCPRB} obrigatorio={true} textoVazio="Selecione uma opção"
                        opcoes={this.state.combos.desoneracaoFolhaCPRB.data} botaoAjuda={textosAjuda.desoneracaoFolhaCPRB} labelCol="col-lg-4" />

                <Combo contexto={this} label={"Isenção Multa"} ref={ (input) => this.listaCampos[8] = input } 
                        nome="isencaoMulta" valor={this.state.isencaoMulta} obrigatorio={true} textoVazio="Selecione uma opção"
                        opcoes={this.state.combos.isencaoMulta.data} botaoAjuda={textosAjuda.isencaoMulta} labelCol="col-lg-4" />

                <Combo contexto={this} label={"Situação PJ"} ref={ (input) => this.listaCampos[9] = input } 
                        nome="situacaoPJ" valor={this.state.situacaoPJ} obrigatorio={true} textoVazio="Selecione uma opção"
                        opcoes={this.state.combos.situacaoPJ.data} botaoAjuda={textosAjuda.situacaoPJ} labelCol="col-lg-4" />

                <Combo contexto={this} label={"Ente Federativo Responsável (EFR)"} ref={ (input) => this.listaCampos[10] = input } 
                        nome="enteFederativoResponsavel" valor={this.state.enteFederativoResponsavel} obrigatorio={true}
                        opcoes={this.state.combos.enteFederativoResponsavel.data} botaoAjuda={textosAjuda.enteFederativoResponsavel} 
                        onChange={this.handleEfrChange} labelCol="col-lg-4" textoVazio="Selecione uma opção" />

                <CampoTexto contexto={this} ref={ (input) => this.listaCampos[11] = input } 
                            placeholder={"CNPJ EFR"} valor={this.state.cnpjEfr} label={"CNPJ EFR"} 
                            nome={"cnpjEfr"} tipo={"text"} obrigatorio={this.state.cnpjEfrObrigatorio} 
                            mascara={"99.999.999/9999-99"} botaoAjuda={textosAjuda.cnpjEfr} col="col-lg-4" />

                <CampoTexto contexto={this} ref={ (input) => this.listaCampos[12] = input }
                            label={"Nome do Contato"} nome={"nomeContato"} tipo={"text"} max={70}
                            placeholder={"Nome do Contato"} obrigatorio={true} valor={this.state.nomeContato} 
                            terminoValidadeobrigatorio={false} botaoAjuda={textosAjuda.nomeContato} col="col-lg-4" />

                <CampoTexto contexto={this} ref={ (input) => this.listaCampos[13] = input } placeholder={"CPF do Contato"}
                            label={"CPF do Contato"} nome={"cpfContato"} tipo={"text"} 
                            valor={this.state.cpfContato} obrigatorio={true} 
                            mascara={"999.999.999-99"} botaoAjuda={textosAjuda.cpfContato} col="col-lg-4" />

                <CampoTexto contexto={this} ref={ (input) => this.listaCampos[14] = input }
                            label={"Telefone Fixo do Contato"} nome={"telefoneFixoContato"} tipo={"text"} 
                            placeholder={"Telefone Fixo do Contato"} valor={this.state.telefoneFixoContato} obrigatorio={false} 
                            mascara={"(99) 9999-9999"} botaoAjuda={textosAjuda.telefoneFixoContato} col="col-lg-4" />

                <CampoTexto contexto={this} ref={ (input) => this.listaCampos[15] = input }
                            label={"Telefone Celular do Contato"} nome={"telefoneCelularContato"} tipo={"text"} 
                            placeholder={"Telefone Celular do Contato"} valor={this.state.telefoneCelularContato} obrigatorio={false} 
                            mascara={"(99) 99999-9999"} botaoAjuda={textosAjuda.telefoneCelularContato} col="col-lg-4" />

                <CampoTexto contexto={this} ref={ (input) => this.listaCampos[16] = input }
                            label={"E-mail do Contato"} nome={"emailContato"} tipo={"text"} 
                            placeholder={"E-mail do Contato"} valor={this.state.emailContato} 
                            obrigatorio={false} botaoAjuda={textosAjuda.emailContato} col="col-lg-4" />

                <Row>
                    <Col>
                        <div align="center">
                            <Col tamanho="5">

                                <PainelErros erros={this.state.erros} />

                                <br />
                                <Botao titulo={"Alterar Dados do Contribuinte"} tipo={"primary"} clicar={this.alterar}
                                    block={true} usaLoading={true} />

                            </Col>
                        </div>
                    </Col>
                </Row>
            </Box>
        );
    }

}