﻿/*Config
    RetornaLista
    Retorno
        -ContribuinteEntidade
    Parametros
        -OID_USUARIO:decimal
*/

SELECT *
FROM EFD_CONTRIBUINTE
INNER JOIN EFD_USUARIO_CONTRIBUINTE ON EFD_USUARIO_CONTRIBUINTE.OID_CONTRIBUINTE = EFD_CONTRIBUINTE.OID_CONTRIBUINTE
WHERE EFD_USUARIO_CONTRIBUINTE.OID_USUARIO = @OID_USUARIO