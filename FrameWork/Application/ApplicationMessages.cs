using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameWork.Application
{
    // This class contains constant messages used throughout the application for user feedback.
    // It covers various scenarios like login, logout, registration, and error handling messages.
    public class ApplicationMessages
    {
        public const string UserNameExist = "نام کاربری از قبل وجود دارد";
        public const string LoginSucceeded = "شما با موفقیت وارد شدید";
        public const string NotFound = "کاربری یافت نشد";
        public const string WrongPassword = "رمز عبور اشتباه میباشد";
        public const string SignUpSucceeded = "ثبت نام با موفقیت انجام شد";
        public const string LogOutSucceeded = "خروج با موفقیت انجام شد";
        public const string NotSamePassword = "رمز های عبور یکسان نمیباشد";
        public const string UpdateUser = "مشخصات کاربر بروزرسانی شد";
    }
}
