using System.Diagnostics;
using Biblioteca.Data;
using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BibliotecaContext _context;

        public HomeController(ILogger<HomeController> logger, BibliotecaContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalLivros = await _context.Livros.CountAsync();

            ViewBag.TotalAutores = await _context.Autores.CountAsync();

            ViewBag.TotalEditoras = await _context.Editoras.CountAsync();

            ViewBag.TotalLeitores = await _context.Leitores.CountAsync();

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
