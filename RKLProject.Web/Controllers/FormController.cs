using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RKLProject.Core.DTOs;
using RKLProject.Core.IServices;
using RKLProject.Core.MongoDbServices.IServices;
using RKLProject.Core.Utilities;
using RKLProject.Utilities.Identity;

namespace RKLProject.Web.Controllers
{
    public class FormController : BaseController
    {
        #region Constructor

        private readonly IFormService _formService;
        private readonly IMongoFormService<CompletedUserFormDTO> _userForm;
        public FormController(IFormService _formService, IMongoFormService<CompletedUserFormDTO> _userForm)
        {
            this._formService = _formService;
            this._userForm = _userForm;
        }

        #endregion

        #region Operations

        [HttpPost("save-form")]
        public async Task<IActionResult> AddNewForm([FromBody] FormDTO form)
        {
            long userId = User.GetUserId();
            long formId = await _formService.SaveNewFormAndReturnId(userId, form.FormName, form.uniqueId);
            await _formService.SaveDetailsOfForm(formId, form.FormDetailsList);
            return JsonResponseStatus.Success();
        }

        [HttpGet("get-forms")]
        public async Task<IActionResult> GetForms()
        {
            long userId = User.GetUserId();
            var forms = await _formService.GetFormsByUserId(userId);
            return JsonResponseStatus.Success(forms);
        }

        [HttpPost("delete-form")]
        public async Task<IActionResult> DeleteForm([FromForm] long formId)
        {
            await _formService.DeleteForm(formId);
            return JsonResponseStatus.Success();
        }

        [HttpPost("get-form")]
        public async Task<IActionResult> GetForm([FromForm] long formId)
        {
            var form = await _formService.GetFormById(formId);

            return JsonResponseStatus.Success(form);
        }

        [HttpPost("get-form-by-unique-id")]
        public async Task<IActionResult> GetFormByUniqueId([FromForm] string uniqueId)
        {
            return JsonResponseStatus.Success(await _formService.GetFormByUniqueId(uniqueId));
        }

        [HttpGet("get-forms-count")]
        public async Task<long> GetFormsCount()
        {
            var count = await _formService.TotalFormsCount();

            return count;
        }

        [HttpPost("active-form")]
        public async Task<IActionResult> ActiveOneForm([FromForm] long formId = 0)
        {
            await _formService.UnactiveForms();

            if (formId != 0)
                await _formService.ActiveForm(formId);

            return JsonResponseStatus.Success();
        }

        [HttpGet("get-active-form")]
        public async Task<IActionResult> GetActiveForm()
        {
            return JsonResponseStatus.Success(await _formService.GetActiveForm());
        }

        [HttpGet("get-total-forms")]
        public async Task<IActionResult> GetTotalForms()
        {
            return JsonResponseStatus.Success(await _formService.TotalFormsCount());
        }

        #region MongoDb Data Get/Set

        [HttpPost("save-user-form")]
        public async Task<IActionResult> SaveUserPost([FromBody] CompletedUserFormDTO userForm)
        {
            await _userForm.InsertRecord("", userForm);
            return JsonResponseStatus.Success();
        }

        [HttpGet("get-users-forms")]
        public async Task<IActionResult> GetAllUsersFroms()
        {
            return JsonResponseStatus.Success(await _userForm.LoadRecords(""));
        }

        #endregion

        #endregion

        #region Organization Forms Section

        [HttpPost("organization-form")]
        public async Task<IActionResult> GetOrganizationForm([FromForm] string organizationUniqueId)
        {
            return JsonResponseStatus.Success(await _formService.GetFormsByOrganizationUniqueId(organizationUniqueId));
        }

        #endregion

    }
}
