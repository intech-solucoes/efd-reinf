import React, { Component } from 'react';
import { handleFieldChange } from '@intechprev/react-lib'; 
import { Botao, CampoTexto, Combo, Checkbox, Box, Col, Row, PainelErros } from '../../components';
import ArquivosGerados from './ArquivosGerados';

import { DominioService } from '@intechprev/efdreinf-service';

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
            periodoInicial: "",
            periodoFinal: "",
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
                periodo: false,
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

            arquivosGerados: [
                {
                    registro: "R-1000",
                    dataGeracao: "30/09/2018 17:21:46",
                    ambiente: "Produção",
                    status: "Gerado",
                    usuario: "Cleber"
                },
                {
                    registro: "R-2010",
                    dataGeracao: "20/09/2018 09:56:03",
                    ambiente: "Pré-Produção",
                    status: "Enviado",
                    usuario: "Cleber"
                }
            ]
        }

        this.visibilidade = this.state.visibilidade;
        this.combos = this.state.combos;
    }

    componentDidMount = async () => {
        this.setState({ contribuinte: localStorage.getItem("nomeContribuinte") })
        this.combos.tipoOperacao = await DominioService.BuscarPorCodigo("DMN_OPER_REGISTRO");
        this.combos.ambienteEnvio = await DominioService.BuscarPorCodigo("DMN_TIPO_AMBIENTE_EFD");
        // Contribuinte - buscar o contribuinte logado
        // Usuário responsável - buscar usuários vinculados ao contribuinte
        this.combos.dominioSimNao = await DominioService.BuscarPorCodigo("DMN_SN");
        this.carregaComboReferencia();
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

    gerar = async () => { 
        // Validações de campos obrigatórios.
        await this.limparErros();

        for(var i = 0; i < this.listaCampos.length; i++) {
            var campo = this.listaCampos[i];

            // if(campo !== null)
            //     campo.validar();

            // if(campo && campo.possuiErros)
            //     await this.adicionarErro(campo.erros);

        }

        var periodoInicial = this.state.periodoInicial.split("-");
        var periodoFinal = this.state.periodoFinal.split("-");

        periodoInicial = new Date(periodoInicial[0], periodoInicial[1] - 1, periodoInicial[2]);
        periodoFinal = new Date(periodoFinal[0], periodoFinal[1] - 1, periodoFinal[2]);
        
        if(periodoInicial.getMonth() !== periodoFinal.getMonth()) 
            this.adicionarErro("As datas devem pertencer ao mesmo mês.");
        
        var msParaDia = 1000 * 60 * 60 * 24;    // Valor que representa um dia em milissegundos.
        var diferencaDias = (periodoFinal - periodoInicial) / msParaDia;    // Divide-se por msParaDia pois a diferença entre duas datas resulta no valor em milissegundos.

        if(diferencaDias < 0)
            this.adicionarErro("A data final deve ser superior à data inicial.");
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
    }

    onChangeR1070 = async (checkbox) => {
        this.onChange(checkbox);

        this.handleVisibilidade("ambienteEnvio");
    }

    onChangeR2010 = async (checkbox) => {
        this.onChange(checkbox);

        this.handleVisibilidade("tipoOperacao");
        this.handleVisibilidade("ambienteEnvio");
        this.handleVisibilidade("periodo");
    }

    onChangeR2098 = async (checkbox) => {
        this.onChange(checkbox);

        this.handleVisibilidade("ambienteEnvio");
        this.handleVisibilidade("referencia");
    }

    onChangeR2099 = async (checkbox) => {
        this.onChange(checkbox);

        this.handleVisibilidade("ambienteEnvio");
        this.handleVisibilidade("referencia");
        this.handleVisibilidade("contratacaoServicos");
        this.handleVisibilidade("prestacaoServicos");
        this.handleVisibilidade("associacaoDesportiva");
        this.handleVisibilidade("repasseAssociacaoDesportiva");
        this.handleVisibilidade("producaoRural");
        this.handleVisibilidade("competencia");
    }

    handleVisibilidade = async (campo) => {
        this.visibilidade[campo] = !this.state.visibilidade[campo];
        if(campo !== "contribuinte")
            await this.setState({ 
                [campo]: "",
                visibilidade: this.visibilidade
            });
    }

    carregaComboReferencia = async () => { 
        var dataAtual = new Date();
        var mesesAnteriores = [];
        var anosAnteriores = [];
        // não implementado.
        for(var i = 1; i < dataAtual.getMonth() + 1; i++) {
            mesesAnteriores.push(i);
        }
        for(i = 1970; i <= dataAtual.getFullYear(); i++) {
            anosAnteriores.push(i);
        }
        
        await this.setState({ referenciaAno: anosAnteriores, referenciaMes: mesesAnteriores });
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
                                   opcoes={[{NOM_DOMINIO: "Usuário 1", SIG_DOMINIO: 1}]} />
                        }

                        {this.state.visibilidade.periodo &&
                            <Row>
                                <Col>
                                    <div className="form-group row">
                                        <div className="col-lg-2 col-md-12 text-lg-right col-form-label">
                                            <b><label htmlFor="periodoInicial">
                                                Período *
                                            </label></b>
                                        </div>

                                        <div className="col-3">
                                            <input className="form-control" name="periodoInicial" ref={ (input) => this.listaCampos[4] = input } 
                                                   id="periodoInicial" type="date" value={this.state.periodoInicial} onChange={(e) => handleFieldChange(this, e)} />
                                        </div>

                                        <div className="col-form-label">
                                            <b><label htmlFor="periodoFinal">
                                                a
                                            </label></b>
                                        </div>

                                        <div className="col-3">
                                            <input className="form-control" name="periodoFinal" ref={ (input) => this.listaCampos[5] = input } 
                                                   id="periodoFinal" type="date" value={this.state.periodoFinal} onChange={(e) => handleFieldChange(this, e)} />
                                        </div>
                                    </div>
                                </Col>
                            </Row>
                        }

                        {this.state.visibilidade.referencia &&
                            <Row>
                                <Col>
                                    <div className="form-group row">
                                        <div className="col-lg-2 col-md-12 text-lg-right col-form-label">
                                            <b><label htmlFor="referencia">
                                                Referência *
                                            </label></b>
                                        </div>

                                        <div className="col-2">
                                            <select className="form-control" id="referenciaAno" ref={ (input) => this.listaCampos[6] = input } name="referenciaAno" onChange={() => {}}>
                                                <option value="">Selecione uma opção</option>
                                                {
                                                    this.state.referenciaAno.map((ano, index) => { 
                                                        return (
                                                            <option key={index} value={ano}>{ano}</option>
                                                        );
                                                    })
                                                }

                                            </select>
                                        </div>

                                        <div className="col-2">
                                            <select className="form-control" id="referenciaMes" ref={ (input) => this.listaCampos[7] = input } name="referenciaMes" onChange={() => {}}>
                                                <option value="">Selecione uma opção</option>
                                                {
                                                    this.state.referenciaMes.map((mes, index) => { 
                                                        return (
                                                            <option key={index} value={mes}>{mes}</option>
                                                        );
                                                    })
                                                }
                                            </select>
                                        </div>
                                    </div>
                                </Col>
                            </Row>
                        }

                        {this.state.visibilidade.contratacaoServicos &&
                            <Combo contexto={this} ref={ (input) => this.listaCampos[8] = input } 
                                    label={"Contratou serviços sujeitos à retenção de contribuição previdenciária?"}
                                    nome="contratacaoServicos" valor={this.state.contratacaoServicos} obrigatorio={true}
                                    opcoes={this.state.combos.dominioSimNao.data} />
                        }

                        {this.state.visibilidade.prestacaoServicos &&
                            <Combo contexto={this} ref={ (input) => this.listaCampos[9] = input } 
                                    label={"Prestou serviços sujeitos à retenção de contribuição previdenciária?"} 
                                    nome="prestacaoServicos" valor={this.state.prestacaoServicos} obrigatorio={true}
                                    opcoes={this.state.combos.dominioSimNao.data} />
                        }

                        {this.state.visibilidade.associacaoDesportiva &&
                            <Combo contexto={this} ref={ (input) => this.listaCampos[10] = input } 
                                    label={"A associação desportiva que mantém equipe de futebol profissional, possui informações sobre recursos recebidos?"}
                                    nome="associacaoDesportiva" valor={this.state.associacaoDesportiva} obrigatorio={true}
                                    opcoes={this.state.combos.dominioSimNao.data} />
                        }

                        {this.state.visibilidade.repasseAssociacaoDesportiva &&
                            <Combo contexto={this} ref={ (input) => this.listaCampos[11] = input } 
                                    label={"Possui informações sobre repasses efetuados à associação desportiva que mantém equipe de futebol profissional?"}
                                    nome="repasseAssociacaoDesportiva" valor={this.state.repasseAssociacaoDesportiva} obrigatorio={true}
                                    opcoes={this.state.combos.dominioSimNao.data} />
                        }

                        {this.state.visibilidade.producaoRural && 
                            <Combo contexto={this} ref={ (input) => this.listaCampos[12] = input } 
                                    label={"O produtor rural PJ/Agroindústria possui informações de comercialização de produção?"} 
                                    nome="producaoRural" valor={this.state.producaoRural} obrigatorio={true}
                                    opcoes={this.state.combos.dominioSimNao.data} />
                        }

                        {this.state.visibilidade.pagamentosDiversos &&
                            <Combo contexto={this} ref={ (input) => this.listaCampos[13] = input } 
                                    label={"Possui informações de pagamentos diversos no período de apuração?"}
                                    nome="pagamentosDiversos" valor={this.state.pagamentosDiversos} obrigatorio={true}
                                    opcoes={this.state.combos.dominioSimNao.data} />                                                                                                                                                                                                    
                        }

                        {this.state.visibilidade.competencia && 
                            <Row>
                                <Col>
                                    <div className="form-group row">
                                        <div className="col-lg-2 col-md-12 text-lg-right col-form-label">
                                            <b><label htmlFor="competencia">
                                                Competência a partir da qual não houve movimento, cuja situação perdura até a competência atual. *
                                            </label></b>
                                        </div>

                                        <div className="col-2">
                                            <select className="form-control" ref={ (input) => this.listaCampos[14] = input } id="competenciaAno" name="competencia">
                                                <option>AAAA</option>
                                            </select>
                                        </div>
                                        <div className="col-2">
                                            <select className="form-control" ref={ (input) => this.listaCampos[15] = input } id="competenciaMes" name="competencia">
                                                <option>MM</option>
                                            </select>
                                        </div>
                                    </div>
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
