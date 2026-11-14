namespace InventarioFarmaciaAPI.Models;

public static class Roles
{
    public const string Administrador = "Administrador";
    public const string Empleado = "Empleado";
}

public class Usuario
{
    public int Id { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string ContrasenaHash { get; set; } = string.Empty;
    public string Rol { get; set; } = Roles.Empleado;
    public bool Activo { get; set; } = true;
}
