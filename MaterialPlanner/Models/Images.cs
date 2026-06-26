using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialPlanner.Models
{
    [Table("Images")]
    public class Image
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        [Column(TypeName = "nvarchar(150)")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "nvarchar(300)")]
        public string Path { get; set; } = string.Empty;

        // 🔗 FK hacia MaterialDetails
        public int MaterialDetailsId { get; set; }

        [ForeignKey("MaterialDetailsId")]
        public MaterialDetails MaterialDetails { get; set; } = null!;

        // ⏱️ Timestamps
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column(TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }
    }
}