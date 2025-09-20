using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AlJawad.DefaultCQRS.Models.Responses
{
    public class ResponseList<T> : Response<IEnumerable<T>>
    {
        [DataMember]
        public int PageCount { get; set; }
        [DataMember]
        public int PageSize { get; set; }
        [DataMember]
        public int Page { get; set; }
        [DataMember]
        public int Total { get; set; }
    }

}
