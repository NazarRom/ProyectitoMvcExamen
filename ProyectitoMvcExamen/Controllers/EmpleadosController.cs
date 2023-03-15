using Microsoft.AspNetCore.Mvc;
using ProyectitoMvcExamen.Models;
using ProyectitoMvcExamen.Repositories;

namespace ProyectitoMvcExamen.Controllers
{
    public class EmpleadosController : Controller
    {
        private RepositoryDepartamento repo;
        public EmpleadosController(RepositoryDepartamento repo)
        {
            this.repo = repo;
        }
        public IActionResult Index()
        {
            return View();
        }
       
        //one partial View with Empleados
        public IActionResult _EmpleadosPartial()
        {
            List<Empleado> empleados = this.repo.GetEmpleados();
            return PartialView("_EmpleadosPartial", empleados);
        }

        public IActionResult _OficioDetail(string oficio)
        {
            List<Empleado> empleados = this.repo.GetEmpleadosPorOficio(oficio);
            return PartialView("_OficioDetail", empleados);
        }

        public async Task<IActionResult> EmpoleadosDepartamento(int deptno , int? posicion, int? registro)
        {
            if (posicion == null)
            {
                posicion = 1;
                List<Empleado> empleados = this.repo.GetEmpleadosDepartamento(deptno);
                return View(empleados);
            }
            else
            {
                ModelPaginarEmpleados model = await this.repo.GetPaginarEmpleadosDepartamentoAsync(posicion.Value, deptno, registro.Value);
                List<Empleado> empleados = model.Empleados;
                ViewData["Registros"] = model.NumeroRegistros;
                ViewData["Deptno"] = deptno;
                ViewData["Registro"] = registro;
                return View(empleados);
            }
            
        }
        [HttpPost]
        public async Task<IActionResult> EmpoleadosDepartamento(int deptno, int? registro)
        {
           
                ModelPaginarEmpleados model = await this.repo.GetPaginarEmpleadosDepartamentoAsync(1, deptno, registro.Value);
                List<Empleado> empleados = model.Empleados;
                ViewData["Registros"] = model.NumeroRegistros;
                ViewData["Deptno"] = deptno;
                ViewData["Registro"] = registro;
                return View(empleados);
            
        }

    }
}
