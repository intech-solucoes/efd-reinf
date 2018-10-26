import { BaseService } from "@intechprev/react-lib";

class Upload extends BaseService {
    BuscarPorOidArquivoUpload(oidArquivoUpload) { 
        return this.CriarRequisicao("GET", `/upload/${oidArquivoUpload}`);
    }

    BuscarPorOidUsuarioContribuinte(OidUsuarioContribuinte) {
        return this.CriarRequisicao("GET", `/upload/porOidUsuarioContribuinte/${OidUsuarioContribuinte}`);
    }

    BuscarPorOidUsuarioContribuinteStatus(oidUsuarioContribuinte, status) {
        return this.CriarRequisicao("GET", `/upload/porOidUsuarioContribuinte/${oidUsuarioContribuinte}/${status}`);
    }

    Relatorio(oidArquivoUpload) {
        return this.CriarRequisicaoBlob("GET", `/upload/relatorio/${oidArquivoUpload}`);
    }

    Deletar(oidArquivoUpload) {
        return this.CriarRequisicao("DELETE", `/upload/${oidArquivoUpload}`);
    }
}

export default new Upload();
