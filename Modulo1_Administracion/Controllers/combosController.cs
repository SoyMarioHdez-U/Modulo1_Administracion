using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Modulo1_Administracion.Models;

namespace Modulo1_Administracion.Controllers
{
    public class combosController : Controller
    {
        private readonly DulceSaborContext _DulceSaborContext;

        public combosController(DulceSaborContext context)
        {
            _DulceSaborContext = context;
        }

        // GET: combos
        public IActionResult Index()

        {

            //Aquí estamos invocando el listado de puestos de la tabla puestos
            var listaDePlatos = (from m in _DulceSaborContext.items_menu
                                  select m).ToList();
            ViewData["listadoDePlatos"] = new SelectList(listaDePlatos, "id_item_menu", "nombre");

            var listaDeEstados = (from m in _DulceSaborContext.estados
                                 select m).ToList();
            ViewData["listadoDeEstados"] = new SelectList(listaDeEstados, "id_estado", "nombre");

            //Aquí estamos invocando el listado de departamentos de la tabla departamentos
            //var listaDeDepartamentos = (from m in _DulceSaborContext.departamentos
            //                            select m).ToList();
            //ViewData["listadoDeDepartamentos"] = new SelectList(listaDeDepartamentos, "id", "departamento");



            //Aquí estamos solicitando el listado de los Departamentos en la bd
            //var listadoDeDeClientes = (from c in _DulceSaborContext.clientes
            //                           join d in _DulceSaborContext.departamentos on c.id_departamento equals d.id
            //                           join p in _DulceSaborContext.puestos on c.id_puesto equals p.id
            //                           select new
            //                           {
            //                               id = c.id,

            //                               nombre = c.nombre,
            //                               apellido = c.apellido,
            //                               email = c.email,
            //                               direccion = c.direccion,
            //                               genero = c.genero,
            //                               id_departamento = d.departamento,
            //                               id_puesto = p.puesto,
            //                               estado_registro = c.estado_registro,
            //                               created_at = c.created_at,
            //                               updated_at = c.updated_at

            //                           }).ToList();
            //ViewData["listadoDeClientes"] = listadoDeDeClientes;



            return View();
        }

        // GET: combos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return View();
        }

        // GET: combos/Create
        public IActionResult CreateCombo(combos nuevoCombos)
        {
            _DulceSaborContext.Add(nuevoCombos);
            _DulceSaborContext.SaveChanges();

            return RedirectToAction("Index");

        }

        // POST: combos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_combo,descripcion,precio,imagen,id_estado")] combos combos)
        {
            
            return View();
        }

        // GET: combos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return View();
        }

        // POST: combos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_combo,descripcion,precio,imagen,id_estado")] combos combos)
        {
            return View(combos);
        }

        // GET: combos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return View();
        }

        // POST: combos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            return RedirectToAction();
        }
    }
}
