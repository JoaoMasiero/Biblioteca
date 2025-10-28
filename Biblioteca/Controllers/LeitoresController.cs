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
    public class LeitoresController : Controller
    {
        private readonly BibliotecaContext _context;

        public LeitoresController(BibliotecaContext context)
        {
            _context = context;
        }

        // GET: Leitores
        public async Task<IActionResult> Index()
        {
            return View(await _context.Leitores.ToListAsync());
        }

        // GET: Leitores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leitor = await _context.Leitores
                .FirstOrDefaultAsync(m => m.LeitorId == id);
            if (leitor == null)
            {
                return NotFound();
            }

            return View(leitor);
        }

        // GET: Leitores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Leitores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LeitorId,LeitorName,Telefone,Email,CEP")] Leitor leitor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(leitor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(leitor);
        }

        // GET: Leitores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leitor = await _context.Leitores.FindAsync(id);
            if (leitor == null)
            {
                return NotFound();
            }
            return View(leitor);
        }

        // POST: Leitores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LeitorId,LeitorName,Telefone,Email,CEP")] Leitor leitor)
        {
            if (id != leitor.LeitorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leitor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeitorExists(leitor.LeitorId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(leitor);
        }

        // GET: Leitores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leitor = await _context.Leitores
                .FirstOrDefaultAsync(m => m.LeitorId == id);
            if (leitor == null)
            {
                return NotFound();
            }

            return View(leitor);
        }

        // POST: Leitores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leitor = await _context.Leitores.FindAsync(id);
            if (leitor != null)
            {
                _context.Leitores.Remove(leitor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeitorExists(int id)
        {
            return _context.Leitores.Any(e => e.LeitorId == id);
        }
    }
}
