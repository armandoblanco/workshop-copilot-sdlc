namespace ContosoSegurosCR.Models;

/// <summary>
/// Tipos de póliza disponibles en Contoso Seguros CR.
/// </summary>
public enum TipoPoliza
{
    /// <summary>Seguro de salud con cobertura médica.</summary>
    Salud,

    /// <summary>Seguro de vida.</summary>
    Vida,

    /// <summary>Seguro de vehículos.</summary>
    Vehiculo
}

/// <summary>
/// Estados del ciclo de vida de una póliza.
/// </summary>
public enum EstadoPoliza
{
    /// <summary>Póliza vigente y con cobertura activa.</summary>
    Activa,

    /// <summary>Póliza suspendida temporalmente.</summary>
    Inactiva,

    /// <summary>Póliza cancelada permanentemente.</summary>
    Cancelada
}

/// <summary>
/// Representa una póliza de seguro contratada por un cliente.
/// </summary>
public class Poliza
{
    /// <summary>Identificador único de la póliza.</summary>
    public int Id { get; set; }

    /// <summary>Número de póliza visible para el cliente (ej: POL-2024-00001).</summary>
    public string NumeroPoliza { get; set; } = string.Empty;

    /// <summary>Identificador del cliente propietario de la póliza.</summary>
    public int ClienteId { get; set; }

    /// <summary>Tipo de seguro cubierto por esta póliza.</summary>
    public TipoPoliza Tipo { get; set; }

    /// <summary>Estado actual de la póliza.</summary>
    public EstadoPoliza Estado { get; set; }

    /// <summary>Monto máximo de cobertura en colones costarricenses.</summary>
    public decimal MontoMaximoCoberturaColones { get; set; }

    /// <summary>Prima mensual en colones costarricenses.</summary>
    public decimal PrimaMensualColones { get; set; }

    /// <summary>Fecha en que inicia la vigencia de la póliza (UTC).</summary>
    public DateTime FechaInicio { get; set; }

    /// <summary>Fecha en que vence la vigencia de la póliza (UTC).</summary>
    public DateTime FechaVencimiento { get; set; }

    /// <summary>Descripción adicional o notas de la póliza.</summary>
    public string? Notas { get; set; }
}
