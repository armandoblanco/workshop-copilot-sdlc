using ContosoSegurosCR.DTOs;
using ContosoSegurosCR.Models;

namespace ContosoSegurosCR.Services;

/// <summary>
/// Servicio para la gestión de pólizas de seguros de Contoso Seguros CR.
/// Los datos se almacenan en memoria y se pierden al reiniciar la aplicación.
/// </summary>
public class PolizaServicio
{
    private readonly List<Poliza> _polizas;
    private int _siguienteId = 6;

    public PolizaServicio()
    {
        _polizas = new List<Poliza>
        {
            new Poliza
            {
                Id = 1,
                NumeroPoliza = "POL-2023-00001",
                ClienteId = 1,
                Tipo = TipoPoliza.Salud,
                Estado = EstadoPoliza.Activa,
                MontoMaximoCoberturaColones = 10_000_000m,
                PrimaMensualColones = 45_000m,
                FechaInicio = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                FechaVencimiento = new DateTime(2025, 12, 31, 0, 0, 0, DateTimeKind.Utc),
                Notas = "Cobertura familiar incluida"
            },
            new Poliza
            {
                Id = 2,
                NumeroPoliza = "POL-2023-00002",
                ClienteId = 1,
                Tipo = TipoPoliza.Vehiculo,
                Estado = EstadoPoliza.Activa,
                MontoMaximoCoberturaColones = 5_000_000m,
                PrimaMensualColones = 28_000m,
                FechaInicio = new DateTime(2023, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                FechaVencimiento = new DateTime(2025, 5, 31, 0, 0, 0, DateTimeKind.Utc)
            },
            new Poliza
            {
                Id = 3,
                NumeroPoliza = "POL-2022-00015",
                ClienteId = 2,
                Tipo = TipoPoliza.Vida,
                Estado = EstadoPoliza.Activa,
                MontoMaximoCoberturaColones = 25_000_000m,
                PrimaMensualColones = 60_000m,
                FechaInicio = new DateTime(2022, 3, 15, 0, 0, 0, DateTimeKind.Utc),
                FechaVencimiento = new DateTime(2027, 3, 14, 0, 0, 0, DateTimeKind.Utc)
            },
            new Poliza
            {
                Id = 4,
                NumeroPoliza = "POL-2024-00008",
                ClienteId = 3,
                Tipo = TipoPoliza.Salud,
                Estado = EstadoPoliza.Activa,
                MontoMaximoCoberturaColones = 8_000_000m,
                PrimaMensualColones = 38_000m,
                FechaInicio = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                FechaVencimiento = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc)
            },
            new Poliza
            {
                Id = 5,
                NumeroPoliza = "POL-2021-00003",
                ClienteId = 2,
                Tipo = TipoPoliza.Vehiculo,
                Estado = EstadoPoliza.Inactiva,
                MontoMaximoCoberturaColones = 3_000_000m,
                PrimaMensualColones = 18_000m,
                FechaInicio = new DateTime(2021, 7, 1, 0, 0, 0, DateTimeKind.Utc),
                FechaVencimiento = new DateTime(2023, 6, 30, 0, 0, 0, DateTimeKind.Utc),
                Notas = "Póliza suspendida por mora en pagos"
            }
        };
    }

    /// <summary>
    /// Obtiene todas las pólizas registradas.
    /// </summary>
    /// <returns>Lista de todas las pólizas.</returns>
    public IEnumerable<Poliza> ObtenerTodas() => _polizas.AsReadOnly();

    /// <summary>
    /// Obtiene las pólizas de un cliente específico.
    /// </summary>
    /// <param name="clienteId">Identificador del cliente.</param>
    /// <returns>Lista de pólizas del cliente.</returns>
    public IEnumerable<Poliza> ObtenerPorCliente(int clienteId) =>
        _polizas.Where(p => p.ClienteId == clienteId).ToList();

    /// <summary>
    /// Obtiene una póliza por su identificador único.
    /// </summary>
    /// <param name="id">Identificador de la póliza.</param>
    /// <returns>La póliza encontrada.</returns>
    /// <exception cref="KeyNotFoundException">Si no existe una póliza con ese ID.</exception>
    public Poliza ObtenerPorId(int id)
    {
        var poliza = _polizas.FirstOrDefault(p => p.Id == id);
        if (poliza is null)
            throw new KeyNotFoundException($"No existe una póliza con ID {id}.");
        return poliza;
    }

    /// <summary>
    /// Crea una nueva póliza para un cliente.
    /// </summary>
    /// <param name="dto">Datos de la póliza a crear.</param>
    /// <returns>La póliza creada con su ID y número de póliza asignados.</returns>
    /// <exception cref="ArgumentException">Si el monto de cobertura o prima son inválidos.</exception>
    public Poliza Crear(PolizaCrearDto dto)
    {
        if (dto.MontoMaximoCoberturaColones <= 0)
            throw new ArgumentException("El monto máximo de cobertura debe ser mayor a cero.");
        if (dto.PrimaMensualColones <= 0)
            throw new ArgumentException("La prima mensual debe ser mayor a cero.");
        if (dto.FechaVencimiento <= dto.FechaInicio)
            throw new ArgumentException("La fecha de vencimiento debe ser posterior a la fecha de inicio.");

        var poliza = new Poliza
        {
            Id = _siguienteId,
            NumeroPoliza = $"POL-{DateTime.UtcNow.Year}-{_siguienteId:D5}",
            ClienteId = dto.ClienteId,
            Tipo = dto.Tipo,
            Estado = EstadoPoliza.Activa,
            MontoMaximoCoberturaColones = dto.MontoMaximoCoberturaColones,
            PrimaMensualColones = dto.PrimaMensualColones,
            FechaInicio = dto.FechaInicio,
            FechaVencimiento = dto.FechaVencimiento,
            Notas = dto.Notas
        };

        _siguienteId++;
        _polizas.Add(poliza);
        return poliza;
    }

    /// <summary>
    /// Actualiza el estado de una póliza existente.
    /// </summary>
    /// <param name="id">Identificador de la póliza.</param>
    /// <param name="dto">Nuevo estado a asignar.</param>
    /// <returns>La póliza con el estado actualizado.</returns>
    /// <exception cref="KeyNotFoundException">Si no existe una póliza con ese ID.</exception>
    public Poliza ActualizarEstado(int id, PolizaActualizarEstadoDto dto)
    {
        var poliza = ObtenerPorId(id);
        poliza.Estado = dto.NuevoEstado;
        return poliza;
    }
}
