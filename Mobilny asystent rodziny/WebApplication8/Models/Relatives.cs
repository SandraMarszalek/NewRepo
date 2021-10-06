using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApplication8.Areas.Identity.Data;

namespace WebApplication8.Models
{
    public class Relatives
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string GroupCode { get; set; }
        public IList<RelativesApplicationUser> RelativesApplicationUsers { get; set; }
        public string GroupName { get; set; }
    }
}
