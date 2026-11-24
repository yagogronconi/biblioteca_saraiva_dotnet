using BibliotecaApi.Models;

namespace BibliotecaApi.Repositories.Interfaces;

public interface ILivroRepository
{
    Task<List<Livro>> ListarTodosAsync(int? autorId = null);
    Task<Livro?> BuscarPorIdAsync(int id);
    Task<bool> IsbnExisteAsync(string isbn, int? ignoreLivroId = null);
    Task<bool> PossuiEmprestimosAsync(int livroId);
    Task<Livro> AdicionarAsync(Livro livro);
    Task<Livro> AtualizarAsync(Livro livro);
    Task RemoverAsync(Livro livro);
}