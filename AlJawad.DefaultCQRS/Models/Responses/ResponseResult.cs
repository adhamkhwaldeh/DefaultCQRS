using AlJawad.DefaultCQRS.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlJawad.DefaultCQRS.Models.Responses
{
    public class ResponseResult : Response<string>
    {
        public ResponseResult()
        {

        }
        public ResponseResult(string result)
        {
            if (result.HasValue())
            {
                this.Data = result;
            }
            else this.Data = "No Result";
        }

        public static ResponseResult Successful => new ResponseResult() { Data = "Successful", Status = true };
        public static ResponseResult Failed => new ResponseResult() { Data = "Failed", Status = false };

    }

}
