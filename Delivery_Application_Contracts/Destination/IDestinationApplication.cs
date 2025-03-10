using System.Data;
using System.Linq.Expressions;


namespace Delivery_Application_Contracts.Destination
{
    // The IDestinationApplication interface outlines the operations for
    // a destination application.It includes methods for creation, editing,
    // existence check, retrieval of all destinations, and fetching details
    // for editing a destination.Each method has specific inputs and outputs.
    // also we take our dependencies from classes in this layer
    public interface IDestinationApplication
    {
        Task CreateAsync(CreateDestination command);
        Task EditAsync(EditDestination command);
        Task<bool> ExistAsync(string name,int? id);
        Task<List<DestinationViewModel>> GetDestinationsAsync(string userId);
        Task<EditDestination> GetEditDetailsAsync(int id);
    }
}
