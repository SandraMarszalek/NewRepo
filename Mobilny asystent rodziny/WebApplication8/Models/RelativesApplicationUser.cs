using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication8.Areas.Identity.Data;

namespace WebApplication8.Models
{
    public class RelativesApplicationUser
    {
        public string UserId { get; set; }
        public int GroupId { get; set; }
        public Relatives Relatives { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
