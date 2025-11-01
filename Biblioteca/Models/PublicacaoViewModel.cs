using Microsoft.AspNetCore.Mvc.Rendering;

namespace Biblioteca.Models
{
    public class PublicacaoViewModel
    {
        public int EditoraId { get; set; }
        public int LivroId { get; set; }
        public int Edicao { get; set; }
        public int DataPublicacao { get; set; }
        public SelectList? Editoras { get; set; }
        public SelectList? Livros { get; set; }
    }
}
