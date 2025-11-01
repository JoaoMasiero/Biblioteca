using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class Leitor
    {
        public int LeitorId { get; set; }
        [Required(ErrorMessage = "O nome do leitor é obrigatório!")]
        [DisplayName("Nome")]
        public string LeitorName { get; set; }
        [Required(ErrorMessage = "O telefone do leitor é obrigatório!")]
        [RegularExpression(@"\(\d{2}\) \d{5}-\d{4}", ErrorMessage = "O telefone deve estar no formato (99) 99999-9999")]
        public int Telefone { get; set; }
        [Required(ErrorMessage = "O E-mail do leitor é obrigatório!")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "O e-mail informado não é válido!")]
        public int Email { get; set; }
        [Required(ErrorMessage = "O CEP do leitor é obrigatório!")]
        [RegularExpression(@"\d{5}-\d{3}", ErrorMessage = "O CEP deve estar no formato 99999-000")]
        public int CEP { get; set; }
    }
}
