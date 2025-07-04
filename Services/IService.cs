using BancoKata.DTO;
using BancoKata.Models;

namespace BancoKata.Services;

/// <summary>
/// Interface for service operations related to token, account, credit history, and data credit.
/// </summary>
public interface IService
{
    /// <summary>
    /// Requests a token using configuration values.
    /// </summary>
    Task RequestTokenFromConfigAsync();

    /// <summary>
    /// Consults account information asynchronously.
    /// </summary>
    /// <param name="request">The request data for the account consultation.</param>
    /// <returns>An array of CreditoDTO objects.</returns>
    Task<CreditoDTO[]> ConsultarCuentaAsync(ConsultaRequest request);

    /// <summary>
    /// Consults credit history asynchronously.
    /// </summary>
    /// <param name="request">The request data for the credit history consultation.</param>
    /// <returns>A ResponseDTO object with credit history information.</returns>
    Task<ResponseDTO> ConsultarHistorialCrediticioAsync(ConsultaRequest request);

    /// <summary>
    /// Consults data credit asynchronously.
    /// </summary>
    /// <param name="request">The request data for the data credit consultation.</param>
    /// <returns>A ResponseDTO object with data credit information.</returns>
    Task<ResponseDTO> ConsultarDataCreditoAsync(ConsultaRequest request);
}
