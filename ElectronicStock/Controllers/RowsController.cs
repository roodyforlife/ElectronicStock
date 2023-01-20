using ElectronicStock.BaseContext;
using ElectronicStock.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicStock.Controllers
{
    public class RowsController : Controller
    {
        private readonly DataContext _context;

        public RowsController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var dataContext = _context.Rows.Include(p => p.Product);
            return View(await dataContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var row = await _context.Rows
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.RowId == id);
            if (row == null)
            {
                return NotFound();
            }

            return View(row);
        }

        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductTitle");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Row row)
        {
            if (ModelState.IsValid)
            {
                _context.Add(row);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductTitle", row.ProductId);
            return View(row);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var row = await _context.Rows.FindAsync(id);
            if (row == null)
            {
                return NotFound();
            }

            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductTitle", row.ProductId);
            return View(row);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Row row)
        {
            if (id != row.RowId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(row);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductCategoryExists(row.RowId))
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

            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductTitle", row.ProductId);
            return View(row);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var row = await _context.Rows.FindAsync(id);
            _context.Rows.Remove(row);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductCategoryExists(int id)
        {
            return _context.Rows.Any(e => e.RowId == id);
        }
    }
}
