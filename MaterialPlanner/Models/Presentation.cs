using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialPlanner.Models
{
    [Table("Presentations")]
    public class Presentation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(200)")]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
    }
}