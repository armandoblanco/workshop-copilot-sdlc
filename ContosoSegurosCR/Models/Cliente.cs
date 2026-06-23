namespace ContosoSegurosCR.Models;

/// <summary>
/// Representa un cliente asegurado de Contoso Seguros CR.
/// </summary>
public class Cliente
{
    /// <summary>Identificador único del cliente.</summary>
    public int Id { get; set; }

    /// <summary>Nombre completo del cliente.</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Número de cédula costarricense (sin guiones).</summary>
    public string Cedula { get; set; } = string.Empty;

    /// <summary>Correo electrónico de contacto.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Teléfono de contacto en formato costarricense.</summary>
    public string Telefono { get; set; } = string.Empty;

    /// <summary>Provincia de residencia.</summary>
    public string Provincia { get; set; } = string.Empty;

    /// <summary>Fecha de nacimiento del cliente.</summary>
    public DateTime FechaNacimiento { get; set; }
}
