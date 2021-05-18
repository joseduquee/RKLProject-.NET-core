using RKLProject.Entities.Common;

namespace RKLProject.Entities.Client
{
    public class Client : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public long OrganizationId { get; set; }

        #region Relations

        public Organization.Organization Organization { get; set; }

        #endregion
    }
}