using System.Text.Json.Serialization;
using Bank.Enums;

namespace Bank.Models;

/// <summary>
/// Represents a credit history with its details and status.
/// </summary>
public class CreditHistory
{
    [JsonPropertyName("cancelados")]
    public Credit[] Canceled { get; set; } = Array.Empty<Credit>();

    [JsonPropertyName("vigentes")]
    public Credit[] Current { get; set; } = Array.Empty<Credit>();
}

/// <summary>
/// Represents a credit account with its details and status.
/// </summary>
public class Credit
{
    [JsonPropertyName("numero_cuenta")]
    public int AccountNumber { get; set; }

    [JsonPropertyName("saldo")]
    public decimal Balance { get; set; }

    [JsonIgnore]
    [JsonPropertyName("monto")]
    public decimal Amount { get; set; }

    [JsonPropertyName("mora")]
    public decimal LateFee { get; set; }

    [JsonPropertyName("dias_mora")]
    public int DaysLate { get; set; }

    [JsonPropertyName("capital")]
    public decimal Principal { get; set; }

    [JsonIgnore]
    [JsonPropertyName("fecha_cancelacion")]
    public DateOnly? CancellationDate { get; set; }

    [JsonPropertyName("fecha_apertura")]
    public  DateOnly OpeningDate { get; set; }

    [JsonPropertyName("estado")]
    public string StatusCode { get; set; } = string.Empty;

    [JsonIgnore]
    public ProductStatus Status => Enum.TryParse(StatusCode, out ProductStatus status)
        ? status
        : throw new InvalidOperationException($"Invalid status code: {StatusCode}");
    
}