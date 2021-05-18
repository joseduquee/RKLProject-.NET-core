using System.Collections.Generic;
using System.Threading.Tasks;
using RKLProject.Core.DTOs;
using RKLProject.Entities.Forms;
using RKLProject.Entities.Organization;

namespace RKLProject.Core.IServices
{
    public interface IOrganizationService
    {
        Task<List<Organization>> GetOrganizations();
        Task<List<Form>> GetFormsByOrganizationId(long organizationId);
        Task AddOrganization(Organization organization, long userId);
        Task<RegisterResponse> AddAdminToOrganization(RegisterUserDTO user);
        Task<Organization> GetOrganizationByUniqueId(string uniqueId);
        Task<ICollection<Form>> getFormsOfOrganization(string uniqueId);
        Task<long> GetTotalOrganizationCount();
    }
}