using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Modulo1_Administracion.Data;
using Modulo1_Administracion.Models;

namespace Modulo1_Administracion.Controllers
{
    public class mesasController : Controller
    {
        private readonly DulceSaborContext _context;

        public mesasController(DulceSaborContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index_mesa(int? numPag)
        {

            var listaDeEstados = (from e in _context.estados
                                  where e.tipo_estado == "Mesa"
                                  select e).ToList();
            ViewData["listadoDeEstados"] = new SelectList(listaDeEstados, "id_estado", "nombre");


            var listadoDeMesas = (from v in _context.v_mesas_estado
                                  select v);

            int cantidadRegistros = 5;

            return View(await Paginacion<v_mesas_estado>.CrearPaginacion(listadoDeMesas.AsNoTracking(), numPag?? 1, cantidadRegistros));
        }



        [HttpPost]
        public ActionResult CrearMesa(mesas nuevaMesa)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    
                    _context.mesas.Add(nuevaMesa);

                    
                    _context.SaveChanges();

                    
                    return RedirectToAction("Index_mesa");
                }
                catch (Exception ex)
                {
                   
                    Console.WriteLine("Error al guardar la nueva mesa: " + ex.Message);
                    return RedirectToAction("Error"); 
                }
            }
            else
            {
                
                return View(nuevaMesa);
            }
        }


        // GET: mesas
        public async Task<IActionResult> Index()
        {
            return View(await _context.mesas.ToListAsync());
        }

        // GET: mesas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mesas = await _context.mesas
                .FirstOrDefaultAsync(m => m.id_mesa == id);
            if (mesas == null)
            {
                return NotFound();
            }

            return View(mesas);
        }

        // GET: mesas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: mesas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_mesa,cantidad_personas,id_estado,nombre_mesa")] mesas mesas)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mesas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mesas);
        }

        // GET: mesas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mesas = await _context.mesas.FindAsync(id);
            if (mesas == null)
            {
                return NotFound();
            }
            return View(mesas);
        }

        // POST: mesas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_mesa,cantidad_personas,id_estado,nombre_mesa")] mesas mesas)
        {
            if (id != mesas.id_mesa)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mesas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!mesasExists(mesas.id_mesa))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index_mesa));
            }
            return View(mesas);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarEstado(int id)
        {
            var mesa = await _context.mesas.FindAsync(id);
            if (mesa == null)
            {
                return NotFound();
            }

            
            if (mesa.id_estado == 3)
            {
                mesa.id_estado = 4; 
            }
            else if (mesa.id_estado == 4)
            {
                mesa.id_estado = 3; 
            }       
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index_mesa));
        }



        // GET: mesas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mesas = await _context.mesas
                .FirstOrDefaultAsync(m => m.id_mesa == id);
            if (mesas == null)
            {
                return NotFound();
            }

            return View(mesas);
        }

        // POST: mesas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mesas = await _context.mesas.FindAsync(id);
            if (mesas != null)
            {
                _context.mesas.Remove(mesas);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool mesasExists(int id)
        {
            return _context.mesas.Any(e => e.id_mesa == id);
        }
    }
}
