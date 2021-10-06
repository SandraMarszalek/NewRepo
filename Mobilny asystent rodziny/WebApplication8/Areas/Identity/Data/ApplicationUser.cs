using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.MicroKernel.SubSystems.Conversion;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication8.Models;

namespace WebApplication8.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "text")]
        public string FirstName { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string User { get; set; }

        public IList<RelativesApplicationUser> RelativesApplicationUsers { get; set; }
        public IList<UserMapLocation> UsersMapLocations { get; set; }
    }
}
