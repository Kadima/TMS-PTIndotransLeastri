using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ServiceStack;
using ServiceStack.ServiceHost;
using ServiceStack.OrmLite;

namespace WebApi.ServiceModel.Event
{
				[Route("/event/action/list/container/{PhoneNumber}/{JobNo}", "Get")]
				[Route("/event/action/list/container", "Get")] //container?PhoneNumber= & JobNo=
    public class List_Container : IReturn<CommonResponse>
    {
        public string PhoneNumber { get; set; }
        public string JobNo { get; set; }
    }
    public class List_Container_Response
    {
        public string JobNo { get; set; }
        public int JobLineItemNo { get; set; }
        public int LineItemNo { get; set; }
        public string ContainerNo { get; set; }
        public string Description { get; set; }
        public string Remark { get; set; }
        public string ItemName { get; set; }
        public string AllowSkipFlag { get; set; }
								public string DoneFlag { get; set; }
    }
    public class List_Container_Logic
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }
        public List<List_Container_Response> GetList(List_Container request)
        {
            List<List_Container_Response> Result = null;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection("TMS"))
                {
                    Result = db.Select<List_Container_Response>(
																								"Select Jmjm4.JobNo, Jmjm4.JobLineItemNo, Jmjm4.LineItemNo," +
																								"IsNull(Jmjm4.ContainerNo,'') AS ContainerNo, IsNull(Jmjm3.Description,'') AS Description," +
																								"IsNull(Jmjm4.Remark,'') AS Remark, IsNull(Jmjm4.ItemName,'') AS ItemName," +
																								"IsNull(Jmje1.AllowSkipFlag,'') AS AllowSkipFlag, IsNull(Jmjm4.DoneFlag,'') AS DoneFlag " +
                        "From Jmjm4 Left Join Jmjm3 On Jmjm4.JobNo=Jmjm3.JobNo And Jmjm4.JobLineItemNo=Jmjm3.LineItemNo " +
                        "Left Join Jmje1 On Jmjm3.EventCode=Jmje1.EventCode " +
																								"Where Jmjm4.PhoneNumber='" + request.PhoneNumber + "' And Jmjm4.JobNo='" + request.JobNo + "'"
                    );
                }
            }
            catch { throw; }
            return Result;
        }
    }
}
