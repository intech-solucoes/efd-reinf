import { BaseService } from "@intechprev/react-lib";

class GeracaoXmlService extends BaseService {
    GerarR1000(oidContribuinte, tipoAmbiente) {
        return this.CriarRequisicao("GET", `/geracaoXml/gerarR1000/${oidContribuinte}/${tipoAmbiente}`);
    }

    GerarR2010(oidContribuinte, tipoOperacao, tipoAmbiente, dtaInicial, dtaFinal) {
        return this.CriarRequisicao("GET", `/geracaoXml/gerarR2010/${oidContribuinte}/${tipoOperacao}/${tipoAmbiente}/${dtaInicial}/${dtaFinal}`);
    }

    GerarR2098(oidContribuinte, tipoAmbiente, ano, mes) {
        return this.CriarRequisicao("GET", `/geracaoXml/gerarR2098/${oidContribuinte}/${tipoAmbiente}/${ano}/${mes}`);
    }
}

export default GeracaoXmlService