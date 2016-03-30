using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApi.ServiceModel;
using WebApi.ServiceModel.Tms;
using WebApi.ServiceModel.Freight;

namespace WebApi.ServiceInterface.Tms
{
    public class TableService
				{
								public void TS_Jmjm(Auth auth, Jmjm request, Jmjm_Logic logic, CommonResponse ecr, string[] token, string uri)
								{
												if (auth.AuthResult(token, uri))
												{
																if (uri.IndexOf("/jmjm1/sps") > 0)
																{
																				ecr.data.results = logic.Get_JobContainer_SpsList(request);
																}																
																ecr.meta.code = 200;
																ecr.meta.message = "OK";
												}
												else
												{
																ecr.meta.code = 401;
																ecr.meta.message = "Unauthorized";
												}
								}
								public void TS_Sibl(Auth auth, Sibl request, Sibl_Logic logic, CommonResponse ecr, string[] token, string uri)
								{
												if (auth.AuthResult(token, uri))
												{
																if (uri.IndexOf("/sibl2/list") > 0)
																{
																				ecr.data.results = logic.Get_Sibl2_List(request);
																}
																else if (uri.IndexOf("/sibl2/update") > 0)
																{
																				ecr.data.results = logic.Update_Sibl2(request);
																}
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
