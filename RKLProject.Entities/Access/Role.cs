using RKLProject.Entities.Common;
using System.Collections.Generic;

namespace RKLProject.Entities.Access
{
    public class Role : BaseEntity
    {
        #region Properties

        public string RoleTitle { get; set; }

        #endregion

        #region Relations
        public virtual ICollection<UserRole> UserRoles { get; set; } //eppppdfd

        #endregion
    }
}
