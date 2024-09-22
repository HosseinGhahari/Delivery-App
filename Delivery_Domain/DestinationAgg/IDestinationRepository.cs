using Delivery_Application_Contracts.Destination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_Domain.DestinationAgg
{

    // In this section, we establish the interface for the Destination Repository.
    // The implementation of these interfaces is carried out in their respective
    // repositories within the Infrastructure layer.
    public interface IDestinationRepository
    {
        Task<Destination> GetAsync(int id);
        Task CreateAsync(Destination destination);
        Task<bool> ExistAsync(string name);
        Task<List<DestinationViewModel>> GetAllAsync(string userId);
        Task<EditDestination> GetEditDetailsAsync(int id);
        Task SaveChangesAsync();
    }

}
