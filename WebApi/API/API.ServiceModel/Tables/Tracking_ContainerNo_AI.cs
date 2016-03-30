using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApi.ServiceModel.Tables
{
    public class Tracking_ContainerNo_AI
    {
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
