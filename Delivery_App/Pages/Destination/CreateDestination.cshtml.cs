using Delivery_Application_Contracts.Destination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Delivery_App.Pages.Destination
{
    public class CreateDestinationModel : PageModel
    {
        public CreateDestination command { get; set; }

        private readonly IDestinationApplication _destinationApplication;
        public CreateDestinationModel(IDestinationApplication destinationApplication)
        {
            _destinationApplication = destinationApplication;
        }

        public RedirectToPageResult OnPost(CreateDestination command) 
        {
            if(_destinationApplication.Exist(command.DestinationName))
            {
                TempData["Exist"] = "مقصد مورد نظر تکراری میباشد";
                return RedirectToPage();
            }

            if(ModelState.IsValid)
            {
                _destinationApplication.Create(command);
                return RedirectToPage("./Index");
            }

            TempData["Error"] = "لطفا مقادیر خواسته شده را به صورت صحیح تکمیل نمایید";
            return RedirectToPage();
        }
    }
}
