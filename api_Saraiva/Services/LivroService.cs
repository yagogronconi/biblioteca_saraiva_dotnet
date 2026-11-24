using BibliotecaApi.Exceptions;
using BibliotecaApi.Models;
using BibliotecaApi.Repositories.Interfaces;
using BibliotecaApi.Services.Interfaces;

namespace BibliotecaApi.Services;

public class LivroService : ILivroService
{
    private readonly ILivroRepository _livroRepository;
    private readonly IAutorRepository _autorRepository;

    public LivroService(ILivroRepository livroRepository, IAutorRepository autorRepository)
    {
        _livroRepository = livroRepository;
        _autorRepository = autorRepository;
    }

    public async Task<List<Livro>> ListarTodosAsync(int? autorId = null)
    {
        // RN12: se autorId não existir, retorna lista vazia (200 OK)
        return await _livroRepository.ListarTodosAsync(autorId);
    }

    public async Task<Livro> BuscarPorIdAsync(int id)
    {
        var livro = await _livroRepository.BuscarPorIdAsync(id);
        if (livro is null)
            throw new NotFoundException("Livro não encontrado.");

        return livro;
    }

    public async Task<Livro> CriarAsync(Livro livro)
    {
        // Ordem obrigatória: 400 -> 404 -> 409
        ValidarLivro(livro);

        var autor = await _autorRepository.BuscarPorIdAsync(livro.AutorId);
        if (autor is null)
            throw new NotFoundException("Autor não encontrado.");

        var isbnLimpo = livro.Isbn.Trim();
        var livroComMesmoIsbn = await _livroRepository.BuscarPorIsbnAsync(isbnLimpo);
        if (livroComMesmoIsbn is not null)
            throw new ConflictException("Já existe um livro cadastrado com este ISBN.");

        var novoLivro = new Livro
        {
            Titulo = livro.Titulo.Trim(),
            Isbn = isbnLimpo,
            AnoPublicacao = livro.AnoPublicacao,
            AutorId = livro.AutorId
        };

        return await _livroRepository.AdicionarAsync(novoLivro);
    }

    public async Task<Livro> AtualizarAsync(int id, Livro livro)
    {
        // Ordem obrigatória: 400 -> 404 -> 409
        ValidarLivro(livro);

        var existente = await _livroRepository.BuscarPorIdAsync(id);
        if (existente is null)
            throw new NotFoundException("Livro não encontrado.");

        var autor = await _autorRepository.BuscarPorIdAsync(livro.AutorId);
        if (autor is null)
            throw new NotFoundException("Autor não encontrado.");

        var isbnLimpo = livro.Isbn.Trim();
        var livroComMesmoIsbn = await _livroRepository.BuscarPorIsbnAsync(isbnLimpo);
        if (livroComMesmoIsbn is not null && livroComMesmoIsbn.Id != id)
            throw new ConflictException("Já existe outro livro cadastrado com este ISBN.");

        existente.Titulo = livro.Titulo.Trim();
        existente.Isbn = isbnLimpo;
        existente.AnoPublicacao = livro.AnoPublicacao;
        existente.AutorId = livro.AutorId;

        await _livroRepository.AtualizarAsync(existente);
        return (await _livroRepository.BuscarPorIdAsync(id))!;
    }

    public async Task ExcluirAsync(int id)
    {
        var existente = await _livroRepository.BuscarPorIdAsync(id);
        if (existente is null)
            throw new NotFoundException("Livro não encontrado.");

        if (await _livroRepository.PossuiEmprestimosAsync(id))
            throw new ConflictException("Não é possível excluir um livro que possui empréstimos registrados.");

        await _livroRepository.RemoverAsync(existente);
    }

    private static void ValidarLivro(Livro? livro)
    {
        if (livro is null)
            throw new BadRequestException("Os dados do livro são obrigatórios.");

        var titulo = livro.Titulo?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(titulo))
            throw new BadRequestException("O título do livro é obrigatório.");
        if (titulo.Length > 150)
            throw new BadRequestException("O título do livro deve ter no máximo 150 caracteres.");

        var isbn = livro.Isbn?.Trim() ?? string.Empty;
        if (isbn.Length != 13 || !isbn.All(char.IsDigit))
            throw new BadRequestException("O ISBN deve conter exatamente 13 dígitos numéricos.");

        var anoAtual = DateTime.Now.Year;
        if (livro.AnoPublicacao < 1000 || livro.AnoPublicacao > anoAtual)
            throw new BadRequestException($"O ano de publicação deve estar entre 1000 e {anoAtual}.");
    }
}