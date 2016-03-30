using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApi
{
    public partial class Default : System.Web.UI.Page
    {
        public static string AssemblyFileVersion()
        {
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
            if (attributes.Length == 0)
            {
                return "";
            }
            else
            {
                return ((AssemblyFileVersionAttribute)attributes[0]).Version;
            }
        }
        public static string strVersion = AssemblyFileVersion();  //Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static string strNetVersion = Environment.Version.ToString();
        public static string strServiceIIS = HttpContext.Current.Request.ServerVariables["SERVER_SOFTWARE"];
        public static string strServiceTime = DateTime.Now.ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
												string path = "/WebApi/bin";
            string param = "";
            string method = "GET";
            string strServerURL = "www.sysfreight.net:8081/WebApi";
            string[] urls = new string[3];
												urls[0] = strServerURL + "/WebApi.dll";
												urls[1] = strServerURL + "/WebApi.ServiceInterface.dll";
												urls[2] = strServerURL + "/WebApi.ServiceModel.dll";
            foreach(string url in urls){
                var result = HttpUtility.Request(method, url, path, param, onComplete);
            }
            Response.Redirect("Default.aspx");
        }
        
        private void onComplete(HttpStatusCode code, string result)
        {
            Response.Write("<script>alert('" + result + "')</script>");
        }
    }
}