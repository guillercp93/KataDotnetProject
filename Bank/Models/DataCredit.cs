using System.Text.Json.Serialization;

namespace Bank.Models;

/// <summary>
/// Represents a comprehensive credit data record containing various types of financial accounts.
/// </summary>
public class DataCredit
{
    [JsonPropertyName("creditos")]
    public List<CreditAccount> Credits { get; set; } = new();

    [JsonPropertyName("hipotecarios")]
    public List<MortgageAccount> Mortgages { get; set; } = new();

    [JsonPropertyName("ahorros")]
    public List<SavingsAccount> Savings { get; set; } = new();

    [JsonPropertyName("tarjetas")]
    public List<CardAccount> Cards { get; set; } = new();

    [JsonPropertyName("huellas")]
    public List<Footprint> Footprints { get; set; } = new();
}

/// <summary>
/// Represents a credit account with basic account information and status.
/// </summary>
public class CreditAccount
{
    [JsonPropertyName("numero_de_cuenta")]
    public long AccountNumber { get; set; }

    [JsonPropertyName("entidad")]
    public string Entity { get; set; } = string.Empty;

    [JsonPropertyName("estado")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("comportamiento")]
    public string Behavior { get; set; } = string.Empty;

    [JsonPropertyName("tipo_credito")]
    public string CreditType { get; set; } = "Credito";
}

/// <summary>
/// Represents a mortgage account with property loan details.
/// </summary>
public class MortgageAccount
{
    [JsonPropertyName("nro_cuenta")]
    public long AccountNumber { get; set; }

    [JsonPropertyName("entidad")]
    public string Entity { get; set; } = string.Empty;

    [JsonPropertyName("estado")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("comportamiento")]
    public string Behavior { get; set; } = string.Empty;

    [JsonPropertyName("tipo_credito")]
    public string CreditType { get; set; } = "Credito Hipotecario";
}

/// <summary>
/// Represents a savings account with basic account information and opening date.
/// </summary>
public class SavingsAccount
{
    [JsonPropertyName("nro_cta")]
    public long AccountNumber { get; set; }

    [JsonPropertyName("entidad")]
    public string Entity { get; set; } = string.Empty;

    [JsonPropertyName("estado")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("fecha_apertura")]
    public DateTime OpeningDate { get; set; }

    [JsonPropertyName("tipo_credito")]
    public string CreditType { get; set; } = "Cuenta de Ahorro";
}

/// <summary>
/// Represents a credit card account with credit limit and franchise details.
/// </summary>
public class CardAccount
{
    [JsonPropertyName("numero_cuenta")]
    public long AccountNumber { get; set; }

    [JsonPropertyName("franquicia")]
    public string Franchise { get; set; } = string.Empty;

    [JsonPropertyName("cupo")]
    public decimal CreditLimit { get; set; }

    [JsonPropertyName("entidad")]
    public string Entity { get; set; } = string.Empty;

    [JsonPropertyName("estado")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("comportamiento")]
    public string Behavior { get; set; } = string.Empty;

    [JsonPropertyName("tipo_credito")]
    public string CreditType { get; set; } = "Tarjeta";
}

/// <summary>
/// Represents a credit inquiry footprint showing when and by whom a credit check was performed.
/// </summary>
public class Footprint
{
    [JsonPropertyName("entidad")]
    public string Entity { get; set; } = string.Empty;

    [JsonPropertyName("fecha")]
    public DateTime Date { get; set; }
}