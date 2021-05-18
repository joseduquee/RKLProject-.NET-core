using RKLProject.Core.DTOs;
using RKLProject.Core.IServices;
using RKLProject.DataLayer.Interfaces;
using System.Threading.Tasks;
using RKLProject.Entities.User;
using System.Linq;
using RKLProject.Core.Security;
using RKLProject.Entities.Access;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RKLProject.Entities.Organization;

namespace RKLProject.Core.Services
{
    public class UserService : IUserService
    {
        #region Constructor

        private IUnitOfWork unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #endregion

        #region Properties

        public async Task<LoginResponse> LoginUser(LoginUserDTO loginUser)
        {
            loginUser.Email = loginUser.Email.SanitizeText();
            loginUser.Password = loginUser.Password.SanitizeText();
            loginUser.Password = PasswordHelper.EncodePasswordMd5(loginUser.Password);

            if (!await IsUserExist(loginUser)) return LoginResponse.Exist;

            if (!await IsUserActivated(loginUser)) return LoginResponse.NotActive;

            return LoginResponse.Success;
        }

        public async Task<RegisterResponse> RegisterUser(RegisterUserDTO registerUser)
        {
            var repository = await unitOfWork.GetRepository<IGenericRepository<User>, User>();

            if (await IsUserExist(registerUser.Email)) return RegisterResponse.Exist;

            #region Sanitize Data

            registerUser.Email = registerUser.Email.SanitizeText();
            registerUser.Password = registerUser.Password.SanitizeText();
            registerUser.ConfirmPassword = registerUser.ConfirmPassword.SanitizeText();
            registerUser.FirstName = registerUser.FirstName.SanitizeText();
            registerUser.LastName = registerUser.LastName.SanitizeText();

            #endregion

            if (registerUser.Password != registerUser.ConfirmPassword)
            {
                return RegisterResponse.Error;
            }

            var user = new User
            {
                Email = registerUser.Email,
                IsDelete = false,
                FirstName = registerUser.FirstName,
                LastName = registerUser.LastName,
                Password = PasswordHelper.EncodePasswordMd5(registerUser.Password),
                IsActive = true,
                ActiveCode = Guid.NewGuid().ToString()
            };
            await repository.AddEntity(user);

            await unitOfWork.SaveChanges();

            var userRoleRepository = await unitOfWork.GetRepository<IGenericRepository<UserRole>, UserRole>();
            var roleRepository = await unitOfWork.GetRepository<IGenericRepository<Role>, Role>();

            var adminRoleId = roleRepository.GetEntitiesQuery().FirstOrDefault(r => r.RoleTitle == "Admin").Id;

            await userRoleRepository.AddEntity(new UserRole
            {
                UserId = user.Id,
                RoleId = adminRoleId
            });
            await unitOfWork.SaveChanges();

            return RegisterResponse.Success;

        }
        public async Task<UserInfoDTO> getUserByEmailAddress(string email)
        {
            var repository = await unitOfWork.GetRepository<IGenericRepository<User>, User>();

            var user = repository.GetEntitiesQuery().Where(u => u.Email == email)
                .Include(u => u.UserRoles).ThenInclude(r => r.Role).FirstOrDefault();

            var userOrganizationRepository = await unitOfWork.GetRepository<IGenericRepository<UserOrganization>, UserOrganization>();

            var organizations = userOrganizationRepository.GetEntitiesQuery().Where(uo => uo.UserId == user.Id)
                .Select(uo => uo.Organization).ToList();


            return new UserInfoDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsActive = user.IsActive,
                Roles = user.UserRoles.Select(ur => ur.Role.RoleTitle).ToList(),
                Organizations = organizations
            };
        }
        public async Task<UserInfoDTO> getUserByUId(long id)
        {
            var repository = await unitOfWork.GetRepository<IGenericRepository<User>, User>();

            var user = repository.GetEntitiesQuery().Where(u => u.Id == id)
                .Include(u => u.UserRoles).ThenInclude(r => r.Role).FirstOrDefault();

            var userOrganizationRepository = await unitOfWork.GetRepository<IGenericRepository<UserOrganization>, UserOrganization>();

            var organizations = userOrganizationRepository.GetEntitiesQuery().Where(uo => uo.UserId == user.Id)
                .Select(uo => uo.Organization).ToList();


            return new UserInfoDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsActive = user.IsActive,
                Roles = user.UserRoles.Select(ur => ur.Role.RoleTitle).ToList(),
                Organizations = organizations
            };
        }

        #endregion

        #region Tools

        public async Task<bool> IsUserExist(LoginUserDTO loginUser)
        {
            var repository = await unitOfWork.GetRepository<IGenericRepository<User>, User>();

            return repository.GetEntitiesQuery()
                .Any(u => u.Email == loginUser.Email && u.Password == loginUser.Password);
        }

        public async Task<bool> IsUserActivated(LoginUserDTO loginUser)
        {
            var repository = await unitOfWork.GetRepository<IGenericRepository<User>, User>();

            return repository.GetEntitiesQuery()
                .Any(u => u.Email == loginUser.Email && u.Password == loginUser.Password && u.IsActive);
        }

        public async Task<bool> IsUserExist(string email)
        {
            var repository = await unitOfWork.GetRepository<IGenericRepository<User>, User>();

            return repository.GetEntitiesQuery()
                .Any(u => u.Email == email);
        }

        public async Task<List<UserInfoDTO>> GetLatestUsers()
        {
            var repository = await unitOfWork.GetRepository<IGenericRepository<User>, User>();

            var users = repository.GetEntitiesQuery().Take(5).ToList();

            return users.Select(user => new UserInfoDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.IsActive,
                Email = user.Email
            }).ToList();
        }

        public async Task<long> TotalUsersCount()
        {
            var repository = await unitOfWork.GetRepository<IGenericRepository<User>, User>();

            return repository.GetEntitiesQuery().Count();
        }


        #endregion

    }
}