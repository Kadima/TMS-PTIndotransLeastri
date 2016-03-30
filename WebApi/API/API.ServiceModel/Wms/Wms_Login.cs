using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack;
using ServiceStack.ServiceHost;
using ServiceStack.OrmLite;

namespace WebApi.ServiceModel.Wms
{
    [Route("/wms/action/list/login", "Post")]
				[Route("/wms/login/check", "Get")]
    public class Wms_Login : IReturn<CommonResponse>
    {
        public string UserId { get; set; }
        public string Password { get; set; }
    }
    public class Wms_Login_Logic
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }
        public int LoginCheck(Wms_Login request) 
        {
            int Result = -1;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection("WMS"))
                {
                    Result = db.Scalar<int>(
                        "Select count(*) From Saus1 Where UserId={0} And Password={1}",
                        request.UserId,request.Password
                    );
                }
            }
            catch { throw; }
            return Result;
        }
    }
}
