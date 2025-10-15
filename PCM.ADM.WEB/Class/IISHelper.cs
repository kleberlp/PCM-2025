using Microsoft.Web.Administration;
using System.Linq;

namespace PCM.ADM.WEB.Class
{
    public sealed class IISHelper

    {
        public static void CreateApplicationPool(string applicationPoolName)
        {
            using (ServerManager serverManager = new ServerManager())
            {
                if (serverManager.ApplicationPools[applicationPoolName] != null)
                    return;
                ApplicationPool newPool = serverManager.ApplicationPools.Add(applicationPoolName);
                newPool.ManagedRuntimeVersion = "v4.0";
                serverManager.CommitChanges();
            }
        }

        public static void CreateSite(string siteName, string path)
        {
            using (ServerManager serverManager = new ServerManager())
            {
                var sites = serverManager.Sites;
                if (sites[siteName] == null)
                {
                    sites.Add(siteName, "http", "*:80:", path);
                    serverManager.CommitChanges();
                }
            }
        }

        public static void CreateApplication(string siteName, string applicationName, string path)
        {
            using (ServerManager serverManager = new ServerManager())
            {
                var site = GetSite(serverManager, siteName);
                var applications = site.Applications;
                if (applications["/" + applicationName] == null)
                {
                    applications.Add("/" + applicationName, path);
                    serverManager.CommitChanges();
                }
            }
        }

        public static void DropApplication(string siteName, string applicationName)
        {
            using (ServerManager serverManager = new ServerManager())
            {
                var site = GetSite(serverManager, siteName);
                var applications = site.Applications;
                if (applications["/" + applicationName] == null)
                {
                    applications.Remove(applications["/" + applicationName]);
                    serverManager.CommitChanges();
                }
            }
        }

        public static void CreateVirtualDirectory(string siteName, string applicationName, string virtualDirectoryName, string path)
        {
            using (ServerManager serverManager = new ServerManager())
            {
                var application = GetApplication(serverManager, siteName, applicationName);
                application.VirtualDirectories.Add("/" + virtualDirectoryName, path);
                serverManager.CommitChanges();
            }
        }

        public static void SetApplicationApplicationPool(string siteName, string applicationPoolName)
        {
            using (ServerManager serverManager = new ServerManager())
            {
                var site = GetSite(serverManager, siteName);
                if (site != null)
                {
                    foreach (Application app in site.Applications)
                    {
                        app.ApplicationPoolName = applicationPoolName;
                    }
                }
                serverManager.CommitChanges();
            }
        }
        public static Site GetSite(ServerManager serverManager, string siteName)
        {
            return serverManager.Sites.FirstOrDefault(x => x.Name == siteName);
        }

        private static Application GetApplication(ServerManager serverManager, string siteName, string applicationName)
        {
            return serverManager.Sites.FirstOrDefault(x => x.Name == siteName)?.Applications.FirstOrDefault(x => x.Path == "/" + applicationName);
        }

    }
}