using System.Collections.Generic;
using RKLProject.Core.IServices;
using System.Threading.Tasks;
using RKLProject.Core.DTOs;
using RKLProject.DataLayer.Interfaces;
using RKLProject.Entities.Forms;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using RKLProject.Entities.Organization;

namespace RKLProject.Core.Services
{
    public class FormService : IFormService
    {
        #region Constructor

        private IUnitOfWork unitOfWork;

        public FormService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        #endregion

        #region Properties

        public async Task SaveDetailsOfForm(long formId, List<FormDetailsDTO> formdetails)
        {
            var repository = await unitOfWork.GetRepository<IGenericRepository<FormDetail>, FormDetail>();

            List<FormDetail> formDetails = new List<FormDetail>();

            foreach (var details in formdetails)
            {
                formDetails.Add(new FormDetail
                {
                    DisplayName = details.DisplayName,
                    DisplayOrder = details.DisplayOrder,
                    ElementId = details.ElementId,
                    ReadOnly = details.ReadOnly,
                    Required = details.Required,
                    FormId = formId,
                    Type = details.Type
                });
            }

            await repository.AddRenge(formDetails);
            await unitOfWork.SaveChanges();
        }

        public async Task<long> SaveNewFormAndReturnId(long userId, string formName, string organizationUniqueId)
        {
            var repository = await unitOfWork.GetRepository<IGenericRepository<Form>, Form>();
            var organizationRepository = await unitOfWork.GetRepository<IGenericRepository<Organization>, Organization>();
            var organization = organizationRepository.GetEntitiesQuery()
                .FirstOrDefault(o => o.UniqueId == organizationUniqueId);

            Form form = new Form
            {
                FormName = formName,
                UniqueId = Guid.NewGuid().ToString(),
                OrganizationId = organization.Id
            };
            await repository.AddEntity(form);
            await unitOfWork.SaveChanges();

            return form.Id;

        }

        public async Task<long> SaveNewMasterFormAndReturnId(long userId, string formName)
        {
            var repository = await unitOfWork.GetRepository<IGenericRepository<MasterForm>, MasterForm>();
            MasterForm form = new MasterForm
            {
                FormName = formName,
                UserId = userId,
                UniqueId = Guid.NewGuid().ToString()
            };
            await repository.AddEntity(form);
            await unitOfWork.SaveChanges();

            return form.Id;
        }

        public async Task SaveDetailsOfMasterForm(long formId, List<FormDetailsDTO> formdetails)
        {
            var repository = await unitOfWork.GetRepository<IGenericRepository<MasterFormDetails>, MasterFormDetails>();

            List<MasterFormDetails> formDetails = new List<MasterFormDetails>();

            foreach (var details in formdetails)
            {
                formDetails.Add(new MasterFormDetails
                {
                    DisplayName = details.DisplayName,
                    DisplayOrder = details.DisplayOrder,
                    ElementId = details.ElementId,
                    ReadOnly = details.ReadOnly,
                    Required = details.Required,
                    FormId = formId,
                    Type = details.Type
                });
            }

            await repository.AddRenge(formDetails);
            await unitOfWork.SaveChanges();
        }

        public async Task<List<ReturnFormDTO>> GetFormsByUserId(long userId)
        {
            var repository = await unitOfWork.GetRepository<IGenericRepository<Form>, Form>();

            var forms = await repository.GetEntitiesQuery().Where(f => !f.IsDelete).Include(f => f.FormDetails)
                .ToListAsync();

            List<ReturnFormDTO> returnForms = new List<ReturnFormDTO>();

            foreach (var form in forms)
            {
                returnForms.Add(new ReturnFormDTO
                {
                    FormId = form.Id,
                    FormName = form.FormName,
                    IsActive = form.IsActive,
                    UniqueId = form.UniqueId,
                    FormDetailsList = form.FormDetails.Select(fd => new FormDetailsDTO
                    {
                        DisplayName = fd.DisplayName,
                        DisplayOrder = fd.DisplayOrder,
                        ElementId = fd.ElementId,
                        ReadOnly = fd.ReadOnly,
                        Required = fd.Required,
                        Type = fd.Type
                    }).ToList()
                });
            }

            return returnForms;
        }

        public async Task<List<ReturnFormDTO>> GetMasterFormsByUserId(long userId)
        {
            var repository = await unitOfWork.GetRepository<IGenericRepository<MasterForm>, MasterForm>();

            var forms = await repository.GetEntitiesQuery().Where(f =>!f.IsDelete).Include(f => f.FormDetails)
                .ToListAsync();

            List<ReturnFormDTO> returnForms = new List<ReturnFormDTO>();

            foreach (var form in forms)
            {
                returnForms.Add(new ReturnFormDTO
                {
                    FormId = form.Id,
                    FormName = form.FormName,
                    IsActive = form.IsActive,
                    UniqueId = form.UniqueId,
                    FormDetailsList = form.FormDetails.Select(fd => new FormDetailsDTO
                    {
                        DisplayName = fd.DisplayName,
                        DisplayOrder = fd.DisplayOrder,
                        ElementId = fd.ElementId,
                        ReadOnly = fd.ReadOnly,
                        Required = fd.Required,
                        Type = fd.Type
                    }).ToList()
                });
            }

            return returnForms;
        }

        public async Task DeleteForm(long formId)
        {
            var repository = await unitOfWork.GetRepository<IGenericRepository<Form>, Form>();
            var form = await repository.GetEntityById(formId);
            
            if (form.IsActive)
            {
                form.IsActive = false;
                repository.RemoveEntity(form);
                await unitOfWork.SaveChanges();
            }
           
            await unitOfWork.SaveChanges();
        }

