using Bank.DTOs;

namespace Bank.Services;

public interface IHistoryService
{
    Task<CreditHistoryDTO> GetCreditHistoryAsync(
        AccountStatusRequest request,
        string token = "",
        CancellationToken cancellationToken = default
    );
}