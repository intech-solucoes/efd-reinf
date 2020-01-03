import { BaseService } from "@intechprev/react-lib";

class Upload extends BaseService {
    BuscarPorOidArquivoUpload(oidArquivoUpload) { 
        return this.CriarRequisicao("GET", `/upload/${oidArquivoUpload}`);
    }

    BuscarPorOidUsuarioContribuinte(OidUsuarioContribuinte) {
        return this.CriarRequisicao("GET", `/upload/porOidUsuarioContribuinte/${OidUsuarioContribuinte}`);
    }

    BuscarCsvPorOidUsuarioContribuinteStatus(oidUsuarioContribuinte, status) {
        return this.CriarRequisicao("GET", `/upload/CsvPorOidUsuarioContribuinte/${oidUsuarioContribuinte}/${status}`);
    }

    Relatorio(oidArquivoUpload) {
        return this.CriarRequisicaoBlob("GET", `/upload/relatorio/${oidArquivoUpload}`);
    }

    Deletar(oidArquivoUpload) {
        return this.CriarRequisicao("GET", `/upload/deletar/${oidArquivoUpload}`);
    }
}

export default new Upload();