        public async Task DeleteMasterForm(long formId)
        {
            var repository = await unitOfWork.GetRepository<IGenericRepository<MasterForm>, MasterForm>();
            var form = await repository.GetEntityById(formId);

            if (form.IsActive)
            {
                form.IsActive = false;
            }

            repository.RemoveEntity(form);
            await unitOfWork.SaveChanges();
        }

        public async Task<ReturnFormDTO> GetFormById(long formId)
        {
            var repository = await unitOfWork.GetRepository<IGenericRepository<Form>, Form>();

            var form = await repository.GetEntitiesQuery().Where(f => f.Id == formId).Include(f => f.FormDetails).FirstOrDefaultAsync();

            return new ReturnFormDTO
            {
                FormId = form.Id,
                FormName = form.FormName,
                IsActive = form.IsActive,
                FormDetailsList = form.FormDetails.Select(fd => new FormDetailsDTO
                {
                    DisplayName = fd.DisplayName,
                    DisplayOrder = fd.DisplayOrder,
                    ElementId = fd.ElementId,
                    ReadOnly = fd.ReadOnly,
                    Required = fd.Required,
                    Type = fd.Type
                }).ToList()
            };
        }

        public async Task ActiveForm(long formId)
        {
            var repository = await unitOfWork.GetRepository<IGenericRepository<Form>, Form>();

            var form = await repository.GetEntityById(formId);

            form.IsActive = true;

            repository.UpdateEntity(form);

            await unitOfWork.SaveChanges();
        }

        public async Task UnactiveForms()
        {
            var repository = await unitOfWork.GetRepository<IGenericRepository<Form>, Form>();

            var formList = await repository.GetEntitiesQuery().ToListAsync();

            foreach (var form in formList)
            {
                form.IsActive = false;
            }

            repository.UpdateRange(formList);

            await unitOfWork.SaveChanges();
        }

        public async Task<ReturnFormDTO> GetActiveForm()
        {
            var repository = await unitOfWork.GetRepository<IGenericRepository<Form>, Form>();

            var form = await repository.GetEntitiesQuery().Where(f => f.IsActive && !f.IsDelete).Include(f => f.FormDetails).FirstOrDefaultAsync();

            return new ReturnFormDTO
            {
                FormId = form.Id,
                FormName = form.FormName,
                IsActive = form.IsActive,
                FormDetailsList = form.FormDetails.Select(fd => new FormDetailsDTO
                {
                    DisplayName = fd.DisplayName,
                    DisplayOrder = fd.DisplayOrder,
                    ElementId = fd.ElementId,
                    ReadOnly = fd.ReadOnly,
                    Required = fd.Required,
                    Type = fd.Type
                }).ToList()
            };
        }

        public async Task<long> TotalFormsCount()
        {
            var repository = await unitOfWork.GetRepository<IGenericRepository<Form>, Form>();

            return repository.GetEntitiesQuery().Count();
        }

        public async Task<ReturnFormDTO> GetMasterFormByUniqueId(string uniqueId)
        {
            var repository = await unitOfWork.GetRepository<IGenericRepository<MasterForm>, MasterForm>();

            var form = repository.GetEntitiesQuery().Where(f => f.UniqueId == uniqueId).Include(f => f.FormDetails).FirstOrDefault();

            return new ReturnFormDTO
            {
                FormId = form.Id,
                FormName = form.FormName,
                IsActive = form.IsActive,
                FormDetailsList = form.FormDetails.Select(fd => new FormDetailsDTO
                {
                    DisplayName = fd.DisplayName,
                    DisplayOrder = fd.DisplayOrder,
                    ElementId = fd.ElementId,
                    ReadOnly = fd.ReadOnly,
                    Required = fd.Required,
                    Type = fd.Type
                }).ToList()
            };

        }

        public async Task<ReturnFormDTO> GetFormByUniqueId(string uniqueId)
        {
            var repository = await unitOfWork.GetRepository<IGenericRepository<Form>, Form>();

            var form = repository.GetEntitiesQuery().Where(f => f.UniqueId == uniqueId).Include(f => f.FormDetails).FirstOrDefault();

            return new ReturnFormDTO
            {
                FormId = form.Id,
                FormName = form.FormName,
                IsActive = form.IsActive,
                FormDetailsList = form.FormDetails.Select(fd => new FormDetailsDTO
                {
                    DisplayName = fd.DisplayName,
                    DisplayOrder = fd.DisplayOrder,
                    ElementId = fd.ElementId,
                    ReadOnly = fd.ReadOnly,
                    Required = fd.Required,
                    Type = fd.Type
                }).ToList()
            };
        }

        public async Task<List<ReturnFormDTO>> GetFormsByOrganizationUniqueId(string organizationUniqueId)
        {
            var repository = await unitOfWork.GetRepository<IGenericRepository<Organization>, Organization>();

            var organization = repository.GetEntitiesQuery().Where(o => o.UniqueId == organizationUniqueId).Include(o => o.Forms)
                .ThenInclude(f => f.FormDetails)
                .FirstOrDefault();

            return organization.Forms.Select(f => new ReturnFormDTO
            {
                FormName = f.FormName,
                FormId = f.Id,
                UniqueId = f.UniqueId,
                IsActive = f.IsActive,
                FormDetailsList = f.FormDetails.Select(fd => new FormDetailsDTO
                {
                    DisplayName = fd.DisplayName,
                    DisplayOrder = fd.DisplayOrder,
                    ElementId = fd.ElementId,
                    ReadOnly = fd.ReadOnly,
                    Required = fd.Required,
                    Type = fd.Type
                }).ToList()
            }).ToList();
        }



        #endregion

    }
}