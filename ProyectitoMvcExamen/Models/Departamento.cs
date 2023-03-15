using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectitoMvcExamen.Models
{
    [Table("DEPT")]
    public class Departamento
    {
        [Key]
        [Column("Dept_no")]
        public int Dept_no { get; set; }
        [Column("Dnombre")]
        public string Dnombre { get; set; }
        [Column("Loc")]
        public string Localidad { get; set; }
    }
}
