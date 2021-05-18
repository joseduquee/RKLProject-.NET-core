using System.Collections.Generic;
using RKLProject.Entities.Organization;

namespace RKLProject.Core.DTOs
{
    public class UserInfoDTO
    {
        public List<string> Roles { get; set; }
        public List<Organization> Organizations { get; set; }
        public long Id{ get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public string Token { get; set; }

        public int ExpireTime { get; set; }

    }
}
