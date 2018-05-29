using System;
using System.Collections.Generic;

namespace paypart_service_gateway       .Models
{
    

    public partial class ServiceAccount
    {
        public int id { get; set; }
        public int serviceid { get; set; }
        public string bankcode { get; set; }
        public string bankname { get; set; }
        public string accountnumber { get; set; }
        public string accountname { get; set; }
        public string accountref { get; set; }
        public int status { get; set; }
    }
}
