using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication8.Models
{
    public class Map
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public float radius { get; set; }
        [Required]
        public float latitude { get; set; }
        [Required]
        public float longtitude { get; set; }
        public IList<UserMapLocation> UsersMapLocations { get; set; }
    }
}
