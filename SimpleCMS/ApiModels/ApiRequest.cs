using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleCMS.ApiModels
{
    public class ApiRequest
    {
        public HttpRequest Request { get; set; }

        public bool IsValid { get; set; }

        public ApiRequest()
        {
            IsValid = true;
        }
    }
}