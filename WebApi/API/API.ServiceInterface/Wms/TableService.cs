using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApi.ServiceModel;
using WebApi.ServiceModel.Wms;

namespace WebApi.ServiceInterface.Wms
{
    public class TableService
    {
        private class job
        {
            public string JobNo { get; set; }
            public string ContainerCounts { get; set; }
        }
								public void List_Rcbp1(Auth auth, List_Rcbp1 request, List_Rcbp1_Logic list_Rcbp1_Logic, CommonResponse ecr, string[] token, string uri)
								{
												if (auth.AuthResult(token, uri))
												{
																if (uri.IndexOf("/wms/rcbp1") > 0)
																{
																				ecr.data.results = list_Rcbp1_Logic.GetList(request);
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
        public void List_Imgr1(Auth auth, List_Imgr1 request, List_Imgr1_Logic list_Imgr1_Logic, CommonResponse ecr, string[] token, string uri)
        {
            if (auth.AuthResult(token, uri))
            {
                ecr.data.results = list_Imgr1_Logic.GetList(request);
                ecr.meta.code = 200;
                ecr.meta.message = "OK";                
            }
            else
            {
                ecr.meta.code = 401;
                ecr.meta.message = "Unauthorized";
            }
        }
        public void List_Impr1(Auth auth, List_Impr1 request, List_Impr1_Logic list_Impr1_Logic, CommonResponse ecr, string[] token, string uri)
        {
            if (auth.AuthResult(token, uri))
            {
                ecr.data.results = list_Impr1_Logic.GetList(request);
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
        public void List_Imgr2(Auth auth, List_Imgr2 request, List_Imgr2_Logic list_Imgr2_Logic, CommonResponse ecr, string[] token, string uri)
        {
            if (auth.AuthResult(token, uri))
            {
                ecr.data.results = list_Imgr2_Logic.GetList(request);
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
        public void List_Imgi1(Auth auth, List_Imgi1 request, List_Imgi1_Logic list_Imgi1_Logic, CommonResponse ecr, string[] token, string uri)
        {
            if (auth.AuthResult(token, uri))
            {
                ecr.data.results = list_Imgi1_Logic.GetList(request);
                ecr.meta.code = 200;
                ecr.meta.message = "OK";
            }
            else
            {
                ecr.meta.code = 401;
                ecr.meta.message = "Unauthorized";
            }
        }
        public void List_Imgi2(Auth auth, List_Imgi2 request, List_Imgi2_Logic list_Imgi2_Logic, CommonResponse ecr, string[] token, string uri)
        {
            if (auth.AuthResult(token, uri))
            {
                ecr.data.results = list_Imgi2_Logic.GetList(request);
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
        public void List_Imsn1(Auth auth, List_Imsn1 request, List_Imsn1_Logic list_Imsn1_Logic, CommonResponse ecr, string[] token, string uri)
        {
            if (auth.AuthResult(token, uri))
            {
                ecr.data.results = list_Imsn1_Logic.GetList(request);
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
    }
}
