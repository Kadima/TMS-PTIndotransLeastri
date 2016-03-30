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
				[Route("/wms/imsn1", "Get")]				//imsn1?GoodsIssueNoteNo=
    [Route("/wms/action/list/imsn1/{GoodsIssueNoteNo}", "Get")]
    public class List_Imsn1 : IReturn<CommonResponse>
    {
        public string GoodsIssueNoteNo { get; set; }
    }
    public class List_Imsn1_Logic
    {
        private class Imgi1
        {
            public int TrxNo { get; set; }
            public string GoodsIssueNoteNo { get; set; }
        }
        private class Imgi2
        {
            public int TrxNo { get; set; }
            public int LineItemNo { get; set; }
        }
        public IDbConnectionFactory DbConnectionFactory { get; set; }
        public List<Imsn1> GetList(List_Imsn1 request)
        {
            List<Imsn1> Result = null;
            try
            {
																using (var db = DbConnectionFactory.OpenDbConnection("WMS"))
                {
                    Result = db.Select<Imsn1>(
                        "Select * From Imsn1 " +
                        "Left Join Imgi1 On Imsn1.IssueNoteNo = Imgi1.GoodsIssueNoteNo " +
                        "Left Join Imgi2 On Imgi1.TrxNo = Imgi2.TrxNo " +
                        "Where Imsn1.IssueLineItemNo = Imgi2.LineItemNo And Imgi1.GoodsIssueNoteNo={0}",
                        request.GoodsIssueNoteNo
                    );
                }
            }
            catch { throw; }
            return Result;
        }
    }
}
