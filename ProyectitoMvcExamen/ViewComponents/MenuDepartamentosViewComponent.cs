using Microsoft.AspNetCore.Mvc;
using ProyectitoMvcExamen.Models;
using ProyectitoMvcExamen.Repositories;

namespace ProyectitoMvcExamen.ViewComponents
{
    public class MenuDepartamentosViewComponent : ViewComponent
    {
        private RepositoryDepartamento repo;

        public MenuDepartamentosViewComponent(RepositoryDepartamento repo)
        {
            this.repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Departamento> departamentos = this.repo.GetDepartamentos();
            return View(departamentos);
        }
    }
}
