using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApi.ServiceModel.Tables
{
				public class ViewPDF_Ivcr
    {
								public int TrxNo { get; set; }
								public string InvoiceNo { get; set; }
								public Nullable<DateTime> InvoiceDate { get; set; }
								public string CustomerName { get; set; }
								public decimal InvoiceAmt { get; set; }
								public string FileName { get; set; }
    }
}
