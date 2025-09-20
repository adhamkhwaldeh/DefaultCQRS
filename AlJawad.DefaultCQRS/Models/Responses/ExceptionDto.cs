using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlJawad.DefaultCQRS.Models.Responses
{
    public class ExceptionDto : Response<object>
    {
        public ExceptionDto(object result)
        {
            Data = result;
        }
    }

}
