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
using Microsoft.AspNetCore.Identity;

namespace ElectronicStock.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ShopCardsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;

        public ShopCardsController(DataContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/ShopCards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShopCard>>> GetShopCards()
        {
            // Automation
            List<User> users = await _userManager.Users.Include(x => x.ShopCards).ToListAsync();
            foreach (User user in users)
            {
                if (!user.ShopCards.Any(x => x.Status == "basket"))
                {
                    _context.Add(new ShopCard { UserId = user.Id, Status = "basket" });
                    await _context.SaveChangesAsync();
                }
            }

            return await _context.ShopCards.Include(x => x.User).ToListAsync();
        }

        // GET: api/ShopCards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ShopCard>> GetShopCard(int id)
        {
            var shopCard = await _context.ShopCards
                .Include(x => x.User)
                .Include(x => x.Cards).ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(m => m.ShopCardId == id);

            if (shopCard == null)
            {
                return NotFound();
            }

            return shopCard;
        }

        // POST: api/ShopCards
        [HttpPost]
        public async Task<ActionResult<ShopCard>> CreateShopCard(ShopCard shopCard)
        {
            var userShopCards = _context.ShopCards.Where(x => x.UserId == shopCard.UserId && x.Status == "basket").ToList();
            if (userShopCards.Count != 0 && shopCard.Status == "basket")
            {
                ModelState.AddModelError("Status", "User already has shop card with status 'basket'");
            }

            if (ModelState.IsValid)
            {
                _context.Add(shopCard);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetShopCard), new { id = shopCard.ShopCardId }, shopCard);
            }

            return BadRequest(ModelState);
        }

        // PUT: api/ShopCards/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditShopCard(int id, ShopCard shopCard)
        {
            if (id != shopCard.ShopCardId)
            {
                return BadRequest();
            }

            var userShopCards = _context.ShopCards.Where(x => x.UserId == shopCard.UserId && x.Status == "basket").ToList();
            if (userShopCards.Any() && shopCard.Status == "basket")
            {
                ModelState.AddModelError("Status", "User already has shop card with status 'basket'");
            }

            if (shopCard.Status == "delivered")
            {
                var cards = _context.Cards.Include(x => x.Product).ThenInclude(x => x.Rows).Where(x => x.ShopCardId == shopCard.ShopCardId).ToList();
                foreach (Card card in cards)
                {
                    int quantity = card.Quantity;
                    foreach (Row row in card.Product.Rows)
                    {
                        int savingQuantity = row.Quantity;
                        savingQuantity -= quantity;
                        if (savingQuantity > 0)
                        {
                            quantity = 0;
                            row.Quantity = savingQuantity;
                            _context.Rows.Update(row);
                        }
                        else
                        {
                            quantity = Math.Abs(savingQuantity);
                            _context.Rows.Remove(row);
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shopCard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShopCardExists(shopCard.ShopCardId))
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

        // DELETE: api/ShopCards/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShopCard(int id)
        {
            var shopCard = await _context.ShopCards.FindAsync(id);
            if (shopCard == null)
            {
                return NotFound();
            }

            _context.ShopCards.Remove(shopCard);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool ShopCardExists(int id)
        {
            return _context.ShopCards.Any(e => e.ShopCardId == id);
        }
    }
}
