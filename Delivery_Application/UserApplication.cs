using Delivery_Application_Contracts.User;
using Delivery_Domain.AuthAgg;
using FrameWork.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_Application
{
    // This class implements the IUserApplication interface, providing 
    // user-related application services. It uses UserManager and SignInManager 
    // from ASP.NET Core Identity to handle user registration and authentication. 
    // The constructor initializes these managers via dependency injection.
    public class UserApplication : IUserApplication
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _HttpContextAccessor;

        public UserApplication(UserManager<User> userManager, SignInManager<User> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _HttpContextAccessor = httpContextAccessor;
        }

        // This asynchronous method manages user registration using the RegisterUser DTO. 
        // It checks for existing usernames and verifies that the password and confirm 
        // password match. If validations pass, it creates a new User and attempts to 
        // register it using UserManager, returning an OperationResult with success or 
        // failure messages based on the registration outcome.
        public async Task<OpreationResult> RegisterAsync(RegisterUser command)
        {
            OpreationResult opreation = new OpreationResult();
            var existingUser = await _userManager.FindByNameAsync(command.UserName);
            if (existingUser != null)
                return opreation.Failed(ApplicationMessages.UserNameExist);

            if (command.Password != command.ConfirmPassword)
                return opreation.Failed(ApplicationMessages.NotSamePassword);

            var identityUser = new User
            {
                UserName = command.UserName,
                Email = command.UserName
            };

            var result = await _userManager.CreateAsync(identityUser,command.Password);
            if (result.Succeeded)
            {
                return opreation.Succeeded(ApplicationMessages.LoginSucceeded);
            }

            return opreation;
        }

        // This asynchronous method handles user login using the LoginUser DTO. 
        // It retrieves the user by username and checks if the password is correct. 
        // If the user is found and the password matches, it signs in the user using 
        // SignInManager and returns an OperationResult indicating success or failure 
        // with appropriate messages based on the login outcome.
        public async Task<OpreationResult> LoginAsync(LoginUser command)
        {
            OpreationResult opreation = new OpreationResult();

            var user = await _userManager.FindByNameAsync(command.UserName);
            if (user == null) 
                return opreation.Failed(ApplicationMessages.NotFound);

            var password = await _userManager.CheckPasswordAsync(user,command.Password);
            if(!password)
                return opreation.Failed(ApplicationMessages.WrongPassword);

            await _signInManager.SignInAsync(user, password);   

            return opreation.Succeeded(ApplicationMessages.LoginSucceeded);        
        }

        // This asynchronous method handles user logout. It returns an OperationResult 
        // indicating success with a message for successful logout. The actual logout 
        // process should typically be implemented with SignInManager but is not shown here.
        public async Task<OpreationResult> LogOutAsync()
        {
            OpreationResult operation = new OpreationResult();
            return operation.Succeeded(ApplicationMessages.LogOutSucceeded);
        }

        // Asynchronously retrieves the current user's information from the user manager.
        // Returns a UsersViewModel containing the username, or null if the user is not authenticated.
        public async Task<UsersViewModel> GetUsers()
        {
            OpreationResult opreation = new OpreationResult();

            var user = await _userManager.GetUserAsync(_HttpContextAccessor.HttpContext.User);
            if (user == null)
                return null;

            return new UsersViewModel
            {
                UserName = user.UserName,
            };
        }
    }
}
