using System.Collections.Generic;
using RKLProject.Entities.Common;

namespace RKLProject.Entities.Organization
{
    public class Organization : BaseEntity
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UniqueId { get; set; }
        public bool IsPrivate{ get; set; }
        public List<Client.Client> Clients { get; set; }
        public List<UserOrganization> UserOrganizations { get; set; }
        public virtual ICollection<Forms.Form> Forms { get; set; }
    }
}