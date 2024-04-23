using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Modulo1_Administracion.Models;

namespace Modulo1_Administracion.Controllers
{
    public class items_menuController : Controller
    {
        private readonly DulceSaborContext _context;

        public items_menuController(DulceSaborContext context)
        {
            _context = context;
            
        }

        [HttpGet]
        public ActionResult items_menuNew()
        {
            var listaDeCategorias = (from c in _context.categorias
                                     select c).ToList();
            ViewData["listadoDeCategorias"] = new SelectList(listaDeCategorias, "id_categoria", "categoria");

            var listaDeEstados = (from e in _context.estados
                                  where e.tipo_estado == "Menu"
                                  select e).ToList();
            ViewData["listadoDeEstados"] = new SelectList(listaDeEstados, "id_estado", "nombre");

            var listadoDeItems = (from e in _context.items_menu
                                    join m in _context.estados on e.id_estado equals m.id_estado
                                    select new
                                    {
                                        nombre = e.nombre,
                                        estado = m.nombre,
                                        
                                    }
                                    ).ToList();
            ViewData["listadoDeItems"] = listadoDeItems;



            return View();
        }


        [HttpPost]
        public async  Task<ActionResult> CrearItem(items_menu nuevoItem, IFormFile imagen)
        {

            // Leemos el archivo subido
            Stream archivoASubir = imagen.OpenReadStream();

            // Configuramos la conexión hacia Firebase
            string email = "soymariohdez@gmail.com";
            string clave = "catolica";
            string ruta = "dulcesabor-c6f5a.appspot.com";
            string api_key = "AIzaSyDCBa09cv8YGDb8dc7loaIh-D9eG5XGXjI";

            var auth = new FirebaseAuthProvider(new FirebaseConfig(api_key));
            var autenticarFireBase = await auth.SignInWithEmailAndPasswordAsync(email, clave);
            var cancellation = new CancellationTokenSource();
            var tokenUser = autenticarFireBase.FirebaseToken;

            // Subir archivo a Firebase
            var tareaCargarArchivo = new FirebaseStorage(ruta,
                                                        new FirebaseStorageOptions
                                                        {
                                                            AuthTokenAsyncFactory = () => Task.FromResult(tokenUser),
                                                            ThrowOnCancel = true
                                                        }
                                                        ).Child("ItemsMenu").Child(imagen.FileName).PutAsync(archivoASubir, cancellation.Token);

            // Esperar a que se complete la tarea de cargar el archivo
            var urlArchivoCargado = await tareaCargarArchivo;


            //Se agrega a la base de datos


            _context.Add(nuevoItem);
            nuevoItem.imagen = urlArchivoCargado;
            _context.SaveChanges();

            return RedirectToAction("items_menuNew");
        }





        // GET: items_menu
        public async Task<IActionResult> Index()
        {
            return View(await _context.items_menu.ToListAsync());
        }

        // GET: items_menu/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var items_menu = await _context.items_menu
                .FirstOrDefaultAsync(m => m.id_item_menu == id);
            if (items_menu == null)
            {
                return NotFound();
            }

            return View(items_menu);
        }

        // GET: items_menu/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: items_menu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_item_menu,nombre,descripcion,precio,imagen,id_estado,id_categoria")] items_menu items_menu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(items_menu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(items_menu);
        }

        // GET: items_menu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var items_menu = await _context.items_menu.FindAsync(id);
            if (items_menu == null)
            {
                return NotFound();
            }
            return View(items_menu);
        }

        
        // POST: items_menu/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_item_menu,nombre,descripcion,precio,imagen,id_estado,id_categoria")] items_menu items_menu)
        {
            if (id != items_menu.id_item_menu)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(items_menu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!items_menuExists(items_menu.id_item_menu))
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
            return View(items_menu);
        }

        // GET: items_menu/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var items_menu = await _context.items_menu
                .FirstOrDefaultAsync(m => m.id_item_menu == id);
            if (items_menu == null)
            {
                return NotFound();
            }

            return View(items_menu);
        }

        // POST: items_menu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var items_menu = await _context.items_menu.FindAsync(id);
            if (items_menu != null)
            {
                _context.items_menu.Remove(items_menu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool items_menuExists(int id)
        {
            return _context.items_menu.Any(e => e.id_item_menu == id);
        }
    }
}
