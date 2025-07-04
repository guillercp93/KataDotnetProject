using System.Text.Json.Serialization;

namespace BancoKata.Models;

public class TokenResponse
{
    [JsonPropertyName("token")]
    public string Token { get; set; }
    [JsonPropertyName("fecha_vencimiento")]
    public DateTime FechaVencimiento { get; set; }
    [JsonPropertyName("detalles")]
    public TokenResponseDetails Detalles { get; set; }

}

public class TokenResponseDetails
{
    [JsonPropertyName("tipo_token")]
    public string TipoToken { get; set; }
    [JsonPropertyName("emisor")]
    public string Emisor { get; set; }
    [JsonPropertyName("uso")]
    public string Uso { get; set; }
    [JsonPropertyName("alcance")]
    public string[] Alcance { get; set; }
}
