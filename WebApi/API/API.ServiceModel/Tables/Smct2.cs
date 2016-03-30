using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApi.ServiceModel.Tables
{
    public class Smct2
    {
								public int TrxNo { get; set; }
								public int LineItemNo { get; set; }
								public string CargoType { get; set; }
								public string ChargeUnit { get; set; }
								public string ChargeDescription { get; set; }
								public string CurrCode { get; set; }
								public string VatCode { get; set; }
								public decimal MinAmt { get; set; }
								public decimal QuoteAmt { get; set; }
    }
}
