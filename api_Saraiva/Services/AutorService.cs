using BibliotecaApi.Exceptions;
using BibliotecaApi.Models;
using BibliotecaApi.Repositories.Interfaces;
using BibliotecaApi.Services.Interfaces;

namespace BibliotecaApi.Services;

public class AutorService : IAutorService
{
    private readonly IAutorRepository _autorRepository;

    public AutorService(IAutorRepository autorRepository)
    {
        _autorRepository = autorRepository;
    }

    public async Task<List<Autor>> ListarTodosAsync()
    {
        return await _autorRepository.ListarTodosAsync();
    }

    public async Task<Autor> BuscarPorIdAsync(int id)
    {
        var autor = await _autorRepository.BuscarPorIdAsync(id);
        if (autor is null)
            throw new NotFoundException("Autor não encontrado.");

        return autor;
    }

    public async Task<Autor> CriarAsync(Autor autor)
    {
        ValidarAutor(autor);

        var novoAutor = new Autor
        {
            Nome = autor.Nome.Trim(),
            Nacionalidade = autor.Nacionalidade.Trim()
        };

        return await _autorRepository.AdicionarAsync(novoAutor);
    }

    public async Task<Autor> AtualizarAsync(int id, Autor autor)
    {
        // Ordem obrigatória: primeiro 400 (dados), depois 404 (existência)
        ValidarAutor(autor);

        var existente = await _autorRepository.BuscarPorIdAsync(id);
        if (existente is null)
            throw new NotFoundException("Autor não encontrado.");

        existente.Nome = autor.Nome.Trim();
        existente.Nacionalidade = autor.Nacionalidade.Trim();

        await _autorRepository.AtualizarAsync(existente);
        return existente;
    }

    public async Task ExcluirAsync(int id)
    {
        var existente = await _autorRepository.BuscarPorIdAsync(id);
        if (existente is null)
            throw new NotFoundException("Autor não encontrado.");

        if (await _autorRepository.PossuiLivrosAsync(id))
            throw new ConflictException("Não é possível excluir um autor que possui livros cadastrados.");

        await _autorRepository.RemoverAsync(existente);
    }

    private static void ValidarAutor(Autor? autor)
    {
        if (autor is null)
            throw new BadRequestException("Os dados do autor são obrigatórios.");

        var nome = autor.Nome?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(nome))
            throw new BadRequestException("O nome do autor é obrigatório.");
        if (nome.Length > 100)
            throw new BadRequestException("O nome do autor deve ter no máximo 100 caracteres.");

        var nacionalidade = autor.Nacionalidade?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(nacionalidade))
            throw new BadRequestException("A nacionalidade do autor é obrigatória.");
        if (nacionalidade.Length > 50)
            throw new BadRequestException("A nacionalidade do autor deve ter no máximo 50 caracteres.");
    }
}