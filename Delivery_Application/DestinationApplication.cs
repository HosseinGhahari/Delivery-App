using Delivery_Application_Contracts.Destination;
using Delivery_Domain.DestinationAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_Application
{
    public class DestinationApplication : IDestinationApplication
    {

        private readonly IDestinationRepository _destinationRepository;
        public DestinationApplication(IDestinationRepository destinationRepository)
        {
            _destinationRepository = destinationRepository;
        }
        public void Create(CreateDestination command)
        {
            if (_destinationRepository.Exist(command.DestinationName))
                throw new Exception("Destination Already Exists");

            var destination = new Destination(command.DestinationName, command.Price);
            _destinationRepository.Create(destination);
            _destinationRepository.SaveChanges();
        }

        public EditDestination GetEditDetailes(int id)
        {
            return _destinationRepository.GetEditDetailes(id);
        }

        public bool Exist(string name)
        {
            return _destinationRepository.Exist(name);
        }

        public List<DestinationViewModel> GetAll()
        {
            return _destinationRepository.GetAll();
        }

        public void Edit(EditDestination command)
        {
            var destination = _destinationRepository.Get(command.Id);

            if (destination == null)
                throw new Exception();

            destination.Edit(command.DestinationName, command.Price);
            _destinationRepository.SaveChanges();       
        }

    }
}
