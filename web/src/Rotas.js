import React from 'react';

import {
    Home
} from "./pages";

export default function GetRotas() {
    return [
        {
            titulo: "Home",
            icone: "fas fa-home",
            caminho: "/",
            componente: () => <Home />,
            mostrarMenu: true,
            exact: true
        }
    ]
}
