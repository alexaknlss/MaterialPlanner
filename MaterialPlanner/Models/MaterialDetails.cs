using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialPlanner.Models
{
    [Table("MaterialDetails")]
    public class MaterialDetails
    {
        [Key]
        public int Id { get; set; }

        // 🔑 FK ahora nullable (IMPORTANTE para SetNull)
        public int? MaterialId { get; set; }
        public int? BrandId { get; set; }
        public int? ProductId { get; set; }
        public int? PresentationId { get; set; }

        // Relaciones explícitas (también nullable)
        [ForeignKey(nameof(MaterialId))]
        public Materials? Material { get; set; }

        [ForeignKey(nameof(BrandId))]
        public Brands? Brand { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Products? Product { get; set; }

        [ForeignKey(nameof(PresentationId))]
        public Presentation? Presentation { get; set; }

        public bool Status { get; set; }

        public int Construction { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // 🖼️ Relación con imágenes (1 a muchos)
        public ICollection<Image> Images { get; set; } = new List<Image>();
    }
}