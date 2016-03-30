using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack;
using ServiceStack.ServiceHost;
using ServiceStack.OrmLite;
using WebApi.ServiceModel.Tables;

namespace WebApi.ServiceModel.Event
{
    [Route("/event/action/update/done", "Post")]
    public class Update_Done : IReturn<CommonResponse>
    {
        public string JobNo { get; set; }
        public int JobLineItemNo { get; set; }
        public int LineItemNo { get; set; }
        public string DoneFlag { get; set; }
        public DateTime DoneDateTime { get; set; }
        public string Remark { get; set; }
        public string ContainerNo { get; set; }
    }
    public class Update_Done_Logic
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }
        public int UpdateDone(Update_Done request) 
        {
            int Result = -1;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection("TMS"))
                {
                    if (request.DoneDateTime != DateTime.MinValue)
                    {
                        Result = db.Update<Jmjm4>(new { DoneDateTime = request.DoneDateTime, DoneFlag = request.DoneFlag, Remark = request.Remark, ContainerNo = request.ContainerNo }, p => p.JobNo == request.JobNo && p.JobLineItemNo == request.JobLineItemNo && p.LineItemNo == request.LineItemNo);
                    }
                    else
                    {
                        Result = db.Update<Jmjm4>(new { DoneFlag = request.DoneFlag, Remark = request.Remark, ContainerNo = request.ContainerNo }, p => p.JobNo == request.JobNo && p.JobLineItemNo == request.JobLineItemNo && p.LineItemNo == request.LineItemNo);  
                    }
																				if (Result > 0)
																				{
																								InsertContainerNo(request);
																				}
                }
            }
            catch { throw; } 
            return Result;
        }
        public long InsertContainerNo(Update_Done request)
        {
            long Result = -1;
            try
            {
                if (string.IsNullOrEmpty(request.ContainerNo) || request.ContainerNo.Length < 1)
                {
                    return Result;
                }
                using (var db = DbConnectionFactory.OpenDbConnection("TMS"))
                {                    
                    Result = db.Scalar<int>(
                        "Select count(*) From Jmjm6 Where Jmjm6.JobNo={0} And jmjm6.ContainerNo={1}",request.JobNo,request.ContainerNo
                    );
                    if (Result < 1)
                    {
                        int count = db.Scalar<int>(
                            "Select count(*) From Jmjm6 Where Jmjm6.JobNo={0}",request.JobNo
                        );
                        db.InsertParam<Jmjm6>(new Jmjm6 { JobNo = request.JobNo, LineItemNo = count + 1, ContainerNo = request.ContainerNo });
                        Result = 0;
                    }
                    else { Result = -1; }
                }
            }
            catch { throw; }
            return Result;
        }
    }
}
