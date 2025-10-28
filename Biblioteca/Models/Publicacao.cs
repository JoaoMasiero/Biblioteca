namespace Biblioteca.Models
{
    public class Publicacao
    {
        public int LivroId { get; set; }
        public int EditoraId { get; set; }
        public int DataPublicacao { get; set; }
        public int Edicao { get; set; }
        public Livro? Livro { get; set; }
        public Editora? Editora { get; set; }

    }
}
