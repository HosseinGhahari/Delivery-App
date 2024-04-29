using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Delivery_Application_Contracts.Destination
{
    public interface IDestinationApplication
    {
        void Create(CreateDestination command);
        void Edit(EditDestination command);
        bool Exist(string name);
        List<DestinationViewModel> GetAll();
        EditDestination GetEditDetailes(int id);
       

    }
}
