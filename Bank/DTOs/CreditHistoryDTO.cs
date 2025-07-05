using System.Text.Json.Serialization;
using Bank.Models;

namespace Bank.DTOs;

public class CreditHistoryDTO
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
}