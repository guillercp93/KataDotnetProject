using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Bank.Utils;

public class RequestClient : IRequestClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<RequestClient> _logger;

    public RequestClient(IHttpClientFactory factory, ILogger<RequestClient> logger)
    {
        _httpClient = factory.CreateClient("ApiClient") ?? throw new ArgumentNullException(nameof(RequestClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<TResponse> GetAsync<TRequest, TResponse>(
        string url,
        TRequest? request = null,
        string token = "",
        CancellationToken cancellationToken = default)
        where TRequest : class
        where TResponse : class
    {
        try
        {
            var requestUri = url;
            
            // Add query parameters if request object is provided
            if (request != null)
            {
                var queryString = string.Join("&", request.GetType()
                    .GetProperties()
                    .Where(p => p.GetValue(request) != null)
                    .Select(p => $"{Uri.EscapeDataString(p.Name)}={Uri.EscapeDataString(p.GetValue(request)?.ToString() ?? "")}"));
                
                if (!string.IsNullOrEmpty(queryString))
                {
                    requestUri += (url.Contains('?') ? "&" : "?") + queryString;
                }
            }

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
            
            if (!string.IsNullOrWhiteSpace(token))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(requestMessage, cancellationToken);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<TResponse>(content) ?? 
                   throw new InvalidOperationException("Failed to deserialize response");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GET request to {Url}", url);
            throw;
        }
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(
        string url,
        TRequest request,
        string token = "",
        CancellationToken cancellationToken = default)
        where TRequest : class
        where TResponse : class
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync(url, content, cancellationToken);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonSerializer.Deserialize<TResponse>(responseContent) 
                ?? throw new InvalidOperationException("Failed to deserialize the response");

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error making POST request to {Url}", url);
            throw;
        }
    }
}
