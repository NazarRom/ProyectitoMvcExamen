using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectitoMvcExamen.Models
{
    [Table("EMP")]
    public class Empleado
    {
        [Key]
        [Column("Emp_no")]
        public int Emp_no { get; set; }
        [Column("Apellido")]
        public string Apellido { get; set; }
        [Column("Oficio")]
        public string Oficio { get; set; }

        [Column("Salario")]
        public int Salario { get; set; }

        [Column("Dept_no")]
        public int Dept_no { get; set; }
    }
}
