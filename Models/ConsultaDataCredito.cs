using System.Text.Json.Serialization;
namespace BancoKata.Models;

public class ConsultaDataCredito
{
    [JsonPropertyName("creditos")]
    public DataCredito[] Creditos { get; set; }
    [JsonPropertyName("hipotecarios")]
    public DataHipotecario[] Hipotecarios { get; set; }
    [JsonPropertyName("ahorros")]
    public DataAhorro[] Ahorros { get; set; }
    [JsonPropertyName("tarjetas")]
    public DataTarjeta[] Tarjetas { get; set; }
    [JsonPropertyName("huellas")]
    public DataHuella[] Huellas { get; set; }
}

public class DataCredito
{
    [JsonPropertyName("numero_de_cuenta")]
    public int NumeroDeCuenta { get; set; }
    [JsonPropertyName("entidad")]
    public string Entidad { get; set; }
    [JsonPropertyName("estado")]
    public string Estado { get; set; }
    [JsonPropertyName("comportamiento")]
    public string Comportamiento { get; set; }
}

public class DataHipotecario
{
    [JsonPropertyName("nro_cuenta")]
    public int NroCuenta { get; set; }
    [JsonPropertyName("entidad")]
    public string Entidad { get; set; }
    [JsonPropertyName("estado")]
    public string Estado { get; set; }
    [JsonPropertyName("comportamiento")]
    public string Comportamiento { get; set; }
}

public class DataAhorro
{
    [JsonPropertyName("nro_cta")]
    public int NroCta { get; set; }
    [JsonPropertyName("entidad")]
    public string Entidad { get; set; }
    [JsonPropertyName("estado")]
    public string Estado { get; set; }
    [JsonPropertyName("fecha_apertura")]
    public DateTime FechaApertura { get; set; }
}

public class DataTarjeta
{
    [JsonPropertyName("numero_cuenta")]
    public int NumeroCuenta { get; set; }
    [JsonPropertyName("franquicia")]
    public string Franquicia { get; set; }
    [JsonPropertyName("cupo")]
    public int Cupo { get; set; }
    [JsonPropertyName("entidad")]
    public string Entidad { get; set; }
    [JsonPropertyName("estado")]
    public string Estado { get; set; }
    [JsonPropertyName("comportamiento")]
    public string Comportamiento { get; set; }
}

public class DataHuella
{
    [JsonPropertyName("entidad")]
    public string Entidad { get; set; }
    [JsonPropertyName("fecha")]
    public DateTime Fecha { get; set; }
}
