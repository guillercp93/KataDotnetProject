using System.Text.Json.Serialization;

namespace BancoKata.Models;

public class ConsultaHistorialCrediticio
{
    [JsonPropertyName("cancelados")]
    public Credito[] Cancelados { get; set; }
    [JsonPropertyName("vigentes")]
    public Credito[] Vigentes { get; set; }
}

public class Credito
{
    [JsonPropertyName("numero_cuenta")]
    public int NumeroCuenta { get; set; }
    [JsonPropertyName("saldo")]
    public decimal Saldo { get; set; }
    [JsonPropertyName("monto")]
    public decimal Monto { get; set; }
    [JsonPropertyName("mora")]
    public decimal Mora { get; set; }
    [JsonPropertyName("dias_mora")]
    public int DiasMora { get; set; }
    [JsonPropertyName("capital")]
    public decimal Capital { get; set; }
    [JsonPropertyName("fecha_cancelacion")]
    public DateTime? FechaCancelacion { get; set; }
    [JsonPropertyName("fecha_apertura")]
    public DateTime FechaApertura { get; set; }
    [JsonPropertyName("estado")]
    public string Estado { get; set; }
}
