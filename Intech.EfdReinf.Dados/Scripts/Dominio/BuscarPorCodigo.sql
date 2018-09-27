/*Config
    RetornaLista
    Retorno
        -DominioEntidade
    Parametros
        -COD_DOMINIO:string
*/

SELECT *
FROM TBG_DOMINIO
WHERE COD_DOMINIO = @COD_DOMINIO