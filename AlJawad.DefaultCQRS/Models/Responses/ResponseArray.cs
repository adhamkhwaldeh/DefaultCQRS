using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlJawad.DefaultCQRS.Models.Responses
{
    public class ResponseArray<T> : Response<IEnumerable<T>>
    {
        public static ResponseArray<T> Empty => new ResponseArray<T> { Data = new List<T>(), Status = true };

    }

}