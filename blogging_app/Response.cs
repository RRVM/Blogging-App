using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blogging_app.Models
{
    public class cls_Response
    {
        public string Success = "success";
        public string Failure = "failure";
        public string Exception = "exception";
        public string status { get; set; }
        public object data { get; set; }
        public object result { get; set; }
        public string url { get; set; }
    }
}
