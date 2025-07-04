namespace BancoKata.DTO;

public class HistorialCreditoDTO
{
    public string NumeroProducto { get; set; }
    public DateTime FechaApertura { get; set; }
    public string EstadoActual { get; set; } // "Vigente" o "Cancelado"
    public decimal Saldo { get; set; }
    public decimal Mora { get; set; }
    public int DiasEnMora { get; set; }
    public decimal Capital { get; set; }
}
