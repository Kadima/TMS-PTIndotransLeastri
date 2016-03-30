using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApi.ServiceModel.Tables
{
    public class Rcvy1_sps
    {
								public string VoyageID { get; set; }
								public string VoyageNo { get; set; }			
        public string VesselCode { get; set; }
								public Nullable<System.DateTime> CloseDateTime { get; set; }
        public Nullable<System.DateTime> ETD { get; set; }
        public Nullable<System.DateTime> ETA { get; set; }
        public int TranSit { get; set; }
        public string PortofDischargeName { get; set; }
        public string ShippinglineName { get; set; }
    }
}
