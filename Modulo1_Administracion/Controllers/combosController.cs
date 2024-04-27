using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Firebase.Auth;
using Firebase.Storage;
using Modulo1_Administracion.Models;
using Modulo1_Administracion.Data;
using System.Data;

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
        public async Task<IActionResult> Index(int? numPag, string error)
        {
            //Llama a todos los registros de la tabla items_menu para mostrarlas en el combobox "Items".
            var listaDePlatos = (from m in _DulceSaborContext.items_menu
                                  select m).ToList();
            ViewData["listadoDePlatos"] = listaDePlatos;

            //Llama a todos los registros de la tabla estados para mostrarlas en el combobox "Estado".
            var listaDeEstados = (from m in _DulceSaborContext.estados
                                  where m.tipo_estado == "Menu"
                                 select m).ToList();
            ViewData["listadoDeEstados"] = listaDeEstados;

            //Llama a todos los registros de la tabla combos para mostrarlas en la tabla de la vista index de combos.
            var listadoDeCombos = (from m in _DulceSaborContext.combos
                                  select m);

            int cantidadRegistros = 7;

            //return View(await
            //Paginacion<combos>.CrearPaginacion( -> Paginacion -> Nombre de la clase -> <combos> -> <nombre del modelo al que se le va a hacer paginacion>
            //numPag?? 1 -> número de página (Viene de los botones de paginación del index)
            //cantidadRegistros -> Cuantos registros se van a mostrar por cada paginación
            //))

            //La clase "Paginacion" funciona para todos los controladores, solo copien el código del return View y modifiquen lo de "listadoDeCombos".
            return View(await Paginacion<combos>.CrearPaginacion(listadoDeCombos.AsNoTracking(), numPag?? 1, cantidadRegistros));
        }

        [HttpPost]
        public async Task<ActionResult> CreateCombo(IFormFile archivo, string nombre, string platos, decimal precio, int id_estado)
        {
            if (archivo != null && nombre != null && platos != null && precio > 0 && id_estado > 0)
            {
                /*  INICIO GUARDADO DE LA IMAGEN EN FIREBASE STORAGE   */

                //Leemos el archivo subido
                Stream archivoASubir = archivo.OpenReadStream();

                //Configuramos la conexion hacia FireBase
                string email = "rodrigo.monterrosa@catolica.edu.sv";
                string clave = "Sonic2002";
                string ruta = "prueba-dba43.appspot.com";
                string api_key = "AIzaSyBZhr-ye38uR6FnjSygen4Mo8Vph7s_HdU";

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

                /*  FIN GUARDADO DE LA IMAGEN EN FIREBASE STORAGE   */

                /*  INICIO GUARDADO DEL NUEVO REGISTRO "COMBO"   */

                //Se crea este objeto para poder guardar el nuevo registro
                combos combosObj = new combos();

                //Se asignan los valores del nuevo registro a los atributos del objeto
                //objeto.atributo = variable_recibida_en_este_metodo (CreateCombo);

                combosObj.descripcion = nombre;
                combosObj.precio = precio;
                combosObj.imagen = urlArchivoCargado;
                combosObj.id_estado = id_estado;

                //Se hace el INSERT INTO del registro a la tabla Combos.
                _DulceSaborContext.combos.Add(combosObj);
                _DulceSaborContext.SaveChanges();

                /*  FIN GUARDADO DEL NUEVO REGISTRO "COMBO"   */

                /*  INICIO GUARDADO DE TODOS LOS ITEMS(PLATOS) A UN SOLO COMBO   */

                //Lo que se hace es buscar el ID del último combo creado, o sea, el que necesitamos
                var listadoDeCombos = (from c in _DulceSaborContext.combos
                                       select c).ToList();

                //Variable donde se asignará el ID del último combo
                int id_combo = 0;

                //Se recorre toda la consulta hasta encontrar el ID requerido
                foreach (var item in (IEnumerable<dynamic>)listadoDeCombos)
                {
                    id_combo = item.id_combo;
                }

                //La variable "platos" es donde están todos los items elegidos para el combo
                //Lo que contiene es, por ejemplo: 4,5,4, | Cada número es el ID del item, pero hay un problema, el último número termina con una coma
                //Esto hace que se tome en cuenta el espacio después de la coma y haya un error. Por eso se hace lo siguiente:
                //platos.TrimEnd(',').Split(); -> .TrimEnd(',') -> Elimina la última coma. -> .Split(',') -> Divide la cadena por comas.
                //O sea que, en lugar de que 4,5,4, se cuente como una cadena, en realidad se toma como un array de 3 sin la coma final

                //Se elimina la coma final y se divide la cadena. El resultado se está guardando en un Array de Strings
                string[] numArrayPlatos = platos.TrimEnd(',').Split(',');

                //Los ID's ingresados a numArrayPlatos son String ya que vienen de un text area y llevan coma, entonces, necesitamos convertirlos a INT.
                //Para eso creamos este nuevo Array que servirá para la conversión. En new int[numArrayPlatos.Length] le estamos indicando el tamaño
                //que tendrá el array int según la cantidad de items que vayan al combo.
                int[] numIntPlatos = new int[numArrayPlatos.Length];

                //¿Para que necesitamos un FOR?
                //Para recorrer todos los numArrayPlatos (o sea, los ID's de los items), convertirlos string a int y hacer un INSERT INTO a la tabla
                //por cada item añadido al combo.
                for (int i = 0; i < numArrayPlatos.Length; i++)
                {
                    //Se crea un objeto para ayudarnos a ingresar los registros.
                    items_combo_dos items = new items_combo_dos();

                    //Aquí se hace la conversión
                    numIntPlatos[i] = Int32.Parse(numArrayPlatos[i]);

                    //Aquí se asignan los valores para cada campo de la tabla.
                    items.id_combo = id_combo;
                    items.id_items_menu = numIntPlatos[i];

                    //Y, por último, se hace el INSERT INTO por cada item añadido.
                    _DulceSaborContext.items_combo_dos.Add(items);
                    _DulceSaborContext.SaveChanges();
                }

                /*  FIN GUARDADO DE TODOS LOS ITEMS(PLATOS) A UN SOLO COMBO   */

                //Al final redirige al método Index, donde está la vista Index.
                return RedirectToAction("Index");
            }
            else
            {
                ViewData["errorMessage"] = "crear un nuevo combo";
                return View("ErrorCreateCombo");
            }
        }

        public async Task<IActionResult> Details(int? id, int? numPag)
        {
            //Se hace el llamado a la tabla estados para mostrar todos los estados disponibles en el combobox "Estado:".
            var listaDeEstados = (from m in _DulceSaborContext.estados
                                  where m.tipo_estado == "Menu"
                                  select m).ToList();

            //ViewData creado para poder usarlo en la vista Details.cshtml
            ViewData["listadoDeEstados"] = listaDeEstados;

            //Se llama al registro del cual se requieren sus campos
            var combo = (from c in _DulceSaborContext.combos
                         .Where(c => c.id_combo == id)
                         join e in _DulceSaborContext.estados on c.id_estado equals e.id_estado
                         select new
                         {
                             id = c.id_combo,
                             nombre = c.descripcion,
                             precio = c.precio,
                             estado = e.nombre,
                             id_estado = e.id_estado
                         }).ToList();

            //Se hace el ViewData para usarlo en la vista Details.cshtml, pero este es diferente.
            //Ya que la consulta solo es de un registro podemos hacer un objeto para usar sus atributos más fácil en la vista.
            ViewData["infoCombo"] = new obj_combo_estado()
            {
                id_combo = combo.AsEnumerable().FirstOrDefault().id,
                nombre = combo.AsEnumerable().FirstOrDefault().nombre,
                precio = combo.AsEnumerable().FirstOrDefault().precio,
                estado = combo.AsEnumerable().FirstOrDefault().estado,
                id_estado = combo.AsEnumerable().FirstOrDefault().id_estado
            };

            //Este es un llamado a una tabla, pero no cualquier tabla.
            //Esta llamando a una "VISTA" ("VIEW") de la base de datos. Para esto, también, se necesita crear una clase como modelo.
            //Como si fuese una tabla real.
            var listadoDeItemsCombos = (from otc in _DulceSaborContext.obj_items_combos
                                        where otc.id_combo == id
                                        select otc);

            //VieWData creado para poder usarlo en la vista Details.cshtml
            ViewData["listadoDeItemsCombos"] = listadoDeItemsCombos;

            int cantidadRegistros = 6;

            return View(await Paginacion<obj_items_combo>.CrearPaginacion(listadoDeItemsCombos.AsNoTracking(), numPag ?? 1, cantidadRegistros));
        }

        [HttpPost]
        public async Task<IActionResult> EditCombo(int? id, [Bind("id_combo, descripcion, precio, id_estado")] combos combos)
        {
            

            if (!string.IsNullOrEmpty(combos.descripcion) && combos.precio > 0)
            {
                //Aquí no hace falta hacer un objeto con sus asignaciones. Ya que es una actualización (también se puede para crear registros)
                //a una sola tabla. Los datos los toma desde los atributos que están en Bind en la parte de los parámetros de este metodo "EditCombo".
                _DulceSaborContext.Update(combos);
                await _DulceSaborContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                var unCombo = (from c in _DulceSaborContext.combos
                               where c.id_combo == combos.id_combo
                               select c).ToList();

                string nombreCombo = unCombo.AsEnumerable().FirstOrDefault().descripcion;

                ViewData["errorMessage"] = "editar el combo \"" + nombreCombo + "\"";
                return View("ErrorCreateCombo");
            }
        }
    }
}
