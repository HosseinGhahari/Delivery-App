using FrameWork.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_Application_Contracts.User
{
    // This interface defines the contract for user-related operations in the application layer. 
    // It includes asynchronous methods for registering a new user, logging in, and logging out. 
    // Each method returns an OperationResult to indicate the success or failure of the operation.
    public interface IUserApplication
    {
        Task<OpreationResult> RegisterAsync(RegisterUser command);
        Task<OpreationResult> LoginAsync(LoginUser command);
        Task<OpreationResult> LogOutAsync();
        Task<UsersViewModel> GetUsers();
        Task<OpreationResult> EditUser(EditUser command);
    }
}
