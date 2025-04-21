using System.ComponentModel.DataAnnotations;

namespace Magic_vila_API.Models.DTO
{
    public class VilaDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Details { get; set; }
        public string Name { get; set; }
        public int occupancy { get; set; }
        public int sqft { get; set; }
        [Required, MaxLength(100)]
        public double Rate { get; set; }
        public string ImageUrl { get; set; }
        public string Amenity { get; set; }
    }
}
