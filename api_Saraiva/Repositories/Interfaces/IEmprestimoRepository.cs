using BibliotecaApi.Models;

namespace BibliotecaApi.Repositories.Interfaces;

public interface IEmprestimoRepository
{
    Task<List<Emprestimo>> ListarTodosAsync(bool somenteAbertos = false);
    Task<Emprestimo?> BuscarPorIdAsync(int id);
    Task<bool> LivroPossuiEmprestimoAbertoAsync(int livroId);
    Task<Emprestimo> AdicionarAsync(Emprestimo emprestimo);
    Task AtualizarAsync(Emprestimo emprestimo);
}