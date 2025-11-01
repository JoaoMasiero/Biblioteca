using Biblioteca.Models;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Data
{
    public class BibliotecaContext : DbContext
    {
        public BibliotecaContext(DbContextOptions<BibliotecaContext> options) : base(options) { }
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Editora> Editoras { get; set; }
        public DbSet<Leitor> Leitores { get; set; }
        public DbSet<Livro> Livros { get; set; }
        public DbSet<AutorLivro> AutoresLivros { get; set; }
        public DbSet<Publicacao> Publicacoes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AutorLivro>()
                .HasKey(cd => new { cd.AutorId, cd.LivroId });
            modelBuilder.Entity<AutorLivro>()
                .HasOne(cd => cd.Autor)
                .WithMany(c => c.AutoresLivros)
                .HasForeignKey(cd => cd.AutorId);
            modelBuilder.Entity<AutorLivro>()
                .HasOne(cd => cd.Livro)
                .WithMany(c => c.AutoresLivros)
                .HasForeignKey(cd => cd.LivroId);

            modelBuilder.Entity<Publicacao>()
                .HasKey(cd => new { cd.EditoraId, cd.LivroId });
            modelBuilder.Entity<Publicacao>()
               .HasOne(aa => aa.Editora)
               .WithMany(a => a.Publicacoes)
               .HasForeignKey(aa => aa.EditoraId);
            modelBuilder.Entity<Publicacao>()
                .HasOne(aa => aa.Livro)
                .WithMany(av => av.Publicacoes)
                .HasForeignKey(aa => aa.LivroId);
        }

    }
}
