using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BancoKata.DTO;
using BancoKata.Models;
using BancoKata.Services;

namespace BancoKata.Controller
{
    /// <summary>
    /// Controller for handling user client operations such as consulting account, credit history, and data credit.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserClient : ControllerBase
    {
        private readonly IService _services;
        private readonly ILogger<UserClient> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserClient"/> class.
        /// </summary>
        /// <param name="services">Service for token and data operations.</param>
        /// <param name="logger">Logger instance for logging information and errors.</param>
        public UserClient(IService services, ILogger<UserClient> logger)
        {
            _services = services;
            _logger = logger;
        }

        /// <summary>
        /// Processes a credit consultation request, including account, credit history, and data credit information.
        /// </summary>
        /// <param name="request">The consultation request containing user data.</param>
        /// <returns>Returns a response with credit history, account, and data credit information, or an error if the process fails.</returns>
        [HttpPost("consultar")]
        [ProducesResponseType(200, Type = typeof(ResponseDTO))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Consultar([FromBody] ConsultaRequest request)
        {
            // Lógica para procesar la consulta
            if (request == null)
            {
                _logger.LogWarning("ConsultaRequest is null.");
                return BadRequest("Información de consulta no válida.");
            }

            try
            {
                // Query access token from configuration
                await _services.RequestTokenFromConfigAsync();
                // Quyery credit history
                var response = await _services.ConsultarHistorialCrediticioAsync(request);
                // Query account information
                var cuentaResponse = await _services.ConsultarCuentaAsync(request);
                // Query of data credit
                var dataCreditoResponse = await _services.ConsultarDataCreditoAsync(request);
                // Rebuild the response
                response.Cuentas = cuentaResponse;
                response.DataCredito = dataCreditoResponse.DataCredito;
                response.Huellas = dataCreditoResponse.Huellas;

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar la consulta.");
                return BadRequest($"Error al procesar la consulta: {ex.Message}");
            }
        }
    }
}
