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
                // Inclui as publicações (como fizemos antes)
                .Include(l => l.Publicacoes)
                    .ThenInclude(p => p.Editora)
                // NOVO: Inclui a tabela de junção e, a partir dela, os Autores
                .Include(l => l.AutoresLivros)
                    .ThenInclude(al => al.Autor) // 'Autor' é a prop. de navegação em AutorLivro.cs
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
                // Popula a lista de autores para o dropdown
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
                // 1. Criar o objeto Livro principal
                var livro = new Livro
                {
                    LivroNome = viewModel.LivroNome,
                    Genero = viewModel.Genero,
                    AnoLancamento = viewModel.AnoLancamento,
                    Disponivel = viewModel.Disponivel,
                    AutoresLivros = new List<AutorLivro>() // Inicializa a coleção
                };

                // 2. Iterar sobre os IDs dos autores selecionados
                if (viewModel.AutoresSelecionadosIDs != null)
                {
                    foreach (var autorId in viewModel.AutoresSelecionadosIDs)
                    {
                        // 3. Criar a entidade de junção (AutorLivro) para cada autor
                        var autorLivro = new AutorLivro
                        {
                            AutorId = autorId,
                            Livro = livro // O EF é inteligente e ligará o LivroId quando salvar
                        };
                        livro.AutoresLivros.Add(autorLivro);
                    }
                }

                // 4. Adicionar o livro (e seus AutoresLivros) ao contexto
                _context.Add(livro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // 5. Se o modelo for inválido, repopular a lista de autores antes de reexibir
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

            // 1. Busca o livro E seus autores já relacionados
            var livro = await _context.Livros
                .Include(l => l.AutoresLivros) // Traz a tabela de junção
                .FirstOrDefaultAsync(l => l.LivroId == id);

            if (livro == null)
            {
                return NotFound();
            }

            // 2. Cria o ViewModel
            var viewModel = new LivroEditViewModel
            {
                LivroId = livro.LivroId,
                LivroNome = livro.LivroNome,
                Genero = livro.Genero,
                AnoLancamento = livro.AnoLancamento,
                Disponivel = livro.Disponivel,

                // 3. Popula a lista de autores disponíveis (todos)
                AutoresDisponiveis = new SelectList(_context.Autores, "AutorId", "AutorNome"),

                // 4. Popula a lista de IDs que já estão selecionados
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
                    // 1. Busca o livro original do banco, incluindo seus autores atuais
                    var livroParaAtualizar = await _context.Livros
                        .Include(l => l.AutoresLivros) // Essencial para comparar!
                        .FirstOrDefaultAsync(l => l.LivroId == viewModel.LivroId);

                    if (livroParaAtualizar == null)
                    {
                        return NotFound();
                    }

                    // 2. Atualiza as propriedades simples
                    livroParaAtualizar.LivroNome = viewModel.LivroNome;
                    livroParaAtualizar.Genero = viewModel.Genero;
                    livroParaAtualizar.AnoLancamento = viewModel.AnoLancamento;
                    livroParaAtualizar.Disponivel = viewModel.Disponivel;

                    // 3. Atualiza os relacionamentos (a parte mais complexa)

                    // 3a. Remove os autores que foram desmarcados
                    var autoresParaRemover = new List<AutorLivro>();
                    foreach (var autorLivro in livroParaAtualizar.AutoresLivros)
                    {
                        // Se o autor atual (do banco) NÃO está na nova lista de IDs (do form)...
                        if (!viewModel.AutoresSelecionadosIDs.Contains(autorLivro.AutorId))
                        {
                            autoresParaRemover.Add(autorLivro);
                        }
                    }
                    _context.AutoresLivros.RemoveRange(autoresParaRemover); // Remove os desmarcados

                    // 3b. Adiciona os autores que foram recém-marcados
                    foreach (var autorId in viewModel.AutoresSelecionadosIDs)
                    {
                        // Se a nova lista de IDs (do form) tem um ID que NÃO existe no banco...
                        var jaExiste = livroParaAtualizar.AutoresLivros
                                        .Any(al => al.AutorId == autorId);

                        if (!jaExiste)
                        {
                            // Adiciona o novo relacionamento
                            livroParaAtualizar.AutoresLivros.Add(new AutorLivro
                            {
                                AutorId = autorId,
                                LivroId = livroParaAtualizar.LivroId
                            });
                        }
                    }

                    // 4. Salva tudo
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                }
                return RedirectToAction(nameof(Index));
            }

            // Se o modelo for inválido, repopular a lista
            viewModel.AutoresDisponiveis = new SelectList(_context.Autores, "AutorId", "AutorNome");
            return View(viewModel);
        }

        // (Não se esqueça do seu método LivroExists(int id) )
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
