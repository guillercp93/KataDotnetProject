using System.Text.Json.Serialization;

namespace BancoKata.Models;

public class ConsultaCuenta
{
    [JsonPropertyName("numero_cuenta")]
    public int NumeroCuenta { get; set; }
    [JsonPropertyName("saldo")]
    public int Saldo { get; set; }
    [JsonPropertyName("fecha_apertura")]
    public DateTime FechaApertura { get; set; }
    [JsonPropertyName("estado")]
    public string Estado { get; set; }
}
