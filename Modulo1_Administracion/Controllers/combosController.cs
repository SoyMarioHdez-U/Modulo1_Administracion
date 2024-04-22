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
            ViewData["listadoDePlatos"] = listaDePlatos;

            var listaDeEstados = (from m in _DulceSaborContext.estados
                                 select m).ToList();
            ViewData["listadoDeEstados"] = listaDeEstados;

            //Aquí estamos invocando el listado de departamentos de la tabla departamentos
            //var listaDeDepartamentos = (from m in _DulceSaborContext.departamentos
            //                            select m).ToList();
            //ViewData["listadoDeDepartamentos"] = new SelectList(listaDeDepartamentos, "id", "departamento");



            //Aquí estamos solicitando el listado de los Departamentos en la bd
            var listadoDeCombos = (from c in _DulceSaborContext.combos
                                       select new
                                       {
                                           id = c.id_combo,
                                           nombre = c.descripcion,
                                           precio = c.precio
                                       }).ToList();
            ViewData["listadoDeCombos"] = listadoDeCombos;

            return View();
        }

        [HttpPost]
        //public async Task<ActionResult> SubirArchivo(IFormFile archivo)
        //{
        //    //Leemos el archivo subido
        //    Stream archivoASubir = archivo.OpenReadStream();

        //    //Configuramos la conexion hacia FireBase
        //    string email = "carlos.murga1@catolica.edu.sv";
        //    string clave = "h1n12002";
        //    string ruta = "dulcesabor-imagenes.appspot.com";
        //    string api_key = "AIzaSyCUmhGjhkkuvkE5S5bnPXjTHIYn9qW5pl4";

        //    var auth = new FirebaseAuthProvider(new FirebaseConfig(api_key));
        //    var autenticarFireBase = await auth.SignInWithEmailAndPasswordAsync(email, clave);

        //    var cancellation = new CancellationTokenSource();
        //    var tokenUser = autenticarFireBase.FirebaseToken;

        //    var tareaCargarArchivo = new FirebaseStorage(ruta,
        //        new FirebaseStorageOptions
        //        {
        //            AuthTokenAsyncFactory = () => Task.FromResult(tokenUser),
        //            ThrowOnCancel = true
        //        }).Child("Combos").Child(archivo.FileName).PutAsync(archivoASubir, cancellation.Token);

        //    var urlArchivoCargado = await tareaCargarArchivo;

        //    return urlArchivoCargado;
        //}

        // GET: combos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return View();
        }

        // GET: combos/Create
        public IActionResult CreateCombo(items_combo_menu combo_menu)
        {
            int id_combo = 0;
            combos combosObj = new combos();
            items_combo_dos items = new items_combo_dos();

            try
            {
                

            } catch (Exception ex) { ex.ToString(); }

            

            combosObj.descripcion = combo_menu.descripcion;
            combosObj.precio = combo_menu.precio;
            combosObj.imagen = combo_menu.imagen;
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

            items.id_combo = id_combo;
            items.id_items_menu = combo_menu.id_items_menu;

            _DulceSaborContext.items_combo_dos.Add(items);
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
