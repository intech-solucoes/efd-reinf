﻿/*Config
    RetornaLista
    Retorno
        -ArquivoUploadEntidade
    Parametros
        -OID_USUARIO_CONTRIBUINTE:decimal
        -IND_STATUS:string
*/

SELECT EFD_ARQUIVO_UPLOAD.*,
	EFD_USUARIO.NOM_USUARIO
FROM EFD_ARQUIVO_UPLOAD
INNER JOIN EFD_USUARIO_CONTRIBUINTE ON EFD_USUARIO_CONTRIBUINTE.OID_USUARIO_CONTRIBUINTE = EFD_ARQUIVO_UPLOAD.OID_USUARIO_CONTRIBUINTE
INNER JOIN EFD_USUARIO ON EFD_USUARIO.OID_USUARIO = EFD_USUARIO_CONTRIBUINTE.OID_USUARIO
WHERE EFD_ARQUIVO_UPLOAD.OID_USUARIO_CONTRIBUINTE = @OID_USUARIO_CONTRIBUINTE
  AND (EFD_ARQUIVO_UPLOAD.IND_STATUS = @IND_STATUS OR @IND_STATUS IS NULL)
ORDER BY EFD_ARQUIVO_UPLOAD.DTA_UPLOAD DESC