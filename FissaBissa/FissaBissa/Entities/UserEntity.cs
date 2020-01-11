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

        [ProtectedPersonalData] public string FullName { get; set; }

        [ProtectedPersonalData] public string Address { get; set; }
    }
}
