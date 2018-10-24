import React, { Component } from 'react';
import { Box, Botao } from '../../components';
import { UploadService } from '@intechprev/efdreinf-service';

export default class ArquivosGerados extends Component {

    download = async (oidArquivoUpload) => { 
        try {
            var caminhoArquivo = await UploadService.BuscarPorOidArquivoUpload(oidArquivoUpload);
            caminhoArquivo = caminhoArquivo.data.NOM_ARQUIVO_LOCAL;
            var nomeArquivo = caminhoArquivo.split('\\');
            nomeArquivo = nomeArquivo[1];

            const link = document.createElement('a');
            link.href = caminhoArquivo;
            link.setAttribute('download', nomeArquivo);
            document.body.appendChild(link);
            link.click();
        } catch(err) {
            if(err.response.data) 
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
