using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace paypart_service_gateway.Models
{
    public class ServiceError
    {
        public string error { get; set; }
        public List<string> errorDetails { get; set; }
        public string error_description { get; set; }
    }
}
