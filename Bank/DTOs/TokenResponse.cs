using System.Text.Json.Serialization;

namespace Bank.DTOs;

public class TokenResponse
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = string.Empty;
    
    [JsonPropertyName("fecha_vencimiento")]
    public DateTime ExpiresIn { get; set; }
    
    [JsonPropertyName("detalles")]
    public required DetailToken Details { get; set; }
}

public class DetailToken {
    [JsonPropertyName("tipo_token")]
    public string TokenType { get; set; } = string.Empty;
    
    [JsonPropertyName("emisor")]
    public string Emiter { get; set; } = string.Empty;
    
    [JsonPropertyName("uso")]
    public string Usage { get; set; } = string.Empty;

    [JsonPropertyName("alcance")]
    public string[] Scope { get; set; } = Array.Empty<string>();
}
