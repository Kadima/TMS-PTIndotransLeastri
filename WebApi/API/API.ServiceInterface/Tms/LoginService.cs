using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApi.ServiceModel;
using WebApi.ServiceModel.Freight;

namespace WebApi.ServiceInterface.Tms
{
    public class LoginService
    {
        public void initial(Auth auth, Freight_Login request, Freight_Login_Logic loginLogic, CommonResponse ecr, string[] token, string uri)
        {
            if (auth.AuthResult(token, uri))
            {
																ecr.data.results = loginLogic.LoginCheck(request);
                ecr.meta.code = 200;
                ecr.meta.message = "OK";
            }
            else
            {
                ecr.meta.code = 401;
                ecr.meta.message = "Unauthorized";
            }
        }
    }
}
