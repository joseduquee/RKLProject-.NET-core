using System.Collections.Generic;
using RKLProject.Core.DTOs;
using System.Threading.Tasks;

namespace RKLProject.Core.IServices
{
    public interface IFormService
    {
        Task<long> SaveNewFormAndReturnId(long userId,string formName, string organizationUniqueId);
        Task<long> SaveNewMasterFormAndReturnId(long userId,string formName);
        Task SaveDetailsOfForm(long formId, List<FormDetailsDTO> formdetails);
        Task SaveDetailsOfMasterForm(long formId, List<FormDetailsDTO> formdetails);
        Task<List<ReturnFormDTO>> GetFormsByUserId(long userId);
        Task<List<ReturnFormDTO>> GetMasterFormsByUserId(long userId);
        Task<ReturnFormDTO> GetMasterFormByUniqueId(string uniqueId);
        Task<ReturnFormDTO> GetFormByUniqueId(string uniqueId);
        Task DeleteForm(long formId);
        Task DeleteMasterForm(long formId);
        Task<ReturnFormDTO> GetFormById(long formId);
        Task<List<ReturnFormDTO>> GetFormsByOrganizationUniqueId(string organizationUniqueId);
        Task ActiveForm(long formId);
        Task UnactiveForms();
        Task<ReturnFormDTO> GetActiveForm();
        Task<long> TotalFormsCount();
    }
}