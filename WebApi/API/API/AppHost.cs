using Funq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Reflection;
using ServiceStack;
using ServiceStack.OrmLite;
using ServiceStack.Common.Web;
using ServiceStack.Api.Swagger;
using ServiceStack.ServiceHost;
using ServiceStack.MiniProfiler;
using ServiceStack.Configuration;
using ServiceStack.MiniProfiler.Data;
using ServiceStack.WebHost.Endpoints;
using ServiceStack.ServiceInterface.Cors;
using WebApi.ServiceInterface;

namespace WebApi
{
    public class AppHost : AppHostBase
    {
        private static string ver = Assembly.GetExecutingAssembly().GetName().Version.ToString();
								private static string strSecretKey;
        public AppHost()
            : base("Web Api v" + ver, typeof(ApiServices).Assembly)
        {
        }
        public override void Configure(Container container)
        {
            //ServiceStack.Text.JsConfig.EmitCamelCaseNames = true; ! DO NOT USE THIS !
            //Feature disableFeatures = Feature.Xml | Feature.Jsv | Feature.Csv | Feature.Soap11 | Feature.Soap12 | Feature.Soap;
            SetConfig(new EndpointHostConfig
            {
                DebugMode = false,
                UseCustomMetadataTemplates = true,
                //DefaultContentType = ContentType.Json,
																//GlobalResponseHeaders = {
                //    { "Access-Control-Allow-Origin", "*" },
                //    { "Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS" },
                //    { "Access-Control-Allow-Headers", "Content-Type, Signature" }
                //},
                EnableFeatures = Feature.Json | Feature.Metadata
                //ServiceStackHandlerFactoryPath  = "api"                
            });
            CorsFeature cf = new CorsFeature(allowedOrigins: "*", allowedMethods: "GET, POST, PUT, DELETE, OPTIONS", allowedHeaders: "Content-Type, Signature", allowCredentials: false);
            this.Plugins.Add(cf);
            this.Plugins.Add(new SwaggerFeature());
            //DB
												var dbConnectionFactory = new OrmLiteConnectionFactory(GetConnectionString("Freight"), SqlServerDialect.Provider)
            {
                ConnectionFilter =
                    x =>
                    new ProfiledDbConnection(x, Profiler.Current)
            };
												dbConnectionFactory.RegisterConnection("TMS", GetConnectionString("TMS"), SqlServerDialect.Provider);
												dbConnectionFactory.RegisterConnection("WMS", GetConnectionString("WMS"), SqlServerDialect.Provider);
            container.Register<IDbConnectionFactory>(dbConnectionFactory);
												//
            var secretKey = new WebApi.ServiceModel.SecretKeyFactory(strSecretKey);
            container.Register<WebApi.ServiceModel.ISecretKey>(secretKey);
            //Auth
            container.RegisterAutoWired<WebApi.ServiceModel.Auth>();
            //WMS
												container.RegisterAutoWired<WebApi.ServiceModel.Wms.Wms_Login_Logic>();
												container.RegisterAutoWired<WebApi.ServiceModel.Wms.List_Rcbp1_Logic>();
            container.RegisterAutoWired<WebApi.ServiceModel.Wms.List_Imgr1_Logic>();
            container.RegisterAutoWired<WebApi.ServiceModel.Wms.List_Impr1_Logic>();
            container.RegisterAutoWired<WebApi.ServiceModel.Wms.List_Imgr2_Logic>();
            container.RegisterAutoWired<WebApi.ServiceModel.Wms.List_Imgi1_Logic>();
            container.RegisterAutoWired<WebApi.ServiceModel.Wms.List_Imgi2_Logic>();
            container.RegisterAutoWired<WebApi.ServiceModel.Wms.List_Imsn1_Logic>();
            container.RegisterAutoWired<WebApi.ServiceModel.Wms.Confirm_Imgr1_Logic>();
												//TMS
												container.RegisterAutoWired<WebApi.ServiceModel.Tms.Jmjm_Logic>();
												container.RegisterAutoWired<WebApi.ServiceModel.Tms.Sibl_Logic>();
            //Event
            container.RegisterAutoWired<WebApi.ServiceModel.Event.Event_Login_Logic>();
            container.RegisterAutoWired<WebApi.ServiceModel.Event.List_Jmjm6_Logic>();
            container.RegisterAutoWired<WebApi.ServiceModel.Event.List_JobNo_Logic>();
            container.RegisterAutoWired<WebApi.ServiceModel.Event.Update_Done_Logic>();
            container.RegisterAutoWired<WebApi.ServiceModel.Event.List_Container_Logic>();
            //Freight
												container.RegisterAutoWired<WebApi.ServiceModel.Freight.Freight_Login_Logic>();
												container.RegisterAutoWired<WebApi.ServiceModel.Freight.Saus_Logic>();
												container.RegisterAutoWired<WebApi.ServiceModel.Freight.Rcbp_Logic>();
												container.RegisterAutoWired<WebApi.ServiceModel.Freight.Smsa_Logic>();
												container.RegisterAutoWired<WebApi.ServiceModel.Freight.Smct_Logic>();
												container.RegisterAutoWired<WebApi.ServiceModel.Freight.Plvi_Logic>();
            container.RegisterAutoWired<WebApi.ServiceModel.Freight.Rcvy_Logic>();
												container.RegisterAutoWired<WebApi.ServiceModel.Freight.Tracking_Logic>();
												container.RegisterAutoWired<WebApi.ServiceModel.Freight.ViewPDF_Logic>();
        }
        #region DES
        //private string DESKey = "F322186F";
        //private string DESIV = "F322186F";
        private static string DESEncrypt(string strPlain, string strDESKey, string strDESIV)
        {
            string DESEncrypt = "";
            try
            {
                byte[] bytesDESKey = ASCIIEncoding.ASCII.GetBytes(strDESKey);
                byte[] bytesDESIV = ASCIIEncoding.ASCII.GetBytes(strDESIV);
                byte[] inputByteArray = Encoding.Default.GetBytes(strPlain);
                DESCryptoServiceProvider desEncrypt = new DESCryptoServiceProvider();
                desEncrypt.Key = bytesDESKey;
                desEncrypt.IV = bytesDESIV;
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, desEncrypt.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(inputByteArray, 0, inputByteArray.Length);
                        csEncrypt.FlushFinalBlock();
                        StringBuilder str = new StringBuilder();
                        foreach (byte b in msEncrypt.ToArray())
                        {
                            str.AppendFormat("{0:X2}", b);
                        }
                        DESEncrypt = str.ToString();
                    }
                }
            }
            catch
            { }
            return DESEncrypt;
        }
        private static string DesDecrypt(string strValue)
        {
            string DesDecrypt = "";
            if (string.IsNullOrEmpty(strValue))
            {
                return DesDecrypt;
            }
            try
            {
                byte[] DESKey = new byte[] { 70, 51, 50, 50, 49, 56, 54, 70 };
                byte[] DESIV = new byte[] { 70, 51, 50, 50, 49, 56, 54, 70 };
                DES desprovider = new DESCryptoServiceProvider();
                byte[] inputByteArray = new byte[strValue.Length / 2];
                int intI;
                for (intI = 0; intI < strValue.Length / 2; intI++)
                {
                    inputByteArray[intI] = (byte)(Convert.ToInt32(strValue.Substring(intI * 2, 2), 16));
                }
                desprovider.Key = DESKey;
                desprovider.IV = DESIV;
                using (MemoryStream ms = new MemoryStream())
                {
                    CryptoStream cs = new CryptoStream(ms, desprovider.CreateDecryptor(), CryptoStreamMode.Write);
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    DesDecrypt = Encoding.Default.GetString(ms.ToArray());
                }
            }
            catch
            { throw; }
            return DesDecrypt;
        }
        #endregion
        private static string GetConnectionString(string type)
        {
            string IniConnection = "";
            string strAppSetting = "";
            string[] strDataBase = new string[3];
            if (string.IsNullOrEmpty(strAppSetting))
            {
																if (string.Equals(type, "TMS"))
																{
																				strAppSetting = System.Configuration.ConfigurationManager.AppSettings["TMS_DB"];
																}else if(string.Equals(type,"WMS"))
																{
																				strAppSetting = System.Configuration.ConfigurationManager.AppSettings["WMS_DB"];
																}
																else if (string.Equals(type, "Freight"))
																{
																				strAppSetting = System.Configuration.ConfigurationManager.AppSettings["Mobile_DB"];
																}
																strSecretKey = System.Configuration.ConfigurationManager.AppSettings["SecretKey"];
                strDataBase = strAppSetting.Split(',');
                int intCnt;
                for (intCnt = 0; intCnt <= strDataBase.Length - 1; intCnt++)
                {
                    //if (strDataBase[intCnt].ToLower() == strCatalog.ToLower())
                    //{
                    strAppSetting = System.Configuration.ConfigurationManager.AppSettings[strDataBase[intCnt]];
                    string[] strDatabaseInfo;
                    strDatabaseInfo = strAppSetting.Split(',');
                    if (strDatabaseInfo.Length == 6)
                    {
                        IniConnection = System.Configuration.ConfigurationManager.AppSettings[strDatabaseInfo[5]];
                        string strConnection = "";
                        strConnection = IniConnection.Replace("#DataSource", strDatabaseInfo[0]);
                        strConnection = strConnection.Replace("#Catalog", strDatabaseInfo[1]);
                        strConnection = strConnection.Replace("#UserName", strDatabaseInfo[2]);
                        strConnection = strConnection.Replace("#Password", DesDecrypt(strDatabaseInfo[3]));
                        return strConnection;
                    }
                    //}
                }
            }
            return "";
        }
    }
}