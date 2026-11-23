using BibliotecaApi.Models;

namespace BibliotecaApi.Services.Interfaces;

public interface IEmprestimoService
{
    Task<List<Emprestimo>> ListarTodosAsync(bool somenteAbertos = false);
    Task<Emprestimo> BuscarPorIdAsync(int id);
    Task<Emprestimo> EmprestarAsync(Emprestimo emprestimo);
    Task<Emprestimo> DevolverAsync(int id);
}