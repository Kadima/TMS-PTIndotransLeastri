using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApi.ServiceModel.Tables
{
    public class Smct1
    {
								public int TrxNo { get; set; }
								public string PartyCode { get; set; }
								public string PartyName { get; set; }
								public string PortOfLoadingCode { get; set; }
								public string PortOfDischargeCode { get; set; }
								public Nullable<DateTime> EffectiveDate { get; set; }
								public Nullable<DateTime> ExpiryDate { get; set; }
								public string ModuleCode { get; set; }
								public string JobType { get; set; }
								public string TableType { get; set; }
								public string Description { get; set; }
    }
}
