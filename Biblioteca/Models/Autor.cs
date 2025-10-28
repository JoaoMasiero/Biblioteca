namespace Biblioteca.Models
{
    public class Autor
    {
        public int AutorId { get; set; }
        public string AutorNome { get; set; }
        public string PaisOrigem { get; set; }
        public ICollection<AutorLivro> AutoresLivros { get; set; } = new List<AutorLivro>();
    }
}
