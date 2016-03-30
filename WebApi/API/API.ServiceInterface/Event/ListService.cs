using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApi.ServiceModel;
using WebApi.ServiceModel.Event;

namespace WebApi.ServiceInterface.Event
{
    public class ListService
    {
        public void ListContainer(Auth auth, List_Container request, List_Container_Logic list_Container_Logic, CommonResponse ecr, string[] token, string uri)
        {
            if (auth.AuthResult(token, uri))
            {
                ecr.data.results = list_Container_Logic.GetList(request);
                if (ecr.data.results != null)
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
        public void ListJmjm6(Auth auth, List_Jmjm6 request, List_Jmjm6_Logic list_Jmjm6_Logic, CommonResponse ecr, string[] token, string uri)
        {
            if (auth.AuthResult(token, uri))
            {
                ecr.data.results = list_Jmjm6_Logic.GetList(request);
                if (ecr.data.results != null)
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
        public void ListJobNo(Auth auth, List_JobNo request, List_JobNo_Logic list_JobNo_Logic, CommonResponse ecr, string[] token, string uri)
        {
            if (auth.AuthResult(token, uri))
            {
																ecr.data.results = list_JobNo_Logic.GetList(request);
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
