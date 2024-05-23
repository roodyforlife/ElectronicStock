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

    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string title, int costFrom, int costTo, int guaranteeFrom, int guaranteeTo, ProductSort sort = ProductSort.TitleAsc)
        {
            IQueryable<Product> products = _context.Products.Include(x => x.ProductCategories).ThenInclude(x => x.Category)
                .Include(x => x.Rows);

            if (!string.IsNullOrEmpty(title))
            {
                products = products.Where(x => x.ProductTitle.Contains(title));
            }

            products = products.Where(x => x.Cost >= costFrom);

            if (costTo != 0)
            {
                products = products.Where(x => x.Cost <= costTo);
            }

            products = products.Where(x => x.Guarantee >= guaranteeFrom);

            if (guaranteeTo != 0)
            {
                products = products.Where(x => x.Guarantee <= guaranteeTo);
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

            return await products.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products
                .Include(x => x.ProductCategories).ThenInclude(x => x.Category)
                .Include(x => x.Rows)
                .FirstOrDefaultAsync(m => m.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product, IFormFile image)
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
                return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
            }
            return BadRequest(ModelState);
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditProduct(int id, [FromBody] Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingProduct = await _context.Products.FindAsync(id);
                    if (existingProduct == null)
                    {
                        return NotFound();
                    }

                    existingProduct.ProductTitle = product.ProductTitle;
                    existingProduct.Description = product.Description;
                    existingProduct.Cost = product.Cost;
                    existingProduct.Weight = product.Weight;
                    existingProduct.Dimensions = product.Dimensions;
                    existingProduct.Guarantee = product.Guarantee;
                    existingProduct.StorageConditions = product.StorageConditions;
                    existingProduct.Discount = product.Discount;
                    existingProduct.CreditAvailable = product.CreditAvailable;

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
                return NoContent();
            }
            return BadRequest(ModelState);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
