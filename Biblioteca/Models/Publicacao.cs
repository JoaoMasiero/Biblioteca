using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class Publicacao
    {
        public int LivroId { get; set; }
        public int EditoraId { get; set; }
        public int DataPublicacao { get; set; }
        [DisplayName("Edição")]
        public int Edicao { get; set; }
        public Livro? Livro { get; set; }
        public Editora? Editora { get; set; }

    }
}
