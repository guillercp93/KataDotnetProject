namespace BancoKata.DTO;

public class CreditoDTO
{
    public string Numero { get; set; }
    public string Estado { get; set; }
    public decimal Saldo { get; set; }
    public DateTime? FechaApertura { get; set; }
}
