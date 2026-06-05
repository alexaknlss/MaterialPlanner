using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialPlanner.Models
{
    [Table("MaterialDetails")]
    public class MaterialDetails
    {
        [Key]
        public int Id { get; set; }

        public int MaterialId { get; set; }
        public int BrandId { get; set; }
        public int ProductId { get; set; }
        public int PresentationId { get; set; }

        // Relaciones explícitas
        [ForeignKey(nameof(MaterialId))]
        public Materials Material { get; set; } = null!;

        [ForeignKey(nameof(BrandId))]
        public Brands Brand { get; set; } = null!;

        [ForeignKey(nameof(ProductId))]
        public Products Product { get; set; } = null!;

        [ForeignKey(nameof(PresentationId))]
        public Presentation Presentation { get; set; } = null!;

        public bool Status { get; set; }

        public int Construction { get; set; }
    }
}