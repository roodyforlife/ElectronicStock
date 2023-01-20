using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElectronicStock.BaseContext;
using ElectronicStock.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using ElectronicStock.Enums;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ElectronicStock.Controllers
{
    public class ProductsController : Controller
    {
        private readonly DataContext _context;

        public ProductsController(DataContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(string title, int costFrom, int costTo, ProductSort sort = ProductSort.TitleAsc)
        {
            IQueryable<Product> products = _context.Products.Include(x => x.productCategories).ThenInclude(x => x.Category)
                .Include(x => x.Rows);

            if(!String.IsNullOrEmpty(title))
            {
                products = products.Where(x => x.ProductTitle.Contains(title));
            }

            products = products.Where(x => x.Cost >= costFrom);

            if(costTo != 0)
            {
                products = products.Where(x => x.Cost <= costTo);
            }

            switch (sort)
            {
                case ProductSort.TitleDesc:
                    products = products.OrderByDescending(x => x.ProductTitle);
                    break;
                case ProductSort.CostAsc:
                    products = products.OrderBy(x => x.Cost);
                    break;
                case ProductSort.CostDesc:
                    products = products.OrderByDescending(x => x.Cost);
                    break;
                case ProductSort.DateAsc:
                    products = products.OrderBy(x => x.CreateDate);
                    break;
                case ProductSort.DateDesc:
                    products = products.OrderByDescending(x => x.CreateDate);
                    break;
                default:
                    products = products.OrderBy(x => x.ProductTitle);
                    break;
            }

            ViewBag.Sort = (List<SelectListItem>)Enum.GetValues(typeof(ProductSort)).Cast<ProductSort>()
                .Select(x => new SelectListItem
                {
                    Text = x.GetType()
            .GetMember(x.ToString())
            .FirstOrDefault()
            .GetCustomAttribute<DisplayAttribute>()?
            .GetName(),
                    Value = x.ToString(),
                    Selected = (x == sort)
                }).ToList();

            ViewBag.ProductTitle = title;
            ViewBag.CostFrom = costFrom;
            ViewBag.CostTo = costTo;
            return View(await products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(x => x.productCategories).ThenInclude(x => x.Category)
                .Include(x => x.Rows)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductTitle,Description,Cost,Discount,CreateDate,CreditAvailable,Weight,Dimensions,Guarantee,StorageConditions")] Product product,
            IFormFile image)
        {
            if (image != null)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(image.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)image.Length);
                }

                product.Image = imageData;
            }
            else
            {
                ModelState.AddModelError("Image", "Upload image");
            }

            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductTitle,Description,Cost,Discount,CreateDate,CreditAvailable,Weight,Dimensions,Guarantee,StorageConditions")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Product newProduct = _context.Products.FirstOrDefault(x => x.ProductId == id);
                    newProduct.ProductTitle = product.ProductTitle;
                    newProduct.Description = product.Description;
                    newProduct.Cost = product.Cost;
                    newProduct.Weight = product.Weight;
                    newProduct.Dimensions = product.Dimensions;
                    newProduct.Guarantee = product.Guarantee;
                    newProduct.StorageConditions = product.StorageConditions;
                    newProduct.Discount = product.Discount;
                    newProduct.CreditAvailable = product.CreditAvailable;
                    // _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
