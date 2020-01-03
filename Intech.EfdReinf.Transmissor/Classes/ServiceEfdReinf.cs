using Intech.EfdReinf.Entidades;
using Intech.Lib.JWT;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Intech.EfdReinf.Transmissor.Classes
{
    public class ServiceEfdReinf
    {
        public static JsonWebToken Login(string email, string senha) =>
            CriarRequisicaoPost<UsuarioLogin, JsonWebToken>("usuario/login", new UsuarioLogin { Email = email, Senha = senha });

        public static UsuarioEntidade BuscarUsuario() =>
            CriarRequisicaoGet<UsuarioEntidade>("usuario");

        public static List<ContribuinteEntidade> BuscarContribuintesAtivos() =>
            CriarRequisicaoGet<List<ContribuinteEntidade>>("contribuinte/ativos");

        public static List<AnoR2010> BuscarDatasR2010(decimal oidContribuinte) =>
            CriarRequisicaoGet<List<AnoR2010>>($"geracaoXml/datas/{oidContribuinte}");

        public static List<AnoR2010> BuscarDatasProcessadasEnviadasR2010(decimal oidContribuinte) =>
            CriarRequisicaoGet<List<AnoR2010>>($"geracaoXml/datasProcessadasEnviadas/{oidContribuinte}");

        public static List<string> BuscarPrestadores(decimal oidContribuinte) =>
            CriarRequisicaoGet<List<string>>($"geracaoXml/prestadores/{oidContribuinte}");

        public static void UpdateRecibo(decimal oidR2010, string numRecibo) =>
            CriarRequisicaoGet($"geracaoXml/atualizarR2010/{oidR2010}/{numRecibo}");

        #region Private Methods

        private static void CriarRequisicaoGet(string endpoint)
        {
            var webRequest = WebRequest.Create(Global.ENDERECO_BASE_API_EFD + endpoint);
            webRequest.Method = WebRequestMethods.Http.Get;
            webRequest.ContentType = "application/json; charset=utf-8";

            if (Global.Token != null)
                webRequest.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {Global.Token}");

            var response = (HttpWebResponse)webRequest.GetResponse();

            string resultadoJson = null;

            using (var streamReader = new System.IO.StreamReader(response.GetResponseStream()))
                resultadoJson = streamReader.ReadToEnd();
        }

        private static TRetorno CriarRequisicaoGet<TRetorno>(string endpoint)
        {
            var webRequest = WebRequest.Create(Global.ENDERECO_BASE_API_EFD + endpoint);
            webRequest.Method = WebRequestMethods.Http.Get;
            webRequest.ContentType = "application/json; charset=utf-8";

            if(Global.Token != null)
                webRequest.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {Global.Token}");

            var response = (HttpWebResponse)webRequest.GetResponse();

            string resultadoJson = null;

            using (var streamReader = new System.IO.StreamReader(response.GetResponseStream()))
                resultadoJson = streamReader.ReadToEnd();

            return JsonConvert.DeserializeObject<TRetorno>(resultadoJson, new JsonSerializerSettings() { DateFormatString = "dd/MM/yyyy HH:mm:ss" });
        }

        private static TRetorno CriarRequisicaoPost<TEnvio, TRetorno>(string endpoint, TEnvio dados)
        {
            try
            {
                var webRequest = WebRequest.Create(Global.ENDERECO_BASE_API_EFD + endpoint);
                webRequest.Method = WebRequestMethods.Http.Post;
                webRequest.ContentType = "application/json; charset=utf-8";

                if (Global.Token != null)
                    webRequest.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {Global.Token}");

                var jsonSerialize = JsonConvert.SerializeObject(dados);

                using (var streamWriter = new System.IO.StreamWriter(webRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonSerialize);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var response = (HttpWebResponse)webRequest.GetResponse();

                string resultadoJson = null;

                using (var streamReader = new System.IO.StreamReader(response.GetResponseStream()))
                    resultadoJson = streamReader.ReadToEnd();

                return JsonConvert.DeserializeObject<TRetorno>(resultadoJson);
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    throw new Exception(reader.ReadToEnd());
                }

                throw;
                //var webResponse = (HttpWebResponse)ex.Response;

                //// if the status code is 'file unavailable' then the file doesn't exist
                //// may be different depending upon FTP server software
                //if (webResponse.StatusCode == HttpStatusCode.BadRequest)
                //{

                //}

                //// some other error - like maybe internet is down
                //throw;
            }
        }

        #endregion
    }
}