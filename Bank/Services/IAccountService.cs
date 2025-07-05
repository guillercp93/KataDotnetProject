using Bank.DTOs;

namespace Bank.Services;

/// <summary>
/// Provides operations for retrieving account status information.
/// </summary>
public interface IAccountService
{
    /// <summary>
    /// Retrieves the status of one or more accounts based on the provided request parameters.
    /// </summary>
    /// <param name="request">The account status request containing search criteria.</param>
    /// <param name="token">Authentication token for the request.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
    /// <returns>An array of account responses matching the request criteria.</returns>
    Task<AccountResponse[]> GetAccountStatusAsync(AccountStatusRequest request,
                                                string token,
                                                CancellationToken cancellationToken = default);
}
