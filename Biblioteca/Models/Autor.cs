using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class Autor
    {
        public int AutorId { get; set; }
        [Required(ErrorMessage = "O nome do autor é obrigatório!")]
        [DisplayName("Nome")]
        public string AutorNome { get; set; }
        [Required(ErrorMessage = "O país de origem é obrigatório!")]
        [DisplayName("País de Origem")]
        public string PaisOrigem { get; set; }
        public ICollection<AutorLivro> AutoresLivros { get; set; } = new List<AutorLivro>();
    }
}
