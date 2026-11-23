using BibliotecaApi.Models;

namespace BibliotecaApi.Services.Interfaces;

public interface IAutorService
{
    Task<List<Autor>> ListarTodosAsync();
    Task<Autor> BuscarPorIdAsync(int id);
    Task<Autor> CriarAsync(Autor autor);
    Task<Autor> AtualizarAsync(int id, Autor autor);
    Task ExcluirAsync(int id);
}