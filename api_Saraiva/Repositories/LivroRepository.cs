using  BibliotecaApi.Data;
using BibliotecaApi.Models;
using BibliotecaApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaApi.Repositories;


public class LivroRepository : ILivroRepository
{
    private readonly BibliotecaContext _context;

    public LivroRepository(BibliotecaContext context)
    {
        _context = context;
    }

    public async Task<List<Livro>> ListarTodosAsync(int? autorId = null)
    {
        var query = _context.Livros
            .Include(l => l.Autor)
            .AsQueryable();
        
        if (autorId.HasValue)
        {
            query = query.Where(l => l.AutorId == autorId.Value);
        }

        return await query
            .OrderBy(l => l.Titulo)
            .ToListAsync();
    }

    public async Task<Livro?> BuscarPorIdAsync(int id)
    {
        return await _context.Livros
            .Include(l => l.Autor)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<bool> BuscarPorIsbnAsync(string isbn)
    {
        return await _context.Livros.AnyAsync(l => l.Isbn == isbn);
    }

    public async Task<bool> PossuiEmprestimosAsync(int livroId)
    {
        return await _context.Emprestimos.AnyAsync(e => e.LivroId == livroId);
    }

    public async Task<Livro> AdicionarAsync(Livro livro)
    {
        _context.Livros.Add(livro);
        await _context.SaveChangesAsync();
        return (await BuscarPorIdAsync(livro.Id))!;
    }

    public async Task<Livro> AtualizarAsync(Livro livro)
    {
        _context.Livros.Update(livro);
        await _context.SaveChangesAsync();
        return (await BuscarPorIdAsync(livro.Id))!;
    }

    public async Task RemoverAsync(Livro livro)
    {
        _context.Livros.Remove(livro);
        await _context.SaveChangesAsync();
    }
}