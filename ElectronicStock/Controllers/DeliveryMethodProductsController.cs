using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElectronicStock.BaseContext;
using ElectronicStock.Models;

namespace ElectronicStock.Controllers
{
    public class DeliveryMethodProductsController : Controller
    {
        private readonly DataContext _context;

        public DeliveryMethodProductsController(DataContext context)
        {
            _context = context;
        }

        // GET: DeliveryMethodProducts
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.DeliveryMethodProducts.Include(d => d.DeliveryMethod).Include(d => d.Product);
            return View(await dataContext.ToListAsync());
        }

        // GET: DeliveryMethodProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliveryMethodProduct = await _context.DeliveryMethodProducts
                .Include(d => d.DeliveryMethod)
                .Include(d => d.Product)
                .FirstOrDefaultAsync(m => m.DeliveryMethodProductId == id);
            if (deliveryMethodProduct == null)
            {
                return NotFound();
            }

            return View(deliveryMethodProduct);
        }

        // GET: DeliveryMethodProducts/Create
        public IActionResult Create()
        {
            ViewData["DeliveryMethodId"] = new SelectList(_context.DeliveryMethods, "DeliveryMethodId", "MethodName");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductTitle");
            return View();
        }

        // POST: DeliveryMethodProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DeliveryMethodProductId,ProductId,DeliveryMethodId")] DeliveryMethodProduct deliveryMethodProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deliveryMethodProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeliveryMethodId"] = new SelectList(_context.DeliveryMethods, "DeliveryMethodId", "MethodName", deliveryMethodProduct.DeliveryMethodId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductTitle", deliveryMethodProduct.ProductId);
            return View(deliveryMethodProduct);
        }

        // GET: DeliveryMethodProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliveryMethodProduct = await _context.DeliveryMethodProducts.FindAsync(id);
            if (deliveryMethodProduct == null)
            {
                return NotFound();
            }
            ViewData["DeliveryMethodId"] = new SelectList(_context.DeliveryMethods, "DeliveryMethodId", "MethodName", deliveryMethodProduct.DeliveryMethodId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductTitle", deliveryMethodProduct.ProductId);
            return View(deliveryMethodProduct);
        }

        // POST: DeliveryMethodProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DeliveryMethodProductId,ProductId,DeliveryMethodId")] DeliveryMethodProduct deliveryMethodProduct)
        {
            if (id != deliveryMethodProduct.DeliveryMethodProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deliveryMethodProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeliveryMethodProductExists(deliveryMethodProduct.DeliveryMethodProductId))
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
            ViewData["DeliveryMethodId"] = new SelectList(_context.DeliveryMethods, "DeliveryMethodId", "MethodName", deliveryMethodProduct.DeliveryMethodId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductTitle", deliveryMethodProduct.ProductId);
            return View(deliveryMethodProduct);
        }

        // GET: DeliveryMethodProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliveryMethodProduct = await _context.DeliveryMethodProducts
                .Include(d => d.DeliveryMethod)
                .Include(d => d.Product)
                .FirstOrDefaultAsync(m => m.DeliveryMethodProductId == id);
            if (deliveryMethodProduct == null)
            {
                return NotFound();
            }

            return View(deliveryMethodProduct);
        }

        // POST: DeliveryMethodProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deliveryMethodProduct = await _context.DeliveryMethodProducts.FindAsync(id);
            _context.DeliveryMethodProducts.Remove(deliveryMethodProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeliveryMethodProductExists(int id)
        {
            return _context.DeliveryMethodProducts.Any(e => e.DeliveryMethodProductId == id);
        }
    }
}
