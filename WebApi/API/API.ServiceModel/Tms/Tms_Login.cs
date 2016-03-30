using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack;
using ServiceStack.ServiceHost;
using ServiceStack.OrmLite;

namespace WebApi.ServiceModel.Tms
{
				[Route("/tms/login", "Post")]
				[Route("/tms/login/check", "Get")]
    public class Tms_Login : IReturn<CommonResponse>
    {
        public string PhoneNumber { get; set; }
        public string CustomerCode { get; set; }
        public string JobNo { get; set; }
    }
    public class Tms_Login_Logic
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }
								public int LoginCheck(Tms_Login request) 
        {
            int Result = -1;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection("TMS"))
                {
                    if (request.PhoneNumber != null && request.PhoneNumber.Length > 0)
                    {
                        Result = db.Scalar<int>("Select count(*) From Jmjm4 Where PhoneNumber='" + request.PhoneNumber + "'");
                    }
                    else if (request.CustomerCode != null && request.CustomerCode.Length > 0 && request.JobNo != null && request.JobNo.Length > 0)
                    {
                        Result = db.Scalar<int>("Select count(*) From Jmjm6 Left Join Jmjm1 on Jmjm6.JobNo=Jmjm1.JobNo Where Jmjm1.StatusCode<>'DEL' and Jmjm1.JobNo='" + request.JobNo + "'");
                    }                    
                }
            }
            catch { throw; }
            return Result;
        }
								public string GetUserInfo(Tms_Login request)
        {
            string Result = "";
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection("TMS"))
                {
                    Result = db.QuerySingle<string>("Select Top 1 ISNULL(DriverName,'') From Jmjm4 Where PhoneNumber=" + Modfunction.SQLSafeValue(request.PhoneNumber));
                }
            }
            catch { throw; }
            return Result;
        }
    }
}
