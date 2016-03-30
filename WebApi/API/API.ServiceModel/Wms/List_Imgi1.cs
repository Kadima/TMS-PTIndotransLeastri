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

				[Route("/wms/imgi1", "Get")]				//imgi1?GoodsIssueNoteNo= & CustomerCode=
    [Route("/wms/action/list/imgi1/{CustomerCode}", "Get")]
    [Route("/wms/action/list/imgi1/gin/", "Get")]
    [Route("/wms/action/list/imgi1/gin/{GoodsIssueNoteNo}", "Get")]
    public class List_Imgi1 : IReturn<CommonResponse>
    {
        public string CustomerCode { get; set; }
        public string GoodsIssueNoteNo { get; set; }
    }
    public class List_Imgi1_Logic
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }
        public List<Imgi1> GetList(List_Imgi1 request)
        {
            List<Imgi1> Result = null;
            try
            {
																using (var db = DbConnectionFactory.OpenDbConnection("WMS"))
                {
                    if (!string.IsNullOrEmpty(request.CustomerCode))
                    {
                        Result = db.SelectParam<Imgi1>(
                            i => i.CustomerCode != null && i.CustomerCode != "" && i.StatusCode != null && i.StatusCode != "DEL" && i.StatusCode!="EXE" && i.StatusCode!="CMP" && i.CustomerCode == request.CustomerCode
                        ).OrderByDescending(i => i.IssueDateTime).ToList<Imgi1>();
                    }
                    else if (!string.IsNullOrEmpty(request.GoodsIssueNoteNo))
                    {
                        Result = db.SelectParam<Imgi1>(
                            i => i.CustomerCode != null && i.CustomerCode != "" && i.StatusCode != null && i.StatusCode != "DEL" && i.StatusCode!="EXE" && i.StatusCode!="CMP" && i.GoodsIssueNoteNo.StartsWith(request.GoodsIssueNoteNo)
                        ).OrderByDescending(i => i.IssueDateTime).ToList<Imgi1>();
                    }                  
                }
            }
            catch { throw; }
            return Result;
        }
    }
}
