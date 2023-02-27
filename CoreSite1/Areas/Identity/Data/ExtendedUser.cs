using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Microsoft.AspNetCore.Identity
{
    public class ExtendedUser : IdentityUser
    {
        [PersonalData]
        public string Name { get; set; }
        [PersonalData]
        public byte[] Photo { get; set; }
        [PersonalData]
        public DateTime DOB { get; set; }

    }
}
