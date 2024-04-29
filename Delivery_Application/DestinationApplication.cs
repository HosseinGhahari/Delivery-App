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
        // Here, we inject our interface from the Infrastructure layer
        // and inherit from 'IDestinationRepository' to perform the primary operations.
        // also provides a clear and concise explanation of how the interface from
        // the Infrastructure layer is being used and the role of ‘IDestinationRepository’

        private readonly IDestinationRepository _destinationRepository;
        public DestinationApplication(IDestinationRepository destinationRepository)
        {
            _destinationRepository = destinationRepository;
        }


        // Creates a new destination with the given command.
        // Checks if the destination already exists in the repository.
        // If it does, throws an exception.
        // If not, creates a new destination and adds it to the repository.
        // Finally, saves the changes to the repository.
        public void Create(CreateDestination command)
        {
            if (_destinationRepository.Exist(command.DestinationName))
                throw new Exception("Destination Already Exists");

            var destination = new Destination(command.DestinationName, command.Price);
            _destinationRepository.Create(destination);
            _destinationRepository.SaveChanges();
        }


        // Retrieves the details of a destination for editing.
        // The destination is identified by its id.
        public EditDestination GetEditDetailes(int id)
        {
            return _destinationRepository.GetEditDetailes(id);
        }


        // Checks if a destination with the given name exists in
        // the repository Returns true if it exists, false otherwise.
        public bool Exist(string name)
        {
            return _destinationRepository.Exist(name);
        }


        // Retrieves all destinations from the repository.
        public List<DestinationViewModel> GetAll()
        {
            return _destinationRepository.GetAll();
        }


        // Edits an existing destination with the given command.
        // Retrieves the destination from the repository using its id.
        // If the destination doesn't exist, throws an exception.
        // If it does, edits the destination and saves the changes to the repository.
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
