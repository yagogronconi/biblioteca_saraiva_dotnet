using BibliotecaApi.Models;

namespace BibliotecaApi.Repositories;

public interface IAutorRepository
{
    Task<List<Autor>> GetAllAsync();
    Task<Autor?> GetByIdAsync(int id);
    Task<Autor> CreateAsync(Autor autor);
    Task<Autor> UpdateAsync(Autor autor);
    Task DeleteAsync(Autor autor);
    Task<bool> HasLivrosAsync(int autorId);
}