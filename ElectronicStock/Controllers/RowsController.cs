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

    [Route("api/[controller]")]
    [ApiController]
    public class RowsController : ControllerBase
    {
        private readonly DataContext _context;

        public RowsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Rows
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Row>>> GetRows()
        {
            return await _context.Rows.Include(p => p.Product).ToListAsync();
        }

        // GET: api/Rows/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Row>> GetRow(int id)
        {
            var row = await _context.Rows.Include(p => p.Product).FirstOrDefaultAsync(m => m.RowId == id);

            if (row == null)
            {
                return NotFound();
            }

            return row;
        }

        // POST: api/Rows
        [HttpPost]
        public async Task<ActionResult<Row>> CreateRow(Row row)
        {
            if (ModelState.IsValid)
            {
                _context.Add(row);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetRow), new { id = row.RowId }, row);
            }

            return BadRequest(ModelState);
        }

        // PUT: api/Rows/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditRow(int id, Row row)
        {
            if (id != row.RowId)
            {
                return BadRequest();
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
                    if (!RowExists(row.RowId))
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

        // DELETE: api/Rows/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRow(int id)
        {
            var row = await _context.Rows.FindAsync(id);
            if (row == null)
            {
                return NotFound();
            }

            _context.Rows.Remove(row);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool RowExists(int id)
        {
            return _context.Rows.Any(e => e.RowId == id);
        }
    }
}
