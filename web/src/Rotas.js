import React from 'react';

import {
    Home, MinhaConta, AlterarSenha, GeracaoXml, ImportacaoArquivos, EditarContribuinte
} from "./pages";

export default function GetRotas() {
    return [
        {
            titulo: "Página Inicial",
            icone: "fas fa-home",
            caminho: "/",
            componente: (routeProps) => <Home routeProps={routeProps} />,
            mostrarMenu: true,
            exact: true
        },
        {
            titulo: "Minha Conta",
            icone: "fas fa-user",
            caminho: "/minhaConta",
            componente: (routeProps) => <MinhaConta routeProps={routeProps} />,
            mostrarMenu: true,
            exact: false
        },
        {
            titulo: "Alterar Senha",
            icone: "",
            caminho: "/alterarSenha",
            componente: (routeProps) => <AlterarSenha routeProps={routeProps} />,
            mostrarMenu: false,
            exact: false
        },
        {
            titulo: "Geração XML",
            icone: "fas fa-table",
            caminho: "/geracaoXml",
            componente: (routeProps) => <GeracaoXml routeProps={routeProps} />,
            mostrarMenu: true,
            exact: false
        },
        {
            titulo: "Importação de Arquivos",
            icone: "fas fa-file-import",
            caminho: "/importacaoArquivos",
            componente: (routeProps) => <ImportacaoArquivos routeProps={routeProps} />,
            mostrarMenu: true,
            exact: false
        },
        {
            titulo: "Editar Contribuinte",
            icone: "fas fa-user",
            caminho: "/editarContribuinte",
            componente: (routeProps) => <EditarContribuinte routeProps={routeProps} />,
            mostrarMenu: true,
            exact: false
        }
    ]
}
