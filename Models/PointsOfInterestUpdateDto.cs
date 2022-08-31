using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace City_info.Models
{
    public class PointsOfInterestUpdateDto
    {
        [Required(ErrorMessage = "please , Provide a name")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(200, ErrorMessage = "the content is too biger")]
        public string? Description { get; set; }
    }
}
