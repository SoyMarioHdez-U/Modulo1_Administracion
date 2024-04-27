using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modulo1_Administracion.Data;
using Modulo1_Administracion.Models;

namespace Modulo1_Administracion.Controllers
{
    public class promocionesController : Controller
    {
        private readonly DulceSaborContext _DulceSaborContext;

        public promocionesController(DulceSaborContext context)
        {
            _DulceSaborContext = context;
        }
        

        // GET: promociones
        public async Task<IActionResult> Index(int? numPag)
        {
            //Llama a todos los registros de la tabla items_menu para mostrarlas en el combobox "Items".
            var listaDePlatos = (from m in _DulceSaborContext.items_menu
                                 select m).ToList();
            ViewData["listadoDePlatos"] = listaDePlatos;

            //Llama a todos los registros de la tabla estados para mostrarlas en el combobox "Estado".
            var listaDeEstados = (from m in _DulceSaborContext.estados
                                  select m).ToList();
            ViewData["listadoDeEstados"] = listaDeEstados;

            //Llama a todos los registros de la tabla  promociones para mostrarlas en la tabla de la vista index de promociones.
            var listadoDePromociones = (from m in _DulceSaborContext.promociones
                                   select m);

            int cantidadRegistros = 15;

            //return View(await
            //Paginacion<items_promo>.CrearPaginacion( -> Paginacion -> Nombre de la clase -> <items_promo> -> <nombre del modelo al que se le va a hacer paginacion>
            //numPag?? 1 -> número de página (Viene de los botones de paginación del index)
            //cantidadRegistros -> Cuantos registros se van a mostrar por cada paginación
            //))

           
            return View(await Paginacion<promociones>.CrearPaginacion(listadoDePromociones.AsNoTracking(), numPag ?? 1, cantidadRegistros));
        }


