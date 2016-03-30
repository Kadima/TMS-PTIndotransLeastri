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
    [Route("/wms/action/list/impr1/{BarCode}", "Get")]
    public class List_Impr1 : IReturn<CommonResponse>
    {
        public string BarCode { get; set; }
    }
    public class List_Impr1_Logic
    {        
        public IDbConnectionFactory DbConnectionFactory { get; set; }
        public Impr1 GetList(List_Impr1 request)
        {
            Impr1 Result = null;
            try
            {
																using (var db = DbConnectionFactory.OpenDbConnection("WMS"))
                {
                    Result = db.QuerySingle<Impr1>(
                        "Select * From Impr1 Where IsNull(ProductCode,'')<>'' And IsNull(StatusCode,'')<>'DEL' And UserDefine01=" + Modfunction.SQLSafeValue(request.BarCode)
                    );
                }
            }
            catch { throw; }
            return Result;
        }
    }
}
