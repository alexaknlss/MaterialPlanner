using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialPlanner.Models
{
    [Table("Brands")]
    public class Brands
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The name is required.")]
        [Column(TypeName = "nvarchar(100)")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column(TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }
    }
}