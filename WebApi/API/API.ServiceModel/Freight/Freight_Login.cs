using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack;
using ServiceStack.ServiceHost;
using ServiceStack.OrmLite;

namespace WebApi.ServiceModel.Freight
{
    [Route("/freight/login", "Post")]
				[Route("/freight/login/check", "Get")]
    public class Freight_Login : IReturn<CommonResponse>
    {
        public string UserId { get; set; }
								public string Password { get; set; }
								public string Md5Stamp { get; set; }
    }
    public class Freight_Login_Logic
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }
        public int LoginCheck(Freight_Login request) 
        {
            int Result = -1;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection())
                {
																				string strSql = "Select count(*) From Saus1 Where UserId='" + request.UserId + "' And Password=";
																				if (string.IsNullOrEmpty(request.Md5Stamp))
																				{
																								strSql = strSql + "'" + request.Password + "'";
																				}
																				else
																				{
																								strSql = strSql + "'" + request.Md5Stamp + "'";
																				}
																				Result = db.Scalar<int>(strSql);
                }
            }
            catch { throw; }
            return Result;
        }
    }
}
