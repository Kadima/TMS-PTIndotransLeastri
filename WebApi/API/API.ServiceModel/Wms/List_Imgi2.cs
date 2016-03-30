using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ServiceStack;
using ServiceStack.ServiceHost;
using ServiceStack.OrmLite;

namespace WebApi.ServiceModel.Wms
{
				[Route("/wms/imgi2", "Get")]				//imgi2?GoodsIssueNoteNo=
    [Route("/wms/action/list/imgi2/{GoodsIssueNoteNo}", "Get")]
    public class List_Imgi2 : IReturn<CommonResponse>
    {
        public string GoodsIssueNoteNo { get; set; }
    }
    public class List_Imgi2_Response
    {
        public int TrxNo { get; set; }
        public int LineItemNo { get; set; }
        public string StoreNo { get; set; }
        public int ProductTrxNo { get; set; }
        public string ProductCode { get; set; }
        public string DimensionFlag { get; set; }
        public int PackingQty { get; set; }
        public int WholeQty { get; set; }
        public int LooseQty { get; set; }
        public string ProductName { get; set; }
        public string SerialNoFlag { get; set; }
        public string SerialNo { get; set; }
        public string UserDefine01 { get; set; }
    }
    public class List_Imgi2_Logic
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }
        public List<List_Imgi2_Response> GetList(List_Imgi2 request)
        {
            List<List_Imgi2_Response> Result = null;
            try
            {
																using (var db = DbConnectionFactory.OpenDbConnection("WMS"))
                {
                    Result = db.Select<List_Imgi2_Response>(
                        "Select * From Imgi2 " +
                        "Left Join Imgi1 On Imgi2.TrxNo=Imgi1.TrxNo " +
                        "Left join Impr1 On Imgi2.ProductTrxNo=Impr1.TrxNo " +
                        "Where Imgi1.GoodsIssueNoteNo={0}",
                        request.GoodsIssueNoteNo
                    );
                }
            }
            catch { throw; }
            return Result;
        }
    }
}
