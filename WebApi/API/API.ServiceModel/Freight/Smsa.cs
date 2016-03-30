using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ServiceStack;
using ServiceStack.ServiceHost;
using ServiceStack.OrmLite;
using WebApi.ServiceModel.Tables;

namespace WebApi.ServiceModel.Freight
{
				[Route("/freight/smsa1/count", "Get")]		//count?SalesmanName=
				[Route("/freight/smsa1/sps", "Get")]				//sps?RecordCount= & SalesmanName=
				[Route("/freight/smsa2/create", "Post")]
				[Route("/freight/smsa2/update", "Post")]
				[Route("/freight/smsa2/read", "Get")]			//read?TrxNo= (& LineItemNo= )
				[Route("/freight/smsa2/delete", "Get")]	//delete?TrxNo= & LineItemNo=
				public class Smsa : IReturn<CommonResponse>
    {
								public string SalesmanName { get; set; }
								public string RecordCount { get; set; }
								public string TrxNo { get; set; }
								public string LineItemNo { get; set; }
								public Smsa2 smsa2 { get; set; }
    }
				public class Smsa_Logic
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }
								public int GetCount(Smsa request)
								{
												int Result = -1;
												try
												{
																using (var db = DbConnectionFactory.OpenDbConnection())
																{
																				if (!string.IsNullOrEmpty(request.SalesmanName))
																				{
																								Result = db.Scalar<int>(
																												"Select count(*) From Smsa1 Where (Select Top 1 SalesmanName From Rcsm1 Where SalesmanCode=Smsa1.SalesmanCode) Like '" + request.SalesmanName + "%'"
																								);
																				}
																}
												}
												catch { throw; }
												return Result;
								}
								public List<Smsa1> GetSpsList(Smsa request)
								{
												List<Smsa1> Result = null;
												try
												{
																using (var db = DbConnectionFactory.OpenDbConnection())
																{
																				int count = int.Parse(request.RecordCount);
																				string strWhere = "";
																				if (!string.IsNullOrEmpty(request.SalesmanName))
																				{
																								strWhere = " Where (Select Top 1 SalesmanName From Rcsm1 Where SalesmanCode=Smsa1.SalesmanCode) LIKE '" + request.SalesmanName + "%'";
																				}
																				string strSelect = "SELECT " +
																				"s1.*, (Select Top 1 SalesmanName From Rcsm1 Where SalesmanCode=s1.SalesmanCode) AS SalesmanName" +
																				" FROM Smsa1 s1," +
																				"(SELECT TOP " + (count + 20) + " row_number() OVER (ORDER BY TrxNo ASC) n, TrxNo FROM Smsa1 " + strWhere + ") s2" +
																				" WHERE s1.TrxNo = s2.TrxNo AND s2.n > " + count;
																				string strOrderBy = " ORDER BY s2.n ASC";
																				string strSQL = strSelect + strOrderBy;
																				Result = db.Select<Smsa1>(strSQL);
																}
												}
												catch { throw; }
												return Result;
								}
								public List<Smsa2> Read_Smsa2(Smsa request)
								{
												List<Smsa2> Result = null;
												try
												{
																using (var db = DbConnectionFactory.OpenDbConnection())
																{
																				if (!string.IsNullOrEmpty(request.LineItemNo))
																				{
																								Result = db.Select<Smsa2>("Select * From Smsa2 Where TrxNo=" + int.Parse(request.TrxNo) + " And LineItemNo=" + int.Parse(request.LineItemNo));
																				}
																				else
																				{
																								Result = db.Select<Smsa2>("Select * From Smsa2 Where TrxNo=" + int.Parse(request.TrxNo));
																				}
																}
												}
												catch { throw; }
												return Result;
								}
								public int Insert_Smsa2(Smsa request)
								{
												int Result = -1;
												try
												{
																using (var db = DbConnectionFactory.OpenDbConnection())
																{
																				db.Insert(
																								new Smsa2
																								{
																												TrxNo = request.smsa2.TrxNo,
																												LineItemNo = request.smsa2.LineItemNo,
																												DateTime = DateTime.Now,
																												Action = request.smsa2.Action,
																												Conclusion = request.smsa2.Conclusion,
																												CustomerCode = request.smsa2.CustomerCode,
																												CustomerName = request.smsa2.CustomerName,
																												Description = request.smsa2.Description,
																												Discussion = request.smsa2.Discussion,
																												QuotationNo = request.smsa2.QuotationNo,
																												Reference = request.smsa2.Reference,
																												Remark = request.smsa2.Remark,
																												Status = request.smsa2.Status
																								}
																				);
																				Result = 1;
																}
												}
												catch { throw; }
												return Result;
								}
								public int Update_Smsa2(Smsa request)
								{
												int Result = -1;
												try
												{
																using (var db = DbConnectionFactory.OpenDbConnection())
																{
																				Result = db.Update<Smsa2>(
																								new
																								{
																												Action = request.smsa2.Action,
																												Conclusion = request.smsa2.Conclusion,
																												CustomerCode = request.smsa2.CustomerCode,
																												CustomerName = request.smsa2.CustomerName,
																												//DateTime = request.smsa2.DateTime,
																												Description = request.smsa2.Description,
																												Discussion = request.smsa2.Discussion,
																												QuotationNo = request.smsa2.QuotationNo,
																												Reference = request.smsa2.Reference,
																												Remark = request.smsa2.Remark,
																												Status = request.smsa2.Status
																								},
																								p => p.TrxNo == request.smsa2.TrxNo && p.LineItemNo == request.smsa2.LineItemNo
																				);
																}
												}
												catch { throw; }
												return Result;
								}
    }
}
