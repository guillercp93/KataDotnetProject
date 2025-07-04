namespace BancoKata.DTO;

public class DataCreditoDTO
{
    public string NumeroObligacion { get; set; }
    public string TipoObligacion { get; set; } // Tarjeta, Crédito, Crédito Hipotecario, Cuenta de Ahorros
    public string Estado { get; set; }
    public decimal? Cupo { get; set; } // Solo para tarjetas
    public string Franquicia { get; set; } // Solo para tarjetas
}
