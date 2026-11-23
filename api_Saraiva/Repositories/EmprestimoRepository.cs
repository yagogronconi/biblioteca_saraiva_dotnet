using BibliotecaApi.Data;
using BibliotecaApi.Models;
using BibliotecaApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaApi.Repositories;

public class EmprestimoRepository : IEmprestimoRepository
{
    private readonly BibliotecaContext _context;

    public EmprestimoRepository(BibliotecaContext context)
    {
        _context = context;
    }

    public async Task<List<Emprestimo>> ListarTodosAsync(bool somenteAbertos = false)
    {
        var query = _context.Emprestimos
            .Include(e => e.Livro)
                .ThenInclude(l => l!.Autor)
            .AsQueryable();

        if (somenteAbertos)
        {
            query = query.Where(e => e.DataDevolucao == null);
        }

        return await query
            .OrderByDescending(e => e.DataEmprestimo)
            .ToListAsync();
    }

    public async Task<Emprestimo?> BuscarPorIdAsync(int id)
    {
        return await _context.Emprestimos
            .Include(e => e.Livro)
                .ThenInclude(l => l!.Autor)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<bool> LivroPossuiEmprestimoAbertoAsync(int livroId)
    {
        return await _context.Emprestimos
            .AnyAsync(e => e.LivroId == livroId && e.DataDevolucao == null);
    }

    public async Task<Emprestimo> AdicionarAsync(Emprestimo emprestimo)
    {
        _context.Emprestimos.Add(emprestimo);
        await _context.SaveChangesAsync();
        return (await BuscarPorIdAsync(emprestimo.Id))!;
    }

    public async Task AtualizarAsync(Emprestimo emprestimo)
    {
        _context.Emprestimos.Update(emprestimo);
        await _context.SaveChangesAsync();
    }
}