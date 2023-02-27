using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreSite1.Data;
using CoreSite1.Models;

namespace CoreSite1.Pages.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JSTOrderItems : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JSTOrderItems(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/JSTOrderItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CoreSite1.Models.OrderItem>>> GetOrderItem()
        {
            return await _context.OrderItem.ToListAsync();
        }

        // GET: api/JSTOrderItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CoreSite1.Models.OrderItem>> GetOrderItem(int id)
        {
            var orderItem = await _context.OrderItem.FindAsync(id);

            if (orderItem == null)
            {
                return NotFound();
            }

            return orderItem;
        }

        // PUT: api/JSTOrderItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderItem(int id, CoreSite1.Models.OrderItem orderItem)
        {
            if (id != orderItem.OrderItemId)
            {
                return BadRequest();
            }

            _context.Entry(orderItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderItemExists(id))
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

        // POST: api/JSTOrderItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CoreSite1.Models.OrderItem>> PostOrderItem(CoreSite1.Models.OrderItem orderItem)
        {
            _context.OrderItem.Add(orderItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrderItem", new { id = orderItem.OrderItemId }, orderItem);
        }

        // DELETE: api/JSTOrderItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CoreSite1.Models.OrderItem>> DeleteOrderItem(int id)
        {
            var orderItem = await _context.OrderItem.FindAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }

            _context.OrderItem.Remove(orderItem);
            await _context.SaveChangesAsync();

            return orderItem;
        }

        private bool OrderItemExists(int id)
        {
            return _context.OrderItem.Any(e => e.OrderItemId == id);
        }
    }
}
