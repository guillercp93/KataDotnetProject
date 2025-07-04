using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BancoKata.DTO;
using BancoKata.Models;
using BancoKata.Services;

namespace BancoKata.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserClient : ControllerBase
    {
        private readonly TokenService _services;
        private readonly ILogger<UserClient> _logger;

        public UserClient(TokenService services, ILogger<UserClient> logger)
        {
            _services = services;
            _logger = logger;
        }

        [HttpPost("consultar")]
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
                // Consultar el token
                await _services.RequestTokenFromConfigAsync();
                // Consulta de historial crediticio
                var response = await _services.ConsultarHistorialCrediticioAsync(request);
                // Consulta de la cuentas
                var cuentaResponse = await _services.ConsultarCuentaAsync(request);
                // Consulta data crédito
                var dataCreditoResponse = await _services.ConsultarDataCreditoAsync(request);
                // formar la respuesta
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
