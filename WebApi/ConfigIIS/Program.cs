using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Collections;

namespace ConfigIIS
{
    class Program
    {
        static void Main(string[] args)
								{
												Console.Title = "WebAPI Configuration";
												string msg = "";
												bool blnWait = true;
												int WaitTime = 0;
            try
            {
																//PrintLatticeChar("IIS");
																//for (int i = 0; i < 15; i++)
																//{
																//				Console.WriteLine();
																//}
																msg = "==============================================";
																ConsoleColorWrite(msg, ConsoleColor.Green);
																msg = "==                                          ==";
																ConsoleColorWrite(msg, ConsoleColor.Green);
																msg = "== Freight Mobile APP Server Side Installer ==";
																ConsoleColorWrite(msg, ConsoleColor.Green);
																msg = "==                                          ==";
																ConsoleColorWrite(msg, ConsoleColor.Green);
																msg = "==        Power By (C)2015 SysMagic.        ==";
																ConsoleColorWrite(msg, ConsoleColor.Green);
																msg = "==                                          ==";
																ConsoleColorWrite(msg, ConsoleColor.Green);
																msg = "==============================================";
																ConsoleColorWrite(msg, ConsoleColor.Green);
																msg = "!! Note: Press Enter To Use Default Setting !!";
																ConsoleColorWrite(msg, ConsoleColor.Yellow);
																msg = "Environment Ready,Press Any Key To Continue...";
																ConsoleColorWrite(msg, ConsoleColor.Green);
																Console.ReadKey();
                string folderPath = "C:\\inetpub\\wwwroot\\WebApi";
																while (blnWait)
																{
																				msg = "Enter Application Folder Path.\nDefault is 'C:\\inetpub\\wwwroot\\WebApi'";
																				ConsoleColorWrite(msg, ConsoleColor.Cyan);
																				folderPath = ReadLineString(folderPath);
																				if (Directory.Exists(folderPath))
																				{
																								blnWait = false;
																								if (!FolderSecurityHelper.ExistFolderRights(folderPath))
																								{
																												FolderSecurityHelper.SetFolderRights(folderPath);
																								}
																				}
																				else
																				{
																								WaitTime++;
																								if (WaitTime > 3)
																								{
																												msg = "Failed To Many Times, Press Any Key To Close.";
																												ConsoleColorWrite(msg, ConsoleColor.Red);
																												Console.ReadLine();
																												return;
																								}
																								else
																								{
																												msg = "Application Folder Path '" + folderPath + "' Not Exist.";
																												ConsoleColorWrite(msg, ConsoleColor.Red);
																								}
																				}
																}																
																string applicationPoolName = "WebApiService";
																blnWait = true;
																WaitTime = 0;
																while (blnWait)
																{
																				msg = "Enter Application Pool Name.\nDefault is 'WebApiService'";
																				ConsoleColorWrite(msg, ConsoleColor.Cyan);
																				applicationPoolName = ReadLineString(applicationPoolName);
																				if (!IISControlHelper.ExistApplicationPool(applicationPoolName))
																				{
																								blnWait = false;
																								IISControlHelper.CreateApplicationPool(applicationPoolName);
																				}
																				else
																				{
																								WaitTime++;
																								if (WaitTime > 3)
																								{
																												msg = "Failed To Many Times, Press Any Key To Close.";
																												ConsoleColorWrite(msg, ConsoleColor.Red);
																												Console.ReadLine();
																												return;
																								}
																								else
																								{
																												msg = "Application Pool Name '" + applicationPoolName + "' Already Exist.";
																												ConsoleColorWrite(msg, ConsoleColor.Red);
																												msg = "Enter 'Y' To Override Or 'S' To Skip This Step.";
																												ConsoleColorWrite(msg, ConsoleColor.Cyan);
																												string s = Console.ReadLine();
																												if (s.ToUpper() == 'Y'.ToString())
																												{
																																blnWait = false;
																																IISControlHelper.DeleteApplicationPool(applicationPoolName);
																																IISControlHelper.CreateApplicationPool(applicationPoolName);
																												}
																												else if (s.ToUpper() == 'S'.ToString())
																												{
																																blnWait = false;
																												}
																								}
																				}
																}																
																string applicationPath = "WebApi";
																blnWait = true;
																WaitTime = 0;
																int siteIndex = 0;
																while (blnWait)
																{
																				msg = "Enter Application Name.\nDefault is 'WebApi'";
																				ConsoleColorWrite(msg, ConsoleColor.Cyan);
																				applicationPath = "/" + ReadLineString(applicationPath);
																				ArrayList al = IISControlHelper.ListSites();
																				if (al.Count > 1)
																				{
																								msg = "Detect More then One WebSite, Enter The Number To Chose One.";
																								ConsoleColorWrite(msg, ConsoleColor.Cyan);
																								for (int i = 0; i < al.Count; i++)
																								{
																												msg = "( " + (i+1).ToString() + " )  " + al[i].ToString();
																												ConsoleColorWrite(msg, ConsoleColor.White);
																								}
																								string s = Console.ReadLine();
																								siteIndex = int.Parse(s) - 1;

																				}
																				if (!IISControlHelper.ExistApplication(applicationPath, siteIndex))
																				{
																								blnWait = false;
																								IISControlHelper.CreateApplication(applicationPath, siteIndex, folderPath, applicationPoolName);
																				}
																				else
																				{
																								WaitTime++;
																								if (WaitTime > 3)
																								{
																												msg = "Failed To Many Times, Press Any Key To Close.";
																												ConsoleColorWrite(msg, ConsoleColor.Red);
																												Console.ReadLine();
																												return;
																								}
																								else
																								{
																												msg = "Application Name '" + applicationPath + "' Already Exist.";
																												ConsoleColorWrite(msg, ConsoleColor.Red);
																												msg = "Enter 'Y' To Override Or 'S' To Skip This Step.";
																												ConsoleColorWrite(msg, ConsoleColor.Cyan);
																												string s = Console.ReadLine();
																												if (s.ToUpper() == 'Y'.ToString())
																												{
																																blnWait = false;
																																IISControlHelper.DeleteApplication(applicationPath, siteIndex);
																																IISControlHelper.CreateApplication(applicationPath, siteIndex, folderPath, applicationPoolName);
																												}
																												else if (s.ToUpper() == 'S'.ToString())
																												{
																																blnWait = false;
																												}
																								}
																				}
																}
																msg = "Install Success! Press Any Key To Continue...";
																ConsoleColorWrite(msg, ConsoleColor.Green);
																Console.ReadLine();
																return;
            }
            catch (Exception ex) {
																ConsoleColorWrite(ex.Message, ConsoleColor.Red);
                Console.ReadLine();
            }
        }

