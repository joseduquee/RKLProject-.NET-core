using RKLProject.Entities.Common;

namespace RKLProject.Entities.Forms
{
    public class MasterFormDetails : BaseEntity
    {
        #region Properties

        public string DisplayName { get; set; }
        public int DisplayOrder { get; set; }
        public bool Required { get; set; }
        public string ElementId { get; set; }
        public string Type { get; set; }
        public bool ReadOnly { get; set; }
        public long FormId { get; set; }

        #endregion

        #region Relations

        public virtual MasterForm Form { get; set; }

        #endregion
    }
}