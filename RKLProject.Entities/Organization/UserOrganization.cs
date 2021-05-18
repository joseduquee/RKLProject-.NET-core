using RKLProject.Entities.Common;

namespace RKLProject.Entities.Organization
{
    public class UserOrganization : BaseEntity
    {
        public long UserId { get; set; }
        public long OrganizationId { get; set; }

        #region Relations

        public User.User User { get; set; }
        public Organization Organization { get; set; }

        #endregion
    }
}