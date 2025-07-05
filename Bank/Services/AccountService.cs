using System.Text.Json;
using Bank.DTOs;
using Bank.Models;
using Microsoft.Extensions.Options;

namespace Bank.Services;

/// <summary>
/// Service implementation for handling account-related operations.
/// </summary>
public class AccountService : IAccountService
{
    private readonly ILogger<AccountService> _logger;
    private readonly ServiceSettings _settings;
    private readonly IRequestClient _requestClient;

    public AccountService(
        IOptions<ServiceSettings> settings,
        ILogger<AccountService> logger,
        IRequestClient requestClient)
    {
        _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _requestClient = requestClient ?? throw new ArgumentNullException(nameof(requestClient));
    }

    public async Task<AccountResponse[]> GetAccountStatusAsync(
        AccountStatusRequest request,
        string token,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(token);

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new ArgumentException("Token cannot be null or whitespace.", nameof(token));
        }

        _logger.LogInformation("Retrieving account status for document: {DocumentId}", request.DocumentId);

        try
        {
            var response = await _requestClient.PostAsync<AccountStatusRequest, AccountResponse[]>(_settings.AccountServiceUrl,
                                                                                                 request,
                                                                                                 token,
                                                                                                 cancellationToken
                                                                                                );
            return response;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error retrieving account status for document: {DocumentId}", request?.DocumentId);
            throw new ApplicationException("An error occurred while retrieving account status. Please try again later.", ex);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Error deserializing account status response for document: {DocumentId}", request?.DocumentId);
            throw new ApplicationException("Error processing account information. Please try again later.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error retrieving account status for document: {DocumentId}", request?.DocumentId);
            throw;
        }
    }
}
