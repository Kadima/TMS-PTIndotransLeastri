using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApi.ServiceModel.Tables
{
    public class Imgr2
    {
        public int TrxNo { get; set; }
        public int LineItemNo { get; set; }
        public int ProductTrxNo { get; set; }
        public string ProductCode { get; set; }
        public string DimensionFlag { get; set; }
        public int PackingQty { get; set; }
        public int WholeQty { get; set; }
        public int LooseQty { get; set; }
    }
}
