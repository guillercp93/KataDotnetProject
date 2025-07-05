using System.Text.Json.Serialization;
using Bank.Models;

namespace Bank.DTOs;

public class AccountStatusResponse
{
    [JsonPropertyName("estado_mora")]
    public string StatusLate { get; set; } = string.Empty;
        
    [JsonPropertyName("monto_mora")]
    public decimal TotalAmountLate { get; set; }
    
    [JsonPropertyName("dias_mora")]
    public int TotalDaysLate { get; set; }
    
    [JsonPropertyName("ultimo_credito")]
    public LastCredit? LastCredit { get; set; }
    
    [JsonPropertyName("historial")]
    public Credit[] History { get; set; } = [];

    [JsonPropertyName("Cuentas")]
    public AccountResponse[] Accounts { get; set; } = [];
    
    [JsonPropertyName("obligaciones_dataCredito")]
    public DataCreditDTO[] Credits { get; set; } = [];

    [JsonPropertyName("huellas")]
    public Footprint[] Footprints { get; set; } = [];
}

public class LastCredit
{
    [JsonPropertyName("numero_cuenta")]
    public int AccountNumber { get; set; }

    [JsonPropertyName("saldo")]
    public decimal Balance { get; set; }

    [JsonPropertyName("estado")]
    public string Status { get; set; } = string.Empty;
}