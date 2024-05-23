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
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly DataContext _context;

        public CardsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Cards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Card>>> GetCards()
        {
            return await _context.Cards
                .Include(c => c.Product)
                .Include(c => c.ShopCard)
                .ToListAsync();
        }

        // GET: api/Cards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Card>> GetCard(int id)
        {
            var card = await _context.Cards
                .Include(c => c.Product)
                .Include(c => c.ShopCard)
                .FirstOrDefaultAsync(m => m.CardId == id);

            if (card == null)
            {
                return NotFound();
            }

            return card;
        }

        // POST: api/Cards
        [HttpPost]
        public async Task<ActionResult<Card>> CreateCard([FromBody] Card card)
        {
            var products = await _context.Products
                .Include(x => x.Rows)
                .FirstOrDefaultAsync(x => x.ProductId == card.ProductId);

            if (products == null || products.Rows.Sum(x => x.Quantity) < card.Quantity)
            {
                return BadRequest("There are not so many goods in stock");
            }

            _context.Cards.Add(card);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCard), new { id = card.CardId }, card);
        }

        // PUT: api/Cards/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCard(int id, [FromBody] Card card)
        {
            if (id != card.CardId)
            {
                return BadRequest();
            }

            var products = await _context.Products
                .Include(x => x.Rows)
                .FirstOrDefaultAsync(x => x.ProductId == card.ProductId);

            if (products == null || products.Rows.Sum(x => x.Quantity) < card.Quantity)
            {
                return BadRequest("There are not so many goods in stock");
            }

            _context.Entry(card).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CardExists(id))
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

        // DELETE: api/Cards/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteCard(int id)
        {
            var card = await _context.Cards.FindAsync(id);
            if (card == null)
            {
                return NotFound();
            }

            _context.Cards.Remove(card);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CardExists(int id)
        {
            return _context.Cards.Any(e => e.CardId == id);
        }
    }
}
