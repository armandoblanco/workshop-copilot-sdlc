using System.Net;
using System.Net.Http.Json;
using ContosoSegurosCR.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ContosoSegurosCR.Tests;

/// <summary>
/// Pruebas de integración para los endpoints de Pólizas.
/// Levanta la API completa en memoria con WebApplicationFactory.
/// Este archivo es la referencia de patrón para los tests de integración
/// de Reclamaciones que los participantes crearán en el Módulo 5.
/// </summary>
public class PolizaIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _cliente;

    public PolizaIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _cliente = factory.CreateClient();
    }

    [Fact]
    public async Task GET_Polizas_DebeRetornar200ConListaDeDatos()
    {
        // Act
        var respuesta = await _cliente.GetAsync("/api/polizas");

        // Assert
        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);
        var polizas = await respuesta.Content.ReadFromJsonAsync<List<Poliza>>();
        Assert.NotNull(polizas);
        Assert.NotEmpty(polizas);
    }

    [Fact]
    public async Task GET_PolizaPorId_CuandoExiste_DebeRetornar200()
    {
        // Act
        var respuesta = await _cliente.GetAsync("/api/polizas/1");

        // Assert
        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);
        var poliza = await respuesta.Content.ReadFromJsonAsync<Poliza>();
        Assert.NotNull(poliza);
        Assert.Equal(1, poliza.Id);
    }

    [Fact]
    public async Task GET_PolizaPorId_CuandoNoExiste_DebeRetornar404()
    {
        // Act
        var respuesta = await _cliente.GetAsync("/api/polizas/9999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, respuesta.StatusCode);
    }

    [Fact]
    public async Task POST_Poliza_CuandoDatosValidos_DebeRetornar201ConNuevaPoliza()
    {
        // Arrange
        var dto = new
        {
            ClienteId = 1,
            Tipo = 0, // TipoPoliza.Salud
            MontoMaximoCoberturaColones = 15000000,
            PrimaMensualColones = 55000,
            FechaInicio = DateTime.UtcNow,
            FechaVencimiento = DateTime.UtcNow.AddYears(2)
        };

        // Act
        var respuesta = await _cliente.PostAsJsonAsync("/api/polizas", dto);

        // Assert
        Assert.Equal(HttpStatusCode.Created, respuesta.StatusCode);
        Assert.NotNull(respuesta.Headers.Location);
        var polizaCreada = await respuesta.Content.ReadFromJsonAsync<Poliza>();
        Assert.NotNull(polizaCreada);
        Assert.True(polizaCreada.Id > 0);
    }

    [Fact]
    public async Task POST_Poliza_CuandoMontoEsCero_DebeRetornar400()
    {
        // Arrange
        var dto = new
        {
            ClienteId = 1,
            Tipo = 0,
            MontoMaximoCoberturaColones = 0, // inválido
            PrimaMensualColones = 30000,
            FechaInicio = DateTime.UtcNow,
            FechaVencimiento = DateTime.UtcNow.AddYears(1)
        };

        // Act
        var respuesta = await _cliente.PostAsJsonAsync("/api/polizas", dto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, respuesta.StatusCode);
    }

    [Fact]
    public async Task GET_PolizasPorCliente_DebeRetornarSoloPolizasDelCliente()
    {
        // Act
        var respuesta = await _cliente.GetAsync("/api/polizas/cliente/1");

        // Assert
        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);
        var polizas = await respuesta.Content.ReadFromJsonAsync<List<Poliza>>();
        Assert.NotNull(polizas);
        Assert.All(polizas, p => Assert.Equal(1, p.ClienteId));
    }
}
