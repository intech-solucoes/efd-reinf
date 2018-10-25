import { BaseService } from "@intechprev/react-lib";

class ImportacaoCsvService extends BaseService {
    ImportarCsv(oidArquivoUpload, oidContribuinte) {
        return this.CriarRequisicao("POST", `/Importacao/importarCsv/${oidArquivoUpload}/${oidContribuinte}`);
    }
}

export default new ImportacaoCsvService();