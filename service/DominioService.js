import { BaseService } from "@intechprev/react-lib";

class DominioService extends BaseService {
    BuscarPorCodigo(codigo) {
        return this.CriarRequisicao("GET", `/dominio/porCodigo/${codigo}`);
    }
}

export default new DominioService();