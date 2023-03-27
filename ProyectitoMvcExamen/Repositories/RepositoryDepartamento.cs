using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectitoMvcExamen.Data;
using ProyectitoMvcExamen.Models;
using System.Data;
#region procedure

//create procedure sp_grupo_empleados_departameto
//(@dept int , @posicion int, @registro int , @numeroregistros int out)
//as
//select @numeroregistros = COUNT(EMP_NO) from EMP where DEPT_NO = @dept
//                                        select EMP_NO, APELLIDO, OFICIO, SALARIO, DEPT_NO from
//                                        (select CAST(ROW_NUMBER() over (order by apellido) as int) as posicion,
//EMP_NO, APELLIDO, OFICIO, SALARIO, DEPT_NO
//from EMP
//where DEPT_NO = @dept) as query
//where query.posicion >= @posicion and query.posicion<(@posicion + @registro)
#endregion
namespace ProyectitoMvcExamen.Repositories
{
    public class RepositoryDepartamento
    {
        private DepartamentoContext context;

        public RepositoryDepartamento(DepartamentoContext context)
        {
            this.context = context;
        }

        public List<Empleado> GetEmpleados()
        {
            return this.context.Empleados.ToList();
        }

        public List<Departamento> GetDepartamentos()
        {
            return this.context.Departamentos.ToList();
        }

        public List<Empleado> GetEmpleadosPorOficio(string oficio)
        {
            var consulta = from data in this.context.Empleados
                           where data.Oficio == oficio
                           select data;
            return consulta.ToList();
        }

        public List<Empleado> GetEmpleadosDepartamento(int deptno)
        {
            var consulta = from data in this.context.Empleados
                           where data.Dept_no == deptno
                           select data;
            return consulta.ToList();
        }

      

        public async Task<ModelPaginarEmpleados> GetPaginarEmpleadosDepartamentoAsync(int posicion, int deptno, int registro)
        {
            string sql = "sp_grupo_empleados_departameto @dept, @posicion, @registro, @numeroregistros out";
            SqlParameter pamnumregistros = new SqlParameter("@numeroregistros", -1);
            pamnumregistros.Direction = ParameterDirection.Output;
            SqlParameter pamdept = new SqlParameter("@dept", deptno);
            SqlParameter pamposicion = new SqlParameter("@posicion", posicion);
            SqlParameter pamdregistro = new SqlParameter("@registro", registro);

            var consulta = this.context.Empleados.FromSqlRaw(sql, pamdept, pamposicion, pamdregistro, pamnumregistros);
            List<Empleado> empleados = await consulta.ToListAsync();
            int registros = (int)pamnumregistros.Value;
            return new ModelPaginarEmpleados
            {
                NumeroRegistros = registros,
                Empleados = empleados
            };
        }

        public async Task<Empleado> ExisteEmpleado(string apellido, int empno)
        {
            var consulta = this.context.Empleados.Where(x => x.Apellido == apellido && x.Emp_no == empno);
            return consulta.FirstOrDefault();
        }

        public Empleado GetEmpPaginacion(int dept,int posicion, ref int numeroEscenas)
        {
            //VOY A RECUPERAR LA COLECCION DE ESCENAS DE UNA PELICULA
            //PARA ELLO, UTILIZAMOS EL METODO ANTERIOR
            List<Empleado> empList = this.GetEmpleadosDepartamento(dept);
            numeroEscenas = empList.Count;
            //VAMOS A PAGINAR DE UNO EN UNO
            Empleado empleado =
                empList.Skip(posicion).Take(1).FirstOrDefault();
            return empleado;
        }

        public Departamento GetDepartamentoById(int deptno)
        {
            return this.context.Departamentos.FirstOrDefault(x => x.Dept_no == deptno);
        }


    }
}
