using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApi.ServiceModel.Tables
{
				public class Tracking_OrderNo
    {
								public int TrxNo { get; set; }
								public string PortOfLoadingName { get; set; }
								public string PortOfDischargeName { get; set; }
								public string CommodityDescription { get; set; }
								public string DeliveryTypeName { get; set; }
								public string DestName { get; set; }
								public string OriginName { get; set; }
								public string BookingNo { get; set; }
								public string JobNo { get; set; }
								public string AirportDeptName { get; set; }
								public string AirportDestName { get; set; }
        //public Nullable<System.DateTime> FirstFlightDate { get; set; }
    }
}