        [HttpPost]
        public async Task<ActionResult> CreatePromociones(IFormFile archivo,string descripcion,
            decimal precio, DateTime fecha_inicio, DateTime fecha_final,string imagen, int id_estado, string nombre)
        {
            /*  INICIO GUARDADO DE LA IMAGEN EN FIREBASE STORAGE   */

            //Leemos el archivo subido
            Stream archivoASubir = archivo.OpenReadStream();

            //Configuramos la conexion hacia FireBase
            string email = "arrivillagariveran@gmail.com";
            string clave = "12&10#2017*";
            string ruta = "nose-819e2.appspot.com";
            string api_key = "AIzaSyCO4YDlTb4ZQo08K5mAfeReGLjKDFQpAVQ";

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

            /*  INICIO GUARDADO DEL NUEVO REGISTRO "Promociones"   */

            //Se crea este objeto para poder guardar el nuevo registro
            promociones promosObj = new promociones();

            //Se asignan los valores del nuevo registro a los atributos del objeto
            //objeto.atributo = variable_recibida_en_este_metodo (Createpromociones);

            promosObj.descripcion = descripcion;
            promosObj.precio = precio;
            promosObj.fecha_inicio = fecha_inicio;
            promosObj.fecha_final = fecha_final;
            promosObj.imagen = urlArchivoCargado;
            promosObj.id_estado = id_estado;
            promosObj.nombre = nombre;

            //Se hace el INSERT INTO del registro a la tabla Combos.
            _DulceSaborContext.promociones.Add(promosObj);
            _DulceSaborContext.SaveChanges();

            /*  FIN GUARDADO DEL NUEVO REGISTRO "promociones"   */

            /*  INICIO GUARDADO DE TODOS LOS ITEMS(PLATOS) A UNa sola propomocion   */

            //Lo que se hace es buscar el ID de la utima promocion creada, o sea, la que necesitamos
            var listadoDePromociones = (from c in _DulceSaborContext.promociones
                                   select c).ToList();

            //Variable donde se asignará el ID del último combo
            int id_promo = 0;

            //Se recorre toda la consulta hasta encontrar el ID requerido
            foreach (var item in (IEnumerable<dynamic>)listadoDePromociones)
            {
                id_promo = item.id_promo;
            }

            //La variable "descripcion" es donde están todos los items elegidos para el combo
            //Lo que contiene es, por ejemplo: 4,5,4, | Cada número es el ID del item, pero hay un problema, el último número termina con una coma
            //Esto hace que se tome en cuenta el espacio después de la coma y haya un error. Por eso se hace lo siguiente:
            //platos.TrimEnd(',').Split(); -> .TrimEnd(',') -> Elimina la última coma. -> .Split(',') -> Divide la cadena por comas.
            //O sea que, en lugar de que 4,5,4, se cuente como una cadena, en realidad se toma como un array de 3 sin la coma final

            //Se elimina la coma final y se divide la cadena. El resultado se está guardando en un Array de Strings
            string[] numArrayDescripcion = descripcion.TrimEnd(',').Split(',');

            //Los ID's ingresados a numArrayPlatos son String ya que vienen de un text area y llevan coma, entonces, necesitamos convertirlos a INT.
            //Para eso creamos este nuevo Array que servirá para la conversión. En new int[numArrayPlatos.Length] le estamos indicando el tamaño
            //que tendrá el array int según la cantidad de items que vayan al combo.
            int[] numIntDescripcion = new int[numArrayDescripcion.Length];

            //¿Para que necesitamos un FOR?
            //Para recorrer todos los numArrayDescripcion (o sea, los ID's de los items), convertirlos string a int y hacer un INSERT INTO a la tabla
            //por cada item añadido al combo.
            for (int i = 0; i < numArrayDescripcion.Length; i++)
            {
                //Se crea un objeto para ayudarnos a ingresar los registros.
                items_promociones items = new items_promociones();

                //Aquí se hace la conversión
                numIntDescripcion[i] = Int32.Parse(numArrayDescripcion[i]);

                //Aquí se asignan los valores para cada campo de la tabla.
                items.id_promo = id_promo;
                items.id_items_combo = numIntDescripcion[i];

                //Y, por último, se hace el INSERT INTO por cada item añadido.
                _DulceSaborContext.items_promo.Add(items);
                _DulceSaborContext.SaveChanges();
            }

            /*  FIN GUARDADO DE TODOS LOS ITEMS(PLATOS) A UN SOLO COMBO   */

            //Al final redirige al método Index, donde está la vista Index.
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int? id, int? numPag)
        {
            //Se hace el llamado a la tabla estados para mostrar todos los estados disponibles en el combobox "Estado:".
            var listaDeEstados = (from m in _DulceSaborContext.estados
                                  select m).ToList();

            //ViewData creado para poder usarlo en la vista Details.cshtml
            ViewData["listadoDeEstados"] = listaDeEstados;

            //Se llama al registro del cual se requieren sus campos
            var promo = (from c in _DulceSaborContext.promociones
                         .Where(c => c.id_promo == id)
                         join e in _DulceSaborContext.estados on c.id_estado equals e.id_estado
                         select new
                         {
                             id = c.id_promo,
                             nombre = c.descripcion,
                             precio = c.precio,
                             fecha_inicio = c.fecha_inicio,
                             fecha_final = c.fecha_final,
                             estado = e.nombre
                         }).ToList();

            //Se hace el ViewData para usarlo en la vista Details.cshtml, pero este es diferente.
            //Ya que la consulta solo es de un registro podemos hacer un objeto para usar sus atributos más fácil en la vista.
            ViewData["infoPromo"] = new bj_promo_estado()
            {
                id_promo = promo.AsEnumerable().FirstOrDefault().id,
                nombre = promo.AsEnumerable().FirstOrDefault().nombre,
                precio = promo.AsEnumerable().FirstOrDefault().precio,
                fecha_inicio = promo.AsEnumerable().FirstOrDefault().fecha_inicio,
                fecha_final = promo.AsEnumerable().FirstOrDefault().fecha_final,
                estado = promo.AsEnumerable().FirstOrDefault().estado
            };

            //Este es un llamado a una tabla, pero no cualquier tabla.
            //Esta llamando a una "VISTA" ("VIEW") de la base de datos. Para esto, también, se necesita crear una clase como modelo.
            //Como si fuese una tabla real.
            var listadoDeItemsPromo = (from otc in _DulceSaborContext.v_itemsPromoCombos

                                       where otc.id == id
                                       select otc);

            ////VieWData creado para poder usarlo en la vista Details.cshtml
            ViewData["listadoDeItemsPromociones"] = listadoDeItemsPromo;

            int cantidadRegistros = 6;

            return View(await Paginacion<v_itemsPromoCombos>.CrearPaginacion(listadoDeItemsPromo.AsNoTracking(), numPag ?? 1, cantidadRegistros));
        }


        [HttpPost]
        public async Task<IActionResult> EditPromo(int id, [Bind("id_promo, descripcion, precio,fecha_inicio,fecha_final, id_estado")] promociones promociones)
        {
            //Aquí no hace falta hacer un objeto con sus asignaciones. Ya que es una actualización (también se puede para crear registros)
            //a una sola tabla. Los datos los toma desde los atributos que están en Bind en la parte de los parámetros de este metodo "EditPromo".
            _DulceSaborContext.Update(promociones);
            await _DulceSaborContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }




    }
}