namespace ContosoSegurosCR.DTOs;

/// <summary>
/// DTO para crear un nuevo cliente.
/// </summary>
public record ClienteCrearDto(
    string Nombre,
    string Cedula,
    string Email,
    string Telefono,
    string Provincia,
    DateTime FechaNacimiento
);

/// <summary>
/// DTO para actualizar los datos de un cliente existente.
/// </summary>
public record ClienteActualizarDto(
    string Nombre,
    string Email,
    string Telefono,
    string Provincia
);
