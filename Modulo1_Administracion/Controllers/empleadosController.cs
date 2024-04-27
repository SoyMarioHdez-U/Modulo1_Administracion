using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Modulo1_Administracion.Models;
using Modulo1_Administracion.Data;

namespace Modulo1_Administracion.Controllers
{
    public class empleadosController : Controller
    {
        private readonly DulceSaborContext _context;

        public empleadosController(DulceSaborContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> empleadosIndex(int? numPag) // Porque no utiliza IActionResult
        {
            var listaDeCargos = (from c in _context.cargos
                                 select c).ToList();
            ViewData["listadoDeCargos"] = new SelectList(listaDeCargos, "id_cargo", "cargo");

            var listaDeEstados = (from e in _context.estados
                                  where e.tipo_estado == "Trabajo"
                                  select e).ToList();
            ViewData["listadoDeEstados"] = new SelectList(listaDeEstados, "id_estado", "nombre");


            var listadoDeEmpleados = (from v in _context.v_empleado_cargo
                                      select v);

            int cantidadRegistros = 7;

            return View(await Paginacion<v_empleado_cargo>.CrearPaginacion(listadoDeEmpleados.AsNoTracking(), numPag ?? 1, cantidadRegistros));
        }
        public ActionResult CrearEmpleado(empleados nuevoEmpleado) // Porque no utiliza IActionResult
        {
            _context.Add(nuevoEmpleado);
            _context.SaveChanges();
            return RedirectToAction("empleadosIndex");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarEstado(int id)
        {
            var empleado = await _context.empleados.FindAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }

            
            if (empleado.id_estado == 5)
            {
                empleado.id_estado = 6;
            }
            else if (empleado.id_estado == 6)
            {
                empleado.id_estado = 5;
            }

            
            await _context.SaveChangesAsync();

            
            return RedirectToAction(nameof(empleadosIndex));
        }





























        // GET: empleados
        public async Task<IActionResult> Index()
        {
            return View(await _context.empleados.ToListAsync());
        }

        // GET: empleados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleados = await _context.empleados
                .FirstOrDefaultAsync(m => m.id_empleado == id);
            if (empleados == null)
            {
                return NotFound();
            }

            return View(empleados);
        }

        // GET: empleados/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: empleados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_empleado,nombre,apellido,direccion,telefono,correo,id_cargo,id_estado")] empleados empleados)
        {
            if (ModelState.IsValid)
            {
                _context.Add(empleados);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(empleados);
        }

        // GET: empleados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleados = await _context.empleados.FindAsync(id);
            if (empleados == null)
            {
                return NotFound();
            }
            return View(empleados);
        }

        // POST: empleados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_empleado,nombre,apellido,direccion,telefono,correo,id_cargo,id_estado")] empleados empleados)
        {
            if (id != empleados.id_empleado)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empleados);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!empleadosExists(empleados.id_empleado))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(empleadosIndex));
            }
            return View(empleados);
        }

        // GET: empleados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleados = await _context.empleados
                .FirstOrDefaultAsync(m => m.id_empleado == id);
            if (empleados == null)
            {
                return NotFound();
            }

            return View(empleados);
        }

        // POST: empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empleados = await _context.empleados.FindAsync(id);
            if (empleados != null)
            {
                _context.empleados.Remove(empleados);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool empleadosExists(int id)
        {
            return _context.empleados.Any(e => e.id_empleado == id);
        }
    }
}
