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
				[Route("/freight/rcvy1/sps", "Get")]				// sps?PortOfDischargeName=
				[Route("/freight/rcvy1", "Get")]								// ?PortOfDischargeName=
    public class Rcvy : IReturn<CommonResponse>
    {
        public string PortOfDischargeName { get; set; }
    }
    public class Rcvy_Logic
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }
        public HashSet<string> Get_Rcvy1_List(Rcvy request)
        {
            HashSet<string> Result = null;
            try
            {
																using (var db = DbConnectionFactory.OpenDbConnection())
                {
                    if (!string.IsNullOrEmpty(request.PortOfDischargeName))
                    {
                        Result = db.HashSet<string>(
																												"Select PortOfDischargeName from rcvy1 where PortOfDischargeName is not null and PortOfDischargeName <> '' and PortOfDischargeName LIKE '" + request.PortOfDischargeName + "%' Order By PortOfDischargeName ASC"
                        );
                    }
                    else
                    {
                        Result = db.HashSet<string>(
																												"Select PortOfDischargeName from rcvy1 where PortOfDischargeName is not null and PortOfDischargeName<>'' Order By PortofDischargeName ASC"
                        );
                    }
                }
            }
            catch { throw; }
            return Result;
        }
        public List<Rcvy1_sps> Get_Rcvy1_SpsList(Rcvy request)
        {
            List<Rcvy1_sps> Result = null;
            try
            {
																using (var db = DbConnectionFactory.OpenDbConnection())
                {
																				string strSQL = "SELECT VoyageID,VoyageNo,VesselCode,CloseDateTime,ETD,ETA,datediff(D,ETD,ETA) TranSit,PortofDischargeName," +
																								"(select top 1 ShippinglineName from rcsl1 where shippinglinecode=rcvy1.shippinglinecode)  ShippinglineName " +
																								"FROM rcvy1 Where StatusCode='USE' And ETD >= Convert(varchar(12),getdate(),112) And PortofDischargeName='" + request.PortOfDischargeName + "' Order By UpdateDateTime Desc";
																				Result = db.Select<Rcvy1_sps>(strSQL);
                }
            }
            catch { throw; }
            return Result;
        }
    }
}
