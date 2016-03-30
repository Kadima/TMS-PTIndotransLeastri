using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Net;
using Microsoft.Web.Administration;

namespace ConfigIIS
{
    /// <summary>
    /// IIS 操作方法集合 (IIS7 or higher)
    /// </summary>
    public class IISControlHelper
    {
								public static ArrayList ListSites()
								{
												ArrayList al = new ArrayList();
												ServerManager iisManager = new ServerManager();
												foreach (Site site in iisManager.Sites)
												{
																al.Add(site.Name);
												}
												return al;
								}

        public static bool ExistApplication(string applicationName, int siteIndex)
        {
            ServerManager iisManager = new ServerManager();
												foreach (Application a in iisManager.Sites[siteIndex].Applications)
            {
                if (a.Path.Equals(applicationName))
                {
                    return true;
                }
            }
            return false;
        }

        public static void CreateApplication(string applicationPath, int siteIndex, string folderPath, string applicationPoolName)
        {
            ServerManager iisManager = new ServerManager();
												iisManager.Sites[siteIndex].Applications.Add(applicationPath, folderPath);
												iisManager.Sites[siteIndex].Applications[applicationPath].ApplicationPoolName = applicationPoolName;
            iisManager.CommitChanges();
        }

        public static void DeleteApplication(string applicationPath,int siteIndex)
        {
            ServerManager iisManager = new ServerManager();
												iisManager.Sites[siteIndex].Applications.Remove(iisManager.Sites[siteIndex].Applications[applicationPath]);
            iisManager.CommitChanges();
        }

        public static bool ExistApplicationPool(string appPoolName)
        {
            ServerManager iisManager = new ServerManager();
            foreach (ApplicationPool ap in iisManager.ApplicationPools)
            {
                if (ap.Name.Equals(appPoolName))
                {
                    return true;
                }
            }
            return false;
        }

        public static void CreateApplicationPool(string appPoolName)
        {
            ServerManager iisManager = new ServerManager();
            ApplicationPool appPool = iisManager.ApplicationPools.Add(appPoolName);
            appPool.AutoStart = true;
            appPool.ManagedPipelineMode = ManagedPipelineMode.Integrated;
            appPool.ManagedRuntimeVersion = "v4.0";
            appPool.ProcessModel.IdentityType = ProcessModelIdentityType.ApplicationPoolIdentity;
            iisManager.CommitChanges();            
        }

        public static void DeleteApplicationPool(string poolName)
        {
            ServerManager iisManager = new ServerManager();
            ApplicationPool appPool = iisManager.ApplicationPools[poolName];
            iisManager.ApplicationPools.Remove(appPool);
            iisManager.CommitChanges();
        }
    }
}
