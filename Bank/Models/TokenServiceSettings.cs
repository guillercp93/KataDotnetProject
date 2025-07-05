namespace Bank.Models;

/// <summary>
/// Represents the configuration settings for the token service.
/// </summary>
public class TokenServiceSettings
{
    /// <summary>
    /// The configuration section name for token service settings.
    /// </summary>
    public const string SectionName = "TokenServiceSettings";
    
    /// <summary>
    /// Gets or sets the base URL of the token service endpoint.
    /// </summary>
    public string Url { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the username for token authentication.
    /// </summary>
    public string User { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the password for token authentication.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
