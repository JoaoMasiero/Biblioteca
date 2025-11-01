using System.Diagnostics;
using Biblioteca.Data;
using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Controllers
{
    public class HomeController : Controller
    {
        private readonly BibliotecaContext _context;

        public HomeController(BibliotecaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> Consultas()
        {

            var consultaJoin = await _context.Publicacoes
                .Include(p => p.Livro)
                .Include(p => p.Editora)
                .Select(p => new
                {
                    NomeLivro = p.Livro.LivroNome,
                    NomeEditora = p.Editora.EditoraNome,
                    AnoPublicacao = p.DataPublicacao
                })
                .ToListAsync();

            ViewBag.Consulta1 = consultaJoin;

            var consultaGrupo = await _context.Publicacoes
                .GroupBy(p => p.Editora.EditoraNome)
                .Select(grupo => new
                {
                    NomeEditora = grupo.Key,
                    TotalDeLivros = grupo.Count()
                })
                .ToListAsync();

            ViewBag.Consulta2 = consultaGrupo;

            var consultaWhereHaving = await _context.Publicacoes
                .Include(p => p.Livro)
                .Include(p => p.Editora)
                .Where(p => p.Livro.AnoLancamento > 2000)
                .GroupBy(p => p.Editora.EditoraNome)
                .Select(grupo => new
                {
                    NomeEditora = grupo.Key,
                    TotalLivrosRecentes = grupo.Count()
                })

                .Where(resultado => resultado.TotalLivrosRecentes > 1)
                .ToListAsync();

            ViewBag.Consulta3 = consultaWhereHaving;

            return View();
        }
}
}
