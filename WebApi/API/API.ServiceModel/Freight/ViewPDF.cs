using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ServiceStack;
using ServiceStack.Common.Web;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;
using WebApi.ServiceModel.Tables;

namespace WebApi.ServiceModel.Freight
{
				[Route("/freight/view/pdf", "Get")] // pdf?FolderName=
				[Route("/freight/view/pdf/file", "Get")] // file?FolderName= & Key= & FileName=
				public class ViewPDF : IReturn<CommonResponse>
				{
								public string FolderName { get; set; }
								public string Key { get; set; }
								public string FileName { get; set; }
				}
				public class ViewPDF_Logic
				{
								public IDbConnectionFactory DbConnectionFactory { get; set; }
								public struct TrxNoPDFName
								{
												public string Key;
												public string FileName;
								}
								private List<TrxNoPDFName> tnPDF = null;
								private void SortAsFileCreationTime(ref FileInfo[] arrFi)
								{
												Array.Sort<FileInfo>(arrFi, delegate(FileInfo x, FileInfo y) { return y.CreationTime.CompareTo(x.CreationTime); });
								}
								public void GetAllDirList(string strPath)
								{
												try
												{																
																if (Directory.Exists(strPath))
																{
																				DirectoryInfo di = new DirectoryInfo(strPath);
																				DirectoryInfo[] diA = di.GetDirectories();
																				for (int i = 0; i <= diA.Length - 1; i++)
																				{
																								TrxNoPDFName tnn = new TrxNoPDFName();
																								tnn.Key = diA[i].Name;
																								FileInfo[] arrFi = diA[i].GetFiles();
																								if (arrFi.Length > 0)
																								{
																												SortAsFileCreationTime(ref arrFi);
																												tnn.FileName = arrFi[0].Name;
																								}
																								else
																								{
																												tnn.FileName = "";
																								}
																								tnPDF.Add(tnn);
																								GetAllDirList(diA[i].FullName);
																				}
																}																		
												}
												catch { throw; }
								}
								public object Get_List(ViewPDF request)
								{
												object Result = null;
												tnPDF = new List<TrxNoPDFName>();
												string strPath = "";
												string eDocumentPath = "";
												try
												{
																using (var db = DbConnectionFactory.OpenDbConnection())
																{
																				string strSQL = "Select Top 1 eDocumentPath From Saco1";
																				List<Saco1> saco1 = db.Select<Saco1>(strSQL);
																				if (saco1.Count > 0)
																				{
																								eDocumentPath = saco1[0].eDocumentPath;
																				}
																}
																if (!string.IsNullOrEmpty(request.FolderName))
																{
																				strPath = eDocumentPath + "\\" + request.FolderName + "\\eDoc";
																				GetAllDirList(strPath);
																}
																if (tnPDF.Count > 0)
																{
																				string strKeys = "";
																				for (int i = 0; i <= tnPDF.Count - 1; i++)
																				{
																								strKeys = strKeys + "'" + tnPDF[i].Key + "',";
																				}
																				if (strKeys.LastIndexOf(",").Equals(strKeys.Length-1)){
																								strKeys = strKeys.Substring(0,strKeys.Length-1);
																				}
																				using (var db = DbConnectionFactory.OpenDbConnection())
																				{
																								string strSQL = "";
																								switch(request.FolderName.ToLower())
																								{
																												case "ivcr1":
																																strSQL = "Select TrxNo,InvoiceNo,InvoiceDate,CustomerName,InvoiceAmt From Ivcr1 Where TrxNo in (" + strKeys + ")";
																																List<ViewPDF_Ivcr> rIvcr = db.Select<ViewPDF_Ivcr>(strSQL);
																																foreach (ViewPDF_Ivcr vi in rIvcr)
																																{
																																				for (int i = 0; i <= tnPDF.Count - 1; i++)
																																				{
																																								if (tnPDF[i].Key.Equals(vi.TrxNo.ToString()))
																																								{
																																												vi.FileName = tnPDF[i].FileName;
																																												break;
																																								}
																																				}
																																}
																																Result = rIvcr;
																																break;
																												case "jmjm1":
																																strSQL = "Select JobNo,JobDate,CustomerName,InvoiceLocalAmt From Jmjm1 Where JobNo in (" + strKeys + ")";
																																List<ViewPDF_Jmjm> rJmjm = db.Select<ViewPDF_Jmjm>(strSQL);
																																foreach (ViewPDF_Jmjm vi in rJmjm)
																																{
																																				for (int i = 0; i <= tnPDF.Count - 1; i++)
																																				{
																																								if (tnPDF[i].Key.Equals(vi.JobNo.ToString()))
																																								{
																																												vi.FileName = tnPDF[i].FileName;
																																												break;
																																								}
																																				}
																																}
																																Result = rJmjm;
																																break;
																												case "slcu1":
																																strSQL = "Select TrxNo,InvoiceNo,InvoiceDate,CustomerName,InvoiceAmt From Ivcr1 Where TrxNo in (" + strKeys + ")";
																																List<ViewPDF_Jmjm> rSlcu = db.Select<ViewPDF_Jmjm>(strSQL);
																																foreach (ViewPDF_Jmjm vi in rSlcu)
																																{
																																				for (int i = 0; i <= tnPDF.Count - 1; i++)
																																				{
																																								if (tnPDF[i].Key.Equals(vi.JobNo.ToString()))
																																								{
																																												vi.FileName = tnPDF[i].FileName;
																																												break;
																																								}
																																				}
																																}
																																Result = rSlcu;
																																break;
																								}
																				}																				
																}																
												}
												catch { throw; }
												return Result;
								}
								public byte[] Get_File(ViewPDF request)
								{
												byte[] Result = null;
												string strPath = "";
												string eDocumentPath = "";
												try
												{
																using (var db = DbConnectionFactory.OpenDbConnection())
																{
																				string strSQL = "Select Top 1 eDocumentPath From Saco1";
																				List<Saco1> saco1 = db.Select<Saco1>(strSQL);
																				eDocumentPath = saco1[0].eDocumentPath;
																				strPath = eDocumentPath + "\\" + request.FolderName + "\\eDoc\\" + request.Key + "\\" + request.FileName;
																				using (FileStream fsRead = new FileStream(strPath, FileMode.Open))
																				{
																								int fsLen = (int)fsRead.Length;
																								byte[] heByte = new byte[fsLen];
																								int r = fsRead.Read(heByte, 0, heByte.Length);
																								Result = heByte;
																				}
																}
												}
												catch { throw; }
												return Result;
								}
				}
}
