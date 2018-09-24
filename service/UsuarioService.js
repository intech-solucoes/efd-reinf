import { BaseService } from "@intechprev/react-lib";

class UsuarioService extends BaseService {
    Login(Email, Senha) {
        return this.CriarRequisicao("POST", `/usuario/login`, { Email, Senha });
    }

    ReenviarConfirmacao(Email, Senha) {
        return this.CriarRequisicao("POST", `/usuario/reenviarConfirmacao`, { Email, Senha });
    }

    Criar(TXT_EMAIL, PWD_USUARIO, NOM_USUARIO, COD_CPF, COD_TELEFONE_FIXO, COD_TELEFONE_CEL) {
        return this.CriarRequisicao("POST", `/usuario/criar`, { TXT_EMAIL, PWD_USUARIO, NOM_USUARIO, COD_CPF, COD_TELEFONE_FIXO, COD_TELEFONE_CEL });
    }

    RecuperarSenha(Email) {
        return this.CriarRequisicao("POST", `/usuario/recuperarSenha`, { Email });
    }

    Buscar() {
        return this.CriarRequisicao("GET", `/usuario`);
    }

    Atualizar(usuario) {
        return this.CriarRequisicao("PUT", "/usuario", usuario);
    }

    AlterarSenha(senhaAtual, senhaNova) {
        return this.CriarRequisicao("PUT", "/usuario/alterarSenha", { senhaAtual, senhaNova });
    }
}

export default new UsuarioService();