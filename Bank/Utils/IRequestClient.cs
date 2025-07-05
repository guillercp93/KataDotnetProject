/// <summary>
/// Defines a generic HTTP client interface for making REST API requests.
/// </summary>
public interface IRequestClient
{
    /// <summary>
    /// Sends a POST request to the specified URL with the provided request data.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request data.</typeparam>
    /// <typeparam name="TResponse">The expected response type.</typeparam>
    /// <param name="url">The relative or absolute URL to send the request to.</param>
    /// <param name="request">The request data to send.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the deserialized response.</returns>
    Task<TResponse> PostAsync<TRequest, TResponse>(
        string url,
        TRequest request,
        string token = "",
        CancellationToken cancellationToken = default)
        where TRequest : class
        where TResponse : class;

    /// <summary>
    /// Sends a GET request to the specified URL.
    /// </summary>
    /// <typeparam name="TResponse">The expected response type.</typeparam>
    /// <param name="url">The relative or absolute URL to send the request to.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the deserialized response.</returns>
    Task<TResponse> GetAsync<TRequest, TResponse>(
        string url,
        TRequest? request = null,
        string token = "",
        CancellationToken cancellationToken = default)
        where TRequest : class
        where TResponse : class;
}