using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RKLProject.Core.DTOs;
using RKLProject.Core.IServices;
using RKLProject.Core.MongoDbServices.IServices;
using RKLProject.Core.Utilities;
using RKLProject.Entities.Organization;
using RKLProject.Utilities.Identity;

namespace RKLProject.Web.Controllers
{
    public class SuperAdminController : BaseController
    {
        //!!!!!!!!!! LOGIN is in AccountController like other users !!!!!!!!!!!!//

        //!!!!!!!!!! LOGOUT is in AccountController like other users !!!!!!!!!!!!//

        //!!!!!!!!!! REGISTER is in AccountController like other users !!!!!!!!!!!!//

        #region Constructor

        private readonly IFormService _formService;
        private readonly IOrganizationService _organizationService;
        private readonly IMongoFormService<CompletedUserFormDTO> _userForm;
        public SuperAdminController(IFormService _formService, IOrganizationService _organizationService, IMongoFormService<CompletedUserFormDTO> _userForm)
        {
            this._formService = _formService;
            this._userForm = _userForm;
            this._organizationService = _organizationService;
        }

        #endregion

        #region Forms Section

        [HttpGet("get-master-forms")]
        public async Task<IActionResult> GetForms()
        {
            long userId = User.GetUserId();
            var forms = await _formService.GetMasterFormsByUserId(userId);
            return JsonResponseStatus.Success(forms);
        }

        [HttpPost("save-master-form")]
        public async Task<IActionResult> AddNewForm([FromBody] FormDTO form)
        {
            long userId = User.GetUserId();
            long formId = await _formService.SaveNewMasterFormAndReturnId(userId, form.FormName);
            await _formService.SaveDetailsOfMasterForm(formId, form.FormDetailsList);
            return JsonResponseStatus.Success();
        }

        [HttpPost("delete-master-form")]
        public async Task<IActionResult> DeleteForm([FromForm] long formId)
        {
            await _formService.DeleteMasterForm(formId);
            return JsonResponseStatus.Success();
        }

        [HttpPost("get-form-by-unique-id")]
        public async Task<IActionResult> GetFormByUniqueId([FromForm] string uniqueId)
        {
            return JsonResponseStatus.Success(await _formService.GetMasterFormByUniqueId(uniqueId));
        }

        #endregion

        #region Organizations Section

        [HttpGet("get-organizations")]
        public async Task<IActionResult> GetOrganizations()
        {
            return JsonResponseStatus.Success(await _organizationService.GetOrganizations());
        }

        [HttpPost("get-organization-forms-by-id")]
        public async Task<IActionResult> GetOrganizationFroms([FromForm]long organizationId)
        {
            return JsonResponseStatus.Success(await _organizationService.GetFormsByOrganizationId(organizationId));
        }

        [HttpPost("add-organization")]
        public async Task<IActionResult> AddOrganization([FromBody] Organization organization)
        {
            long userId = User.GetUserId();
            organization.UniqueId = Guid.NewGuid().ToString();
            await _organizationService.AddOrganization(organization, userId);

            return JsonResponseStatus.Success(organization);
        }

        [HttpPost("get-organization")]
        public async Task<IActionResult> GetOrganizationByUniqueId([FromBody] string uniqueId)
        {
            return JsonResponseStatus.Success(await _organizationService.GetOrganizationByUniqueId(uniqueId));
        }

        [HttpPost("get-organization-forms")]
        public async Task<IActionResult> GetOrganizationForms([FromForm] string uniqueId)
        {
            return JsonResponseStatus.Success(await _organizationService.getFormsOfOrganization(uniqueId));
        }

        [HttpPost("add-admin-to-organization")]
        public async Task<IActionResult> AddAdminToOrganization([FromBody] RegisterUserDTO user)
        {
            var response = await _organizationService.AddAdminToOrganization(user);
            switch (response)
            {
                case RegisterResponse.Exist:
                    return JsonResponseStatus.NotFound(new {message = "User Exixst"});
                case RegisterResponse.Success:
                    return JsonResponseStatus.Success();
            }
            return JsonResponseStatus.Error();
        }

        #endregion

    }
}
