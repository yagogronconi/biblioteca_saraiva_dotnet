using BibliotecaApi.Models;

namespace BibliotecaApi.Services.Interfaces;

public interface ILivroService
{
    Task<List<Livro>> ListarTodosAsync(int? autorId = null);
    Task<Livro> BuscarPorIdAsync(int id);
    Task<Livro> CriarAsync(Livro livro);
    Task<Livro> AtualizarAsync(int id, Livro livro);
    Task ExcluirAsync(int id);
}