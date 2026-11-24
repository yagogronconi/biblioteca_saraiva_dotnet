using BibliotecaApi.Exceptions;
using BibliotecaApi.Models;
using BibliotecaApi.Repositories.Interfaces;
using BibliotecaApi.Services.Interfaces;

namespace BibliotecaApi.Services;

public class EmprestimoService : IEmprestimoService
{
    private readonly IEmprestimoRepository _emprestimoRepository;
    private readonly ILivroRepository _livroRepository;

    public EmprestimoService(IEmprestimoRepository emprestimoRepository, ILivroRepository livroRepository)
    {
        _emprestimoRepository = emprestimoRepository;
        _livroRepository = livroRepository;
    }

    public async Task<List<Emprestimo>> ListarTodosAsync(bool somenteAbertos = false)
    {
        return await _emprestimoRepository.ListarTodosAsync(somenteAbertos);
    }

    public async Task<Emprestimo> BuscarPorIdAsync(int id)
    {
        var emprestimo = await _emprestimoRepository.BuscarPorIdAsync(id);
        if (emprestimo is null)
            throw new NotFoundException("Empréstimo não encontrado.");

        return emprestimo;
    }

    public async Task<Emprestimo> EmprestarAsync(Emprestimo emprestimo)
    {
        // Ordem obrigatória: 400 -> 404 -> 409
        if (emprestimo is null)
            throw new BadRequestException("Os dados do empréstimo são obrigatórios.");

        var nomeLeitor = emprestimo.NomeLeitor?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(nomeLeitor))
            throw new BadRequestException("O nome do leitor é obrigatório.");
        if (nomeLeitor.Length > 100)
            throw new BadRequestException("O nome do leitor deve ter no máximo 100 caracteres.");

        var livro = await _livroRepository.BuscarPorIdAsync(emprestimo.LivroId);
        if (livro is null)
            throw new NotFoundException("Livro não encontrado.");

        if (await _emprestimoRepository.LivroPossuiEmprestimoAbertoAsync(emprestimo.LivroId))
            throw new ConflictException("Livro já está emprestado.");

        var novoEmprestimo = new Emprestimo
        {
            LivroId = emprestimo.LivroId,
            NomeLeitor = nomeLeitor,
            DataEmprestimo = DateTime.Now,
            DataDevolucao = null
        };

        return await _emprestimoRepository.AdicionarAsync(novoEmprestimo);
    }

    public async Task<Emprestimo> DevolverAsync(int id)
    {
        var emprestimo = await _emprestimoRepository.BuscarPorIdAsync(id);
        if (emprestimo is null)
            throw new NotFoundException("Empréstimo não encontrado.");

        if (emprestimo.DataDevolucao is not null)
            throw new ConflictException("Este empréstimo já foi devolvido.");

        emprestimo.DataDevolucao = DateTime.Now;
        await _emprestimoRepository.AtualizarAsync(emprestimo);

        return (await _emprestimoRepository.BuscarPorIdAsync(id))!;
    }
}