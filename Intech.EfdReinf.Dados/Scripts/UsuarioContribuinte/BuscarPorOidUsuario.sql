/*Config
    RetornaLista
    Retorno
        -UsuarioContribuinteEntidade
    Parametros
        -OID_USUARIO:decimal
*/

SELECT *
FROM EFD_USUARIO_CONTRIBUINTE
WHERE OID_USUARIO = @OID_USUARIO