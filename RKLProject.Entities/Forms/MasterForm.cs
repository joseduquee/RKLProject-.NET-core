using System.Collections.Generic;
using RKLProject.Entities.Common;

namespace RKLProject.Entities.Forms
{
    public class MasterForm : BaseEntity
    {

        #region Properties

        public string FormName { get; set; }
        public long OrganizationId { get; set; }
        public string UniqueId { get; set; }
        public long UserId { get; set; }
        public bool IsActive { get; set; }

        #endregion

        #region Relations

        public virtual ICollection<MasterFormDetails> FormDetails { get; set; }
        public virtual User.User User { get; set; }

        #endregion

    }
}