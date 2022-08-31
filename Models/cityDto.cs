using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace City_info.Models
{
    public class cityDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int NumberOfPointsOfInterest {
            get {
                return PointsOfInterest.Count; 
            }
            }
        public ICollection<PointsOfInterestDto> PointsOfInterest { get; set; } = new List<PointsOfInterestDto>();
    }
}
