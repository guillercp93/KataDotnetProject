namespace Bank.Models;

/// <summary>
/// Configuration settings for the AccountService.
/// </summary>
public class ServiceSettings
{
    public const string SectionName = "ServiceSettings";

    /// <summary>
    /// Gets or sets the base URL for the Account Service API endpoint.
    /// </summary>
    public string AccountServiceUrl { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the base URL for the Credit History Service API endpoint.
    /// </summary>
    public string HistoryServiceUrl { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the base URL for the Data Credit Service API endpoint.
    /// </summary>
    public string DataCreditServiceUrl { get; set; } = string.Empty;
}
