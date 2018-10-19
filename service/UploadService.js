import { BaseService } from "@intechprev/react-lib";

class Upload extends BaseService {
    BuscarPorOidUsuarioContribuinte(oidContribuinte) {
        return this.CriarRequisicao("GET", `/upload/${oidContribuinte}`);
    }

    BuscarPorOidUsuarioContribuinteStatus(oidContribuinte, status) {
        return this.CriarRequisicao("GET", `/upload/${oidContribuinte}/${status}`);
    }

    Relatorio(oidArquivoUpload) {
        return this.CriarRequisicaoBlob("GET", `/upload/relatorio/${oidArquivoUpload}`);
    }

    Deletar(oidArquivoUpload) {
        return this.CriarRequisicao("DELETE", `/upload/${oidArquivoUpload}`);
    }
}

export default new Upload();
