using System.Text.Json.Serialization;

namespace BibliotecaApi.Models;

public class Livro
{
    public int Id {get; set;}
    public string Titulo {get; set;} = string.Empty;
    public string Isbn {get; set;} = string.Empty;
    public int AnoPublicacao {get; set;}
    public int AutorId {get; set;}


    public virtual Autor? Autor {get; set;}

    [JsonIgnore]
    public virtual List<Emprestimo> Emprestimos {get; set;} = new();
}