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
    public class UserApplication : IUserApplication
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserApplication(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

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

        public async Task<OpreationResult> LogOutAsync()
        {
            OpreationResult operation = new OpreationResult();
            return operation.Succeeded(ApplicationMessages.LogOutSucceeded);
        }
    }
}
