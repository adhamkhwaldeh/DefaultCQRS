using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlJawad.DefaultCQRS.Models.Responses
{
    public class ResponseBool : Response<bool>
    {
        public static ResponseBool True => new ResponseBool() { Data = true, Status = true };
        public static ResponseBool False => new ResponseBool() { Data = false, Status = true };
    }
}
