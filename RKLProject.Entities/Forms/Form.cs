using System.Collections.Generic;
using RKLProject.Entities.Common;

namespace RKLProject.Entities.Forms
{
    public class Form : BaseEntity
    {
        #region Properties

        public string FormName { get; set; }
        public string UniqueId { get; set; }
        public long OrganizationId { get; set; }
        public bool IsActive { get; set; }

        #endregion

        #region Relations

        public virtual ICollection<FormDetail> FormDetails { get; set; }
        public  Organization.Organization Organization { get; set; }
        public List<Client.Client> Clients { get; set; }

        #endregion

    }
}
