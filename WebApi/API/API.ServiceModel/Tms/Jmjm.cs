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
				[Route("/tms/jmjm1/sps", "Get")]				// sps?RecordCount=
    [Route("/tms/jmjm1", "Get")]
    public class Jmjm : IReturn<CommonResponse>
				{
								public string RecordCount { get; set; }
    }
				public class Jmjm_Logic
    {
								private class JobJCT
								{
												public string JobNo { get; set; }
												public string TrxNo { get; set; }
												public string ContainerCounts { get; set; }
												public string TaskDoneCounts { get; set; }
								}
        public IDbConnectionFactory DbConnectionFactory { get; set; }

								public object Get_JobContainer_SpsList(Jmjm request)
        {
												List<Jmjm1> JmjmList = null;
												List<JobJCT> JobList = new List<JobJCT>();
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection("TMS"))
                {
																				int count = int.Parse(request.RecordCount);
																				string strWhere = " Where JobNo in (Select JobNo From Sibl1 Where TrxNo in (Select TrxNo From Sibl2 Where ISNULL(ContainerNo,'')<>''))";
																				string strSelect = "SELECT " +
																				"j1.* " +
																				"FROM Jmjm1 j1," +
																				"(SELECT TOP " + (count + 20) + " row_number() OVER (ORDER BY JobNo ASC) n, JobNo FROM Jmjm1 " + strWhere + ") j2 " +
																				"WHERE j1.JobNo = j2.JobNo AND j2.n > " + count;
																				string strOrderBy = " ORDER BY j2.n ASC";
																				string strSQL = strSelect + strOrderBy;
																				JmjmList = db.Select<Jmjm1>(strSQL);
																				if (JmjmList.Count > 0)
																				{
																								foreach (Jmjm1 j1 in JmjmList)
																								{
																												JobJCT j = new JobJCT();
																												j.JobNo = j1.JobNo;
																												j.TrxNo = GetTrxNo(j1.JobNo).ToString();
																												j.ContainerCounts = GetCount(j1.JobNo).ToString();
																												j.TaskDoneCounts = GetDoneCount(j1.JobNo).ToString();
																												JobList.Add(j);
																								}
																				}
                }
            }
            catch { throw; }
												return JobList;
        }
								public int GetTrxNo(string strJobNo)
								{
												int Result = -1;
												try
												{
																using (var db = DbConnectionFactory.OpenDbConnection("TMS"))
																{
																				string strSQL = "Select top 1 TrxNo From Sibl1 Where JobNo='" + strJobNo + "'";
																				Result = db.Scalar<int>(strSQL);
																}
												}
												catch { throw; }
												return Result;
								}
        public int GetCount(string strJobNo)
        {
												int Result = -1;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection("TMS"))
                {
																				string strSQL = "SELECT count(*) FROM Sibl2 Where ISNULL(ContainerNo,'')<>'' And TrxNo=(Select top 1 TrxNo From Sibl1 Where JobNo='" + strJobNo + "')";
																				Result = db.Scalar<int>(strSQL);
                }
            }
            catch { throw; }
            return Result;
        }
								public int GetDoneCount(string strJobNo)
								{
												int Result = -1;
												try
												{
																using (var db = DbConnectionFactory.OpenDbConnection("TMS"))
																{
																				string strSQL = "SELECT count(*) FROM Sibl2 Where ISNULL(ContainerNo,'')<>'' And ISNULL(CargoStatusCode,'')='Y' And TrxNo=(Select top 1 TrxNo From Sibl1 Where JobNo='" + strJobNo + "')";
																				Result = db.Scalar<int>(strSQL);
																}
												}
												catch { throw; }
												return Result;
								}
    }
}
