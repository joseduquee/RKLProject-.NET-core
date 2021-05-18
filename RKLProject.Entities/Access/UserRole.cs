using RKLProject.Entities.Common;

namespace RKLProject.Entities.Access
{
    public class UserRole : BaseEntity
    {
        #region Properties
        public long RoleId { get; set; }
        public long UserId { get; set; }

        #endregion

        #region Relations
        public virtual User.User User { get; set; }
        public virtual Role Role { get; set; }

        #endregion
    }
}
