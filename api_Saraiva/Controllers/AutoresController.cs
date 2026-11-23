using BibliotecaApi.Exceptions;
using BibliotecaApi.Models;
using BibliotecaApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaApi.Controllers;

[ApiController]
[Route("api/autores")]
public class AutoresController : ControllerBase
{
    private readonly IAutorService _autorService;

    public AutoresController(IAutorService autorService)
    {
        _autorService = autorService;
    }

    [HttpGet]
    public async Task<IActionResult> ListarTodos()
    {
        var autores = await _autorService.ListarTodosAsync();
        return Ok(autores);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        try
        {
            var autor = await _autorService.BuscarPorIdAsync(id);
            return Ok(autor);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] Autor autor)
    {
        try
        {
            var criado = await _autorService.CriarAsync(autor);
            return CreatedAtAction(nameof(BuscarPorId), new { id = criado.Id }, criado);
        }
        catch (BadRequestException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] Autor autor)
    {
        try
        {
            var atualizado = await _autorService.AtualizarAsync(id, autor);
            return Ok(atualizado);
        }
        catch (BadRequestException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Excluir(int id)
    {
        try
        {
            await _autorService.ExcluirAsync(id);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ConflictException ex)
        {
            return Conflict(ex.Message);
        }
    }
}