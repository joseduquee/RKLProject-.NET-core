using RKLProject.Core.IServices;
using RKLProject.Entities.Organization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RKLProject.DataLayer.Interfaces;
using RKLProject.Entities.Forms;
using Microsoft.EntityFrameworkCore;
using RKLProject.Core.DTOs;

namespace RKLProject.Core.Services
{
    public class OrganizationService : IOrganizationService
    {
        #region Constructor

        private IUnitOfWork unitOfWork;
        private IUserService userService;

        public OrganizationService(IUnitOfWork unitOfWork, IUserService userService)
        {
            this.unitOfWork = unitOfWork;
            this.userService = userService;
        }

        #endregion

        #region Properties

        public async Task<List<Organization>> GetOrganizations()
        {
            var repository = await unitOfWork.GetRepository<IGenericRepository<Organization>, Organization>();

            return repository.GetEntitiesQuery().ToList();
        }
        public async Task<List<Form>> GetFormsByOrganizationId(long organizationId)
        {
            var repository = await unitOfWork.GetRepository<IGenericRepository<Form>, Form>();

            return repository.GetEntitiesQuery().Where(f => f.OrganizationId == organizationId).ToList();
        }

        public async Task AddOrganization(Organization organization, long userId)
        {
            var repository = await unitOfWork.GetRepository<IGenericRepository<Organization>, Organization>();
            var newOrganization = new Organization
            {
                Address = organization.Address,
                Name = organization.Name,
                Title = organization.Title,
                IsPrivate = organization.IsPrivate,
                PhoneNumber = organization.PhoneNumber,
                UniqueId = organization.UniqueId,
                IsDelete = false
            };
            await repository.AddEntity(newOrganization);

            await unitOfWork.SaveChanges();

            var organizationUserRepository = await unitOfWork.GetRepository<IGenericRepository<UserOrganization>, UserOrganization>();

            var userOrganization = new UserOrganization
            {
                UserId = userId,
                OrganizationId = newOrganization.Id
            };

            await organizationUserRepository.AddEntity(userOrganization);

            await unitOfWork.SaveChanges();
        }

        public async Task<Organization> GetOrganizationByUniqueId(string uniqueId)
        {
            var repository = await unitOfWork.GetRepository<IGenericRepository<Organization>, Organization>();
            return repository.GetEntitiesQuery().FirstOrDefault(o => o.UniqueId == uniqueId);
        }

        public async Task<ICollection<Form>> getFormsOfOrganization(string uniqueId)
        {
            var repository = await unitOfWork.GetRepository<IGenericRepository<Organization>, Organization>();
            var organization = repository.GetEntitiesQuery().Where(o => o.UniqueId == uniqueId).Include(o => o.Forms).First();
            return organization.Forms;
        }

        public async Task<long> GetTotalOrganizationCount()
        {
            var repository = await unitOfWork.GetRepository<IGenericRepository<Organization>, Organization>();

            return repository.GetEntitiesQuery().Count();
        }

        public async Task<RegisterResponse> AddAdminToOrganization(RegisterUserDTO user)
        {
            var UserOrganizationRepository = await unitOfWork.GetRepository<IGenericRepository<UserOrganization>, UserOrganization>();

            var OrganizationRepository = await unitOfWork.GetRepository<IGenericRepository<Organization>, Organization>();

            var organization = OrganizationRepository.GetEntitiesQuery().FirstOrDefault(o => o.UniqueId == user.OrganizationUniqueId);
               

            if (organization != null)
            {
                bool isUserExist = await userService.IsUserExist(user.Email);
                if (isUserExist)
                {
                    return RegisterResponse.Exist;
                }
                await userService.RegisterUser(user);

                var newUser = await userService.getUserByEmailAddress(user.Email);

                var newUserId = newUser.Id;

                var organizationId = organization.Id;

                var newUserOrganization = new UserOrganization
                {
                    UserId = newUserId,
                    OrganizationId = organizationId
                };

                await UserOrganizationRepository.AddEntity(newUserOrganization);

                await unitOfWork.SaveChanges();

                return RegisterResponse.Success;
            }

            return RegisterResponse.Error;
        }

        #endregion

    }
}