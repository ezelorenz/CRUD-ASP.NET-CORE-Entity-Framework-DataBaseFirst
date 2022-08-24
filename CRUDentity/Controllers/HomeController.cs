using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRUDentity.Models;
using CRUDentity.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CRUDentity.Controllers
{
    public class HomeController : Controller
    {
        private readonly CRUDENTITYContext _DBcontext;
        public HomeController(CRUDENTITYContext context)
        {
            _DBcontext = context;
        }

        public IActionResult Index()
        {
            //creacion lista empleados donde seran asignados cada uno de los que estén en la DB

            List<Empleado> lista = _DBcontext.Empleados.Include(c => c.oCargo).ToList();
            return View(lista);
        }
        [HttpGet] //Creacion de objeto empleado, vacio, donde lo único que va a tener 
                  // es la lista desplagable del cargo de cada uno. Se puede ver en Value
        public IActionResult Empleado_Detalle(int idEmpleado)
        {
            EmpleadoVM oEmpleadoVM = new EmpleadoVM()
            {
                oEmpleado = new Empleado(),
                oListaCargo = _DBcontext.Cargos.Select(cargo => new SelectListItem()
                {
                    Text = cargo.Descripcion,
                    Value = cargo.IdCargo.ToString()
                }).ToList()
            };
           
            if(idEmpleado != 0)
            {
                oEmpleadoVM.oEmpleado = _DBcontext.Empleados.Find(idEmpleado);
            }

            return View(oEmpleadoVM);


        }

        [HttpPost]
        public IActionResult Empleado_Detalle(EmpleadoVM oEmpleadoVM)
        {   //Si no hay empleado asignado, crear empleado nuevo
            if (oEmpleadoVM.oEmpleado.IdEmpleado == 0)
            {
                _DBcontext.Empleados.Add(oEmpleadoVM.oEmpleado);
            }
            else
            {
                _DBcontext.Empleados.Update(oEmpleadoVM.oEmpleado);
            }

            //Guardar los cambios en la base de datos
            _DBcontext.SaveChanges();

            //Redirecciona hacia la lista de empleados
            return RedirectToAction("Index", "Home");
        }

        [HttpGet] 
        public IActionResult Eliminar(int idEmpleado)
        {
            Empleado oEmpleado = _DBcontext.Empleados.Include(c => c.oCargo).Where(e => e.IdEmpleado == idEmpleado).FirstOrDefault();
            return View(oEmpleado);
        }

        [HttpPost]
        public IActionResult Eliminar(Empleado oEmpleado)
        {
            _DBcontext.Empleados.Remove(oEmpleado);
            _DBcontext.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}