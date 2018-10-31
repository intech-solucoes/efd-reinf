import { BaseService } from "@intechprev/react-lib";

class UsuarioService extends BaseService {
    Criar(NOM_RAZAO_SOCIAL, IND_TIPO_INSCRICAO, COD_CNPJ_CPF, DTA_INICIO_VALIDADE, DTA_FIM_VALIDADE, IND_CLASSIF_TRIBUT, IND_OBRIGADA_ECD,
          IND_DESONERACAO_CPRB, IND_ISENCAO_MULTA, IND_SITUACAO_PJ, IND_EFR, COD_CNPJ_EFR, NOM_CONTATO, COD_CPF_CONTATO, COD_FONE_FIXO_CONTATO, 
          COD_FONE_CELULAR_CONTATO, TXT_EMAIL_CONTATO) {

        return this.CriarRequisicao("POST", `/contribuinte`, { NOM_RAZAO_SOCIAL, IND_TIPO_INSCRICAO, COD_CNPJ_CPF, DTA_INICIO_VALIDADE, 
            DTA_FIM_VALIDADE, IND_CLASSIF_TRIBUT, IND_OBRIGADA_ECD, IND_DESONERACAO_CPRB, IND_ISENCAO_MULTA, IND_SITUACAO_PJ, IND_EFR, COD_CNPJ_EFR, 
            NOM_CONTATO, COD_CPF_CONTATO, COD_FONE_FIXO_CONTATO, COD_FONE_CELULAR_CONTATO, TXT_EMAIL_CONTATO });
    }

    Listar() {
        return this.CriarRequisicao("GET", "/contribuinte");
    }

    ListarAtivos() {
        return this.CriarRequisicao("GET", "/contribuinte/ativos");
    }

    BuscarPorOidContribuinte(oidContribuinte) { 
        return this.CriarRequisicao("GET", `/contribuinte/${oidContribuinte}`);
    }
}

export default new UsuarioService();