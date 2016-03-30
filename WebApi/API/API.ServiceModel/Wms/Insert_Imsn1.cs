using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ServiceStack;
using ServiceStack.ServiceHost;
using ServiceStack.OrmLite;
using WebApi.ServiceModel.Tables;

namespace WebApi.ServiceModel.Wms
{
    [Route("/wms/action/insert/imsn1", "Post")]
    public class Insert_Imsn1 : IReturn<CommonResponse>
    {
        public string IssueNoteNo { get; set; }
        public string IssueLineItemNo { get; set; }
        public string ReceiptNoteNo { get; set; }
        public string ReceiptLineItemNo { get; set; }
        public string SerialNo { get; set; }
    }
    public class Insert_Imsn1_Logic
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }
        public long UpdateImsn1(Insert_Imsn1 request)
        {
            long Result = -1;
            int intResult = -1;
            try
            {
																using (var db = DbConnectionFactory.OpenDbConnection("WMS"))
                {
                    if (request.IssueNoteNo.Length > 0)
                    {
                        intResult = db.Scalar<int>(
                            "Select count(*) From Imsn1 Where IssueNoteNo={0} And IssueLineItemNo={1} And SerialNo={2}",
                            request.IssueNoteNo,request.IssueLineItemNo,request.SerialNo
                        );
                        if (intResult < 1)
                        {
                            db.Insert(new Imsn1 { IssueNoteNo = request.IssueNoteNo, IssueLineItemNo = request.IssueLineItemNo, SerialNo = request.SerialNo });
                            Result = 1;
                        }
                    }
                    else
                    {
                        intResult = db.Scalar<int>(
                            "Select count(*) From Imsn1 Where ReceiptNoteNo={0} And ReceiptLineItemNo={1} And SerialNo={2}",
                            request.ReceiptNoteNo,request.ReceiptLineItemNo,request.SerialNo
                        );
                        if (intResult < 1)
                        {
                            db.Insert(new Imsn1 { ReceiptNoteNo = request.ReceiptNoteNo, ReceiptLineItemNo = request.ReceiptLineItemNo, SerialNo = request.SerialNo });
                            Result = 1;
                        }
                    }
                    
                }
            }
            catch { throw; }
            return Result;
        }
    }
}
