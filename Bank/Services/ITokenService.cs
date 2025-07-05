namespace Bank.Services;

/// <summary>
/// Defines the contract for a service that manages authentication tokens.
/// This service is responsible for obtaining and caching access tokens used for API authentication.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Asynchronously retrieves a valid access token.
    /// If a valid token is already cached, it returns the cached token.
    /// Otherwise, it requests a new token from the authentication service.
    /// </summary>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> that represents the asynchronous operation.
    /// The task result contains the access token as a string.
    /// </returns>
    /// <exception cref="OperationCanceledException">
    /// Thrown when the operation is canceled via the cancellation token.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the token cannot be obtained or is invalid.
    /// </exception>
    /// <example>
    /// <code>
    /// // Example of using GetAccessTokenAsync
    /// var token = await tokenService.GetAccessTokenAsync();
    /// // Use the token for authenticated requests
    /// </code>
    /// </example>
    Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default);
}
