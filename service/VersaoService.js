import { BaseService } from "@intechprev/react-lib";

class UsuarioService extends BaseService {
    Get() {
        return this.CriarRequisicao("GET", `/`);
    }

    ValidarToken() {
        return this.CriarRequisicao("GET", `/validarToken`);
    }
}

export default new UsuarioService();