using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElectronicStock.BaseContext;
using ElectronicStock.Models;
using ElectronicStock.Enums;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ElectronicStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly DataContext _context;

        public CategoriesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories(string name, DateTime dateFrom, DateTime dateTo, CategorySort sort = CategorySort.NameAsc)
        {
            IQueryable<Category> categories = _context.Categories;

            if (!String.IsNullOrEmpty(name))
            {
                categories = categories.Where(x => x.Name.Contains(name));
            }

            categories = categories.Where(x => x.CreateDate >= dateFrom);

            if (dateTo.Year != 1)
            {
                categories = categories.Where(x => x.CreateDate <= dateTo);
            }

            switch (sort)
            {
                case CategorySort.NameDesc:
                    categories = categories.OrderByDescending(x => x.Name);
                    break;
                case CategorySort.DateAsc:
                    categories = categories.OrderBy(x => x.CreateDate);
                    break;
                case CategorySort.DateDesc:
                    categories = categories.OrderByDescending(x => x.CreateDate);
                    break;
                default:
                    categories = categories.OrderBy(x => x.Name);
                    break;
            }

            return await categories.ToListAsync();
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(m => m.CategoryId == id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory([FromBody] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryId }, category);
            }
            return BadRequest(ModelState);
        }

        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCategory(int id, [FromBody] Category category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(id))
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

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }
    }
}
