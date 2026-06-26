using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialPlanner.Models
{
    [Table("MaterialDetails")]
    public class MaterialDetails
    {
        [Key]
        public int Id { get; set; }

        // 🔑 FK nullable para permitir SetNull
        public int? MaterialId { get; set; }
        public int? BrandId { get; set; }
        public int? ProductId { get; set; }
        public int? PresentationId { get; set; }

        // Relaciones
        [ForeignKey(nameof(MaterialId))]
        public Materials? Material { get; set; }

        [ForeignKey(nameof(BrandId))]
        public Brands? Brand { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Products? Product { get; set; }

        [ForeignKey(nameof(PresentationId))]
        public Presentation? Presentation { get; set; }

        // Cantidad consumida
        [Required]
        public int Consumption { get; set; }

        // Unidad seleccionada desde un Select
        public int? UnitId { get; set; }

        [ForeignKey(nameof(UnitId))]
        public Units? Unit { get; set; }

        public bool Status { get; set; }

     

        // 🖼️ Relación con imágenes (1 a muchos)
        public ICollection<Image> Images { get; set; } = new List<Image>();

        // ⏱️ Timestamps
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column(TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }
    }
}