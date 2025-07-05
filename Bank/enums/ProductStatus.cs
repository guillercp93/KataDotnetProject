namespace Bank.Enums;

/// <summary>
/// Represents the possible statuses of a financial product.
/// </summary>
public enum ProductStatus
{
    /// <summary>
    /// The product is open and active.
    /// </summary>
    Active = 1,
    
    /// <summary>
    /// The product has been cancelled.
    /// </summary>
    Cancelled = 2,
    
    /// <summary>
    /// The product is frozen.
    /// </summary>
    Frozen = 3,
    
    /// <summary>
    /// The product has been refinanced.
    /// </summary>
    Refinanced = 4,
    
    /// <summary>
    /// The product has been written off as bad debt.
    /// </summary>
    ChargedOff = 5,
    
    /// <summary>
    /// The product is under legal collection process.
    /// </summary>
    InLegalCollection = 6,
    
    /// <summary>
    /// The product is blocked.
    /// </summary>
    Blocked = 7
}