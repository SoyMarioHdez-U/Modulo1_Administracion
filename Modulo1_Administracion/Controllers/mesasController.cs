﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet]
        public ActionResult MesasNew()
        {

            var listaDeEstados = (from e in _context.estados
                                  where e.tipo_estado == "Mesa"
                                  select e).ToList();
            ViewData["listadoDeEstados"] = new SelectList(listaDeEstados, "id_estado", "nombre");


            var listaDeMesas = (from m in _context.mesas
                                join e in _context.estados on m.id_estado equals e.id_estado
                                select new
                                {
                                    id = m.id_mesa,     
                                    cantidad_personas = m.cantidad_personas,
                                    estado = e.nombre,
                                    nombre_mesa = m.nombre_mesa
                                }).ToList();
            ViewData["listadoDeMesas"] = listaDeMesas;
            return View();
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
                return RedirectToAction(nameof(MesasNew));
            }
            return View(mesas);
        }

        //POST: Editar estado a inactivo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarEstado(int id)
        {
            var item = await _context.items_menu.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            // Cambia el valor de id_estado entre 1 y 2
            item.id_estado = (item.id_estado == 1) ? 2 : 1;

            // Guarda los cambios en la base de datos
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(MesasNew));
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