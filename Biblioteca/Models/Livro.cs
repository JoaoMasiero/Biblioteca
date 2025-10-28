namespace Biblioteca.Models
{
    public class Livro
    {
        public int LivroId { get; set; }
        public string LivroNome { get; set; }
        public string Genero { get; set; }
        public int AnoLancamento { get; set; }
        public bool Disponivel { get; set; }
        public ICollection<AutorLivro> AutoresLivros { get; set; } = new List<AutorLivro>();
        public ICollection<Publicacao> Publicacoes { get; set; } = new List<Publicacao>();
    }
}
