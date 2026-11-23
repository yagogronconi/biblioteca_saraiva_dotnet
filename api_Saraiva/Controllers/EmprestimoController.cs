using BibliotecaApi.Exceptions;
using BibliotecaApi.Models;
using BibliotecaApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaApi.Controllers;

[ApiController]
[Route("api/emprestimos")]
public class EmprestimosController : ControllerBase
{
    private readonly IEmprestimoService _emprestimoService;

    public EmprestimosController(IEmprestimoService emprestimoService)
    {
        _emprestimoService = emprestimoService;
    }

    [HttpGet]
    public async Task<IActionResult> ListarTodos([FromQuery] bool somenteAbertos = false)
    {
        var emprestimos = await _emprestimoService.ListarTodosAsync(somenteAbertos);
        return Ok(emprestimos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        try
        {
            var emprestimo = await _emprestimoService.BuscarPorIdAsync(id);
            return Ok(emprestimo);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Emprestar([FromBody] Emprestimo emprestimo)
    {
        try
        {
            var criado = await _emprestimoService.EmprestarAsync(emprestimo);
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

    [HttpPut("{id:int}/devolucao")]
    public async Task<IActionResult> Devolver(int id)
    {
        try
        {
            var devolvido = await _emprestimoService.DevolverAsync(id);
            return Ok(devolvido);
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