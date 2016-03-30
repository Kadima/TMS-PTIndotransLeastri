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
    [Route("/wms/action/list/imgr1/{CustomerCode}", "Get")]
    [Route("/wms/action/list/imgr1/grn/", "Get")]
    [Route("/wms/action/list/imgr1/grn/{GoodsReceiptNoteNo}", "Get")]
				[Route("/wms/imgr1", "Get")]				//imgr1?GoodsReceiptNoteNo= & CustomerCode=
    public class List_Imgr1 : IReturn<CommonResponse>
    {
        public string CustomerCode { get; set; }
        public string GoodsReceiptNoteNo { get; set; }
    }
    public class List_Imgr1_Logic
    {        
        public IDbConnectionFactory DbConnectionFactory { get; set; }
        public List<Imgr1> GetList(List_Imgr1 request)
        {
            List<Imgr1> Result = null;
            try
            {
																using (var db = DbConnectionFactory.OpenDbConnection("WMS"))
                {
                    if (!string.IsNullOrEmpty(request.CustomerCode))
                    {
                        Result = db.SelectParam<Imgr1>(
                            i => i.CustomerCode != null && i.CustomerCode != "" && i.GoodsReceiptNoteNo != null && i.GoodsReceiptNoteNo != "" && i.StatusCode != null && i.StatusCode != "DEL" && i.StatusCode != "EXE" && i.StatusCode != "CMP" && i.CustomerCode == request.CustomerCode
                        ).OrderByDescending(i => i.ReceiptDate).ToList<Imgr1>();
                    }
                    else if (!string.IsNullOrEmpty(request.GoodsReceiptNoteNo))
                    {
                        Result = db.SelectParam<Imgr1>(
                             i => i.CustomerCode != null && i.CustomerCode != "" && i.GoodsReceiptNoteNo != null && i.GoodsReceiptNoteNo != "" && i.StatusCode != null && i.StatusCode != "DEL" && i.StatusCode != "EXE" && i.StatusCode != "CMP" && i.GoodsReceiptNoteNo.StartsWith(request.GoodsReceiptNoteNo)
                        );
                    }                  
                }
            }
            catch { throw; }
            return Result;
        }
    }
}
