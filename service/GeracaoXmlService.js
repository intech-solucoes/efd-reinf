import { BaseService } from "@intechprev/react-lib";

class GeracaoXmlService extends BaseService {
    GerarR1000(oidContribuinte, tipoAmbiente) {
        return this.CriarRequisicao("GET", `/geracaoXml/gerarR1000/${oidContribuinte}/${tipoAmbiente}`);
    }
}

export default GeracaoXmlService