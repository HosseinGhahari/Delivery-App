using Delivery_Application_Contracts.Destination;
using Delivery_Domain.AuthAgg;
using Delivery_Domain.DestinationAgg;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        public async Task CreateAsync(CreateDestination command)
        {
            if (await _destinationRepository.ExistAsync(x => x.DestinationName == command.DestinationName))
                throw new Exception("Destination Already Exists");

            var destination = new Destination(command.DestinationName, command.Price , command.UserId);
            await _destinationRepository.CreateAsync(destination);
            await _destinationRepository.SaveChangesAsync();
        }


        // Retrieves the details of a destination for editing.
        // The destination is identified by its id.
        public async Task<EditDestination> GetEditDetailsAsync(int id)
        {
            return await _destinationRepository.GetEditDetailsAsync(id);
        }

        // Retrieves all destinations from the repository.
        public async Task<List<DestinationViewModel>> GetAllAsync(string userId)
        {
            if(string.IsNullOrWhiteSpace(userId))
                throw new UnauthorizedAccessException("User is not authenticated.");

            return await _destinationRepository.GetAllAsync(userId);
        }


        // Edits an existing destination with the given command.
        // Retrieves the destination from the repository using its id.
        // If the destination doesn't exist, throws an exception.
        // If it does, edits the destination and saves the changes to the repository.
        public async Task EditAsync(EditDestination command)
        {
            var destination = await _destinationRepository.GetAsync(command.Id);

            if (destination == null)
                throw new Exception("Destination not found");

            if (await _destinationRepository.ExistAsync(x => x.DestinationName == command.DestinationName && x.Id != command.Id))
                throw new Exception("Desination Already Exist");

            destination.Edit(command.DestinationName, command.Price);
            await _destinationRepository.SaveChangesAsync();
        }

        public async Task<bool> ExistAsync(string name , int? id)
        {
            return await _destinationRepository.ExistAsync(x => x.DestinationName == name && x.Id != id);
        }
    }
}
