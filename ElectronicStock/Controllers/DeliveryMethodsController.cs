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
    public class DeliveryMethodsController : ControllerBase
    {
        private readonly DataContext _context;

        public DeliveryMethodsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/DeliveryMethods
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeliveryMethod>>> GetDeliveryMethods()
        {
            return await _context.DeliveryMethods.ToListAsync();
        }

        // GET: api/DeliveryMethods/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DeliveryMethod>> GetDeliveryMethod(int id)
        {
            var deliveryMethod = await _context.DeliveryMethods.FindAsync(id);

            if (deliveryMethod == null)
            {
                return NotFound();
            }

            return deliveryMethod;
        }

        // POST: api/DeliveryMethods
        [HttpPost]
        public async Task<ActionResult<DeliveryMethod>> CreateDeliveryMethod([FromBody] DeliveryMethod deliveryMethod)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deliveryMethod);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetDeliveryMethod), new { id = deliveryMethod.DeliveryMethodId }, deliveryMethod);
            }
            return BadRequest(ModelState);
        }

        // PUT: api/DeliveryMethods/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditDeliveryMethod(int id, [FromBody] DeliveryMethod deliveryMethod)
        {
            if (id != deliveryMethod.DeliveryMethodId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deliveryMethod);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeliveryMethodExists(id))
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

        // DELETE: api/DeliveryMethods/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeliveryMethod(int id)
        {
            var deliveryMethod = await _context.DeliveryMethods.FindAsync(id);
            if (deliveryMethod == null)
            {
                return NotFound();
            }

            _context.DeliveryMethods.Remove(deliveryMethod);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DeliveryMethodExists(int id)
        {
            return _context.DeliveryMethods.Any(e => e.DeliveryMethodId == id);
        }
    }
}
