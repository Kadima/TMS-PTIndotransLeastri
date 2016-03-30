using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApi.ServiceModel;
using WebApi.ServiceModel.Freight;

namespace WebApi.ServiceInterface.Freight
{
				public class PdfService
				{
								public void PS_View(Auth auth, ViewPDF request, ViewPDF_Logic logic, CommonResponse ecr, string[] token, string uri)
								{
												if (auth.AuthResult(token, uri))
												{
																if (uri.IndexOf("/pdf/file") > 0)
																{
																				ecr.data.results = logic.Get_File(request);
																}
																else if (uri.IndexOf("/pdf") > 0)
																{
																				ecr.data.results = logic.Get_List(request);
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
