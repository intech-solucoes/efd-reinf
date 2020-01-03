/*Config
	RetornaLista
    Retorno
        -string
    Parametros
        -OID_CONTRIBUINTE:decimal
*/

SELECT DISTINCT COD_CNPJ_PRESTADOR
  FROM EFD_R2010
 WHERE OID_CONTRIBUINTE = @OID_CONTRIBUINTE