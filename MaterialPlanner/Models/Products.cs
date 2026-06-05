using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MaterialPlanner.Models
{
    [Index(nameof(SKU), IsUnique = true)]
    [Table("Products")]
    public class Products
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string SKU { get; set; } = string.Empty;
    }
}