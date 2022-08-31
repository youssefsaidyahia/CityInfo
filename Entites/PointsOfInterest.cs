using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace City_info.Entites
{
    public class PointsOfInterest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string? Description { get; set; }
        [ForeignKey("CityId")]
        public City? City { get; set; }
        public  int CityId { get; set; }

        public PointsOfInterest(string name)
        {
            Name = name;
        }
    }
}
