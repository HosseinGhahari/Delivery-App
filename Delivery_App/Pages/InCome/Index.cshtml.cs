using Delivery_Application_Contracts.Delivery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Delivery_App.Pages.InCome
{
    public class IndexModel : BasePageModel
    {
        // our list to fill out the MonthlyInCome
        public List<InComeViewModel> InCome { get; set; }

        private readonly IDeliveryApplication _deliveryApplication;
        public IndexModel(IDeliveryApplication deliveryApplication)
        {
            _deliveryApplication = deliveryApplication;
        }

        // in this method we fill our List with our data
        public async Task OnGetAsync()
        {
            InCome = await _deliveryApplication.GetInComeAsync();        
        }
    }
}
