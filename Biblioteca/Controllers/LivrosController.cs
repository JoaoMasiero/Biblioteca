using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Data;
using Biblioteca.Models;

namespace Biblioteca.Controllers
{
    public class LivrosController : Controller
    {
        private readonly BibliotecaContext _context;

        public LivrosController(BibliotecaContext context)
        {
            _context = context;
        }

        // GET: Livros
        public async Task<IActionResult> Index()
        {
            return View(await _context.Livros.ToListAsync());
        }

        // GET: Livros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _context.Livros
                .Include(l => l.Publicacoes)
                    .ThenInclude(p => p.Editora)
                .Include(l => l.AutoresLivros)
                    .ThenInclude(al => al.Autor) 
                .FirstOrDefaultAsync(m => m.LivroId == id);

            if (livro == null)
            {
                return NotFound();
            }

            return View(livro);
        }

        // GET: Livros/Create
        public IActionResult Create()
        {
            var viewModel = new LivroCreateViewModel
            {
                AutoresDisponiveis = new SelectList(_context.Autores, "AutorId", "AutorNome")
            };
            return View(viewModel);
        }

        // POST: Livros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LivroCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var livro = new Livro
                {
                    LivroNome = viewModel.LivroNome,
                    Genero = viewModel.Genero,
                    AnoLancamento = viewModel.AnoLancamento,
                    Disponivel = viewModel.Disponivel,
                    AutoresLivros = new List<AutorLivro>()
                };

                if (viewModel.AutoresSelecionadosIDs != null)
                {
                    foreach (var autorId in viewModel.AutoresSelecionadosIDs)
                    {
                        var autorLivro = new AutorLivro
                        {
                            AutorId = autorId,
                            Livro = livro
                        };
                        livro.AutoresLivros.Add(autorLivro);
                    }
                }

                _context.Add(livro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            viewModel.AutoresDisponiveis = new SelectList(_context.Autores, "AutorId", "AutorNome");
            return View(viewModel);
        }

        // GET: Livros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _context.Livros
                .Include(l => l.AutoresLivros)
                .FirstOrDefaultAsync(l => l.LivroId == id);

            if (livro == null)
            {
                return NotFound();
            }

            var viewModel = new LivroEditViewModel
            {
                LivroId = livro.LivroId,
                LivroNome = livro.LivroNome,
                Genero = livro.Genero,
                AnoLancamento = livro.AnoLancamento,
                Disponivel = livro.Disponivel,

                AutoresDisponiveis = new SelectList(_context.Autores, "AutorId", "AutorNome"),

                AutoresSelecionadosIDs = livro.AutoresLivros.Select(al => al.AutorId).ToList()
            };

            return View(viewModel);
        }

        // POST: Livros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LivroEditViewModel viewModel)
        {
            if (id != viewModel.LivroId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var livroParaAtualizar = await _context.Livros
                        .Include(l => l.AutoresLivros)
                        .FirstOrDefaultAsync(l => l.LivroId == viewModel.LivroId);

                    if (livroParaAtualizar == null)
                    {
                        return NotFound();
                    }

                    livroParaAtualizar.LivroNome = viewModel.LivroNome;
                    livroParaAtualizar.Genero = viewModel.Genero;
                    livroParaAtualizar.AnoLancamento = viewModel.AnoLancamento;
                    livroParaAtualizar.Disponivel = viewModel.Disponivel;

                    var autoresParaRemover = new List<AutorLivro>();
                    foreach (var autorLivro in livroParaAtualizar.AutoresLivros)
                    {
                        if (!viewModel.AutoresSelecionadosIDs.Contains(autorLivro.AutorId))
                        {
                            autoresParaRemover.Add(autorLivro);
                        }
                    }
                    _context.AutoresLivros.RemoveRange(autoresParaRemover);

                    foreach (var autorId in viewModel.AutoresSelecionadosIDs)
                    {
                        var jaExiste = livroParaAtualizar.AutoresLivros
                                        .Any(al => al.AutorId == autorId);

                        if (!jaExiste)
                        {
                            livroParaAtualizar.AutoresLivros.Add(new AutorLivro
                            {
                                AutorId = autorId,
                                LivroId = livroParaAtualizar.LivroId
                            });
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                }
                return RedirectToAction(nameof(Index));
            }

            viewModel.AutoresDisponiveis = new SelectList(_context.Autores, "AutorId", "AutorNome");
            return View(viewModel);
        }

        private bool LivroExists(int id)
        {
            return _context.Livros.Any(e => e.LivroId == id);
        }

        // GET: Livros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _context.Livros
                .FirstOrDefaultAsync(m => m.LivroId == id);
            if (livro == null)
            {
                return NotFound();
            }

            return View(livro);
        }

        // POST: Livros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var livro = await _context.Livros.FindAsync(id);
            if (livro != null)
            {
                _context.Livros.Remove(livro);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LivroExist(int id)
        {
            return _context.Livros.Any(e => e.LivroId == id);
        }

        public IActionResult LancarEdicao()
        {
            var viewModel = new PublicacaoViewModel
            {
                Livros = new SelectList(_context.Livros, "LivroId", "LivroNome"),

                Editoras = new SelectList(_context.Editoras, "EditoraId", "EditoraNome")
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SalvarPublicacao(PublicacaoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Livros = new SelectList(_context.Livros, "LivroId", "LivroNome");
                model.Editoras = new SelectList(_context.Editoras, "EditoraId", "EditoraNome");

                return View("LancarEdicao", model);
            }

            var Publicacao = await _context.Publicacoes
                .FirstOrDefaultAsync(aa => aa.EditoraId == model.EditoraId && aa.LivroId == model.LivroId);

            if (Publicacao == null)
            {
                Publicacao = new Publicacao
                {
                    EditoraId = model.EditoraId,
                    LivroId = model.LivroId,
                    Edicao = model.Edicao,
                    DataPublicacao = model.DataPublicacao,
                };
                _context.Publicacoes.Add(Publicacao);
            }
            else
            {
                Publicacao.Edicao = model.Edicao;
                Publicacao.DataPublicacao = model.DataPublicacao;
                _context.Publicacoes.Update(Publicacao);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Editoras", new { id = model.EditoraId });
        }

        private bool PublicacaoExists(int id)
        {
            return _context.Livros.Any(e => e.LivroId == id);
        }
    }
}
