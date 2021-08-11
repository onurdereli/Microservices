﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Models
{
    public class ServiceApiSettings
    {
        public string IdentityBaseUri { get; set; }

        public string GatewayBaseUri { get; set; }

        public string PhotoStockUri { get; set; }

        public ServiceApi Catalog { get; set; }
    }

    public class ServiceApi
    {
        public string Path { get; set; }
    }
}
