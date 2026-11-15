namespace BibliotecaApi.Models;

public class Emprestimo
{
    public int Id {get; set;}
    public int LivroId {get; set;}
    public string NomeLeitor {get; set;} = string.Empty;
    public DateTime DataEmprestimo {get; set;}
    public DateTime? DataDevolucao {get; set;}

    public virtual Livro? Livro {get; set;}
}
