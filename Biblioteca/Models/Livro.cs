using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class Livro
    {
        public int LivroId { get; set; }
        [Required(ErrorMessage = "O nome do livro é obrigatório!")]
        [DisplayName("Nome")]
        public string LivroNome { get; set; }
        [Required(ErrorMessage = "O genero do livro é obrigatório!")]
        public string Genero { get; set; }
        [Required(ErrorMessage = "O Ano de lançamento do livro é obrigatório!")]
        [DisplayName("Ano de lançamento")]
        [RegularExpression(@"\d{4}", ErrorMessage = "O Ano deve estar no formato 9999")]
        public int AnoLancamento { get; set; }
        public bool Disponivel { get; set; }
        public ICollection<AutorLivro> AutoresLivros { get; set; } = new List<AutorLivro>();
        public ICollection<Publicacao> Publicacoes { get; set; } = new List<Publicacao>();
    }
}
