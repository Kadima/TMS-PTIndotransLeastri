using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ServiceStack;
using ServiceStack.ServiceHost;
using ServiceStack.OrmLite;

namespace WebApi.ServiceModel.Event
{
				[Route("/event/action/list/jmjm6/{jobno}", "Get")]
				[Route("/event/action/list/jmjm6", "Get")] //jmjm6?jobno=
    public class List_Jmjm6 : IReturn<CommonResponse>
    {
        public string JobNo { get; set; }
    }
    public class List_Jmjm6_Response
    {
        public string JobNo { get; set; }
        public int LineItemNo { get; set; }
        public string ContainerNo { get; set; }
        public string Remark { get; set; }
        public string JobType { get; set; }
        public string VehicleNo { get; set; }
        public string DriverNo { get; set; }
        public string CargoStatusCode { get; set; }
        public Nullable<System.DateTime> TruckDateTime { get; set; }
        public Nullable<System.DateTime> RecevieDateTime { get; set; }
        public Nullable<System.DateTime> ReadyDateTime { get; set; }
        public Nullable<System.DateTime> UnLoadDateTime { get; set; }
    }
    public class List_Jmjm6_Logic
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }
        public List<List_Jmjm6_Response> GetList(List_Jmjm6 request)
        {
            List<List_Jmjm6_Response> Result = null;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection("TMS"))
                {
                    Result = db.Select<List_Jmjm6_Response>(
                        "Select Jmjm6.JobNo, Jmjm6.LineItemNo, Jmjm6.ContainerNo, Jmjm6.Remark, Jmjm1.JobType, Jmjm6.VehicleNo, " +
                        "Jmjm6.DriverNo, Jmjm6.CargoStatusCode, Jmjm6.TruckDateTime, Jmjm6.RecevieDateTime, Jmjm6.ReadyDateTime, Jmjm6.UnLoadDateTime " +
                        "From Jmjm6 Left Join Jmjm1 on Jmjm6.JobNo=Jmjm1.JobNo WHERE Jmjm1.StatusCode<>'DEL' And Jmjm1.JobNo={0}", request.JobNo
                    );
                }
            }
            catch { throw; }
            return Result;
        }
    }
}
