using BibliotecaApi.Exceptions;
using BibliotecaApi.Models;
using BibliotecaApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaApi.Controllers;

[ApiController]
[Route("api/livros")]
public class LivrosController : ControllerBase
{
    private readonly ILivroService _livroService;

    public LivrosController(ILivroService livroService)
    {
        _livroService = livroService;
    }

    [HttpGet]
    public async Task<IActionResult> ListarTodos([FromQuery] int? autorId)
    {
        var livros = await _livroService.ListarTodosAsync(autorId);
        return Ok(livros);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        try
        {
            var livro = await _livroService.BuscarPorIdAsync(id);
            return Ok(livro);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] Livro livro)
    {
        try
        {
            var criado = await _livroService.CriarAsync(livro);
            return CreatedAtAction(nameof(BuscarPorId), new { id = criado.Id }, criado);
        }
        catch (BadRequestException ex)
        {
            return BadRequest(ex.Message);
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

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] Livro livro)
    {
        try
        {
            var atualizado = await _livroService.AtualizarAsync(id, livro);
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
        catch (ConflictException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Excluir(int id)
    {
        try
        {
            await _livroService.ExcluirAsync(id);
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