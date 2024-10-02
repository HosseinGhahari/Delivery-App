using Delivery_Application_Contracts.Delivery;
using Delivery_Application_Contracts.User;
using Delivery_Domain.AuthAgg;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Delivery_App.Pages.AccountManagement
{
    public class EditUserModel : BasePageModel
    {
        public EditUser Command { get; set; }
        private readonly IUserApplication _userApplication;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EditUserModel(IUserApplication userApplication,
            IHttpContextAccessor httpContextAccessor
            , UserManager<User> userManager
            , IDeliveryApplication deliveryApplication) : base(deliveryApplication, userManager)
        {
            _userApplication = userApplication;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task OnGet()
        {
            await base.OnGetUserNameAsync();
            await base.OnGetPricesAsync();
        }

        public async Task<IActionResult> OnPost(EditUser command)
        {
            ModelState.Remove("command.Id");
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            command.Id = userId;

            if (!ModelState.IsValid)
                return Page();

            var result = await _userApplication.EditUserAsync(command);        
            if (result.IsSucceeded)
            {
                return RedirectToPage("/Index"); 
            }
            else
            {
                TempData["ErrorMessage"] = result.Message; 
                return Page(); 
            }
        }
    }
}
