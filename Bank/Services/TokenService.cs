using Bank.DTOs;
using Bank.Models;
using Microsoft.Extensions.Options;

namespace Bank.Services;

public class TokenService : ITokenService
{
    private static readonly SemaphoreSlim _tokenLock = new (1, 1);
    private static string? _cachedToken;
    private static DateTime _tokenExpiration = DateTime.MinValue;
    
    private readonly ILogger<TokenService> _logger;
    private readonly TokenServiceSettings _settings;

    private readonly IRequestClient _requestClient;

    public TokenService(
        IRequestClient requestClient,
        IOptions<TokenServiceSettings> settings,
        ILogger<TokenService> logger)
    {
        _requestClient = requestClient ?? throw new ArgumentNullException(nameof(requestClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
    }

    public async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        // First check without lock for performance
        if (IsTokenValid())
        {
            return _cachedToken!;
        }

        // Only one thread at a time can attempt to get a new token
        if (!await _tokenLock.WaitAsync(TimeSpan.FromSeconds(30), cancellationToken))
        {
            throw new TimeoutException("Could not acquire lock to refresh token");
        }

        try
        {
            // Double-check in case another thread already refreshed the token
            if (IsTokenValid())
            {
                return _cachedToken!;
            }

            _logger.LogInformation("Requesting new token from {Url}", _settings.Url);
            
            // Get new token asynchronously
            var tokenResponse = await RequestNewTokenAsync(cancellationToken);
            ValidateTokenResponse(tokenResponse);
            
            _cachedToken = tokenResponse.Token;
            _tokenExpiration = tokenResponse.ExpiresIn;
            
            var timeLeft = _tokenExpiration - DateTime.UtcNow;
            _logger.LogInformation("Successfully obtained new access token. Expires in {TimeLeft} at {ExpiresAt}", 
                timeLeft, _tokenExpiration);
            
            return _cachedToken;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to obtain access token");
            throw;
        }
        finally
        {
            _tokenLock.Release();
        }
    }

    private bool IsTokenValid()
    {
        _logger.LogDebug("Checking token validity. Cached: {HasToken}, Expiration: {Expiration}, Now: {Now}", 
            !string.IsNullOrEmpty(_cachedToken), _tokenExpiration, DateTime.UtcNow);
            
        // Check if we have a token
        if (string.IsNullOrEmpty(_cachedToken))
        {
            _logger.LogDebug("No cached token available");
            return false;
        }

        // Add a 1-minute buffer to account for clock skew and network latency
        var buffer = TimeSpan.FromMinutes(1);
        var now = DateTime.UtcNow;
        
        // Check if token is expired (with buffer)
        if (now >= _tokenExpiration.Subtract(buffer))
        {
            _logger.LogDebug("Token expired or about to expire at {Expiration}, current time is {Now}", _tokenExpiration, now);
            return false;
        }

        var timeLeft = _tokenExpiration - now;
        _logger.LogDebug("Using valid cached token (expires in {TimeLeft} at {Expiration})", timeLeft, _tokenExpiration);
        return true;
    }

    private async Task<TokenResponse> RequestNewTokenAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Requesting new token from {Url}", _settings.Url);
        
        var content = new TokenRequest
        {
            User = _settings.User,
            Password = _settings.Password
        };
        
        var response = await _requestClient.PostAsync<TokenRequest, TokenResponse>(
            _settings.Url, 
            content, 
            cancellationToken: cancellationToken);
            
        _logger.LogDebug("Received token response. Expires at: {Expiration}", 
            response?.ExpiresIn.ToString("o") ?? "[null]");
            
        return response ?? throw new InvalidOperationException("Token response is null");
    }

    private void ValidateTokenResponse(TokenResponse? tokenResponse)
    {
        if (tokenResponse == null)
        {
            throw new InvalidOperationException("Token response is null");
        }
        
        if (string.IsNullOrEmpty(tokenResponse.Token))
        {
            throw new InvalidOperationException("Token is null or empty in the response");
        }
        
        if (tokenResponse.ExpiresIn == default)
        {
            _logger.LogWarning("No expiration time provided in token response. Setting default of 1 hour.");
            tokenResponse.ExpiresIn = DateTime.UtcNow.AddHours(1);
        }
        else if (tokenResponse.ExpiresIn <= DateTime.UtcNow)
        {
            _logger.LogWarning("Token expiration time is in the past. Adjusting to 1 hour from now.");
            tokenResponse.ExpiresIn = DateTime.UtcNow.AddHours(1);
        }
        
        _logger.LogDebug("Token will expire at {ExpirationTime}", tokenResponse.ExpiresIn);
    }
}
