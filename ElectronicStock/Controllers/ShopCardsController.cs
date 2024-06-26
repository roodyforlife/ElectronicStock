﻿using System;
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
    public class ShopCardsController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;

        public ShopCardsController(DataContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ShopCards
        public async Task<IActionResult> Index()
        {
            // Automation
            List<User> users = await _userManager.Users.Include(x => x.ShopCards).ToListAsync();
            foreach(User user in users)
            {
                if (user.ShopCards.Where(x => x.Status == "basket").ToList().Count() == 0)
                {
                    _context.Add(new ShopCard() { UserId = user.Id, Status = "basket"});
                    await _context.SaveChangesAsync();
                }
            }

            return View(await _context.ShopCards.Include(x => x.User).ToListAsync());
        }

        // GET: ShopCards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopCard = await _context.ShopCards
                .Include(x => x.User)
                .Include(x => x.Cards).ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(m => m.ShopCardId == id);
            if (shopCard == null)
            {
                return NotFound();
            }

            return View(shopCard);
        }

        // GET: ShopCards/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: ShopCards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShopCardId,CreateDate,UserId,Status")] ShopCard shopCard)
        {
            var userShopCards = _context.ShopCards.Where(x => x.UserId == shopCard.UserId && x.Status == "basket").ToList();
            if(userShopCards.Count != 0 && shopCard.Status == "basket")
            {
                ModelState.AddModelError("Status", "User already has shop card with status 'basket'");
            }

            if (ModelState.IsValid)
            {
                _context.Add(shopCard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", shopCard.UserId);
            return View(shopCard);
        }

        // GET: ShopCards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopCard = await _context.ShopCards.FindAsync(id);
            if (shopCard == null)
            {
                return NotFound();
            }
            return View(shopCard);
        }

        // POST: ShopCards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShopCardId,CreateDate,UserId,Status")] ShopCard shopCard)
        {
            if (id != shopCard.ShopCardId)
            {
                return NotFound();
            }

            var userShopCards = _context.ShopCards.Where(x => x.UserId == shopCard.UserId && x.Status == "basket");
            if (userShopCards != null && shopCard.Status == "basket")
            {
                ModelState.AddModelError("Status", "User already has shop card with status 'basket'");
            }

            if(shopCard.Status == "delivered")
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
                            quantity = row.Quantity - quantity;
                            row.Quantity = savingQuantity;
                            _context.Rows.Update(row);
                        }
                        else
                        {
                            quantity = Math.Abs(savingQuantity);
                            _context.Remove(row);
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
                return RedirectToAction(nameof(Index));
            }
            return View(shopCard);
        }

        // GET: ShopCards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopCard = await _context.ShopCards
                .FirstOrDefaultAsync(m => m.ShopCardId == id);
            if (shopCard == null)
            {
                return NotFound();
            }

            return View(shopCard);
        }

        // POST: ShopCards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shopCard = await _context.ShopCards.FindAsync(id);
            _context.ShopCards.Remove(shopCard);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShopCardExists(int id)
        {
            return _context.ShopCards.Any(e => e.ShopCardId == id);
        }
    }
}
