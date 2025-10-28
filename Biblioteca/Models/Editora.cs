namespace Biblioteca.Models
{
    public class Editora
    {
        public int EditoraId { get; set; }
        public string EditoraNome { get; set;}
        public string Cidade { get; set; }
        public int Telefone { get; set; }
        public ICollection<Publicacao> Publicacoes { get; set; } = new List<Publicacao>();
    }
}
