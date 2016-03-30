using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ServiceStack;
using ServiceStack.ServiceHost;
using ServiceStack.OrmLite;
using WebApi.ServiceModel.Tables;

namespace WebApi.ServiceModel.Tms
{
				[Route("/tms/sibl2/list", "Get")]			// list?TrxNo=
				[Route("/tms/sibl2/read", "Get")]			// read?TrxNo= & LineItemNo=
				[Route("/tms/sibl2/update", "Post")]
				[Route("/tms/sibl2/update", "Get")] // update?TrxNo= & LineItemNo= & ContainerNo= & CntrRemark= & CargoStatusCode=
    public class Sibl : IReturn<CommonResponse>
				{
								public string TrxNo { get; set; }
								public string LineItemNo { get; set; }
								public Sibl2 sibl2 { get; set; }
								public string ContainerNo { get; set; }
								public string CntrRemark { get; set; }
								public string CargoStatusCode { get; set; }
    }
				public class Sibl_Logic
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }
								public List<Sibl2> Get_Sibl2_List(Sibl request)
								{
												List<Sibl2> Result = null;
												try
												{
																using (var db = DbConnectionFactory.OpenDbConnection())
																{
																				string strSQL = "Select TrxNo, LineItemNo, ISNULL(ContainerNo,'') AS ContainerNo, ISNULL(CntrRemark,'') AS CntrRemark,ISNULL(CargoStatusCode,'') AS CargoStatusCode, ISNULL(GoodsDescription01,'') AS GoodsDescription01 From Sibl2 Where TrxNo=" + int.Parse(request.TrxNo);
																				Result = db.Select<Sibl2>(strSQL);
																}
												}
												catch { throw; }
												return Result;
								}
								public List<Sibl2> Read_Sibl2_List(Sibl request)
        {
            List<Sibl2> Result = null;
												/*
            try
            {
																using (var db = DbConnectionFactory.OpenDbConnection())
                {
                    if (!string.IsNullOrEmpty(request.BusinessPartyCode))
                    {
																								if (!string.IsNullOrEmpty(request.LineItemNo))
																								{
																												Result = db.Where<Rcbp3>(r1 => r1.BusinessPartyCode == request.BusinessPartyCode && r1.LineItemNo == int.Parse(request.LineItemNo));
																								}
																								else
																								{
																												Result = db.Where<Rcbp3>(r1 => r1.BusinessPartyCode == request.BusinessPartyCode);
																								}
                    }
                }
            }
            catch { throw; }
												 * */
            return Result;
        }
								public int Update_Sibl2(Sibl request)
								{
												int Result = -1;
												try
												{
																using (var db = DbConnectionFactory.OpenDbConnection())
																{
																				Result = db.Update<Sibl2>(
																								new
																								{
																												ContainerNo = request.ContainerNo,
																												CntrRemark = request.CntrRemark,
																												CargoStatusCode = request.CargoStatusCode
																								},
																								p => p.TrxNo == int.Parse(request.TrxNo) && p.LineItemNo == int.Parse(request.LineItemNo)
																				);
																}
												}
												catch { throw; }
												return Result;
								}
    }
}
