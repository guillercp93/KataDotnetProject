using System.Text.Json.Serialization;

namespace Bank.DTOs;

/// <summary>
/// Represents the response containing account information.
/// </summary>
public class AccountResponse
{
    /// <summary>
    /// Gets or sets the account number.
    /// </summary>
    [JsonPropertyName("numero_cuenta")]
    public long AccountNumber { get; set; }

    /// <summary>
    /// Gets or sets the account balance.
    /// </summary>
    [JsonPropertyName("saldo")]
    public decimal Balance { get; set; }

    /// <summary>
    /// Gets or sets the account opening date.
    /// </summary>
    [JsonPropertyName("fecha_apertura")]
    public DateTime OpeningDate { get; set; }

    /// <summary>
    /// Gets or sets the account status.
    /// </summary>
    [JsonPropertyName("estado")]
    public string Status { get; set; } = string.Empty;
}
