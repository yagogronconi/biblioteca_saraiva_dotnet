using BibliotecaApi.Data;
using BibliotecaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaApi.Repositories;

public class AutorRepository : IAutorRepository
{
    private readonly BibliotecaContext _context;
    public AutorRepository(BibliotecaContext context)
    {
        _context = context;
    }

    public async Task<List<Autor>> GetAllAsync()
    {
        return await _context.Autores
            .OrderBy(a => a.Nome)
            .ToListAsync();
    }

    public async Task<Autor?> GetByIdAsync(int id)
    {
        return await _context.Autores
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Autor> CreateAsync(Autor autor)
    {
        _context.Autores.Add(autor);
        await _context.SaveChangesAsync();
        return autor;
    }

    public async Task<Autor> UpdateAsync(Autor autor)
    {
        _context.Autores.Update(autor);
        await _context.SaveChangesAsync();
        return autor;
    }

    public async Task DeleteAsync(Autor autor)
    {
        _context.Autores.Remove(autor);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> HasLivrosAsync(int autorId)
    {
        return await _context.Livros.AnyAsync(l => l.AutorId ==autorId);
    }
}