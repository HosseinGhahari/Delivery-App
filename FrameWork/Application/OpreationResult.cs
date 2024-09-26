using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameWork.Application
{
    // This class handles operation results, indicating success or failure.
    // It provides methods to mark an operation as succeeded or failed,
    // along with an accompanying message.
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
