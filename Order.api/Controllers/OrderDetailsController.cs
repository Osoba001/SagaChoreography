using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AbstractedRabbitMQ.Publishers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Order.api.DataAcess;
using Order.api.Models;
using SagaModel.Orders;

namespace Order.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly OrderDbContext _context;
        private readonly IPublisher _publisher;

        public OrderDetailsController(OrderDbContext context, IPublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }

        // GET: api/OrderDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDetail>>> GetOderDetail()
        {
          if (_context.OderDetail == null)
          {
              return NotFound();
          }
            return await _context.OderDetail.ToListAsync();
        }

        // GET: api/OrderDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetail>> GetOrderDetail(int id)
        {
          if (_context.OderDetail == null)
          {
              return NotFound();
          }
            var orderDetail = await _context.OderDetail.FindAsync(id);

            if (orderDetail == null)
            {
                return NotFound();
            }

            return orderDetail;
        }

        // PUT: api/OrderDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderDetail(int id, OrderDetail orderDetail)
        {
            if (id != orderDetail.Id)
            {
                return BadRequest();
            }

            _context.Entry(orderDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderDetailExists(id))
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

        // POST: api/OrderDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderDetail>> PostOrderDetail(OrderDetail orderDetail)
        {
          if (_context.OderDetail == null)
          {
              return Problem("Entity set 'OrderDbContext.OderDetail'  is null.");
          }
           var res= _context.OderDetail.Add(orderDetail);
            await _context.SaveChangesAsync();
            var orderReq = new OrderRequest { OrderId = orderDetail.Order.Id, ProductId = orderDetail.ProductId, Quantity = orderDetail.Quantity };
            _publisher.Publish<OrderRequest>(orderReq,"order.created",null,null);

            return CreatedAtAction("GetOrderDetail", new { id = orderDetail.Id }, orderDetail);
        }

        // DELETE: api/OrderDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDetail(int id)
        {
            if (_context.OderDetail == null)
            {
                return NotFound();
            }
            var orderDetail = await _context.OderDetail.FindAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            _context.OderDetail.Remove(orderDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderDetailExists(int id)
        {
            return (_context.OderDetail?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
