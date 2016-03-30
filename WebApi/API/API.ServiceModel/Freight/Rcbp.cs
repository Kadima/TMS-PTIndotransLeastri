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
				[Route("/freight/rcbp1/sps", "Get")]				// sps?RecordCount= & BusinessPartyName=
				[Route("/freight/rcbp1/read", "Get")]			// read?TrxNo=
				[Route("/freight/rcbp1/update", "Post")]
				[Route("/freight/rcbp3/read", "Get")]			// read?BusinessPartyCode=
				[Route("/freight/rcbp3/create", "Post")]
				[Route("/freight/rcbp3/update", "Post")]
				[Route("/freight/rcbp3/delete", "Get")]	// delete?BusinessPartyCode= & LineItemNo=
    public class Rcbp : IReturn<CommonResponse>
				{
								public string TrxNo { get; set; }
								public string BusinessPartyName { get; set; }
								public string RecordCount { get; set; }
								public string BusinessPartyCode { get; set; }
								public string LineItemNo { get; set; }
								public Rcbp1 rcbp1 { get; set; }
								public Rcbp3 rcbp3 { get; set; }
    }
    public class Rcbp_Logic
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }
								public List<Rcbp1> Get_Rcbp1_List(Rcbp request)
								{
												List<Rcbp1> Result = null;
												try
												{
																using (var db = DbConnectionFactory.OpenDbConnection())
																{
																				if (!string.IsNullOrEmpty(request.BusinessPartyName))
																				{
																								string strSQL = "Select *,(Select Top 1 CountryName From Rccy1 Where CountryCode=Rcbp1.CountryCode) AS CountryName From Rcbp1 Where IsNUll(StatusCode,'')<>'DEL' And BusinessPartyName LIKE '" + request.BusinessPartyName + "%' Order By BusinessPartyCode Asc";
																								Result = db.Select<Rcbp1>(strSQL);
																				}
																				else if (!string.IsNullOrEmpty(request.TrxNo))
																				{
																								string strSQL = "Select *,(Select Top 1 CountryName From Rccy1 Where CountryCode=Rcbp1.CountryCode) AS CountryName From Rcbp1 Where IsNUll(StatusCode,'')<>'DEL' And TrxNo=" + int.Parse(request.TrxNo);
																								Result = db.Select<Rcbp1>(strSQL);
																				}
																				else
																				{
																								string strSQL = "Select Top 20 *,(Select Top 1 CountryName From Rccy1 Where CountryCode=Rcbp1.CountryCode) AS CountryName From Rcbp1 Where IsNUll(StatusCode,'')<>'DEL' Order By BusinessPartyName Asc";
																								Result = db.Select<Rcbp1>(strSQL);
																				}
																}
												}
												catch { throw; }
												return Result;
								}
								public List<Rcbp1> Get_Rcbp1_SpsList(Rcbp request)
								{
												List<Rcbp1> Result = null;
												try
												{
																using (var db = DbConnectionFactory.OpenDbConnection())
																{
																				int count = int.Parse(request.RecordCount);
																				string strWhere = "";
																				if (!string.IsNullOrEmpty(request.BusinessPartyName))
																				{
																								strWhere = " Where BusinessPartyName LIKE '" + request.BusinessPartyName + "%'";
																				}
																				string strSelect = "SELECT " +
																				"r1.*, (Select Top 1 CountryName From Rccy1 Where CountryCode=r1.CountryCode) AS CountryName " +
																				"FROM Rcbp1 r1," +
																				"(SELECT TOP " + (count + 20) + " row_number() OVER (ORDER BY BusinessPartyName ASC) n, TrxNo FROM Rcbp1 " + strWhere + ") r2 " +
																				"WHERE r1.TrxNo = r2.TrxNo AND r2.n > " + count;
																				string strOrderBy = " ORDER BY r2.n ASC";
																				string strSQL = strSelect + strOrderBy;
																				Result = db.Select<Rcbp1>(strSQL);
																}
												}
												catch { throw; }
												return Result;
								}
								public int Update_Rcbp1(Rcbp request)
								{
												int Result = -1;
												try
												{
																using (var db = DbConnectionFactory.OpenDbConnection())
																{
																				Result = db.Update<Rcbp1>(
																								new
																								{
																												BusinessPartyName = request.rcbp1.BusinessPartyName,
																												Address1 = request.rcbp1.Address1,
																												Address2 = request.rcbp1.Address2,
																												Address3 = request.rcbp1.Address3,
																												Address4 = request.rcbp1.Address4,
																												CityCode = request.rcbp1.CityCode,
																												CountryCode = request.rcbp1.CountryCode,
																												Telephone = request.rcbp1.Telephone,
																												Fax = request.rcbp1.Fax,
																												Email = request.rcbp1.Email,
																												WebSite = request.rcbp1.WebSite
																								},
																								p => p.TrxNo == request.rcbp1.TrxNo
																				);
																}
												}
												catch { throw; }
												return Result;
								}
        public List<Rcbp3> Read_Rcbp3_List(Rcbp request)
        {
            List<Rcbp3> Result = null;
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
            return Result;
        }
								public int Insert_Rcbp3(Rcbp request)
								{
												int Result = -1;
												try
												{
																using (var db = DbConnectionFactory.OpenDbConnection())
																{
																				db.Insert(
																								new Rcbp3
																								{
																												BusinessPartyCode = request.rcbp3.BusinessPartyCode,
																												LineItemNo = request.rcbp3.LineItemNo,
																												Birthday = null,
																												ContactName = request.rcbp3.ContactName,
																												Department = request.rcbp3.Department,
																												Dislike = request.rcbp3.Dislike,
																												Email = request.rcbp3.Email,
																												Facebook = request.rcbp3.Facebook,
																												Fax = request.rcbp3.Fax,
																												Handphone = request.rcbp3.Handphone,
																												Like = request.rcbp3.Like,
																												MSN = request.rcbp3.MSN,
																												NameCard = null,
																												Others = request.rcbp3.Others,
																												QQ = request.rcbp3.QQ,
																												Skype = request.rcbp3.Skype,
																												Telephone = request.rcbp3.Telephone,
																												Title = request.rcbp3.Title,
																												Twitter = request.rcbp3.Twitter
																								}
																				);
																				Result = 1;
																}
												}
												catch { throw; }
												return Result;
								}
								public int Update_Rcbp3(Rcbp request)
								{
												int Result = -1;
												try
												{
																using (var db = DbConnectionFactory.OpenDbConnection())
																{
																				Result = db.Update<Rcbp3>(
																								new
																								{
																												//Birthday = request.rcbp3.Birthday,
																												ContactName = request.rcbp3.ContactName,
																												Department = request.rcbp3.Department,
																												Dislike = request.rcbp3.Dislike,
																												Email = request.rcbp3.Email,
																												Facebook = request.rcbp3.Facebook,
																												Fax = request.rcbp3.Fax,
																												Handphone = request.rcbp3.Handphone,
																												Like = request.rcbp3.Like,
																												MSN = request.rcbp3.MSN,
																												//NameCard = request.rcbp3.NameCard,
																												Others = request.rcbp3.Others,
																												QQ = request.rcbp3.QQ,
																												Skype = request.rcbp3.Skype,
																												Telephone = request.rcbp3.Telephone,
																												Title = request.rcbp3.Title,
																												Twitter = request.rcbp3.Twitter
																								},
																								p => p.BusinessPartyCode == request.rcbp3.BusinessPartyCode && p.LineItemNo == request.rcbp3.LineItemNo
																				);
																}
												}
												catch { throw; }
												return Result;
								}
								public int Delete_Rcbp3(Rcbp request)
								{
												int Result = -1;
												try
												{
																using (var db = DbConnectionFactory.OpenDbConnection())
																{
																				db.Delete<Rcbp3>(r3 => r3.BusinessPartyCode == request.BusinessPartyCode && r3.LineItemNo == int.Parse(request.LineItemNo));
																				Result = 1;
																}
												}
												catch { throw; }
												return Result;
								}
    }
}
