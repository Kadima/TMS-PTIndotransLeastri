using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ServiceStack;
using ServiceStack.ServiceHost;
using ServiceStack.OrmLite;
using WebApi.ServiceModel.Tables;

namespace WebApi.ServiceModel.Wms
{
    [Route("/wms/action/list/imgr2/{GoodsReceiptNoteNo}", "Get")]
    public class List_Imgr2 : IReturn<CommonResponse>
    {
        public string GoodsReceiptNoteNo { get; set; }
    }
    public class List_Imgr2_Logic
    {        
        public IDbConnectionFactory DbConnectionFactory { get; set; }
        public List<Imgr2> GetList(List_Imgr2 request)
        {
            List<Imgr2> Result = null;
            try
            {
																using (var db = DbConnectionFactory.OpenDbConnection("WMS"))
                {
                    Result = db.Select<Imgr2>(
                        "Select * From Imgr2 " +
                        "Left Join Imgr1 On Imgr2.TrxNo = Imgr1.TrxNo " +
                        "Where Imgr1.GoodsReceiptNoteNo={0}",
                        request.GoodsReceiptNoteNo
                    );
                }
            }
            catch { throw; }
            return Result;
        }
    }
}
