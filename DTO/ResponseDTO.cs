namespace BancoKata.DTO;

public class ResponseDTO
{
    public string EstadoMora { get; set; }
    public int DiasMora { get; set; }
    public decimal SaldoMora { get; set; }
    public CreditoDTO UltimoCredito { get; set; }
    public HistorialCreditoDTO[] Historial { get; set; }
    public CreditoDTO[] Cuentas { get; set; }
    public DataCreditoDTO[] DataCredito { get; set; }
    public HuellaDTO[] Huellas { get; set; }
}
