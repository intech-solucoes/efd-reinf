import React, { Component } from 'react';
import { Botao, CampoTexto, Combo, Checkbox, Box, Col, Row, PainelErros } from '../../components';
import { handleFieldChange } from "@intechprev/react-lib";
import ArquivosGerados from './ArquivosGerados';

import { DominioService, GeracaoXmlService, ContribuinteService } from '@intechprev/efdreinf-service';

export default class GeracaoXml extends Component {
    constructor(props) {
        super(props);

        this.listaCampos = [];
        this.erros = [];

        this.state = {
            // States de Campos:
            r1000: false,
            r1070: false,
            r2010: false,
            r2098: false,
            r2099: false,

            tipoOperacao: "",
            ambienteEnvio: "",
            contribuinte: "",
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

            // States de visibilidade e campos desabilitados:
            checkboxR1000Desabilitado: false,
            checkboxR1070Desabilitado: false,
            checkboxR2010Desabilitado: false,
            checkboxR2098Desabilitado: false,
            checkboxR2099Desabilitado: false,

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
                ambienteEnvio: [],
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
        this.datas;
        this.dataAtual = new Date();
    }
    
    componentDidMount = async () => {
        window.scrollTo(0, 0);
        
        try {
            // Busca valores para preenchimento dos campos e combos.
            var nomeContribuinte = localStorage.getItem("nomeContribuinte");
            this.setState({ contribuinte: nomeContribuinte });
            this.combos.tipoOperacao = await DominioService.BuscarPorCodigo("DMN_OPER_REGISTRO");
            this.combos.ambienteEnvio = await DominioService.BuscarPorCodigo("DMN_TIPO_AMBIENTE_EFD");
            this.combos.dominioSimNao = await DominioService.BuscarPorCodigo("DMN_SN");

            // Busca usuários vinculados ao contribuinte.
            var usuarios = await ContribuinteService.BuscarUsuariosPorOidContribuinte(this.oidContribuinte);
            usuarios = usuarios.data.Usuarios;
            var usuariosResponsaveis = [];
            var usuario = {};
            for(var i = 0; i < usuarios.length; i++) {
                usuario = {valor: usuarios[i].Usuario.OID_USUARIO, nome: usuarios[i].Usuario.NOM_USUARIO};
                usuariosResponsaveis.push(usuario);
            }
            this.combos.usuarioResponsavel = usuariosResponsaveis;
    
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

        if(this.state.erros.length === 0) {
            if(this.state.r1000)
                await this.validarR1000();
            if(this.state.r1070)
                await this.validarR1070();
            if(this.state.r2010)
                await this.validarR2010();
            if(this.state.r2098)
                await this.validarR2098();
            if(this.state.r2099)
                await this.validarR2099();
        }
    }

    onChange = async (checkbox) => { 
        await this.limparErros();

        if(checkbox !== 'r1000')
            await this.setState({ r1000: false, checkboxR1000Desabilitado: !this.state.checkboxR1000Desabilitado });
        if(checkbox !== 'r1070')
            await this.setState({ r1070: false, checkboxR1070Desabilitado: !this.state.checkboxR1070Desabilitado });
        if(checkbox !== 'r2010')
            await this.setState({ r2010: false, checkboxR2010Desabilitado: !this.state.checkboxR2010Desabilitado });
        if(checkbox !== 'r2098')
            await this.setState({ r2098: false, checkboxR2098Desabilitado: !this.state.checkboxR2098Desabilitado });
        if(checkbox !== 'r2099')
            await this.setState({ r2099: false, checkboxR2099Desabilitado: !this.state.checkboxR2099Desabilitado });
    }

    onChangeR1000 = async (checkbox) => {
        this.onChange(checkbox);
        this.handleVisibilidade("tipoOperacao");
        this.handleVisibilidade("contribuinte");
        this.handleVisibilidade("usuarioResponsavel");
        this.handleVisibilidade("ambienteEnvio");
        this.combos.tipoOperacao = await DominioService.BuscarPorCodigo("DMN_OPER_REGISTRO");
    }

    onChangeR1070 = async (checkbox) => {
        this.onChange(checkbox);
        this.handleVisibilidade("ambienteEnvio");
    }

    onChangeR2010 = async (checkbox) => {
        this.onChange(checkbox);
        this.handleVisibilidade("tipoOperacao");
        this.handleVisibilidade("ambienteEnvio");
        this.handleVisibilidade("data");
        this.combos.tipoOperacao = await DominioService.BuscarPorCodigo("DMN_EFD_RETIFICADORA");
    }

    onChangeR2098 = async (checkbox) => {
        this.onChange(checkbox);
        await this.carregaReferenciaR2098();
        this.handleVisibilidade("ambienteEnvio");
        this.handleVisibilidade("referencia");
    }

    onChangeR2099 = async (checkbox) => {
        this.onChange(checkbox);
        await this.carregaReferenciaR2099();
        this.handleVisibilidade("ambienteEnvio");
        this.handleVisibilidade("referencia");
        this.handleVisibilidade("contratacaoServicos");
        this.handleVisibilidade("prestacaoServicos");
        this.handleVisibilidade("associacaoDesportiva");
        this.handleVisibilidade("repasseAssociacaoDesportiva");
        this.handleVisibilidade("producaoRural");
        this.handleVisibilidade("pagamentosDiversos");
        this.handleVisibilidade("competencia");
        this.carregaCompetenciaAno();
    }

    handleVisibilidade = async (campo) => {
        this.visibilidade[campo] = !this.state.visibilidade[campo];
        if(campo !== "contribuinte")
            this.setState({ 
                [campo]: "",
                visibilidade: this.visibilidade
            });
    }

    carregaReferenciaR2098 = async () => {
        var datas = await GeracaoXmlService.BuscarDatasPorOidContribuinte(this.oidContribuinte);
        this.datas = datas.data;
        this.combos.referenciaAno = datas.data;
    }

    carregaReferenciaR2099 = async () => {
        var datas = await GeracaoXmlService.BuscarDatasPorOidContribuinte(this.oidContribuinte);
        this.datas = datas.data;
        this.combos.referenciaAno = [];
        var ano = {};
        for(var i = 0; i < this.datas.length; i++) {
            ano = {Ano: this.datas[i].Ano, Ano: this.datas[i].Ano};
            if(this.datas[i].Ano >= 2018)
                this.combos.referenciaAno.push(ano);
        }
    }

    carregaReferenciaMes = async () => {
        this.combos.referenciaMes = [];
        var mes = {};

        // Limpa os meses caso a opção de 'ReferenciaAno' seja vazio.
        if(this.state.referenciaAno === "") {
            this.combos.referenciaMes = [];
            this.setState({ 
                combos: this.combos,
                referenciaMes: ""
            });
        }

        if(this.state.r2098) {
            for(var i = 0; i < this.datas.length; i++) {
                if(this.datas[i].Ano === Number(this.state.referenciaAno)) {
                    for(var j = 0; j < this.datas[i].Meses.length; j++) {
                        mes = {nome: this.datas[i].Meses[j], valor: this.datas[i].Meses[j]}
                        this.combos.referenciaMes.push(mes);
                    }
                    this.setState({ combos: this.combos });
                }
            }
        } else if(this.state.r2099) {
            // Chamar rota que busca as datas exceto R2010
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

    validarR1000 = async () => { 
        try {
            await GeracaoXmlService.GerarR1000(this.oidContribuinte, this.state.usuarioResponsavel, this.state.tipoOperacao, this.state.ambienteEnvio);
            alert("R-1000 Gerado com sucesso!");
            this.buscarArquivosGerados();
        } catch(err) {
            console.error(err);
        }
    }

    validarR1070 = async () => { 
        try {
            await GeracaoXmlService.GerarR1070(this.oidContribuinte, this.state.ambienteEnvio);
            alert("R-1070 Gerado com sucesso!");
            this.buscarArquivosGerados();
        } catch(err) {
            if(err.response)
                alert(err.response.data);
            else
                console.error(err);
        }
    }

    validarR2010 = async () => { 
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
                await GeracaoXmlService.GerarR2010(this.oidContribuinte, this.state.tipoOperacao, this.state.ambienteEnvio, dataInicial, dataFinal);
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
    
    validarR2098 = async () => {
        try {
            await GeracaoXmlService.GerarR2098(this.oidContribuinte, this.state.ambienteEnvio, this.state.referenciaAno, this.state.referenciaMes);
            alert("R-2098 Gerado com sucesso!");
            this.buscarArquivosGerados();
        } catch(err) {
            if(err.response)
                alert(err.response.data);
            else
                console.error(err);
        }
    }

    validarR2099 = async () => {
        var periodo = "01/" + this.state.referenciaMes + "/" + this.state.referenciaAno;
        var competencia = "01/" + this.state.competenciaMes + "/" + this.state.competenciaAno;

        var r2099 = {
            OID_CONTRIBUINTE: this.oidContribuinte,
            IND_AMBIENTE_ENVIO: this.state.ambienteEnvio,
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
        var opcaoSelecionada = this.state.r1000 || this.state.r1070 || this.state.r2010 || this.state.r2098 || this.state.r2099;

        return (
            <div>
                <Box titulo={"Tipo de Registro"}>
                    <Checkbox contexto={this} nome={"r1000"} onChange={() => this.onChangeR1000('r1000')}
                              valor={this.state.r1000} desabilitado={this.state.checkboxR1000Desabilitado} label={"R-1000 - Informações do Contribuinte"} />

                    <Checkbox contexto={this} nome={"r1070"} onChange={() => this.onChangeR1070('r1070')}
                              valor={this.state.r1070} desabilitado={this.state.checkboxR1070Desabilitado} label={"R-1070 - Tabela de Processos Administrativos/Judiciais"} />

                    <Checkbox contexto={this} nome={"r2010"} onChange={() => this.onChangeR2010('r2010')}
                              valor={this.state.r2010} desabilitado={this.state.checkboxR2010Desabilitado} label={"R-2010 - Retenção Contribuição Previdenciária - Serviços Tomados"} />
                                
                    <Checkbox contexto={this} nome={"r2098"} onChange={() => this.onChangeR2098('r2098')}
                              valor={this.state.r2098} desabilitado={this.state.checkboxR2098Desabilitado} label={"R-2098 - Reabertura dos Eventos Periódicos"} />

                    <Checkbox contexto={this} nome={"r2099"} onChange={() => this.onChangeR2099('r2099')}
                              valor={this.state.r2099} desabilitado={this.state.checkboxR2099Desabilitado} label={"R-2099 - Fechamento de Eventos Periódicos"} />
                </Box>

                {opcaoSelecionada &&
                    <Box>
                        {this.state.visibilidade.tipoOperacao &&
                            <Combo contexto={this} label={"Tipo de operação"} ref={ (input) => this.listaCampos[0] = input } 
                                   nome="tipoOperacao" valor={this.state.tipoOperacao} obrigatorio={true} 
                                   opcoes={this.state.combos.tipoOperacao.data} />
                        }

                        {this.state.visibilidade.ambienteEnvio && 
                            <Combo contexto={this} label={"Ambiente para envio"} ref={ (input) => this.listaCampos[1] = input } 
                                   nome="ambienteEnvio" valor={this.state.ambienteEnvio} obrigatorio={true}
                                   opcoes={this.state.combos.ambienteEnvio.data} />
                        }

                        {this.state.visibilidade.contribuinte &&
                            <CampoTexto contexto={this} ref={ (input) => this.listaCampos[2] = input }
                                        label={"Contribuinte"} nome={"contribuinte"} tipo={"text"} 
                                        placeholder={"Contribuinte"} valor={this.state.contribuinte}
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
                                                onChange={(e) => handleFieldChange(this, e)} /> 
                                        </div> 
                                        <div className="col-form-label"> 
                                            <b><label htmlFor="dataFinal"> a </label></b> 
                                        </div> 
                                        <div className="col-3"> <input className="form-control" name="dataFinal" id="dataFinal" type="date" 
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
                                    <Combo contexto={this} ref={ (input) => this.listaCampos[6] = input }
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
                                           nome="competenciaAno" valor={this.state.competenciaAno} comboCol="col-4" onChange={() => this.carregaCompetenciaMes}
                                           opcoes={this.state.combos.competenciaAno} nomeMembro={"nome"} valorMembro={"valor"} />
                                </Col>
                                <Col>
                                    <Combo contexto={this} ref={ (input) => this.listaCampos[14] = input }
                                           nome="competenciaMes" valor={this.state.competenciaMes} comboCol="col-4"
                                           opcoes={this.state.combos.competenciaMes} labelCol="col-lg-4" nomeMembro={"nome"} valorMembro={"valor"} />
                                </Col>
                            </Row>
                        }

                        <PainelErros erros={this.state.erros} />

                        <Botao titulo={"Gerar"} tipo={"primary"} clicar={this.gerar} usaLoading={true} />
                    </Box>
                }

                <ArquivosGerados arquivos={this.state.arquivosGerados} />
            </div>

        )
    }

}
