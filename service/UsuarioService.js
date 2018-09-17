import { BaseService } from "@intechprev/react-lib";

class UsuarioService extends BaseService {
    VerificarLogin() {
        return this.CriarRequisicao("GET", `/usuario`);
    }

    Login(Email, Senha) {
        return this.CriarRequisicao("POST", `/usuario/login`, { Email, Senha });
    }

    Criar(TXT_EMAIL, PWD_USUARIO, NOM_USUARIO, COD_CPF, COD_TELEFONE_FIXO, COD_TELEFONE_CEL) {
        return this.CriarRequisicao("POST", `/usuario/criar`, { TXT_EMAIL, PWD_USUARIO, NOM_USUARIO, COD_CPF, COD_TELEFONE_FIXO, COD_TELEFONE_CEL });
    }
}

export default new UsuarioService();