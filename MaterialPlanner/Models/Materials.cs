using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialPlanner.Models
{
    [Table("Materials")]
    public class Materials
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        [MaxLength(100)]
        public string ItemMaterial { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string Description { get; set; } = string.Empty;
    }
}