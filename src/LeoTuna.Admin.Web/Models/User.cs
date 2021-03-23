using Microsoft.AspNetCore.Identity;
using System;

namespace LeoTuna.Admin.Web.Models
{
    public class User : IdentityUser<Guid>
    {
        public string FullName { get; set; } = string.Empty;

        public User()
        {
            Id = Guid.NewGuid();
        }
    }
}
