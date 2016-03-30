using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ServiceStack;
using ServiceStack.ServiceHost;
using ServiceStack.OrmLite;
using WebApi.ServiceModel.Tables;

namespace WebApi.ServiceModel.Event
{
    [Route("/event/action/list/jobno/{PhoneNumber}", "Get")]
				[Route("/event/action/list/jobno", "Get")] //jobno?PhoneNumber= 
    public class List_JobNo : IReturn<CommonResponse>
    {
        public string PhoneNumber { get; set; }
    }
    public class List_JobNo_Logic
    {
								private class JobJCT
								{
												public string JobNo { get; set; }
												public string ContainerCounts { get; set; }
												public string TaskDoneCounts { get; set; }
								}
        public IDbConnectionFactory DbConnectionFactory { get; set; }
        public object GetList(List_JobNo request)
        {
												List<JobJCT> JobList = new List<JobJCT>();
												HashSet<string> hsResult = null;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection("TMS"))
                {
																				hsResult = db.HashSet<string>(
																								"Select Distinct Jmjm4.JobNo From Jmjm4 Left Join Jmjm3 On Jmjm3.JobNo=Jmjm4.JobNo Where Jmjm4.PhoneNumber='" + request.PhoneNumber + "' And DATEDIFF(day, Jmjm3.StartDateTime, getdate())<=0"
                    );
																				if (hsResult.Count > 0)
																				{
																								foreach (string strJobNo in hsResult)
																								{
																												JobJCT j = new JobJCT();
																												j.JobNo = strJobNo;
																												j.ContainerCounts = GetCount(request.PhoneNumber, strJobNo).ToString();
																												j.TaskDoneCounts = GetDoneCount(request.PhoneNumber, strJobNo).ToString();
																												JobList.Add(j);
																								}
																				}
                }
            }
            catch { throw; }
												return JobList;
        }
        public long GetCount(string strPhoneNumber, string strJobNo)
        {
            long Result = -1;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection("TMS"))
                {
                    Result = db.Count<Jmjm4>(j4 => j4.PhoneNumber == strPhoneNumber && j4.JobNo == strJobNo && j4.DoneFlag != null);
                }
            }
            catch { throw; }
            return Result;
        }
								public long GetDoneCount(string strPhoneNumber, string strJobNo)
								{
												long Result = -1;
												try
												{
																using (var db = DbConnectionFactory.OpenDbConnection("TMS"))
																{
																				Result = db.Count<Jmjm4>(j4 => j4.PhoneNumber == strPhoneNumber && j4.JobNo == strJobNo && j4.DoneFlag == "Y");
																}
												}
												catch { throw; }
												return Result;
								}
    }
}
