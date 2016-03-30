using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApi.ServiceModel;
using WebApi.ServiceModel.Event;

namespace WebApi.ServiceInterface.Event
{
    public class LoginService
    {
								public void initial(Auth auth, Event_Login request, Event_Login_Logic loginLogic, CommonResponse ecr, string[] token, string uri)
        {
            if (auth.AuthResult(token, uri))
            {
                if (loginLogic.LoginCheck(request) > 0)
                {
                    ecr.meta.code = 200;
                    ecr.meta.message = "OK";
                    ecr.data.results = loginLogic.GetUserInfo(request);
                }
                else
                {
                    ecr.meta.code = 612;
                    ecr.meta.message = "Invalid Phone Number";
                }
            }
            else
            {
                ecr.meta.code = 401;
                ecr.meta.message = "Unauthorized";
            }
        }
    }
}
