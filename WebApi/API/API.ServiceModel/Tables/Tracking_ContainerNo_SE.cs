using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApi.ServiceModel.Tables
{
    public class Tracking_ContainerNo_SE
    {
        public string VesselName { get; set; }
        public string VoyageNo { get; set; }
        public string FeederVesselName { get; set; }
        public string FeederVoyage { get; set; }
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
        public Nullable<System.DateTime> ETD { get; set; }
        public Nullable<System.DateTime> ETA { get; set; }
        public Nullable<System.DateTime> ATA { get; set; }
								public string ContainerNo { get; set; }
								public string CityCode { get; set; }
								public string PortOfLoadingName { get; set; }
								public string PortOfDischargeName { get; set; }
								public int NoOf20ftContainer { get; set; }
								public int NoOf40FtContainer { get; set; }
								public int NoOf45FtContainer { get; set; }
								public string UomDescription { get; set; }
    }
}
