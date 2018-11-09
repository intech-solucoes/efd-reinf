import React, { Component } from 'react';
import { Box, Botao } from '../../components';
import { UploadService } from '@intechprev/efdreinf-service';

export default class ArquivosGerados extends Component {

    download = async (oidArquivoUpload) => { 
        var apiUrl = require('../../config').apiUrl;
        try {
            var caminhoArquivo = await UploadService.BuscarPorOidArquivoUpload(oidArquivoUpload);
            caminhoArquivo =  caminhoArquivo.data.NOM_ARQUIVO_LOCAL;
            apiUrl = apiUrl.substring(0, apiUrl.length - 4);
            apiUrl = apiUrl + "/" + caminhoArquivo;

            const link = document.createElement('a');
            link.href = apiUrl;
            document.body.appendChild(link);
            link.click();
        } catch(err) {
            if(err.response) 
                alert(err.response.data);
            else 
                console.error(err);
        }
    }

    render() {
        var temArquivos = this.props.arquivos.length > 0;

        return (
            <Box titulo={"Arquivos Gerados"}>
                {temArquivos && 
                    <table className="table table-striped">
                        <thead>
                            <tr>
                                <th>Registro</th>
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
                                            <td>{arquivo.Tipo}</td>
                                            <td>{arquivo.DataGeracao}</td>
                                            <td>{arquivo.Ambiente}</td>
                                            <td>{arquivo.Status}</td>
                                            <td>{arquivo.Usuario}</td>
                                            <td><Botao clicar={() => this.download(arquivo.OidArquivoUpload)}><i className="fas fa-download"></i></Botao></td>
                                        </tr>
                                    );
                                })
                            }
                        </tbody>
                    </table>
                }
                
                {!temArquivos && 
                    <h4>Não há arquivos gerados</h4>
                }
            </Box>
        )
    }

}
