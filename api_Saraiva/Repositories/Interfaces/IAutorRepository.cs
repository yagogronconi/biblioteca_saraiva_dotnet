using BibliotecaApi.Models;

namespace BibliotecaApi.Repositories.Interfaces;

public interface IAutorRepository
{
    Task<List<Autor>> ListarTodosAsync();
    Task<Autor?> BuscarPorIdAsync(int id);
    Task<bool> PossuiLivrosAsync(int autorId);
    Task<Autor> AdicionarAsync(Autor autor);
    Task AtualizarAsync(Autor autor);
    Task RemoverAsync(Autor autor);
}