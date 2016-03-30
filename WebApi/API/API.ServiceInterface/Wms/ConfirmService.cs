using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApi.ServiceModel;
using WebApi.ServiceModel.Wms;

namespace WebApi.ServiceInterface.Wms
{
    public class ConfirmService
    {
        public void ConfirmImgr1(Auth auth, Confirm_Imgr1 request, Confirm_Imgr1_Logic confirmImgr1Logic, CommonResponse ecr, string[] token, string uri)
        {
            if (auth.AuthResult(token, uri))
            {
                if (confirmImgr1Logic.Confirm(request) > -1)
                {
                    ecr.meta.code = 200;
                    ecr.meta.message = "OK";
                }
                else
                {
                    ecr.meta.code = 612;
                    ecr.meta.message = "The specified resource does not exist";
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
