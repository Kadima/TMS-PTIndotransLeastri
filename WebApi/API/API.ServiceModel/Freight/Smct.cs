using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ServiceStack;
using ServiceStack.ServiceHost;
using ServiceStack.OrmLite;
using WebApi.ServiceModel.Tables;
using System.Windows.Forms;

namespace WebApi.ServiceModel.Freight
{
				[Route("/freight/smct1/sps", "Get")]								//sps?RecordCount= &
				[Route("/freight/smct1", "Get")]
				[Route("/freight/smct2", "Get")]												//smct2?TrxNo=
    public class Smct : IReturn<CommonResponse>
				{
								public string RecordCount { get; set; }
								public string TableType { get; set; }
								public string PartyName { get; set; }
								public string PortOfLoadingCode { get; set; }
								public string PortOfDischargeCode { get; set; }
								public string EffectiveDate { get; set; }
								public string ExpiryDate { get; set; }
								public string ModuleCode { get; set; }
								public string JobType { get; set; }
								public string TrxNo { get; set; }
    }
				public class Smct_Logic
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }
								public List<Smct1> Get_Smct1_SpsList(Smct request)
								{
												List<Smct1> Result = null;
												try
												{
																using (var db = DbConnectionFactory.OpenDbConnection())
																{
																				int count = int.Parse(request.RecordCount);
																				string strWhere = "";
																				string strFilter = "";
																				if (!string.IsNullOrEmpty(request.TableType))
																				{
																								if (string.Equals(request.TableType, "S"))
																								{
																												if (strFilter.Length > 0)
																												{
																																strFilter = strFilter + " And ";
																												}
																												strFilter = strFilter + "  ISNULL(TableType,'') = 'S'";
																								}
																								else if (string.Equals(request.TableType, "C") || string.Equals(request.TableType, "SC"))
																								{
																												if (strFilter.Length > 0)
																												{
																																strFilter = strFilter + " And ";
																												}
																												strFilter = strFilter + "  (ISNULL(TableType,'') = 'C' Or ISNULL(TableType,'') = 'SC')";
																								}																								
																				}
																				if (!string.IsNullOrEmpty(request.PartyName))
																				{
																								if (strFilter.Length > 0)
																								{
																												strFilter = strFilter + " And ";
																								}
																								strFilter = strFilter + "  PartyCode in (Select BusinessPartyCode From Rcbp1 Where BusinessPartyName LIKE '" + request.PartyName + "%')";
																				}
																				if (!string.IsNullOrEmpty(request.PortOfLoadingCode))
																				{
																								if (strFilter.Length > 0)
																								{
																												strFilter = strFilter + " And ";
																								}
																								strFilter = strFilter + " PortOfLoadingCode in (Select PortCode From Rcsp1 Where PortName LIKE '" + request.PortOfLoadingCode + "%')";
																				}
																				if (!string.IsNullOrEmpty(request.PortOfDischargeCode))
																				{
																								if (strFilter.Length > 0)
																								{
																												strFilter = strFilter + " And ";
																								}
																								strFilter = strFilter + " PortOfDischargeCode in (Select PortCode From Rcsp1 Where PortName LIKE '" + request.PortOfDischargeCode + "%')";
																				}
																				if (!string.IsNullOrEmpty(request.ModuleCode))
																				{
																								if (strFilter.Length > 0)
																								{
																												strFilter = strFilter + " And ";
																								}
																								strFilter = strFilter + " ModuleCode LIKE '" + request.ModuleCode + "%'";
																				}
																				if (!string.IsNullOrEmpty(request.JobType))
																				{
																								if (strFilter.Length > 0)
																								{
																												strFilter = strFilter + " And ";
																								}
																								strFilter = strFilter + " JobType in (Select JobType From Jmjt1 Where JobDescription LIKE '" + request.JobType + "%'";
																				}
																				if (!string.IsNullOrEmpty(request.EffectiveDate))
																				{
																								if (strFilter.Length > 0)
																								{
																												strFilter = strFilter + " And ";
																								}
																								strFilter = strFilter + " convert(varchar(10),EffectiveDate,120) LIKE '" + request.EffectiveDate + "%'";
																				}
																				if (!string.IsNullOrEmpty(request.ExpiryDate))
																				{
																								if (strFilter.Length > 0)
																								{
																												strFilter = strFilter + " And ";
																								}
																								strFilter = strFilter + " convert(varchar(10),ExpiryDate,120) LIKE '" + request.ExpiryDate + "%'";
																				}
																				if (strFilter.Length > 0)
																				{
																								strWhere = strWhere + " Where " + strFilter;
																				}
																				string strSelect = "SELECT " +
																				"s1.*" +
																				" FROM Smct1 s1," +
																				"(SELECT TOP " + (count + 20) + " row_number() OVER (ORDER BY TrxNo ASC) n, TrxNo FROM Smct1 " + strWhere + ") s2" +
																				" WHERE s1.TrxNo = s2.TrxNo AND s2.n > " + count;
																				string strOrderBy = " ORDER BY s2.n ASC";
																				string strSQL = strSelect + strOrderBy;
																				Result = db.Select<Smct1>(strSQL);
																}
												}
												catch { throw; }
												return Result;
								}
								public List<Smct2> Get_Smct2_List(Smct request)
								{
												List<Smct2> Result = null;
												try
												{
																using (var db = DbConnectionFactory.OpenDbConnection())
																{
																				if (!string.IsNullOrEmpty(request.TrxNo))
																				{
																								string strSelect = "SELECT * FROM Smct2 WHERE TrxNo=" + int.Parse(request.TrxNo);
																								string strOrderBy = " ORDER BY LineItemNo ASC";
																								string strSQL = strSelect + strOrderBy;
																								Result = db.Select<Smct2>(strSQL);
																				}																				
																}
												}
												catch { throw; }
												return Result;
								}
    }
}
