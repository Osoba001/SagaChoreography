using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventory.mvc.DataAccess;
using Inventory.mvc.Models;

namespace Inventory.mvc.Controllers
{
    public class InventoryModelsController : Controller
    {
        private readonly InventoryDbContext _context;

        public InventoryModelsController(InventoryDbContext context)
        {
            _context = context;
        }

        // GET: InventoryModels
        public async Task<IActionResult> Index()
        {
              return _context.Inventory != null ? 
                          View(await _context.Inventory.ToListAsync()) :
                          Problem("Entity set 'InventoryDbContext.Inventory'  is null.");
        }

        // GET: InventoryModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Inventory == null)
            {
                return NotFound();
            }

            var inventoryModel = await _context.Inventory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventoryModel == null)
            {
                return NotFound();
            }

            return View(inventoryModel);
        }

        // GET: InventoryModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: InventoryModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Quantity,ProductId")] InventoryModel inventoryModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inventoryModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(inventoryModel);
        }

        // GET: InventoryModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Inventory == null)
            {
                return NotFound();
            }

            var inventoryModel = await _context.Inventory.FindAsync(id);
            if (inventoryModel == null)
            {
                return NotFound();
            }
            return View(inventoryModel);
        }

        // POST: InventoryModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Quantity,ProductId")] InventoryModel inventoryModel)
        {
            if (id != inventoryModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventoryModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryModelExists(inventoryModel.Id))
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
            return View(inventoryModel);
        }

        // GET: InventoryModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Inventory == null)
            {
                return NotFound();
            }

            var inventoryModel = await _context.Inventory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventoryModel == null)
            {
                return NotFound();
            }

            return View(inventoryModel);
        }

        // POST: InventoryModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Inventory == null)
            {
                return Problem("Entity set 'InventoryDbContext.Inventory'  is null.");
            }
            var inventoryModel = await _context.Inventory.FindAsync(id);
            if (inventoryModel != null)
            {
                _context.Inventory.Remove(inventoryModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventoryModelExists(int id)
        {
          return (_context.Inventory?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
