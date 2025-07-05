using System.Text.Json.Serialization;
using Bank.Models;

namespace Bank.DTOs;

public class DataCreditResponse
{
    [JsonPropertyName("creditos")]
    public DataCreditDTO[] ListCredits { get; set; } = [];

    [JsonPropertyName("huellas")]
    public Footprint[] Footprints { get; set; } = [];
}

public class DataCreditDTO
{
    [JsonPropertyName("numero_obligation")]
    public long AccountNumber { get; set; }

    [JsonPropertyName("tipo_obligaction")]
    public string CreditType { get; set; } = string.Empty;

    [JsonPropertyName("estado")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("cupo")]
    public decimal CreditLimit { get; set; }

    [JsonPropertyName("franquicia")]
    public string Franchise { get; set; } = string.Empty;
}
