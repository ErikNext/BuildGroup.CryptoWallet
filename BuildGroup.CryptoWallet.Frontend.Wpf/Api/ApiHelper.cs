using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml.Linq;

namespace BuildGroup.CryptoWallet.Frontend.Wpf.Api;

public static class ApiHelper
{
    public static HttpClient ApiClient { get; set; }

    public static void InitializeClient()
    {
        ApiClient = new HttpClient();
        ApiClient.BaseAddress = new Uri("https://localhost:7021/");
        ApiClient.DefaultRequestHeaders.Accept.Clear();
        ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }
}