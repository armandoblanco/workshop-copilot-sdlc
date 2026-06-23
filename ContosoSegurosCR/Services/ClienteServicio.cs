using ContosoSegurosCR.DTOs;
using ContosoSegurosCR.Models;

namespace ContosoSegurosCR.Services;

/// <summary>
/// Servicio para la gestión de clientes de Contoso Seguros CR.
/// Los datos se almacenan en memoria y se pierden al reiniciar la aplicación.
/// </summary>
public class ClienteServicio
{
    private readonly List<Cliente> _clientes;
    private int _siguienteId = 4;

    public ClienteServicio()
    {
        _clientes = new List<Cliente>
        {
            new Cliente
            {
                Id = 1,
                Nombre = "María Fernanda Rodríguez Quesada",
                Cedula = "104560789",
                Email = "mfernanda.rodriguez@gmail.com",
                Telefono = "8801-2345",
                Provincia = "San José",
                FechaNacimiento = new DateTime(1985, 3, 14, 0, 0, 0, DateTimeKind.Utc)
            },
            new Cliente
            {
                Id = 2,
                Nombre = "Carlos Alberto Mora Jiménez",
                Cedula = "205670123",
                Email = "camora@hotmail.com",
                Telefono = "7700-9876",
                Provincia = "Alajuela",
                FechaNacimiento = new DateTime(1978, 8, 22, 0, 0, 0, DateTimeKind.Utc)
            },
            new Cliente
            {
                Id = 3,
                Nombre = "Laura Sofía Vargas Méndez",
                Cedula = "302340567",
                Email = "lsofia.vargas@empresa.cr",
                Telefono = "6600-4321",
                Provincia = "Heredia",
                FechaNacimiento = new DateTime(1992, 11, 5, 0, 0, 0, DateTimeKind.Utc)
            }
        };
    }

    /// <summary>
    /// Obtiene todos los clientes registrados.
    /// </summary>
    /// <returns>Lista de todos los clientes.</returns>
    public IEnumerable<Cliente> ObtenerTodos() => _clientes.AsReadOnly();

    /// <summary>
    /// Obtiene un cliente por su identificador único.
    /// </summary>
    /// <param name="id">Identificador del cliente.</param>
    /// <returns>El cliente encontrado.</returns>
    /// <exception cref="KeyNotFoundException">Si no existe un cliente con ese ID.</exception>
    public Cliente ObtenerPorId(int id)
    {
        var cliente = _clientes.FirstOrDefault(c => c.Id == id);
        if (cliente is null)
            throw new KeyNotFoundException($"No existe un cliente con ID {id}.");
        return cliente;
    }

    /// <summary>
    /// Crea un nuevo cliente con los datos proporcionados.
    /// </summary>
    /// <param name="dto">Datos del cliente a crear.</param>
    /// <returns>El cliente creado con su ID asignado.</returns>
    /// <exception cref="ArgumentException">Si algún campo obligatorio está vacío.</exception>
    public Cliente Crear(ClienteCrearDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre))
            throw new ArgumentException("El nombre del cliente es obligatorio.");
        if (string.IsNullOrWhiteSpace(dto.Cedula))
            throw new ArgumentException("La cédula del cliente es obligatoria.");
        if (string.IsNullOrWhiteSpace(dto.Email))
            throw new ArgumentException("El correo electrónico del cliente es obligatorio.");

        var cliente = new Cliente
        {
            Id = _siguienteId++,
            Nombre = dto.Nombre,
            Cedula = dto.Cedula,
            Email = dto.Email,
            Telefono = dto.Telefono,
            Provincia = dto.Provincia,
            FechaNacimiento = dto.FechaNacimiento
        };

        _clientes.Add(cliente);
        return cliente;
    }

    /// <summary>
    /// Actualiza los datos de contacto de un cliente existente.
    /// </summary>
    /// <param name="id">Identificador del cliente a actualizar.</param>
    /// <param name="dto">Nuevos datos del cliente.</param>
    /// <returns>El cliente actualizado.</returns>
    /// <exception cref="KeyNotFoundException">Si no existe un cliente con ese ID.</exception>
    public Cliente Actualizar(int id, ClienteActualizarDto dto)
    {
        var cliente = ObtenerPorId(id);
        cliente.Nombre = dto.Nombre;
        cliente.Email = dto.Email;
        cliente.Telefono = dto.Telefono;
        cliente.Provincia = dto.Provincia;
        return cliente;
    }

    /// <summary>
    /// Elimina un cliente del sistema.
    /// </summary>
    /// <param name="id">Identificador del cliente a eliminar.</param>
    /// <exception cref="KeyNotFoundException">Si no existe un cliente con ese ID.</exception>
    public void Eliminar(int id)
    {
        var cliente = ObtenerPorId(id);
        _clientes.Remove(cliente);
    }
}
