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
				[Route("/freight/saus1/memo/{UserID}", "Get")]
				[Route("/freight/saus1/{UserID}", "Get")]
				[Route("/freight/saus1", "Get")]
				[Route("/freight/saus1/update/memo", "Post")]
    public class Saus : IReturn<CommonResponse>
    {
								public string UserID { get; set; }
								public Saus1 saus1 { get; set; }
    }
				public class Saus_Logic
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }
								public string GetMemo(Saus request)
								{
												string Result = null;
												try
												{
																using (var db = DbConnectionFactory.OpenDbConnection())
																{
																				RichTextBox rtb = new RichTextBox();
																				List<Saus1> ls = db.Select<Saus1>("Select Memo From Saus1 Where UserID='" + request.UserID + "'");
																				if (ls.Count > 0)
																				{
																								rtb.Rtf = ls[0].Memo;
																				}
																				Result = rtb.Text;
																}
												}
												catch { throw; }
												return Result;
								}
								public int Update_Memo(Saus request)
								{
												int Result = -1;
												try
												{
																using (var db = DbConnectionFactory.OpenDbConnection())
																{
																				RichTextBox rtb = new RichTextBox();
																				List<Saus1> ls = db.Select<Saus1>("Select IsNull(Memo,'') From Saus1 Where UserID='" + request.saus1.UserId + "'");
																				if (ls.Count > 0)
																				{
																								rtb.Text = request.saus1.Memo;
																				}
																				Result = db.Update<Saus1>(
																								new
																								{
																												Memo = rtb.Rtf
																								},
																								p => p.UserId == request.saus1.UserId
																				);
																}
												}
												catch { throw; }
												return Result;
								}
    }
}
