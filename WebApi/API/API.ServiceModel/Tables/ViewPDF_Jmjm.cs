using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApi.ServiceModel.Tables
{
				public class ViewPDF_Jmjm
    {
								public string JobNo { get; set; }
								public Nullable<DateTime> JobDate { get; set; }
								public string CustomerName { get; set; }
								public decimal InvoiceLocalAmt { get; set; }
								public string FileName { get; set; }
    }
}
