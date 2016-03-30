using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApi.ServiceModel.Tables
{
    public class Tracking_ContainerNo_AE
    {
        public string FirstToDestCode { get; set; }
        public string FirstByAirlineID { get; set; }
        public string FirstFlightNo { get; set; }
        public Nullable<System.DateTime> FirstFlightDate { get; set; }
        public string SecondToDestCode { get; set; }
        public string SecondByAirlineID { get; set; }
        public string SecondFlightNo { get; set; }
        public Nullable<System.DateTime> SecondFlightDate { get; set; }
        public string ThirdToDestCode { get; set; }
        public string ThirdByAirlineID { get; set; }
        public string ThirdFlightNo { get; set; }
        public Nullable<System.DateTime> ThirdFlightDate { get; set; }
        public string ModuleCode { get; set; }
        public string JobNo { get; set; }
        public string JobType { get; set; }
        public string ReferenceNo { get; set; }
        public string AwbBlNo { get; set; }
        public string MawbOBLNo { get; set; }
        public string OriginCode { get; set; }
        public string DestCode { get; set; }
        public int Pcs { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal Volume { get; set; }
								public string Commodity { get; set; }
								public string UomDescription { get; set; }
								public string OriginName { get; set; }
								public string DestName { get; set; }
    }
}
