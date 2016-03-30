using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApi.ServiceModel;
using WebApi.ServiceModel.Wms;
using WebApi.ServiceModel.Freight;

namespace WebApi.ServiceInterface.Freight
{
    public class TableService
				{
								public void TS_Smsa(Auth auth, Smsa request, Smsa_Logic logic, CommonResponse ecr, string[] token, string uri)
								{
												if (auth.AuthResult(token, uri))
												{
																if (uri.IndexOf("/smsa1/count") > 0)
																{
																				ecr.data.results = logic.GetCount(request);
																}
																else if (uri.IndexOf("/smsa1/sps") > 0)
																{
																				ecr.data.results = logic.GetSpsList(request);
																}
																else if (uri.IndexOf("/smsa2/create") > 0)
																{
																				ecr.data.results = logic.Insert_Smsa2(request);
																}
																else if (uri.IndexOf("/smsa2/update") > 0)
																{
																				ecr.data.results = logic.Update_Smsa2(request);
																}
																else if (uri.IndexOf("/smsa2/read") > 0)
																{
																				ecr.data.results = logic.Read_Smsa2(request);
																}
																else if (uri.IndexOf("/smsa2/delete") > 0)
																{
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
								public void TS_Smct(Auth auth, Smct request, Smct_Logic logic, CommonResponse ecr, string[] token, string uri)
								{
												if (auth.AuthResult(token, uri))
												{
																if (uri.IndexOf("/smct1/sps") > 0)
																{
																				ecr.data.results = logic.Get_Smct1_SpsList(request);
																}
																else if (uri.IndexOf("/smct2") > 0)
																{
																				ecr.data.results = logic.Get_Smct2_List(request);
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
								public void TS_Plvi(Auth auth, Plvi request, Plvi_Logic logic, CommonResponse ecr, string[] token, string uri)
								{
												if (auth.AuthResult(token, uri))
												{
																if (uri.IndexOf("/plvi1/sps") > 0)
																{
																				ecr.data.results = logic.Get_Plvi1_SpsList(request);
																}
																else if (uri.IndexOf("/plvi1/update") > 0)
																{
																				ecr.data.results = logic.Update_Plvi1(request);
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
								public void TS_Rcbp(Auth auth, Rcbp request, Rcbp_Logic logic, CommonResponse ecr, string[] token, string uri)
        {
            if (auth.AuthResult(token, uri))
            {
																if (uri.IndexOf("/freight/rcbp1/sps") > 0)
																{
																				ecr.data.results = logic.Get_Rcbp1_SpsList(request);
																}
																else if (uri.IndexOf("/freight/rcbp1/read") > 0)
																{
																				ecr.data.results = logic.Get_Rcbp1_List(request);
																}
																else if (uri.IndexOf("/freight/rcbp1/update") > 0)
																{
																				ecr.data.results = logic.Update_Rcbp1(request);
																}
																else if (uri.IndexOf("/freight/rcbp3/read") > 0)
																{
																				ecr.data.results = logic.Read_Rcbp3_List(request);
																}
																else if (uri.IndexOf("/freight/rcbp3/create") > 0)
																{
																				ecr.data.results = logic.Insert_Rcbp3(request);
																}
																else if (uri.IndexOf("/freight/rcbp3/update") > 0)
																{
																				ecr.data.results = logic.Update_Rcbp3(request);
																}
																else if (uri.IndexOf("/freight/rcbp3/delete") > 0)
																{
																				ecr.data.results = logic.Delete_Rcbp3(request);
																}
																else
																{
																				ecr.data.results = -1;
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
        public void TS_Saus(Auth auth, Saus request, Saus_Logic logic, CommonResponse ecr, string[] token, string uri)
								{
												if (auth.AuthResult(token, uri))
												{
																if (uri.IndexOf("/memo") > 0)
																{
																				ecr.data.results = logic.GetMemo(request);
																}
																else if (uri.IndexOf("/update/memo") > 0)
																{
																				ecr.data.results = logic.Update_Memo(request);
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
        public void TS_Rcvy(Auth auth, Rcvy request, Rcvy_Logic logic, CommonResponse ecr, string[] token, string uri)
        {
            if (auth.AuthResult(token, uri))
            {
                if(uri.IndexOf("/rcvy1/sps") > 0)
																{
                    ecr.data.results = logic.Get_Rcvy1_SpsList(request);
                }
																else if(uri.IndexOf("/rcvy1") > 0)
																{
                    ecr.data.results = logic.Get_Rcvy1_List(request);
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
        public void TS_Tracking(Auth auth, Tracking request, Tracking_Logic logic, CommonResponse ecr, string[] token, string uri)
        {
            if (auth.AuthResult(token, uri))
            {
																if (uri.IndexOf("/tracking/OrderNo") > 0)
																{
																				ecr.data.results = logic.GetOmtx1List(request);
																}
																else if (uri.IndexOf("/tracking/sps") > 0)
																{
																				ecr.data.results = logic.GetSpsList(request);
																}
																else if (uri.IndexOf("/tracking/count") > 0)
																{
																				ecr.data.results = logic.GetCount(request);
																}																
																else
																{
																				ecr.data.results = logic.GetList(request);
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