								static int[,] GetLatticeArray(string s)
								{
												FontStyle style = FontStyle.Regular;
												string familyName = "Verdana";
												float emSize = 14;
												Font f = new Font(familyName, emSize, style);

												int width = 16;
												int height = 16;
												Bitmap bm = new Bitmap(width, height);

											 Graphics g =	Graphics.FromImage(bm);

												Brush b = new SolidBrush(Color.Black);
												PointF pf = new PointF(-3,-4);

												g.DrawString(s, f, b, pf);
												g.Flush();
												g.Dispose();

												//bm.Save("test.bmp");

												int[,] a = new int[16,16];
												for (int i = 0; i < bm.Width; i++)
												{
																for (int j = 0; j < bm.Height; j++)
																{
																				Color c = bm.GetPixel(j, i);
																				if (c.Name == "0")
																				{
																								a[j,i] = 0;
																				}
																				else
																				{
																								a[j,i] = 1;
																				}
																}
												}
												bm.Dispose();
												return a;
								}

								static void PrintLatticeChar(string s)
								{
												int x = Console.CursorLeft;
												int y = Console.CursorTop;
												foreach (char c in s.ToCharArray())
												{
																int[,] a = GetLatticeArray(c.ToString());
																int charWidth = a.GetLength(0);
																for (int i = 0; i < charWidth; i++)
																{
																				for (int j = 0; j < charWidth; j++)
																				{
																								if (a[j, i] == 1)
																								{
																												Console.Write("#");
																								}
																								else
																								{
																												Console.Write(" ");
																								}
																				}
																				x = Console.CursorLeft - charWidth;
																				if (x <= 0)
																				{
																								x = 1;
																				}
																				Console.CursorLeft = x;
																				Console.CursorTop++;
																}
																if((Console.CursorLeft + charWidth) > Console.WindowWidth){
																				Console.CursorTop += charWidth;
																}else{
																				Console.CursorLeft += charWidth;
																				Console.CursorTop -= charWidth;
																}
												}
								}

								static void ConsoleColorWrite(string msg, ConsoleColor cc)
								{
												Console.BackgroundColor = ConsoleColor.Black;
												Console.ForegroundColor = cc;
												Console.WriteLine(EncodingString(msg, Encoding.UTF8));
												Console.ResetColor();
								}

								static string EncodingString(string msg, Encoding type)
								{
												byte[] srcBytes = Encoding.Default.GetBytes(msg);
												byte[] bytes = Encoding.Convert(Encoding.Default, type, srcBytes);
												return type.GetString(bytes);
								}

								static string ReadLineString(string str)
								{
												string strNew = Console.ReadLine();
												if (strNew.Length > 0)
												{
																str = strNew;
												}
												return str;
								}
    }
}
