/*Config
    Retorno
        -ContribuinteEntidade
    Parametros
        -COD_CNPJ_CPF:string
*/

SELECT *
FROM EFD_CONTRIBUINTE
WHERE COD_CNPJ_CPF = @COD_CNPJ_CPF