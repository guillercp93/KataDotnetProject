using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BancoKata.Models;

namespace BancoKata.Utils;

public static class ClientRequest
{
    private static string _token;

    public static string Token => _token;


    public static async Task<T> GetAsync<T>(string url, object queryParams = null)
    {
        if (queryParams != null)
        {
            var queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            foreach (var prop in queryParams.GetType().GetProperties())
            {
                var value = prop.GetValue(queryParams, null);
                if (value != null)
                {
                    queryString[prop.Name] = value.ToString();
                }
            }
            url += "?" + queryString.ToString();
        }

        using var httpClient = new HttpClient();

        if (!string.IsNullOrEmpty(_token))
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var jsonString = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<T>(jsonString);
    }

    public static async Task<T> PostAsync<T>(string url, object data)
    {
        using var httpClient = new HttpClient();
        if (!string.IsNullOrEmpty(_token))
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

        var content = new StringContent(JsonSerializer.Serialize(data), System.Text.Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync(url, content);
        response.EnsureSuccessStatusCode();
        var jsonString = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<T>(jsonString);
    }

    public static async Task RequestTokenAsync(string tokenUrl, object credentials)
    {
        if (string.IsNullOrWhiteSpace(tokenUrl))
            throw new ArgumentNullException(nameof(tokenUrl), "Token URL cannot be null or empty.");

        using var httpClient = new HttpClient();
        var content = new StringContent(JsonSerializer.Serialize(credentials), System.Text.Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync(tokenUrl, content);
        response.EnsureSuccessStatusCode();
        var jsonString = await response.Content.ReadAsStringAsync();

        TokenResponse tokenResponse = JsonSerializer.Deserialize<TokenResponse>(jsonString);

        if (!string.IsNullOrEmpty(tokenResponse.Token))
        {
            _token = tokenResponse.Token;
        }
        else
        {
            throw new Exception("Token not found in response.");
        }
    }
}
