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
    public class DeliveryMethodProductsController : ControllerBase
    {
        private readonly DataContext _context;

        public DeliveryMethodProductsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/DeliveryMethodProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeliveryMethodProduct>>> GetDeliveryMethodProducts()
        {
            return await _context.DeliveryMethodProducts
                .Include(d => d.DeliveryMethod)
                .Include(d => d.Product)
                .ToListAsync();
        }

        // GET: api/DeliveryMethodProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DeliveryMethodProduct>> GetDeliveryMethodProduct(int id)
        {
            var deliveryMethodProduct = await _context.DeliveryMethodProducts
                .Include(d => d.DeliveryMethod)
                .Include(d => d.Product)
                .FirstOrDefaultAsync(m => m.DeliveryMethodProductId == id);

            if (deliveryMethodProduct == null)
            {
                return NotFound();
            }

            return deliveryMethodProduct;
        }

        // POST: api/DeliveryMethodProducts
        [HttpPost]
        public async Task<ActionResult<DeliveryMethodProduct>> CreateDeliveryMethodProduct([FromBody] DeliveryMethodProduct deliveryMethodProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deliveryMethodProduct);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetDeliveryMethodProduct), new { id = deliveryMethodProduct.DeliveryMethodProductId }, deliveryMethodProduct);
            }
            return BadRequest(ModelState);
        }

        // PUT: api/DeliveryMethodProducts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditDeliveryMethodProduct(int id, [FromBody] DeliveryMethodProduct deliveryMethodProduct)
        {
            if (id != deliveryMethodProduct.DeliveryMethodProductId)
            {
                return BadRequest();
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
                    if (!DeliveryMethodProductExists(id))
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

        // DELETE: api/DeliveryMethodProducts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeliveryMethodProduct(int id)
        {
            var deliveryMethodProduct = await _context.DeliveryMethodProducts.FindAsync(id);
            if (deliveryMethodProduct == null)
            {
                return NotFound();
            }

            _context.DeliveryMethodProducts.Remove(deliveryMethodProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DeliveryMethodProductExists(int id)
        {
            return _context.DeliveryMethodProducts.Any(e => e.DeliveryMethodProductId == id);
        }
    }
}
