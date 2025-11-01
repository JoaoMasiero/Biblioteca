using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class Editora
    {
        public int EditoraId { get; set; }
        [Required(ErrorMessage = "O nome da editora é obrigatório!")]
        [DisplayName("Nome")]
        public string EditoraNome { get; set;}
        [Required(ErrorMessage = "A cidade da Editora é obrigatório!")]
        public string Cidade { get; set; }
        [Required(ErrorMessage = "O telefone da editora é obrigatório!")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "O e-mail informado não é válido!")]
        public int Telefone { get; set; }
        public ICollection<Publicacao> Publicacoes { get; set; } = new List<Publicacao>();
    }
}
