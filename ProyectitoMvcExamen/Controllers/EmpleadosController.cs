using Microsoft.AspNetCore.Mvc;
using ProyectitoMvcExamen.Filters;
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
        [AuthorizeUsuarios(Policy = "PermisosElevados")]
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
        [AuthorizeUsuarios(Policy = "PermisosElevados")]
        public IActionResult EmpleadosPaginacionLinq(int deptno , int? posicion)
        {
            //LINQ FUNCIONA EN BASE 0
            //SQL SERVER FUNCIONA EN BASE 1
            if (posicion == null)
            {
                posicion = 0;
            }
            int numeroEscenas = 0;
            Empleado emp = this.repo.GetEmpPaginacion(deptno, posicion.Value, ref numeroEscenas);
            ViewData["DATOS"] = "Escena " + (posicion + 1)
                + " de " + numeroEscenas;
            int siguiente = posicion.Value + 1;
            if (siguiente >= numeroEscenas)
            {
                siguiente = 0;
            }
            int anterior = posicion.Value - 1;
            if (anterior < 0)
            {
                anterior = numeroEscenas - 1;
            }
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            Departamento dept = this.repo.GetDepartamentoById(deptno);
            ViewData["PELICULA"] = dept;
            return View(emp);
        }

    }
}
