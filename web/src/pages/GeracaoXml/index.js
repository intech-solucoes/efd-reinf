import React, { Component } from 'react';

import { handleFieldChange } from "@intechprev/react-lib";
import { DominioService, GeracaoXmlService, ContribuinteService } from "@intechprev/efdreinf-service";

import { Botao, CampoTexto, Combo, Box, Col, Row, PainelErros, PainelAlerta, Radio } from '../../components';
import { Page } from "../";

import ArquivosGerados from "./ArquivosGerados";

export default class GeracaoXml extends Component {
    constructor(props) {
        super(props);

        this.listaCampos = [];
        this.erros = [];

        this.state = {
            // States dos campos:
            opcaoSelecionada: "r1000",

            tipoOperacao: "",
            ambienteEnvio: "2",
            contribuinte: {},
            nomeContribuinte: "",
            dataInicial: "",
            dataFinal: "",
            referenciaAno: "",
            referenciaMes: "",
            contratacaoServicos: "",
            prestacaoServicos: "",
            associacaoDesportiva: "",
            repasseAssociacaoDesportiva: "",
            producaoRural: "",
            pagamentosDiversos: "",
            competenciaAno: "",
            competenciaMes: "",

            visibilidade: {
                tipoOperacao: false,
                ambienteEnvio: false,
                contribuinte: false,
                usuarioResponsavel: false,
                data: false,
                referencia: false,
                contratacaoServicos: false,
                prestacaoServicos: false,
                associacaoDesportiva: false,
                repasseAssociacaoDesportiva: false,
                producaoRural: false,
                pagamentosDiversos: false,
                competencia: false,
            },

            // States validação:
            erros: [],

            // States de combos e tabela:
            combos: {
                tipoOperacao: [],
                usuarioResponsavel: [],
                referenciaAno: [],
                referenciaMes: [],
                contratacaoServicos: [],
                prestacaoServicos: [],
                associacaoDesportiva: [],
                repasseAssociacaoDesportiva: [],
                producaoRural: [],
                pagamentosDiversos: [],
                competenciaAno: [],
                competenciaMes: [],
                dominioSimNao: []
            },

            arquivosGerados: []
        }

        this.oidContribuinte = localStorage.getItem("contribuinte");
        this.visibilidade = this.state.visibilidade;
        this.combos = this.state.combos;
        this.datas = {};
        this.dataAtual = new Date();
    }
    
