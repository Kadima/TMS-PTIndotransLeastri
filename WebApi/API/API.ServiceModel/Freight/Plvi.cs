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
				[Route("/freight/plvi1/sps", "Get")] // sps?RecordCount= & StatusCode= & (VoucherNo= || VendorName=)
				[Route("/freight/plvi1/update", "Post")]
				public class Plvi : IReturn<CommonResponse>
				{
								public string VoucherNo { get; set; }
								public string VendorName { get; set; }
								public string StatusCode { get; set; }
								public string RecordCount { get; set; }
								public List<Plvi1> plvi1s { get; set; }
    }
				public class Plvi_Logic
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }
								public List<Plvi1> Get_Plvi1_SpsList(Plvi request)
								{
												List<Plvi1> Result = null;
												try
												{
																using (var db = DbConnectionFactory.OpenDbConnection())
																{
																				int count = int.Parse(request.RecordCount);
																				string strWhere = " Where TrxType=5 And IsNull(VoucherNo,'')<>'' And IsNull(StatusCode,'')='" + request.StatusCode + "'";
																				if (!string.IsNullOrEmpty(request.VoucherNo))
																				{
																								strWhere = strWhere + " And VoucherNo LIKE '" + request.VoucherNo + "%'";
																				}
																				else if (!string.IsNullOrEmpty(request.VendorName))
																				{
																								strWhere = strWhere + " And VendorName LIKE '" + request.VendorName + "%'";
																				}
																				string strSelect = "SELECT " +
																				"p1.* " +
																				"FROM Plvi1 p1," +
																				"(SELECT TOP " + (count + 20) + " row_number() OVER (ORDER BY TrxNo ASC) n, TrxNo FROM Plvi1 " + strWhere + ") p2 " +
																				"WHERE p1.TrxNo = p2.TrxNo AND p2.n > " + count;
																				string strOrderBy = " ORDER BY p2.n ASC";
																				string strSQL = strSelect + strOrderBy;
																				Result = db.Select<Plvi1>(strSQL);
																}
												}
												catch { throw; }
												return Result;
								}
								public int Update_Plvi1(Plvi request)
								{
												int Result = -1;
												try
												{
																using (var db = DbConnectionFactory.OpenDbConnection())
																{
																				foreach (Plvi1 p1 in request.plvi1s)
																				{
																								db.Update<Plvi1>(
																												new
																												{
																																StatusCode = p1.StatusCode
																												},
																												p => p.TrxNo == p1.TrxNo
																								);
																				}
																				Result = 1;
																}
												}
												catch { throw; }
												return Result;
								}
    }
}
