using System.Collections.Generic;

namespace RKLProject.Core.DTOs
{
    public class FormDTO
    {
        public long FormId { get; set; }
        public string FormName { get; set; }
        public string uniqueId { get; set; }
        public long UserId { get; set; }
        public bool IsActive { get; set; }

        public List<FormDetailsDTO> FormDetailsList { get; set; }
    }
}
