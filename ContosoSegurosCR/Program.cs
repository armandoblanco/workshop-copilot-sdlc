using ContosoSegurosCR.DTOs;
using ContosoSegurosCR.Services;

var builder = WebApplication.CreateBuilder(args);

// Registro de servicios
builder.Services.AddSingleton<ClienteServicio>();
builder.Services.AddSingleton<PolizaServicio>();
// TODO Módulo 4: Registrar ReclamacionServicio aquí

// Configuración de Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opciones =>
{
    opciones.SwaggerDoc("v1", new()
    {
        Title = "API de Contoso Seguros CR",
        Version = "v1",
        Description = "Sistema de gestión de clientes, pólizas y reclamaciones para Contoso Seguros CR. " +
                      "Workshop: GitHub Copilot SDLC E2E."
    });
});

var app = builder.Build();

// Middleware
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Contoso Seguros CR v1");
    c.RoutePrefix = "swagger";
});
app.UseStaticFiles();
app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

// ============================================================
// ENDPOINTS DE CLIENTES
// ============================================================
var clientes = app.MapGroup("/api/clientes").WithTags("Clientes");

clientes.MapGet("/", (ClienteServicio servicio) => Results.Ok(servicio.ObtenerTodos()))
    .WithName("ObtenerTodosLosClientes")
    .WithOpenApi()
    .Produces<IEnumerable<ContosoSegurosCR.Models.Cliente>>(StatusCodes.Status200OK);

clientes.MapGet("/{id:int}", (int id, ClienteServicio servicio) =>
{
    try
    {
        return Results.Ok(servicio.ObtenerPorId(id));
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound($"No existe un cliente con ID {id}.");
    }
})
.WithName("ObtenerClientePorId")
.WithOpenApi()
.Produces<ContosoSegurosCR.Models.Cliente>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

clientes.MapPost("/", (ClienteCrearDto dto, ClienteServicio servicio) =>
{
    try
    {
        var cliente = servicio.Crear(dto);
        return Results.Created($"/api/clientes/{cliente.Id}", cliente);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
})
.WithName("CrearCliente")
.WithOpenApi()
.Produces<ContosoSegurosCR.Models.Cliente>(StatusCodes.Status201Created)
.Produces(StatusCodes.Status400BadRequest);

clientes.MapPut("/{id:int}", (int id, ClienteActualizarDto dto, ClienteServicio servicio) =>
{
    try
    {
        return Results.Ok(servicio.Actualizar(id, dto));
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound($"No existe un cliente con ID {id}.");
    }
})
.WithName("ActualizarCliente")
.WithOpenApi()
.Produces<ContosoSegurosCR.Models.Cliente>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

clientes.MapDelete("/{id:int}", (int id, ClienteServicio servicio) =>
{
    try
    {
        servicio.Eliminar(id);
        return Results.NoContent();
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound($"No existe un cliente con ID {id}.");
    }
})
.WithName("EliminarCliente")
.WithOpenApi()
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound);

// ============================================================
// ENDPOINTS DE PÓLIZAS
// ============================================================
var polizas = app.MapGroup("/api/polizas").WithTags("Pólizas");

polizas.MapGet("/", (PolizaServicio servicio) => Results.Ok(servicio.ObtenerTodas()))
    .WithName("ObtenerTodasLasPolizas")
    .WithOpenApi()
    .Produces<IEnumerable<ContosoSegurosCR.Models.Poliza>>(StatusCodes.Status200OK);

polizas.MapGet("/cliente/{clienteId:int}", (int clienteId, PolizaServicio servicio) =>
    Results.Ok(servicio.ObtenerPorCliente(clienteId)))
    .WithName("ObtenerPolizasPorCliente")
    .WithOpenApi()
    .Produces<IEnumerable<ContosoSegurosCR.Models.Poliza>>(StatusCodes.Status200OK);

polizas.MapGet("/{id:int}", (int id, PolizaServicio servicio) =>
{
    try
    {
        return Results.Ok(servicio.ObtenerPorId(id));
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound($"No existe una póliza con ID {id}.");
    }
})
.WithName("ObtenerPolizaPorId")
.WithOpenApi()
.Produces<ContosoSegurosCR.Models.Poliza>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

polizas.MapPost("/", (PolizaCrearDto dto, PolizaServicio servicio) =>
{
    try
    {
        var poliza = servicio.Crear(dto);
        return Results.Created($"/api/polizas/{poliza.Id}", poliza);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
})
.WithName("CrearPoliza")
.WithOpenApi()
.Produces<ContosoSegurosCR.Models.Poliza>(StatusCodes.Status201Created)
.Produces(StatusCodes.Status400BadRequest);

polizas.MapPut("/{id:int}/estado", (int id, PolizaActualizarEstadoDto dto, PolizaServicio servicio) =>
{
    try
    {
        return Results.Ok(servicio.ActualizarEstado(id, dto));
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound($"No existe una póliza con ID {id}.");
    }
})
.WithName("ActualizarEstadoPoliza")
.WithOpenApi()
.Produces<ContosoSegurosCR.Models.Poliza>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

// ============================================================
// TODO Módulo 4: Endpoints de Reclamaciones
// Agrega el grupo /api/reclamaciones aquí siguiendo el mismo patrón
// ============================================================

app.Run();

// Necesario para que WebApplicationFactory funcione en los tests de integración
public partial class Program { }