    componentDidMount = async () => {
        window.scrollTo(0, 0);
        
        try {
            // Busca valores para preenchimento dos campos e combos.
            var nomeContribuinte = localStorage.getItem("nomeContribuinte");
            this.setState({ nomeContribuinte: nomeContribuinte });

            this.combos.tipoOperacao = await DominioService.BuscarPorCodigo("DMN_OPER_REGISTRO");
            this.combos.ambienteEnvio = await DominioService.BuscarPorCodigo("DMN_TIPO_AMBIENTE_EFD");
            this.combos.dominioSimNao = await DominioService.BuscarPorCodigo("DMN_SN");
            // Buscar o valor do ambiente de envio do contribuinte logado.

            // Busca usuários vinculados ao contribuinte.
            var usuarios = await ContribuinteService.BuscarPorOidContribuinte(this.oidContribuinte);
            usuarios = usuarios.data.Usuarios;

            var contribuinte = await ContribuinteService.BuscarPorOidContribuinte(this.oidContribuinte);
            await this.setState({
                contribuinte: contribuinte.data
            });

            var usuariosResponsaveis = [];
            var usuario = {};

            for(var i = 0; i < usuarios.length; i++) {
                usuario = {valor: usuarios[i].Usuario.OID_USUARIO, nome: usuarios[i].Usuario.NOM_USUARIO};
                usuariosResponsaveis.push(usuario);
            }

            this.combos.usuarioResponsavel = usuariosResponsaveis;
            await this.setState({ combos: this.combos });
            
            this.alternarCampos();
            this.buscarArquivosGerados();
        } catch(err) {
            console.error(err);
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
    
    toggleAmbienteEnvio = async () => { 
        var contribuinte = await ContribuinteService.AlterarAmbienteEnvio(this.oidContribuinte);
        this.setState({ 
            contribuinte: contribuinte.data
        });
    }

    buscarArquivosGerados = async () => { 
        // Busca arquivos gerados pelo contribuinte logado.
        var arquivosGerados = await GeracaoXmlService.BuscarArquivosGeradosPorOidContribuinte(this.oidContribuinte);
        await this.setState({ arquivosGerados: arquivosGerados.data });
    }
    
    gerar = async () => { 
        // Validações de campos obrigatórios.
        await this.limparErros();

        for(var i = 0; i < this.listaCampos.length; i++) {
            var campo = this.listaCampos[i];
            if(campo !== null && campo !== undefined)
                campo.validar();

            if(campo && campo.possuiErros)
                await this.adicionarErro(campo.erros);
        }

        // Em específico, a validação dos campos de Período é feita separadamente pois o Período não foi feito como um componente.
        if(this.state.opcaoSelecionada === 'r2010') {
            if(this.state.dataInicial === "" || this.state.dataFinal === "")
                this.adicionarErro("Campo \"Período\" obrigatório.");
        }

        if(this.state.erros.length === 0) {
            if(this.state.opcaoSelecionada === 'r1000')
                await this.gerarR1000();
            if(this.state.opcaoSelecionada === 'r1070')
                await this.gerarR1070();
            if(this.state.opcaoSelecionada === 'r2010')
                await this.gerarR2010();
            if(this.state.opcaoSelecionada === 'r2098')
                await this.gerarR2098();
            if(this.state.opcaoSelecionada === 'r2099')
                await this.gerarR2099();
        }
    }

    alternarCampos = async () => {
        var camposVisiveis = [];
        if(this.state.opcaoSelecionada === "r1000") {
            this.combos.tipoOperacao = await DominioService.BuscarPorCodigo("DMN_OPER_REGISTRO");
            camposVisiveis = ["tipoOperacao", "contribuinte", "usuarioResponsavel"];

            await this.handleVisibilidade(camposVisiveis);

        } else if(this.state.opcaoSelecionada === "r1070") {
            camposVisiveis = [];
            await this.handleVisibilidade(camposVisiveis);

        } else if(this.state.opcaoSelecionada === "r2010") {
            this.combos.tipoOperacao = await DominioService.BuscarPorCodigo("DMN_EFD_RETIFICADORA");
            camposVisiveis = ["tipoOperacao", "data"];
            await this.handleVisibilidade(camposVisiveis);

        } else if(this.state.opcaoSelecionada === "r2098") {
            await this.carregaReferencia();
            camposVisiveis = ["referencia", "referenciaAno"];
            await this.handleVisibilidade(camposVisiveis);

        } else if(this.state.opcaoSelecionada === "r2099") {
            await this.carregaReferencia();
            camposVisiveis = ["referencia", "referenciaAno", "contratacaoServicos", "prestacaoServicos", 
                              "associacaoDesportiva", "repasseAssociacaoDesportiva", "producaoRural",
                              "pagamentosDiversos", "competencia"];
                                  
            await this.handleVisibilidade(camposVisiveis);
            await this.carregaCompetenciaAno();
        }
    }

    /**
     * @param campos Array com o 'nome' de cada campo que ficará visível.
     * @description Método que altera o estado de visibilidade de cada campo dentro do array para true, e a visibilidade dos outros campos para false.
     */
    handleVisibilidade = async (campos) => {
        for(var key in this.visibilidade) {
            if(campos.includes(key))
                this.visibilidade[key] = true;
            else
                this.visibilidade[key] = false;
        }

        if(campos !== "contribuinte")
            this.setState({ 
                [campos]: "",
                visibilidade: this.visibilidade
            });
    }

    carregaReferencia = async () => {
        var datas = [];

        if(this.state.opcaoSelecionada === 'r2098')
            datas = await GeracaoXmlService.BuscarDatasPorOidContribuinte(this.oidContribuinte);
        else if(this.state.opcaoSelecionada === 'r2099')
            datas = await GeracaoXmlService.BuscarDatasR2099PorOidContribuinte(this.oidContribuinte);

        this.datas = datas.data;
        this.combos.referenciaAno = this.datas;
    }

    carregaReferenciaMes = async () => {
        this.combos.referenciaMes = [];
        var mes = {};

        // Define o valor do combo Referencia Mes para o padrão.
        await this.setState({ referenciaMes: "" });

        // Limpa os meses caso a opção de "ReferenciaAno" seja vazio.
        if(this.state.referenciaAno === "") {
            this.combos.referenciaMes = [];
            await this.setState({ 
                combos: this.combos,
                referenciaMes: ""
            });
        }

        // Percorre o vetor de anos e posteriormente os meses que há dentro do ano selecionado.
        for(var i = 0; i < this.datas.length; i++) {
            if(this.datas[i].Ano === Number(this.state.referenciaAno)) {
                for(var j = 0; j < this.datas[i].Meses.length; j++) {
                    mes = {nome: this.datas[i].Meses[j], valor: this.datas[i].Meses[j]}
                    this.combos.referenciaMes.push(mes);
                }
               await this.setState({ combos: this.combos });
            }
        }
    }

    carregaCompetenciaAno = async () => { 
        var anoAtual = this.dataAtual.getFullYear();
        var comboCompetenciaAno = [];
        var ano = {};
        for(var i = 2018; i <= anoAtual; i++) {
            ano = {nome: i, valor: i};
            comboCompetenciaAno.push(ano);
        }
        this.combos.competenciaAno = comboCompetenciaAno;
        this.setState({ combos: this.combos });
    }
    
    carregaCompetenciaMes = async () => { 
        var anoAtual = this.dataAtual.getFullYear().toString();
        var mesAtual = this.dataAtual.getMonth() + 1;
        var comboCompetenciaMes = [];
        var mes = {};

        // Define o valor do combo Competencia Mes para o padrão.
        await this.setState({ competenciaMes: "" });

        var qtdeMeses = 12;
        if(this.state.competenciaAno === anoAtual)
            qtdeMeses = mesAtual;
        
        if(this.state.competenciaAno === "")
            qtdeMeses = 0;

        for(var i = 1; i <= qtdeMeses; i++) {
            mes = {nome: i, valor: i};
            comboCompetenciaMes.push(mes);
        }
        
        this.combos.competenciaMes = comboCompetenciaMes;
        this.setState({ combos: this.combos });
    }

    gerarR1000 = async () => { 
        try {
            await GeracaoXmlService.GerarR1000(this.oidContribuinte, this.state.usuarioResponsavel, this.state.tipoOperacao, this.state.contribuinte.IND_TIPO_AMBIENTE);
            alert("R-1000 Gerado com sucesso!");
            this.buscarArquivosGerados();
        } catch(err) {
            console.error(err);
        }
    }

    gerarR1070 = async () => { 
        try {
            await GeracaoXmlService.GerarR1070(this.oidContribuinte, this.state.contribuinte.IND_TIPO_AMBIENTE);
            alert("R-1070 Gerado com sucesso!");
            this.buscarArquivosGerados();
        } catch(err) {
            if(err.response)
                alert(err.response.data);
            else
                console.error(err);
        }
    }

    gerarR2010 = async () => { 
        var dataInicial = this.state.dataInicial.split("-");
        var dataFinal = this.state.dataFinal.split("-");

        dataInicial = new Date(dataInicial[0], dataInicial[1] - 1, dataInicial[2]);
        dataFinal = new Date(dataFinal[0], dataFinal[1] - 1, dataFinal[2]);
        
        var msParaDia = 1000 * 60 * 60 * 24;    // Valor que representa um dia em milissegundos.
        var diferencaDias = (dataFinal - dataInicial) / msParaDia;    // Divide-se por msParaDia pois a diferença entre duas datas resulta no valor em milissegundos.
        if(diferencaDias < 0)
            this.adicionarErro("A data final deve ser superior à data inicial.");

        if(this.state.erros.length === 0) {
            dataInicial = this.state.dataInicial.split("-");
            dataInicial = dataInicial[2] + "." + dataInicial[1] + "." + dataInicial[0];
            dataFinal = this.state.dataFinal.split("-");
            dataFinal = dataFinal[2] + "." + dataFinal[1] + "." + dataFinal[0];
            try {
                await GeracaoXmlService.GerarR2010(this.oidContribuinte, this.state.tipoOperacao, this.state.contribuinte.IND_TIPO_AMBIENTE, dataInicial, dataFinal);
                alert("R2010 Gerado com sucesso!");
                this.buscarArquivosGerados();
            } catch(err) {
                if(err.response)
                    await this.adicionarErro(err.response.data);
                else
                    await this.adicionarErro(err);
            }
        }
        
    }
    
    gerarR2098 = async () => {
        try {
            await GeracaoXmlService.GerarR2098(this.oidContribuinte, this.state.contribuinte.IND_TIPO_AMBIENTE, this.state.referenciaAno, this.state.referenciaMes);
            alert("R-2098 Gerado com sucesso!");
            this.buscarArquivosGerados();
        } catch(err) {
            if(err.response)
                alert(err.response.data);
            else
                console.error(err);
        }
    }

    gerarR2099 = async () => {
        var periodo = "01/" + this.state.referenciaMes + "/" + this.state.referenciaAno;
        var competencia = "01/" + this.state.competenciaMes + "/" + this.state.competenciaAno;

        var r2099 = {
            OID_CONTRIBUINTE: this.oidContribuinte,
            IND_AMBIENTE_ENVIO: this.state.contribuinte.IND_TIPO_AMBIENTE,
            DTA_PERIODO_APURACAO: periodo,
            IND_CONTRATACAO_SERV: this.state.contratacaoServicos,
            IND_PRESTACAO_SERV: this.state.prestacaoServicos,
            IND_ASSOCIACAO_DESPORTIVA: this.state.associacaoDesportiva,
            IND_REPASSE_ASSOC_DESPORT: this.state.associacaoDesportiva,
            IND_PRODUCAO_RURAL: this.state.producaoRural,
            IND_PAGAMENTOS_DIVERSOS: this.state.pagamentosDiversos,
            DTA_COMPETENCIA_SEM_MOV: competencia
        }
        try { 
            await GeracaoXmlService.GerarR2099(this.oidContribuinte, r2099);
            alert("R2099 Gerado com sucesso!");
            this.buscarArquivosGerados();
        } catch(err) {
            console.error(err);
        }
    }

    render() {

        return (
            <Page {...this.props}>
                <PainelAlerta tipo={this.state.contribuinte.IND_TIPO_AMBIENTE === "1" ? "success" : "info"}>
                    <span className="h3">{this.state.contribuinte.IND_TIPO_AMBIENTE === "1" ? "Produção" : "Produção Restrita"}</span>

                    <Botao 
                        clicar={this.toggleAmbienteEnvio}
                        pequeno={true} className={"float-right"}
                        tipo={this.state.contribuinte.IND_TIPO_AMBIENTE === "1" ? "success" : "info"} 
                        titulo={"Trocar Ambiente"}
                    />
                </PainelAlerta>

                <Box titulo={"Tipo de Registro"}>

                    <Radio contexto={this} nome={"r1000"} marcado={this.state.opcaoSelecionada === 'r1000'} 
                           label={"R-1000 - Informações do Contribuinte"} valor={"r1000"} onChange={this.alternarCampos} />

                    <Radio contexto={this} nome={"r1070"} marcado={this.state.opcaoSelecionada === 'r1070'} 
                           label={"R-1070 - Tabela de Processos Administrativos/Judiciais"} valor={"r1070"} onChange={this.alternarCampos} />

                    <Radio contexto={this} nome={"r2010"} marcado={this.state.opcaoSelecionada === 'r2010'} 
                           label={"R-2010 - Retenção Contribuição Previdenciária - Serviços Tomados"} valor={"r2010"} onChange={this.alternarCampos} />
                           
                    <Radio contexto={this} nome={"r2098"} marcado={this.state.opcaoSelecionada === 'r2098'} 
                           label={"R-2098 - Reabertura dos Eventos Periódicos"} valor={"r2098"} onChange={this.alternarCampos} />

                    <Radio contexto={this} nome={"r2099"} marcado={this.state.opcaoSelecionada === 'r2099'} 
                           label={"R-2099 - Fechamento de Eventos Periódicos"} valor={"r2099"} onChange={this.alternarCampos} />
                </Box>

                <Box>
                    {this.state.visibilidade.tipoOperacao &&
                        <Combo contexto={this} label={"Tipo de operação"} ref={ (input) => this.listaCampos[0] = input } 
                               nome="tipoOperacao" valor={this.state.tipoOperacao} obrigatorio={true} 
                               opcoes={this.state.combos.tipoOperacao.data} />
                    }

                    {this.state.visibilidade.contribuinte &&
                        <CampoTexto contexto={this} ref={ (input) => this.listaCampos[2] = input }
                                    label={"Contribuinte"} nome={"contribuinte"} tipo={"text"} 
                                    placeholder={"Contribuinte"} valor={this.state.nomeContribuinte}
                                    obrigatorio={true} desabilitado={true} />
                    }

                    {this.state.visibilidade.usuarioResponsavel &&
                        <Combo contexto={this} label={"Usuário Responsável"} ref={ (input) => this.listaCampos[3] = input } 
                               nome="usuarioResponsavel" valor={this.state.usuarioResponsavel} obrigatorio={true}
                               opcoes={this.state.combos.usuarioResponsavel} nomeMembro={"nome"} valorMembro={"valor"} />
                    }

                    {this.state.visibilidade.data &&
                        <Row>
                            <Col>
                                <div className="form-group row">
                                    <div className="col-lg-2 col-md-12 text-lg-right col-form-label"> 
                                        <b><label htmlFor="dataInicial"> 
                                            Período * 
                                        </label></b> 
                                    </div> 

                                    <div className="col-3"> 
                                        <input className="form-control" name="dataInicial" id="dataInicial" type="date" value={this.state.dataInicial} 
                                                onChange={async (e) => handleFieldChange(this, e)} /> 
                                    </div> 

                                    <div className="col-form-label"> 
                                        <b><label htmlFor="dataFinal"> a </label></b> 
                                    </div> 

                                    <div className="col-3"> 
                                        <input className="form-control" name="dataFinal" id="dataFinal" type="date" 
                                                value={(this.state.dataFinal)} onChange={(e) => handleFieldChange(this, e)} /> 
                                    </div> 
                                </div> 
                            </Col>
                        </Row>
                    }

                    {this.state.visibilidade.referencia &&
                        <Row>
                            <Col className="col-4">
                                <Combo contexto={this} label={"Referência"} ref={ (input) => this.listaCampos[5] = input } labelCol="col-lg-6"
                                       nome="referenciaAno" valor={this.state.referenciaAno} obrigatorio={true} comboCol="col-6"
                                       opcoes={this.state.combos.referenciaAno} nomeMembro={"Ano"} valorMembro={"Ano"} onChange={this.carregaReferenciaMes} />
                            </Col>

                            <Col>
                                <Combo contexto={this} label={"Referência"} ref={ (input) => this.listaCampos[6] = input } mostrarLabel={false}
                                       nome="referenciaMes" valor={this.state.referenciaMes} obrigatorio={true} comboCol="col-3"
                                       opcoes={this.state.combos.referenciaMes} nomeMembro={"nome"} valorMembro={"valor"} />
                            </Col>
                        </Row>
                    }
                    <br />
                    {this.state.visibilidade.contratacaoServicos &&
                        <Combo contexto={this} ref={ (input) => this.listaCampos[7] = input } labelCol="col-lg-4"
                               label={"Contratou serviços sujeitos à retenção de contribuição previdenciária?"}
                               nome="contratacaoServicos" valor={this.state.contratacaoServicos} obrigatorio={true}
                               opcoes={this.state.combos.dominioSimNao.data} />
                    }

                    {this.state.visibilidade.prestacaoServicos &&
                        <Combo contexto={this} ref={ (input) => this.listaCampos[8] = input } labelCol="col-lg-4"
                               label={"Prestou serviços sujeitos à retenção de contribuição previdenciária?"} 
                               nome="prestacaoServicos" valor={this.state.prestacaoServicos} obrigatorio={true}
                               opcoes={this.state.combos.dominioSimNao.data} />
                    }

                    {this.state.visibilidade.associacaoDesportiva &&
                        <Combo contexto={this} ref={ (input) => this.listaCampos[9] = input } labelCol="col-lg-4"
                               label={"A associação desportiva que mantém equipe de futebol profissional, possui informações sobre recursos recebidos?"}
                               nome="associacaoDesportiva" valor={this.state.associacaoDesportiva} obrigatorio={true}
                               opcoes={this.state.combos.dominioSimNao.data} />
                    }

                    {this.state.visibilidade.repasseAssociacaoDesportiva &&
                        <Combo contexto={this} ref={ (input) => this.listaCampos[10] = input } labelCol="col-lg-4"
                               label={"Possui informações sobre repasses efetuados à associação desportiva que mantém equipe de futebol profissional?"}
                               nome="repasseAssociacaoDesportiva" valor={this.state.repasseAssociacaoDesportiva} obrigatorio={true}
                               opcoes={this.state.combos.dominioSimNao.data} />
                    }

                    {this.state.visibilidade.producaoRural && 
                        <Combo contexto={this} ref={ (input) => this.listaCampos[11] = input } labelCol="col-lg-4"
                               label={"O produtor rural PJ/Agroindústria possui informações de comercialização de produção?"} 
                               nome="producaoRural" valor={this.state.producaoRural} obrigatorio={true}
                               opcoes={this.state.combos.dominioSimNao.data} />
                    }

                    {this.state.visibilidade.pagamentosDiversos &&
                        <Combo contexto={this} ref={ (input) => this.listaCampos[12] = input } labelCol="col-lg-4"
                                label={"Possui informações de pagamentos diversos no período de apuração?"}
                                nome="pagamentosDiversos" valor={this.state.pagamentosDiversos} obrigatorio={true}
                                opcoes={this.state.combos.dominioSimNao.data} /> 
                    }

                    {this.state.visibilidade.competencia && 
                        <Row>
                            <Col>
                                <Combo contexto={this} ref={ (input) => this.listaCampos[13] = input } labelCol="col-lg-8"
                                       label={"Competência a partir da qual não houve movimento, cuja situação perdura até a competência atual."}
                                       nome="competenciaAno" valor={this.state.competenciaAno} comboCol="col-4" onChange={() => this.carregaCompetenciaMes()}
                                       opcoes={this.state.combos.competenciaAno} nomeMembro={"nome"} valorMembro={"valor"} obrigatorio={true} />
                            </Col>
                            <Col>
                                <Combo contexto={this} ref={ (input) => this.listaCampos[14] = input }
                                       label={"Competência a partir da qual não houve movimento, cuja situação perdura até a competência atual."}
                                       nome="competenciaMes" valor={this.state.competenciaMes} comboCol="col-4" obrigatorio={true} mostrarLabel={false}
                                       opcoes={this.state.combos.competenciaMes} labelCol="col-lg-4" nomeMembro={"nome"} valorMembro={"valor"} />
                            </Col>
                        </Row>
                    }

                    <PainelErros erros={this.state.erros} />

                    <Botao titulo={"Gerar"} tipo={"primary"} clicar={this.gerar} usaLoading={true} />
                </Box>

                <ArquivosGerados arquivos={this.state.arquivosGerados} />
            </Page>

        )
    }

}
