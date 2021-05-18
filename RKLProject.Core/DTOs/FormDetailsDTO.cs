namespace RKLProject.Core.DTOs
{
    public class FormDetailsDTO
    {
        public string DisplayName { get; set; }
        public int DisplayOrder { get; set; }
        public bool Required { get; set; }
        public string ElementId { get; set; }
        public string Type { get; set; }
        public bool ReadOnly { get; set; }
    }
}
