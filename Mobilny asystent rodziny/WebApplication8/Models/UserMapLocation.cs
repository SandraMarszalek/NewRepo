using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication8.Areas.Identity.Data;

namespace WebApplication8.Models
{
    public class UserMapLocation
    {
        public string UserId { get; set; }
        public int MapId { get; set; }
        public Map Map { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
