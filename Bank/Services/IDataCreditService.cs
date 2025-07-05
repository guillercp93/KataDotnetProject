using Bank.DTOs;

namespace Bank.Services;

/// <summary>
/// Defines the contract for a service that retrieves credit-related data.
/// </summary>
public interface IDataCreditService
{
    /// <summary>
    /// Asynchronously retrieves credit data for the specified account status request.
    /// </summary>
    /// <param name="request">The account status request containing the necessary parameters.</param>
    /// <param name="token">The authentication token for the request.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the data credit response.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="request"/> or <paramref name="token"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="token"/> is empty or whitespace.</exception>
    Task<DataCreditResponse> GetDataCreditAsync(AccountStatusRequest request, string token, CancellationToken cancellationToken = default);
}