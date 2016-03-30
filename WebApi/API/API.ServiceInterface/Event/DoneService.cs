using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApi.ServiceModel;
using WebApi.ServiceModel.Event;

namespace WebApi.ServiceInterface.Event
{
    public class DoneService
    {
        public void initial(Auth auth, Update_Done request, Update_Done_Logic eventdoneLogic, CommonResponse ecr, string[] token, string uri)
        {
            if (auth.AuthResult(token, uri))
            {
																ecr.data.results = eventdoneLogic.UpdateDone(request);
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
