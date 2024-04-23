using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Firebase.Auth;
using Firebase.Storage;
using Modulo1_Administracion.Models;
using static System.Formats.Asn1.AsnWriter;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Modulo1_Administracion.Data;

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
        public async Task<IActionResult> Index(int? numPag)
        {
            //Aquí estamos invocando el listado de puestos de la tabla puestos
            var listaDePlatos = (from m in _DulceSaborContext.items_menu
                                  select m).ToList();
            ViewData["listadoDePlatos"] = listaDePlatos;

            var listaDeEstados = (from m in _DulceSaborContext.estados
                                 select m).ToList();
            ViewData["listadoDeEstados"] = listaDeEstados;

            //Aquí estamos invocando el listado de departamentos de la tabla departamentos
            //var listaDeDepartamentos = (from m in _DulceSaborContext.departamentos
            //                            select m).ToList();
            //ViewData["listadoDeDepartamentos"] = new SelectList(listaDeDepartamentos, "id", "departamento");



            //Aquí estamos solicitando el listado de los Departamentos en la bd
            var combosL = (from m in _DulceSaborContext.combos
                                  select m);
            ViewData["combosL"] = combosL;

            var listadoDeCombos = (from c in _DulceSaborContext.combos
                                       select new
                                       {
                                           id = c.id_combo,
                                           nombre = c.descripcion,
                                           precio = c.precio
                                       });
            ViewData["listadoDeCombos"] = listadoDeCombos.ToList();

            int cantidadRegistros = 15;

            return View(await Paginacion<combos>.CrearPaginacion(combosL.AsNoTracking(), numPag?? 1, cantidadRegistros));
        }

        // GET: combos/Create
        [HttpPost]
        public async Task<ActionResult> CreateCombo(IFormFile archivo, items_combo_menu combo_menu)
        {
            int id_combo = 0;
            combos combosObj = new combos();

            //Leemos el archivo subido
            Stream archivoASubir = archivo.OpenReadStream();

            //Configuramos la conexion hacia FireBase
            string email = "carlos.murga1@catolica.edu.sv";
            string clave = "h1n12002";
            string ruta = "dulcesabor-imagenes.appspot.com";
            string api_key = "AIzaSyCUmhGjhkkuvkE5S5bnPXjTHIYn9qW5pl4";

            var auth = new FirebaseAuthProvider(new FirebaseConfig(api_key));
            var autenticarFireBase = await auth.SignInWithEmailAndPasswordAsync(email, clave);

            var cancellation = new CancellationTokenSource();
            var tokenUser = autenticarFireBase.FirebaseToken;

            var tareaCargarArchivo = new FirebaseStorage(ruta,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(tokenUser),
                    ThrowOnCancel = true
                }).Child("Combos").Child(archivo.FileName).PutAsync(archivoASubir, cancellation.Token);

            var urlArchivoCargado = await tareaCargarArchivo;


            try
            {


            }
            catch (Exception ex) { ex.ToString(); }

            combosObj.descripcion = combo_menu.nombre;
            combosObj.precio = combo_menu.precio;
            combosObj.imagen = urlArchivoCargado;
            combosObj.id_estado = combo_menu.id_estado;

            _DulceSaborContext.combos.Add(combosObj);
            _DulceSaborContext.SaveChanges();

            var listadoDeCombos = (from c in _DulceSaborContext.combos
                                   select new
                                   {
                                       id = c.id_combo,
                                       nombre = c.descripcion,
                                       precio = c.precio
                                   }).ToList();
            ViewData["listadoDeCombos"] = listadoDeCombos;

            foreach (var item in (IEnumerable<dynamic>)ViewData["listadoDeCombos"])
            {
                id_combo = item.id;
            }


            string numPlatos = combo_menu.descripcion.ToString();
            string[] numArrayPlatos = numPlatos.TrimEnd(',').Split(',');
            int[] numIntPlatos = new int[numArrayPlatos.Length];

            for (int i = 0; i < numArrayPlatos.Length; i++)
            {
                items_combo_dos items = new items_combo_dos();

                numIntPlatos[i] = Int32.Parse(numArrayPlatos[i]);

                items.id_combo = id_combo;
                items.id_items_menu = numIntPlatos[i];

                _DulceSaborContext.items_combo_dos.Add(items);
                _DulceSaborContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int? id)
        {
            var listaDeEstados = (from m in _DulceSaborContext.estados
                                  select m).ToList();
            ViewData["listadoDeEstados"] = listaDeEstados;

            if (id == null)
            {
                return NotFound();
            }

            var combo = (from c in _DulceSaborContext.combos
                                   .Where(c => c.id_combo == id)
                                   select new
                                   {
                                       id = c.id_combo,
                                       nombre = c.descripcion,
                                       precio = c.precio
                                   }).ToList();

            var itemsCombo = (from ic in _DulceSaborContext.items_combo_dos
                              .Where(c => c.id_combo == id)
                              join c in _DulceSaborContext.combos on ic.id_combo equals c.id_combo
                              join im in _DulceSaborContext.items_menu on ic.id_items_menu equals im.id_item_menu
                              select new
                              {
                                id = ic.id_items_combo,
                                nombre_plato = im.nombre
                              }).ToList();

            ViewData["itemsCombo"] = itemsCombo;

            var combos = await _DulceSaborContext.combos
                .FirstOrDefaultAsync(m => m.id_combo == id);
            if (combos == null)
            {
                return NotFound();
            }

            return View(combos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCombo(int id, [Bind("id_combo, descripcion, precio, id_estado")] combos combos)
        {
            _DulceSaborContext.Update(combos);
            await _DulceSaborContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
