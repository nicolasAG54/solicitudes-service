using Api.Models;

namespace Api.Data;

public static class DbInitializer
{
    public static void Seed(AppDbContext context)
    {
        if (context.Solicitudes.Any())
            return;

        var solicitudes = new List<Solicitud>
        {
            new()
            {
                Titulo = "Solicitud de acceso al sistema",
                Descripcion = "Acceso requerido para nuevo colaborador",
                Estado = "Pendiente",
                Prioridad = "Alta",
                FechaCreacion = DateTime.UtcNow.AddDays(-3)
            },
            new()
            {
                Titulo = "Error en módulo de reportes",
                Descripcion = "El reporte mensual no carga",
                Estado = "En Proceso",
                Prioridad = "Media",
                FechaCreacion = DateTime.UtcNow.AddDays(-2)
            },
            new()
            {
                Titulo = "Solicitud de equipo nuevo",
                Descripcion = "Laptop para nuevo ingreso",
                Estado = "Resuelta",
                Prioridad = "Alta",
                FechaCreacion = DateTime.UtcNow.AddDays(-5)
            },
            new()
            {
                Titulo = "Problema con credenciales",
                Descripcion = "Usuario no puede iniciar sesión",
                Estado = "Rechazada",
                Prioridad = "Baja",
                FechaCreacion = DateTime.UtcNow.AddDays(-1)
            }
        };

        context.Solicitudes.AddRange(solicitudes);
        context.SaveChanges();
    }
}
