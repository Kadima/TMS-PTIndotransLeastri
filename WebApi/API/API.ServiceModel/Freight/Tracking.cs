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
				[Route("/freight/tracking/count", "Get")]
				[Route("/freight/tracking/count/{FilterName}/{FilterValue}", "Get")]
				[Route("/freight/tracking/sps", "Get")]
				[Route("/freight/tracking/sps/{FilterName}/{RecordCount}/{FilterValue}", "Get")]
				[Route("/freight/tracking", "Get")]
				[Route("/freight/tracking/{FilterName}/{FilterValue}", "Get")]
				[Route("/freight/tracking/{FilterName}/{ModuleCode}/{FilterValue}", "Get")]
    public class Tracking : IReturn<CommonResponse>
    {
        public string FilterName { get; set; }
								public string FilterValue { get; set; }
								public string ModuleCode { get; set; }
								public string RecordCount { get; set; }
    }
    public class Tracking_Logic
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }
        public int GetCount(Tracking request)
        {
            int Result = -1;
            try
            {
																using (var db = DbConnectionFactory.OpenDbConnection())
                {
																				string strFilterName = request.FilterName.ToUpper();
                    if (strFilterName.Equals("OrderNo".ToUpper()))
																				{
																								Result = db.Select<Omtx1>(
																												"Select Top 1 TrxNo From Omtx1 Where OrderNo='" + request.FilterValue + "'"
																								).Count;
																				}																				
																				else
																				{
																								string strWhere = "";
																								if (strFilterName.Equals("ContainerNo".ToUpper()))
																								{
																												strWhere = "charindex('" + request.FilterValue + ",',ISNULL(ContainerNo,'')+',')>0";
																								}
																								else if (strFilterName.Equals("BlNo".ToUpper()))
																								{
																												strWhere = "Jmjm1.AwbBlNo='" + request.FilterValue + "' And (Jmjm1.ModuleCode='SE' Or Jmjm1.ModuleCode='SI')";
																								}
																								else if (strFilterName.Equals("AwbNo".ToUpper()))
																								{
																												strWhere = "Jmjm1.AwbBlNo='" + request.FilterValue + "' And (Jmjm1.ModuleCode='AE' Or Jmjm1.ModuleCode='AI')";
																								}
																								else
																								{
																												strWhere = request.FilterName + "='" + request.FilterValue + "'";
																								}
																								Result = db.Select<Jmjm1>(
																												"Select Jmjm1.JobNo,Jmjm1.JobType,Jmjm1.ModuleCode From Jmjm1 Left Join Jmjt1 on Jmjm1.JobType=Jmjt1.JobType Where " + strWhere
																								).Count;
																				}
                }
            }
            catch { throw; }
            return Result;
        }
								public List<Jmjm1> GetSpsList(Tracking request)
								{
												List<Jmjm1> Result = null;
												try
												{
																using (var db = DbConnectionFactory.OpenDbConnection())
																{
																				string strFilterName = request.FilterName.ToUpper();
																				if (!string.IsNullOrEmpty(request.FilterValue))
																				{
																								int count = int.Parse(request.RecordCount);
																								string strWhere = "";
																								string strSelect = "";
																								string strOrderBy = "";
																								string strSQL = "";
																								if (strFilterName.Equals("ContainerNo".ToUpper()))
																								{
																												strWhere = " Where ContainerNo LIKE '%" + request.FilterValue + "%'";
																												strSelect = "SELECT " +
																																"j1.*,(Select Top 1 UomDescription From Rcum1 Where UomCode=j1.UomCode) AS UomDescription " +
																																"FROM Jmjm1 j1, " +
																																"(SELECT TOP " + (count + 10) + " row_number() OVER (ORDER BY JobNo ASC, JobDate DESC) n, JobNo FROM Jmjm1 " + strWhere + ") j2 " +
																																"WHERE j1.JobNo = j2.JobNo AND j2.n > " + count;
																												strOrderBy = " ORDER BY j2.n ASC";
																												strSQL = strSelect + strOrderBy;
																												Result = db.Select<Jmjm1>(strSQL);
																								}
																								else if (strFilterName.Equals("OrderNo".ToUpper()))
																								{

																								}
																								else
																								{
																												if (strFilterName.Equals("BlNo".ToUpper()))
																												{
																																strWhere = " Where Jmjm1.AwbBlNo='" + request.FilterValue + "' And (Jmjm1.ModuleCode='SE' Or Jmjm1.ModuleCode='SI')";
																												}
																												else if (strFilterName.Equals("AwbNo".ToUpper()))
																												{
																																strWhere = " Where Jmjm1.AwbBlNo='" + request.FilterValue + "' And (Jmjm1.ModuleCode='AE' Or Jmjm1.ModuleCode='AI')";
																												}
																												else
																												{
																																strWhere = " Where " + strFilterName + "='" + request.FilterValue + "'";
																												}
																												strSelect = "SELECT " +
																																"j1.*,(Select Top 1 UomDescription From Rcum1 Where UomCode=j1.UomCode) AS UomDescription " +
																																"FROM Jmjm1 j1, " +
																																"(SELECT TOP " + (count + 10) + " row_number() OVER (ORDER BY JobNo ASC, JobDate DESC) n, JobNo FROM Jmjm1 " + strWhere + ") j2 " +
																																"WHERE j1.JobNo = j2.JobNo AND j2.n > " + count;
																												strOrderBy = " ORDER BY j2.n ASC";
																												strSQL = strSelect + strOrderBy;
																												Result = db.Select<Jmjm1>(strSQL);
																								}
																				}
																}
												}
												catch { throw; }
												return Result;
								}
								public List<Tracking_OrderNo> GetOmtx1List(Tracking request)
								{
												List<Tracking_OrderNo> Result = null;
												try
												{
																using (var db = DbConnectionFactory.OpenDbConnection())
																{
																				List<Omtx1> ResultOmtx1 = db.Select<Omtx1>(
																												"Select Top 1 TrxNo From Omtx1 Where OrderNo='" + request.FilterValue + "'"
																								);
																				if (ResultOmtx1.Count > 0)
																				{
																								int TrxNo = ResultOmtx1[0].TrxNo;
																								Result = db.Select<Tracking_OrderNo>(
																												"Select TrxNo," +
																												"(Select PortName From Rcsp1 Where Rcsp1.PortCode=a.PortOfLoadingCode) AS PortOfLoadingName," +
																												"(Select PortName From Rcsp1 Where Rcsp1.PortCode=a.PortOfDischargeCode) AS PortOfDischargeName," +
																												"(Select DeliveryTypeName From Rcdl1 Where Rcdl1.DeliveryType=a.DeliveryType) AS DeliveryTypeName," +
																												"(Select CityName From Rcct1 Where Rcct1.CityCode=a.DestCode) AS DestName," +
																												"(Select CityName From Rcct1 Where Rcct1.CityCode=a.OriginCode) AS OriginName," +
																												"(Select Top 1 BookingNo From Sebk1 Where Sebk1.CustomerRefNo=a.OrderNo and OrderNo!='') AS BookingNo ," +
																												"(Select Top 1 JobNo From  Sebk1 Where Sebk1.CustomerRefNo=a.OrderNo and OrderNo!='') AS JobNo ," +
																												"(Select  AirportName From Rcap1 Where Rcap1.AirportCode =a.airportDeptCode) AS AirportDeptName," +
																												"(Select  AirportName From Rcap1 Where Rcap1.AirportCode =a.airportDestCode) AS AirportDestName," +
																												"CommodityDescription " +
																												"From Omtx1 a " +
																												"Where TrxNo=" + TrxNo
																								);
																				}																				
																}
												}
												catch { throw; }
												return Result;
								}
								public List<Omtx3> GetOmtx3List(Tracking request)
								{
												List<Omtx3> Result = null;
												try
												{
																using (var db = DbConnectionFactory.OpenDbConnection())
																{

																				if (request.FilterName.ToUpper() == "OrderNo".ToUpper())
																				{
																								Result = db.Select<Omtx3>(
																												"Select TrxNo, LineItemNo, Pcs, GrossWeight, Length, Height, Width, Pcs*Length*Width*Height AS Volume " +
																												"From Omtx3 Where TrxNo=" + request.FilterValue
																								);
																				}
																}
												}
												catch { throw; }
												return Result;
								}
								public object GetList(Tracking request)
								{
												object Result = null;
												try
												{
																using (var db = DbConnectionFactory.OpenDbConnection())
																{
																				string strModuleCode = request.ModuleCode.ToUpper();
																				if (strModuleCode.Equals("AE".ToUpper()))
																				{
																								Result = db.Select<Tracking_ContainerNo_AE>(
																												"select c.FirstToDestCode,c.FirstByAirlineID,c.FirstFlightNo,c.FirstFlightDate," +
																												"c.SecondToDestCode,c.SecondByAirlineID,c.SecondFlightNo,c.SecondFlightDate," +
																											"c.ThirdToDestCode,c.ThirdByAirlineID,c.ThirdFlightNo,c.ThirdFlightDate," +
																												"a.ModuleCode,a.JobNo,a.JobType, a.CustomerRefNo as ReferenceNo,a.AwbBlNo,a.MawbOBLNo,a.OriginCode,a.DestCode,a.OriginName,a.DestName," +
																											"a.Pcs,a.GrossWeight,a.Volume,a.CommodityDescription as Commodity, (Select Top 1 UomDescription From Rcum1 Where UomCode=a.UomCode) AS UomDescription " +
																											"From Jmjm1 a Left Join Aeaw1 c on c.AwbNo=a.AwbBlNo " +
																												"Where a.ModuleCode='AE' and a.JobNo='" + request.FilterValue + "'"
																								);
																				}
																				else if (strModuleCode.Equals("AI".ToUpper()))
																				{
																								Result = db.Select<Tracking_ContainerNo_AI>(
																												"Select a.ModuleCode,a.JobNo,a.JobType,a.CustomerRefNo as ReferenceNo,a.AwbBlNo,a.MawbOBLNo,a.OriginCode,a.DestCode,a.OriginName,a.DestName," +
																												"a.Pcs,a.GrossWeight,a.Volume,a.CommodityDescription as Commodity, (Select Top 1 UomDescription From Rcum1 Where UomCode=a.UomCode) AS UomDescription " +
																												"From Jmjm1 a Left Join Aiaw1 c on c.AwbNo=a.AwbBlNo " +
																												"Where a.ModuleCode='AI' and a.JobNo='" + request.FilterValue + "'"
																								);
																				}
																				else if (strModuleCode.Equals("SE".ToUpper()))
																				{
																								Result = db.Select<Tracking_ContainerNo_SE>(
																												"Select c.VesselName, c.VoyageNo, c.FeederVesselName, c.FeederVoyage, a.ModuleCode," +
																												"a.JobNo, a.JobType, a.CustomerRefNo as ReferenceNo, a.AwbBlNo, a.MawbOBLNo, a.OriginCode, a.DestCode," +
																												"a.Pcs, a.GrossWeight, a.Volume, a.CommodityDescription as Commodity, a.ETD, a.ETA," +
																												"a.PortOfLoadingName, a.PortOfDischargeName, a.NoOf20ftContainer, a.NoOf40ftContainer, a.NoOf45ftContainer, a.ContainerNo," +
																												"(SELECT TOP 1 CityCode From Saco1) AS CityCode, c.AtaDate AS ATA, (Select Top 1 UomDescription From Rcum1 Where UomCode=a.UomCode) AS UomDescription " +
																												"From Jmjm1 a Left Join Sebl1 c on c.BlNo=a.AwbBlNo " +
																												"Where a.ModuleCode='SE' and a.JobNo='" + request.FilterValue + "'"
																								);
																				}
																				else if (strModuleCode.Equals("SI".ToUpper()))
																				{
																								Result = db.Select<Tracking_ContainerNo_SI>(
																												"Select c.VesselName,c.VoyageNo,c.FeederVesselName,c.FeederVoyage,a.ModuleCode," +
																												"a.JobNo,a.JobType,a.CustomerRefNo as ReferenceNo,a.AwbBlNo,a.MawbOBLNo,a.OriginCode,a.DestCode," +
																												"a.Pcs,a.GrossWeight,a.Volume,a.CommodityDescription as Commodity,a.ETD,a.ETA," +
																												"a.PortofLoadingName,a.PortofDischargeName,a.Noof20FtContainer,a.Noof40FtContainer,a.Noof45FtContainer,a.ContainerNo," +
																												"(Select Top 1 UomDescription From Rcum1 Where UomCode=a.UomCode) AS UomDescription " +
																												"From Jmjm1 a Left Join Sebl1 c on c.BlNo=a.AwbBlNo " +
																												"Where a.ModuleCode='SI'  and a.JobNo='" + request.FilterValue + "'"
																								);
																				}
																				else if (!string.IsNullOrEmpty(strModuleCode))
																				{
																								Result = db.Select<Jmjm1>(
																												"Select *,(Select Top 1 UomDescription From Rcum1 Where UomCode=Jmjm1.UomCode) AS UomDescription From Jmjm1 Where ModuleCode='" + request.ModuleCode + "' And JobNo='" + request.FilterValue + "' Order By Jmjm1.JobNo Asc,Jmjm1.JobDate Desc"
																								);
																				}
																}
												}
												catch { throw; }
												return Result;
								}
    }
}
