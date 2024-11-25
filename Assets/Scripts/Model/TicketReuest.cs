using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;

public class TicketReuest
{
    public struct PostData
    {
        public string total_number;
    }

    public List<Ticket> tickets = new();
    private static readonly HttpClient httpClient = new();
    private Auth auth; 

    public TicketReuest(Auth auth)
    {
        this.auth = auth;
    }

    public async Task<RequestResult> Post(int printNum)
    {
        RequestResult ret = new();
        tickets = new();

        string endpoint = Common.tomlRoot.Get<string>("endpoint");
        string url = endpoint + "/api/ticket";

        PostData postData = new() { total_number = printNum.ToString() };
        string json = JsonUtility.ToJson(postData);
        StringContent content = new(json, Encoding.UTF8, "application/json");

        httpClient.DefaultRequestHeaders.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.DefaultRequestHeaders.Add("access-token", auth.AccessToken);
        httpClient.DefaultRequestHeaders.Add("client", auth.Client);
        httpClient.DefaultRequestHeaders.Add("uid", auth.Uid);

        try
        {
            HttpResponseMessage response = httpClient.PostAsync(url, content).Result;
            string contents = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
                JObject jObject = JObject.Parse(contents);
                JToken ticketJsonObjects = jObject["tickets"];
                foreach (JToken ticketJsonObject in ticketJsonObjects)
                {
                    Ticket ticket = new();
                    
                    ticket.id = ticketJsonObject["id"].ToString();
                    ticket.base64QRString = ticketJsonObject["qr_url"].ToString();
                    ticket.createdAt = ticketJsonObject["created_at"].ToString();
                    ticket.qrSaveFilePath = Constants.QR_DIR_PATH + $"/{ticket.id}_qr.png";
                    ticket.ticketSaveFilePath = Constants.TICKET_DIR_PATH + $"/{ticket.id}_ticket.png";

                    await FileSaver.SaveQR(ticket.base64QRString, ticket.qrSaveFilePath);
                    await TicketComposer.Compose(ticket.qrSaveFilePath, ticket.ticketSaveFilePath, ticket);

                    tickets.Add(ticket);
                }

                ret.isValid = true;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                Common.Log("Network error: " + response.ReasonPhrase);
                ret.isValid = false;
                ret.message = response.ReasonPhrase;
            }
            else
            {
                Debug.LogError("Network error: " + response.ReasonPhrase);
                ret.isValid = false;
                ret.message = response.ReasonPhrase;
            }
        }
        catch (HttpRequestException e)
        {
            Debug.LogError("Http request exception: " + e.Message);
            ret.isValid = false;
            ret.message = e.Message;
        }
        catch (Exception e)
        {
            Debug.LogError("Request exception: " + e.Message);
            ret.isValid = false;
            ret.message = e.Message;
        }

        return ret;
    }
}
