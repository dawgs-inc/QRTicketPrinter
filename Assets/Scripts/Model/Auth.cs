using System;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;

public class Auth
{
    public struct PostData
    {
        public string email;
        public string password;
    }

    private static readonly HttpClient httpClient = new HttpClient();
    private string accessToken;
    public string AccessToken
    {
        get { return this.accessToken; }
        private set { this.accessToken = value; }
    }

    private string uid;
    public string Uid
    {
        get { return this.uid; }
        private set { this.uid = value; }
    }

    private string client;
    public string Client
    {
        get { return this.client; }
        private set { this.client = value; }
    }

    public async void RequestSignIn(Action<RequestResult> callback)
    {
        RequestResult finalResult = new RequestResult();

        string endpoint = Common.tomlRoot.Get<string>("endpoint");
        string email = Common.tomlRoot.Get<string>("mail");
        string password = Common.tomlRoot.Get<string>("password");
        string appVersion = Common.tomlRoot.Get<string>("version");

        RequestResult signInRet = await Task.Run<RequestResult>(() =>
        {
            return SignIn(endpoint, email, password);
        });

        if (!signInRet.isValid)
        {
            Common.Log($"Failed to sign in : {signInRet.message}");
            callback?.Invoke(signInRet);
            return;
        }

        finalResult.isValid = true;
        callback?.Invoke(finalResult);
    }

    // private RequestResult GetCsrfToken(string endpoint, string appVersion)
    // {
    //     RequestResult ret = new RequestResult();

    //     string url = endpoint + "/client";
    //     httpClient.DefaultRequestHeaders.Clear();
    //     httpClient.DefaultRequestHeaders.Add("version", appVersion);

    //     try
    //     {
    //         HttpResponseMessage response = httpClient.GetAsync(url).Result;

    //         if (response.IsSuccessStatusCode)
    //         {
    //             string responseBody = response.Content.ReadAsStringAsync().Result;

    //             Match matchedTag = Regex.Match(responseBody, "<meta name=\"csrf-token.+>");
    //             Match matchedKV = Regex.Match(matchedTag.Value, "content=\".+\"");
    //             string csrfToken = matchedKV.Value.Replace("content=", "").Replace("\"", "");

    //             Common.Log("CSRF Token: " + csrfToken);
    //             ret.isValid = true;
    //             ret.stringValue = csrfToken;
    //         }
    //         else
    //         {
    //             Common.Log("Request error: " + response.ReasonPhrase);
    //             ret.isValid = false;
    //             ret.message = response.ReasonPhrase;
    //         }
    //     }
    //     catch (Exception e)
    //     {
    //         Common.Log("Request exception: " + e.Message);
    //         ret.isValid = false;
    //         ret.message = e.Message;
    //     }

    //     return ret;
    // }

    private RequestResult SignIn(string endpoint, string email, string password)
    {
        RequestResult ret = new RequestResult();

        string url = endpoint + "/auth/sign_in";

        PostData postData = new PostData { email = email, password = password };
        string json = JsonUtility.ToJson(postData);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

        httpClient.DefaultRequestHeaders.Clear();

        try
        {
            HttpResponseMessage response = httpClient.PostAsync(url, content).Result;

            ret.statusCode = response.StatusCode;

            if (response.IsSuccessStatusCode)
            {
                AccessToken = response.Headers.Contains("access-token") ? response.Headers.GetValues("access-token").FirstOrDefault() : null;
                Uid = response.Headers.Contains("uid") ? response.Headers.GetValues("uid").FirstOrDefault() : null;
                Client = response.Headers.Contains("client") ? response.Headers.GetValues("client").FirstOrDefault() : null;

                Common.Log("AccessToken: " + AccessToken);
                Common.Log("Uid: " + Uid);
                Common.Log("Client: " + Client);

                ret.isValid = true;
            }
            else
            {
                Common.Log("Request error: " + response.ReasonPhrase);
                ret.isValid = false;
                ret.message = response.ReasonPhrase;
            }
        }
        catch (Exception e)
        {
            Common.Log("Request exception: " + e.Message);
            ret.isValid = false;
            ret.message = e.Message;
        }

        return ret;
    }
}
