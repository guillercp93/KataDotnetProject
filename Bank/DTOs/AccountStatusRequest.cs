using System.Text.Json.Serialization;

namespace Bank.DTOs;

public class AccountStatusRequest
{
    [JsonPropertyName("numeroDocumento")]
    public string DocumentId { get; set; } = string.Empty;

    [JsonPropertyName("tipoDocumento")]
    public string DocumentType { get; set; } = string.Empty;

    [JsonPropertyName("pais")]
    public string CountryId { get; set; } = string.Empty;
}
