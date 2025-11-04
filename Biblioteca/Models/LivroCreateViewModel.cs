using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class LivroCreateViewModel
    {
        // 1. Propriedades do Livro
        [Required(ErrorMessage = "O nome do livro é obrigatório.")]
        [Display(Name = "Nome do Livro")]
        public string LivroNome { get; set; }

        public string Genero { get; set; }

        [Display(Name = "Ano de Lançamento")]
        public int AnoLancamento { get; set; }

        public bool Disponivel { get; set; }

        // 2. Para receber os IDs dos autores selecionados no formulário
        [Required(ErrorMessage = "Selecione ao menos um autor.")]
        [Display(Name = "Autores")]
        public List<int> AutoresSelecionadosIDs { get; set; } = new List<int>();

        // 3. Para popular o dropdown com todos os autores disponíveis
        public SelectList? AutoresDisponiveis { get; set; }
    }
}