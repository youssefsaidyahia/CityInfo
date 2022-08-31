using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace City_info.Models
{
    public class PointsOfInterestDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
