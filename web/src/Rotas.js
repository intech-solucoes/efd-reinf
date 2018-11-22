import React from 'react';

import {
    Login, Cadastro, EsqueciSenha, SelecionarContribuinte,
    Home, MinhaConta, AlterarSenha, GeracaoXml, ImportacaoArquivos, EditarContribuinte
} from "./pages";

function getRotas() {
    return [
        {
            titulo: "Página Inicial",
            icone: "fas fa-home",
            caminho: "/",
            componente: (routeProps) => <Home {...routeProps} />,
            mostrarMenu: true,
            exact: true
        },
        {
            titulo: "Minha Conta",
            icone: "fas fa-user",
            caminho: "/minhaConta",
            componente: (routeProps) => <MinhaConta {...routeProps} />,
            mostrarMenu: true,
            exact: false
        },
        {
            titulo: "Contribuinte",
            icone: "fas fa-building",
            caminho: "/editarContribuinte",
            componente: (routeProps) => <EditarContribuinte {...routeProps} />,
            mostrarMenu: true,
            exact: false
        },
        {
            titulo: "Alterar Senha",
            icone: "",
            caminho: "/alterarSenha",
            componente: (routeProps) => <AlterarSenha {...routeProps} />,
            mostrarMenu: false,
            exact: false
        },
        {
            titulo: "Importação de Arquivos",
            icone: "fas fa-file-import",
            caminho: "/importacaoArquivos",
            componente: (routeProps) => <ImportacaoArquivos {...routeProps} />,
            mostrarMenu: true,
            exact: false
        },
        {
            titulo: "Geração XML",
            icone: "fas fa-table",
            caminho: "/geracaoXml",
            componente: (routeProps) => <GeracaoXml {...routeProps} />,
            mostrarMenu: true,
            exact: false
        },
        {
            titulo: "Login",
            caminho: "/login",
            componente: (routeProps) => <Login {...routeProps} />,
            mostrarMenu: false,
            exact: false
        },
        {
            titulo: "Cadastro",
            caminho: "/cadastro",
            componente: (routeProps) => <Cadastro {...routeProps} />,
            mostrarMenu: false,
            exact: false
        },
        {
            titulo: "Selecionar Contribuinte",
            caminho: "/selecionarContribuinte",
            componente: (routeProps) => <SelecionarContribuinte {...routeProps} />,
            mostrarMenu: false,
            exact: false
        },
        {
            titulo: "Esqueci Minha Senha",
            caminho: "/esqueciSenha",
            componente: (routeProps) => <EsqueciSenha {...routeProps} />,
            mostrarMenu: false,
            exact: false
        }
    ]
}

export default getRotas();