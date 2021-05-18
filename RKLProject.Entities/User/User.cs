using System.Collections.Generic;
using RKLProject.Entities.Access;
using RKLProject.Entities.Common;
using RKLProject.Entities.Forms;
using RKLProject.Entities.Organization;

namespace RKLProject.Entities.User
{
    public class User : BaseEntity
    {
        #region Properties
        public string Email { get; set; }
        public string ActiveCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }

        #endregion

        #region Relations

        public virtual ICollection<UserRole> UserRoles { get; set; }
        public List<UserOrganization> UserOrganizations { get; set; }
        public List<MasterForm> Forms { get; set; }


        #endregion

    }
}
