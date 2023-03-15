using Microsoft.EntityFrameworkCore;
using ProyectitoMvcExamen.Models;

namespace ProyectitoMvcExamen.Data
{
    public class DepartamentoContext : DbContext
    {
        public DepartamentoContext(DbContextOptions<DepartamentoContext> options)
            :base (options) { }

       public DbSet<Empleado> Empleados { get; set; }
       public DbSet<Departamento> Departamentos { get; set; }
    } 
}
