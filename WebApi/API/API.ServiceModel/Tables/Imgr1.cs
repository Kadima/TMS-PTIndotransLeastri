using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApi.ServiceModel.Tables
{
    public class Imgr1
    {
        public int TrxNo { get; set; }
        public string CustomerCode { get; set; }
        public string GoodsReceiptNoteNo { get; set; }
        public Nullable<System.DateTime> ReceiptDate { get; set; }
        public string RefNo { get; set; }
        public string StatusCode { get; set; }
    }
}
