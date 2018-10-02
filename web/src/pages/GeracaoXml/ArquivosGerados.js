import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Box } from '../../components';

export default class ArquivosGerados extends Component {

    render() {
        return (
            <Box titulo={"Arquivos Gerados"}>
                <table className="table table-striped">
                    <thead>
                        <tr>
                            <th>Registros</th>
                            <th>Data de Geração</th>
                            <th>Ambiente</th>
                            <th>Status</th>
                            <th>Usuário</th>
                            <th>Download</th>
                        </tr>
                    </thead>
                    <tbody>
                        {
                            this.props.arquivos.map((arquivo, index) => {
                                return (
                                    <tr key={index}>
                                        <td>{arquivo.registro}</td>
                                        <td>{arquivo.dataGeracao}</td>
                                        <td>{arquivo.ambiente}</td>
                                        <td>{arquivo.status}</td>
                                        <td>{arquivo.usuario}</td>
                                        <td><Link to="/geracaoXml"><i className="fas fa-download"></i></Link></td>
                                    </tr>
                                );
                            })
                        }
                    </tbody>
                </table>
            </Box>
        )
    }

}
