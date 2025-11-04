using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class LivroEditViewModel
    {
        // Precisamos do ID para o POST
        public int LivroId { get; set; }

        [Required(ErrorMessage = "O nome do livro é obrigatório.")]
        [Display(Name = "Nome do Livro")]
        public string LivroNome { get; set; }

        public string Genero { get; set; }

        [Display(Name = "Ano de Lançamento")]
        public int AnoLancamento { get; set; }

        public bool Disponivel { get; set; }

        [Required(ErrorMessage = "Selecione ao menos um autor.")]
        [Display(Name = "Autores")]
        public List<int> AutoresSelecionadosIDs { get; set; } = new List<int>();

        public SelectList? AutoresDisponiveis { get; set; }
    }
}