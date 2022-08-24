using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRUDentity.Models.ViewModels
{
    public class EmpleadoVM
    {
        public Empleado oEmpleado { get; set; }
        public List<SelectListItem> oListaCargo { get; set; }
    }
}
