using FrameWork.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_Application_Contracts.User
{
    public interface IUserApplication
    {
        Task<OpreationResult> RegisterAsync(RegisterUser command);
        Task<OpreationResult> LoginAsync(LoginUser command);
        Task<OpreationResult> LogOutAsync();
    }
}
