using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApi.ServiceModel.Tables
{
				public class Smsa2
    {
        public int TrxNo { get; set; }
								public int LineItemNo { get; set; }
								public string Action { get; set; }
								public string Conclusion { get; set; }
								public string CustomerCode { get; set; }
								public string CustomerName { get; set; }
								public Nullable<System.DateTime> DateTime { get; set; }
								public string Description { get; set; }
								public string Discussion { get; set; }
								public string QuotationNo { get; set; }
								public string Reference { get; set; }
								public string Remark { get; set; }
								public string Status { get; set; }
    }
}
