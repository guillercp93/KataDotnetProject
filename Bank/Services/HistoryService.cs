using Microsoft.Extensions.Options;
using Bank.DTOs;
using Bank.Models;

namespace Bank.Services;

public class HistoryService : IHistoryService
{
    private readonly ILogger<HistoryService> _logger;
    private readonly ServiceSettings _settings;
    private readonly IRequestClient _requestClient;

    public HistoryService(
        IOptions<ServiceSettings> settings,
        ILogger<HistoryService> logger,
        IRequestClient requestClient)
    {
        _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _requestClient = requestClient ?? throw new ArgumentNullException(nameof(requestClient));
    }

    public async Task<CreditHistoryDTO> GetCreditHistoryAsync(AccountStatusRequest request, string token = "", CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(token);

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new ArgumentException("Token cannot be null or whitespace.", nameof(token));
        }
        _logger.LogInformation("Retrieving credit history for document: {DocumentId}", request.DocumentId);

        try
        {
            var externalResponse = await GetExternalCreditHistory(request, token, cancellationToken);
            UpdateCreditStatuses(externalResponse);
            
            var currentCredits = externalResponse.Current;
            var allCredits = currentCredits.Concat(externalResponse.Canceled).ToArray();

            return CreateCreditHistoryDto(currentCredits, allCredits);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error retrieving credit history for document: {DocumentId}", request.DocumentId);
            throw new ApplicationException("An error occurred while retrieving credit history. Please try again later.", ex);
        }
    }

    private async Task<CreditHistory> GetExternalCreditHistory(AccountStatusRequest request, string token, CancellationToken cancellationToken)
    {
        return await _requestClient.GetAsync<AccountStatusRequest, CreditHistory>(
            _settings.HistoryServiceUrl,
            request,
            token,
            cancellationToken);
    }

    private static void UpdateCreditStatuses(CreditHistory history)
    {
        foreach (var credit in history.Current) credit.StatusCode = "Vigente";
        foreach (var credit in history.Canceled) credit.StatusCode = "Cancelado";
    }

    private static CreditHistoryDTO CreateCreditHistoryDto(ICollection<Credit> currentCredits, Credit[] allCredits)
    {
        var hasLatePayments = currentCredits.Any(x => x.DaysLate > 0);
        var lastCredit = allCredits.OrderByDescending(x => x.OpeningDate).Select(x => new LastCredit() {
                AccountNumber = x.AccountNumber,
                Balance = x.Balance,
                Status = x.StatusCode
            }).FirstOrDefault();
        return new()
        {
            StatusLate = hasLatePayments ? "Se encuentra en mora" : "No se encuentra en mora",
            TotalAmountLate = currentCredits.Sum(x => x.LateFee),
            TotalDaysLate = currentCredits.Sum(x => x.DaysLate),
            LastCredit = lastCredit,
            History = allCredits
        };
    }
}
