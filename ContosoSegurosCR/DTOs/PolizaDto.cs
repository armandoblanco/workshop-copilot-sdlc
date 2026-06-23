using ContosoSegurosCR.Models;

namespace ContosoSegurosCR.DTOs;

/// <summary>
/// DTO para crear una nueva póliza.
/// </summary>
public record PolizaCrearDto(
    int ClienteId,
    TipoPoliza Tipo,
    decimal MontoMaximoCoberturaColones,
    decimal PrimaMensualColones,
    DateTime FechaInicio,
    DateTime FechaVencimiento,
    string? Notas
);

/// <summary>
/// DTO para actualizar el estado de una póliza.
/// </summary>
public record PolizaActualizarEstadoDto(
    EstadoPoliza NuevoEstado
);
