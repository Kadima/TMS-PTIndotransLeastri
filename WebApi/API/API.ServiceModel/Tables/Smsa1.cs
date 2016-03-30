using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApi.ServiceModel.Tables
{
				public class Smsa1
    {
        public int TrxNo { get; set; }
								public string SalesmanCode { get; set; }
								public string SalesmanName { get; set; }
								public string ContactName { get; set; }
								public string CustomerCode { get; set; }
								public string CustomerName { get; set; }
								public Nullable<System.DateTime> DateTime { get; set; }
								public string Fax { get; set; }
								public string Telephone { get; set; }
								public string Remark { get; set; }
								public string StatusCode { get; set; }
    }
}
