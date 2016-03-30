using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ServiceStack;
using ServiceStack.ServiceHost;
using ServiceStack.OrmLite;

namespace WebApi.ServiceModel.Wms
{
    [Route("/wms/action/confirm/imgr1", "Post")]
    public class Confirm_Imgr1 : IReturn<CommonResponse>
    {
        public int TrxNo { get; set; }
        public string UserID { get; set; }
    }
    public class Confirm_Imgr1_Logic
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }
        public int Confirm(Confirm_Imgr1 request)
        {
            int Result = -1;
            try
            {
																using (var db = DbConnectionFactory.OpenDbConnection("WMS"))
                {
                    Result = db.SqlScalar<int>("EXEC spi_Imgr_Confirm @TrxNo,@UpdateBy", new { TrxNo = request.TrxNo, UpdateBy = request.UserID });
                    //List<int> results = db.SqlList<int>("EXEC spi_Imgr_Confirm @TrxNo @UpdateBy", new { TrxNo = request.TrxNo, UpdateBy = request.UserID });
                    //using (var cmd = db.SqlProc("spi_Imgr_Confirm", new { TrxNo = request.TrxNo, UpdateBy = request.UserID }))
                    //{
                    //    Result = cmd.ConvertTo<int>();
                    //}
                }
            }
            catch { throw; }
            return Result;
        }
    }
}
