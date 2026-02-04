using Api.Data;
using Api.Models;
using Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
namespace Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SolicitudesController : ControllerBase
{
    private readonly AppDbContext _context;

    public SolicitudesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/solicitudes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Solicitud>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = _context.Solicitudes
            .OrderByDescending(s => s.FechaCreacion);

        var total = await query.CountAsync();

        var solicitudes = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new
        {
            total,
            page,
            pageSize,
            data = solicitudes
        });
    }

    // GET: api/solicitudes/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Solicitud>> GetById(int id)
    {
        var solicitud = await _context.Solicitudes.FindAsync(id);

        if (solicitud == null)
            return NotFound();

        return Ok(solicitud);
    }

    // POST: api/solicitudes
    [HttpPost]
    public async Task<ActionResult<Solicitud>> Create(SolicitudCreateDto dto)
    {
        var solicitud = new Solicitud
        {
            Titulo = dto.Titulo,
            Descripcion = dto.Descripcion,
            Prioridad = dto.Prioridad,
            Estado = "Pendiente",
            FechaCreacion = DateTime.UtcNow
        };

        _context.Solicitudes.Add(solicitud);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = solicitud.Id }, solicitud);
    }

    // PUT: api/solicitudes/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, SolicitudUpdateDto dto)
    {
        var solicitud = await _context.Solicitudes.FindAsync(id);

        if (solicitud == null)
            return NotFound();

        if (solicitud.Estado == "Resuelta")
            return BadRequest("No se puede editar una solicitud resuelta.");

        solicitud.Titulo = dto.Titulo;
        solicitud.Descripcion = dto.Descripcion;
        solicitud.Prioridad = dto.Prioridad;
        solicitud.FechaActualizacion = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // PATCH: api/solicitudes/{id}/estado
    [HttpPatch("{id}/estado")]
    public async Task<IActionResult> ChangeEstado(int id, SolicitudEstadoDto dto)
    {
        var solicitud = await _context.Solicitudes.FindAsync(id);

        if (solicitud == null)
            return NotFound();

        solicitud.Estado = dto.Estado;
        solicitud.FechaActualizacion = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/solicitudes/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var solicitud = await _context.Solicitudes.FindAsync(id);

        if (solicitud == null)
            return NotFound();

        _context.Solicitudes.Remove(solicitud);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
