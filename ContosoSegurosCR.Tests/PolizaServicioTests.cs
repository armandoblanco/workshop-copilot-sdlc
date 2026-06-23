using ContosoSegurosCR.DTOs;
using ContosoSegurosCR.Models;
using ContosoSegurosCR.Services;
using Xunit;

namespace ContosoSegurosCR.Tests;

/// <summary>
/// Pruebas unitarias para PolizaServicio.
/// Este archivo es la referencia de patrón para los tests de ReclamacionServicio
/// que los participantes crearán en el Módulo 5.
/// </summary>
public class PolizaServicioTests
{
    private PolizaServicio CrearServicio() => new PolizaServicio();

    [Fact]
    public void ObtenerTodas_DebeRetornarDatosDePruebaPrecargados()
    {
        // Arrange
        var servicio = CrearServicio();

        // Act
        var resultado = servicio.ObtenerTodas();

        // Assert
        Assert.NotEmpty(resultado);
        Assert.Equal(5, resultado.Count());
    }

    [Fact]
    public void ObtenerPorId_CuandoExiste_DebeRetornarPolizaCorrecta()
    {
        // Arrange
        var servicio = CrearServicio();

        // Act
        var poliza = servicio.ObtenerPorId(1);

        // Assert
        Assert.NotNull(poliza);
        Assert.Equal("POL-2023-00001", poliza.NumeroPoliza);
        Assert.Equal(EstadoPoliza.Activa, poliza.Estado);
    }

    [Fact]
    public void ObtenerPorId_CuandoNoExiste_DebeLanzarKeyNotFoundException()
    {
        // Arrange
        var servicio = CrearServicio();

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => servicio.ObtenerPorId(9999));
    }

    [Fact]
    public void ObtenerPorCliente_DebeRetornarSoloPolizasDelCliente()
    {
        // Arrange
        var servicio = CrearServicio();

        // Act — cliente 1 tiene 2 pólizas en los datos de ejemplo
        var polizasCliente1 = servicio.ObtenerPorCliente(1).ToList();

        // Assert
        Assert.All(polizasCliente1, p => Assert.Equal(1, p.ClienteId));
        Assert.Equal(2, polizasCliente1.Count);
    }

    [Fact]
    public void Crear_CuandoDatosValidos_DebeRetornarPolizaConIdYNumeroAsignados()
    {
        // Arrange
        var servicio = CrearServicio();
        var dto = new PolizaCrearDto(
            ClienteId: 1,
            Tipo: TipoPoliza.Salud,
            MontoMaximoCoberturaColones: 12_000_000m,
            PrimaMensualColones: 50_000m,
            FechaInicio: DateTime.UtcNow,
            FechaVencimiento: DateTime.UtcNow.AddYears(2),
            Notas: null
        );

        // Act
        var poliza = servicio.Crear(dto);

        // Assert
        Assert.True(poliza.Id > 0);
        Assert.False(string.IsNullOrWhiteSpace(poliza.NumeroPoliza));
        Assert.Equal(EstadoPoliza.Activa, poliza.Estado);
        Assert.Equal(12_000_000m, poliza.MontoMaximoCoberturaColones);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-500000)]
    public void Crear_CuandoMontoEsCeroONegativo_DebeLanzarArgumentException(decimal montoInvalido)
    {
        // Arrange
        var servicio = CrearServicio();
        var dto = new PolizaCrearDto(
            ClienteId: 1,
            Tipo: TipoPoliza.Vehiculo,
            MontoMaximoCoberturaColones: montoInvalido,
            PrimaMensualColones: 20_000m,
            FechaInicio: DateTime.UtcNow,
            FechaVencimiento: DateTime.UtcNow.AddYears(1),
            Notas: null
        );

        // Act & Assert
        Assert.Throws<ArgumentException>(() => servicio.Crear(dto));
    }

    [Fact]
    public void Crear_CuandoFechaVencimientoEsAnteriorAInicio_DebeLanzarArgumentException()
    {
        // Arrange
        var servicio = CrearServicio();
        var dto = new PolizaCrearDto(
            ClienteId: 1,
            Tipo: TipoPoliza.Vida,
            MontoMaximoCoberturaColones: 20_000_000m,
            PrimaMensualColones: 55_000m,
            FechaInicio: DateTime.UtcNow,
            FechaVencimiento: DateTime.UtcNow.AddDays(-1), // fecha inválida
            Notas: null
        );

        // Act & Assert
        Assert.Throws<ArgumentException>(() => servicio.Crear(dto));
    }

    [Fact]
    public void ActualizarEstado_CuandoPolizaExiste_DebeActualizarEstadoCorrectamente()
    {
        // Arrange
        var servicio = CrearServicio();
        var dto = new PolizaActualizarEstadoDto(EstadoPoliza.Inactiva);

        // Act
        var polizaActualizada = servicio.ActualizarEstado(1, dto);

        // Assert
        Assert.Equal(EstadoPoliza.Inactiva, polizaActualizada.Estado);
    }

    [Fact]
    public void ActualizarEstado_CuandoPolizaNoExiste_DebeLanzarKeyNotFoundException()
    {
        // Arrange
        var servicio = CrearServicio();
        var dto = new PolizaActualizarEstadoDto(EstadoPoliza.Cancelada);

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => servicio.ActualizarEstado(9999, dto));
    }
}
