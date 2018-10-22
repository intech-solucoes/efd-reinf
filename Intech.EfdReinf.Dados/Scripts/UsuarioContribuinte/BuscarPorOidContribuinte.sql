/*Config
    RetornaLista
    Retorno
        -UsuarioContribuinteEntidade
    Parametros
        -OID_CONTRIBUINTE:decimal
*/

SELECT *
FROM EFD_USUARIO_CONTRIBUINTE
WHERE OID_CONTRIBUINTE = @OID_CONTRIBUINTE