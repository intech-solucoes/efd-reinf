import React, { Component } from 'react';

import { ContribuinteService } from "@intechprev/efdreinf-service";

import { Botao, Box, PainelAlerta, Radio } from '../../components';
import { Page } from "../";

import ArquivosGerados from "./ArquivosGerados";
import Campos from "./Campos";

export default class GeracaoXml extends Component {
    constructor(props) {
        super(props);

        this.state = {
            // States dos campos:
            opcaoSelecionada: "r1000",
            contribuinte: {},

            arquivosGerados: []
        }

        this.oidContribuinte = localStorage.getItem("oidContribuinte");

        this.page = React.createRef();
        this.campos = React.createRef();
    }
    
    componentDidMount = async () => {
        window.scrollTo(0, 0);
        
        try {
            // Busca valores para preenchimento dos campos e combos.
            var nomeContribuinte = localStorage.getItem("nomeContribuinte");
            this.setState({ nomeContribuinte: nomeContribuinte });
            
            var contribuinte = await ContribuinteService.BuscarPorOidContribuinte(this.oidContribuinte);
            await this.setState({
                contribuinte: contribuinte.data
            });

            await this.campos.current.alternarCampos();

            await this.page.current.loading(false);
        } catch(err) {
            console.error(err);
        }
    }
    
    toggleAmbienteEnvio = async () => { 
        var contribuinte = await ContribuinteService.AlterarAmbienteEnvio(this.oidContribuinte);
        this.setState({ 
            contribuinte: contribuinte.data
        });
    }
    
    render() {

        return (
            <Page {...this.props} ref={this.page}>
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
                           label={"R-1000 - Informações do Contribuinte"} valor={"r1000"} onChange={async () => {await this.campos.current.alternarCampos()}} />

                    <Radio contexto={this} nome={"r1070"} marcado={this.state.opcaoSelecionada === 'r1070'} 
                           label={"R-1070 - Tabela de Processos Administrativos/Judiciais"} valor={"r1070"} onChange={async () => {await this.campos.current.alternarCampos()}} />

                    <Radio contexto={this} nome={"r2010"} marcado={this.state.opcaoSelecionada === 'r2010'} 
                           label={"R-2010 - Retenção Contribuição Previdenciária - Serviços Tomados"} valor={"r2010"} onChange={async () => {await this.campos.current.alternarCampos()}} />
                           
                    <Radio contexto={this} nome={"r2098"} marcado={this.state.opcaoSelecionada === 'r2098'} 
                           label={"R-2098 - Reabertura dos Eventos Periódicos"} valor={"r2098"} onChange={async () => {await this.campos.current.alternarCampos()}} />

                    <Radio contexto={this} nome={"r2099"} marcado={this.state.opcaoSelecionada === 'r2099'} 
                           label={"R-2099 - Fechamento de Eventos Periódicos"} valor={"r2099"} onChange={async () => {await this.campos.current.alternarCampos()}} />
                </Box>

                <Campos opcaoSelecionada={this.state.opcaoSelecionada} ref={this.campos} />

                <ArquivosGerados />
            </Page>

        )
    }

}
