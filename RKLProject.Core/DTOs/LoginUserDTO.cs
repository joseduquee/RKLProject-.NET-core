namespace RKLProject.Core.DTOs
{
    public class LoginUserDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public enum LoginResponse
    {
        Success,
        NotActive,
        Exist
    }
}
