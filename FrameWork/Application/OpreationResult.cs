using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameWork.Application
{
    public class OpreationResult
    {
        public bool IsSucceeded { get; set; }
        public string Message { get; set; }

        public OpreationResult Succeeded(string message)
        {
            IsSucceeded = true;
            Message = message;
            return this;
        }

        public OpreationResult Failed(string message)
        {
            IsSucceeded = false;
            Message = message;
            return this;
        }
    }
}
