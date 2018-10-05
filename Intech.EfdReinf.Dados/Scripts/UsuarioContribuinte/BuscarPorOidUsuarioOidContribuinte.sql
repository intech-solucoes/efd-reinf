/*Config
    Retorno
        -UsuarioContribuinteEntidade
    Parametros
        -OID_USUARIO:decimal
        -OID_CONTRIBUINTE:decimal
*/

SELECT *
FROM EFD_USUARIO_CONTRIBUINTE
WHERE OID_USUARIO = @OID_USUARIO
  AND OID_CONTRIBUINTE = @OID_CONTRIBUINTE