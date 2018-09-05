/*Config
    Retorno
        -UsuarioEntidade
    Parametros
        -TXT_TOKEN:string
*/

SELECT *
FROM EFD_USUARIO
WHERE TXT_TOKEN = @TXT_TOKEN