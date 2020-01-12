using System;
using Microsoft.AspNetCore.Identity;

namespace FissaBissa.Entities
{
    public class UserEntity : IdentityUser<Guid>
    {
        public UserEntity()
        {
            SecurityStamp = Guid.NewGuid().ToString();
        }

        [PersonalData]
        public string FullName { get; set; }

        [PersonalData]
        public string Address { get; set; }
    }
}
