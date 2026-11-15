using BibliotecaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaApi.Data;

public class BibliotecaContext : DbContext
{
    public BibliotecaContext(DbContextOptions<BibliotecaContext> options) : base(options)
    {
        
    }

    public DbSet<Autor> Autores {get; set;}
    public DbSet<Livro> Livros {get; set;}
    public DbSet<Emprestimo> Emprestimos {get; set;}


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Autores
        modelBuilder.Entity<Autor>(e =>
        {
            e.ToTable("Autores");
            e.HasKey(a => a.Id);
            e.Property(a => a.Nome).IsRequired().HasMaxLength(100);
            e.Property(a => a.Nacionalidade).IsRequired().HasMaxLength(50);
        });

        //Livros
        modelBuilder.Entity<Livro>(e =>
        {
            e.ToTable("Livros");
            e.HasKey(l => l.Id);
            e.Property(l => l.Titulo).IsRequired().HasMaxLength(150);
            e.Property(l => l.Isbn).IsRequired().HasMaxLength(13);
            e.HasIndex(l => l.Isbn).IsUnique();
            e.Property(l => l.AnoPublicacao).IsRequired();


            e.HasOne(l => l.Autor)
                .WithMany(a => a.Livros)
                .HasForeignKey(l => l.AutorId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        });

        //Emprestimos
        modelBuilder.Entity<Emprestimo>(e =>
        {
            e.ToTable("Emprestimos");
            e.HasKey(m => m.Id);
            e.Property(m => m.NomeLeitor).IsRequired().HasMaxLength(100);
            e.Property(m => m.DataEmprestimo).IsRequired();


            e.HasOne(m => m.Livro)
                .WithMany(l => l.Emprestimos)
                .HasForeignKey(m => m.LivroId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}