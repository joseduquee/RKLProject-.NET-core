using System.Collections.Generic;
using System.Threading.Tasks;
using RKLProject.Core.DTOs;

namespace RKLProject.Core.IServices
{
    public interface IUserService
    {
        Task<RegisterResponse> RegisterUser(RegisterUserDTO registerUser);
        Task<LoginResponse> LoginUser(LoginUserDTO loginUser);
        Task<bool> IsUserExist(LoginUserDTO loginUser);
        Task<bool> IsUserExist(string email);
        Task<bool> IsUserActivated(LoginUserDTO loginUser);
        Task<UserInfoDTO> getUserByEmailAddress(string email);
        Task<UserInfoDTO> getUserByUId(long id);
        Task<List<UserInfoDTO>> GetLatestUsers();
        Task<long> TotalUsersCount();
    }
}