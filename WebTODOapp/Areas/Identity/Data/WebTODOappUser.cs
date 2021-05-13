using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Castle.MicroKernel.SubSystems.Conversion;
using Microsoft.AspNetCore.Identity;

namespace WebTODOapp.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the WebTODOappUser class
    public class WebTODOappUser : IdentityUser
    {
        [PersonalData]
        public string Firstname { get; set; }
        [PersonalData]
        public string LastName { get; set; }
    }
}
