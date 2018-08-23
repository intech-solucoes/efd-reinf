/*Config
    Retorno
        -UsuarioEntidade
    Parametros
        -TXT_EMAIL:string
*/

SELECT *
FROM EFD_USUARIO
WHERE TXT_EMAIL = @TXT_EMAIL