namespace Api.Dtos;

public class SolicitudCreateDto
{
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Prioridad { get; set; } = "Media";
}
