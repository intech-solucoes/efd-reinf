import React from 'react';

import {
    Home, MinhaConta, AlterarSenha, GeracaoXml
} from "./pages";

export default function GetRotas() {
    return [
        {
            titulo: "Página Inicial",
            icone: "fas fa-home",
            caminho: "/",
            componente: () => <Home />,
            mostrarMenu: true,
            exact: true
        },
        {
            titulo: "Minha Conta",
            icone: "fas fa-user",
            caminho: "/minhaConta",
            componente: () => <MinhaConta />,
            mostrarMenu: true,
            exact: false
        },
        {
            titulo: "Alterar Senha",
            icone: "",
            caminho: "/alterarSenha",
            componente: () => <AlterarSenha />,
            mostrarMenu: false,
            exact: false
        },
        {
            titulo: "Geração XML",
            icone: "fas fa-table",
            caminho: "/geracaoXml",
            componente: () => <GeracaoXml />,
            mostrarMenu: true,
            exact: false
        }
    ]
}
