using System.Text.Json;
using BancoKata.DTO;
using BancoKata.Models;
using BancoKata.Utils;

namespace BancoKata.Services;

public class Service : IService
{
    private readonly IConfiguration _configuration;

    public Service(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task RequestTokenFromConfigAsync()
    {
        var urlToken = _configuration["DataRequest:UrlToken"];
        var usuario = _configuration["DataRequest:Usuario"];
        var contrasena = _configuration["DataRequest:Contrasena"];

        var credentials = new
        {
            username = usuario,
            password = contrasena
        };

        await ClientRequest.RequestTokenAsync(urlToken, credentials);
    }

    public async Task<CreditoDTO[]> ConsultarCuentaAsync(ConsultaRequest request)
    {
        string urlConsultaCta = _configuration["DataRequest:UrlConsultaCta"];
        ConsultaCuenta[] response = await ClientRequest.PostAsync<ConsultaCuenta[]>(urlConsultaCta, request);

        if (response == null || response.Length == 0)
        {
            throw new Exception("No se encontraron cuentas para la consulta.");
        }

        return response.Select(c => new CreditoDTO
        {
            Numero = c.NumeroCuenta.ToString(),
            Estado = c.Estado,
            Saldo = c.Saldo,
            FechaApertura = c.FechaApertura
        }).ToArray();
    }

    public async Task<ResponseDTO> ConsultarHistorialCrediticioAsync(ConsultaRequest request)
    {
        string urlConsultaHistCred = _configuration["DataRequest:UrlConsultaHistCred"];
        ConsultaHistorialCrediticio externalResponse = await ClientRequest.GetAsync<ConsultaHistorialCrediticio>(urlConsultaHistCred, request);

        if (externalResponse == null)
        {
            throw new Exception("No se encontró historial crediticio para la consulta.");
        }

        bool esMora = externalResponse.Vigentes.Any(c => c.DiasMora > 0);
        decimal totalMora = externalResponse.Vigentes.Sum(c => c.Mora);
        int totalDiasMora = externalResponse.Vigentes.Sum(c => c.DiasMora);
        Credito ultimoCredito = externalResponse.Vigentes.OrderByDescending(c => c.FechaApertura).FirstOrDefault(); //debo obtener numero,estado, saldo

        // Cambiar el campo Estado para los créditos en Vigentes y Cancelados
        foreach (var credito in externalResponse.Vigentes)
        {
            credito.Estado = "Vigente";
        }
        foreach (var credito in externalResponse.Cancelados)
        {
            credito.Estado = "Cancelado";
        }

        Credito[] totalCreditos = externalResponse.Vigentes.Concat(externalResponse.Cancelados).ToArray();

        var response = new ResponseDTO
        {
            EstadoMora = esMora ? "Se encuentra en mora" : "No se encuentra en mora",
            SaldoMora = totalMora,
            DiasMora = totalDiasMora,
            UltimoCredito = ultimoCredito != null ? new CreditoDTO
            {
                Numero = ultimoCredito.NumeroCuenta.ToString(),
                Estado = ultimoCredito.Estado,
                Saldo = ultimoCredito.Saldo,
                FechaApertura = ultimoCredito.FechaApertura
            } : null,
            Historial = totalCreditos.Select(c => new HistorialCreditoDTO
            {
                NumeroProducto = c.NumeroCuenta.ToString(),
                FechaApertura = c.FechaApertura,
                EstadoActual = c.Estado,
                Saldo = c.Saldo,
                Mora = c.Mora,
                DiasEnMora = c.DiasMora,
                Capital = c.Capital
            }).ToArray(),
            Cuentas = [],
            Huellas = [],
            DataCredito = []
        };
        Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(response));
        return response;
    }

    public async Task<ResponseDTO> ConsultarDataCreditoAsync(ConsultaRequest request)
    {
        string urlConsultaDataCredito = _configuration["DataRequest:UrlDataCredito"];
        ConsultaDataCredito externalResponse = await ClientRequest.PostAsync<ConsultaDataCredito>(urlConsultaDataCredito, request);

        if (externalResponse == null)
        {
            throw new Exception("No se encontraron datos de crédito para la consulta.");
        }


        // Transformar la respuesta a DataCreditoDTO
        var creditos = externalResponse.Creditos.Select(c => new DataCreditoDTO
        {
            NumeroObligacion = c.NumeroDeCuenta.ToString(),
            TipoObligacion = "Crédito",
            Estado = c.Estado,
            Cupo = null,
            Franquicia = ""
        }).ToArray();

        var hipotecas = externalResponse.Hipotecarios.Select(h => new DataCreditoDTO
        {
            NumeroObligacion = h.NroCuenta.ToString(),
            TipoObligacion = "Crédito Hipotecario",
            Estado = h.Estado,
            Cupo = null,
            Franquicia = ""
        }).ToArray();

        var ahorros = externalResponse.Ahorros.Select(a => new DataCreditoDTO
        {
            NumeroObligacion = a.NroCta.ToString(),
            TipoObligacion = "Cuenta de Ahorros",
            Estado = a.Estado,
            Cupo = null,
            Franquicia = ""
        }).ToArray();

        var tarjetas = externalResponse.Tarjetas.Select(t => new DataCreditoDTO
        {
            NumeroObligacion = t.NumeroCuenta.ToString(),
            TipoObligacion = "Tarjeta",
            Estado = t.Estado,
            Cupo = t.Cupo,
            Franquicia = t.Franquicia
        }).ToArray();

        DataCreditoDTO[] dataCreditos = creditos
            .Concat(hipotecas)
            .Concat(ahorros)
            .Concat(tarjetas)
            .ToArray();

        ResponseDTO response = new ResponseDTO
        {
            DataCredito = dataCreditos,
            Huellas = externalResponse.Huellas.Select(h => new HuellaDTO
            {
                EntidadConsulta = h.Entidad,
                FechaConsulta = h.Fecha
            }).ToArray(),
        };

        return response;

    }
}
