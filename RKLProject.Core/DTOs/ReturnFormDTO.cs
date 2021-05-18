using System.Collections.Generic;

namespace RKLProject.Core.DTOs
{
    public class ReturnFormDTO
    {
        public long FormId { get; set; }
        public string FormName { get; set; }
        public string UniqueId { get; set; }
        public long UserId { get; set; }
        public bool IsActive { get; set; }
        public List<FormDetailsDTO> FormDetailsList { get; set; }
    }
}
