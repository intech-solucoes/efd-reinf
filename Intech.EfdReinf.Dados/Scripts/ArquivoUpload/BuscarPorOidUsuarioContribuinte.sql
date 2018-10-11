/*Config
    RetornaLista
    Retorno
        -ArquivoUploadEntidade
    Parametros
        -OID_USUARIO_CONTRIBUINTE:decimal
*/

SELECT *
FROM EFD_ARQUIVO_UPLOAD
WHERE OID_USUARIO_CONTRIBUINTE = @OID_USUARIO_CONTRIBUINTE