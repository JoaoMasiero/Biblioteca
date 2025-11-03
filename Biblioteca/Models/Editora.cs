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
        [Required(ErrorMessage = "A cidade da editora é obrigatório!")]
        public string Cidade { get; set; }
        [Required(ErrorMessage = "O telefone da editora é obrigatório!")]
        [RegularExpression(@"\(\d{2}\) \d{5}-\d{4}", ErrorMessage = "O telefone deve estar no formato (99) 99999-9999")]
        public int Telefone { get; set; }
        public ICollection<Publicacao> Publicacoes { get; set; } = new List<Publicacao>();
    }
}
