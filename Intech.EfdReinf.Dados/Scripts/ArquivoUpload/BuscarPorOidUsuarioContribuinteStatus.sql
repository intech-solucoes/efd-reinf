/*Config
    RetornaLista
    Retorno
        -ArquivoUploadEntidade
    Parametros
        -OID_USUARIO_CONTRIBUINTE:decimal
        -IND_STATUS:string
*/

SELECT *
FROM EFD_ARQUIVO_UPLOAD
WHERE OID_USUARIO_CONTRIBUINTE = @OID_USUARIO_CONTRIBUINTE
  AND IND_STATUS = @IND_STATUS