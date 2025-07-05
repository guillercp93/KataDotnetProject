namespace Bank.DTOs;

public class AccountStatusRequest
{
    public string DocumentId { get; set; } = string.Empty;
    public string DocumentType { get; set; } = string.Empty;
    public string CountryId { get; set; } = string.Empty;
}
