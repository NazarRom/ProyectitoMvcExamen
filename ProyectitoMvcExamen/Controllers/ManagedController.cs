using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ProyectitoMvcExamen.Models;
using ProyectitoMvcExamen.Repositories;
using System.Security.Claims;

namespace ProyectitoMvcExamen.Controllers
{
    public class ManagedController : Controller
    {
        private RepositoryDepartamento repo;

        public ManagedController(RepositoryDepartamento repo)
        {
            this.repo = repo;
        }

        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(string user, int pass)
        {
            Empleado empleado = await this.repo.ExisteEmpleado(user, pass);
            if (empleado != null)
            {
                ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);

                Claim claimId = new Claim(ClaimTypes.NameIdentifier, empleado.Emp_no.ToString());
                identity.AddClaim(claimId);

                Claim claimName = new Claim(ClaimTypes.Name, empleado.Apellido.ToString());
                identity.AddClaim(claimName);

                Claim claimOficio = new Claim(ClaimTypes.Role, empleado.Oficio.ToString());
                identity.AddClaim(claimOficio);

                Claim claimSalario = new Claim(("Salario"), empleado.Salario.ToString());
                identity.AddClaim(claimSalario);

                Claim claimDeptno = new Claim(("Departamento"), empleado.Dept_no.ToString());
                identity.AddClaim(claimDeptno);

                ClaimsPrincipal usePrincipal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, usePrincipal);
                string controller = TempData["controller"].ToString();
                string action = TempData["action"].ToString();
                return RedirectToAction(action, controller);

            }
            else
            {
                ViewData["MENSAJE"] = "Usuario/Password incorrectos";
                return View();
            }


        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("IdProductos");
            return RedirectToAction("Index", "Empleados");
        }
    }
}
