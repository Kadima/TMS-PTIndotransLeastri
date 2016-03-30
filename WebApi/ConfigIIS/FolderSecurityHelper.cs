using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.AccessControl;

namespace ConfigIIS
{
    public class FolderSecurityHelper
    {
        public static bool ExistFolderRights(string FolderPath)
        {
            DirectorySecurity dSecurity = Directory.GetAccessControl(FolderPath, AccessControlSections.All);
            foreach (FileSystemAccessRule rule in dSecurity.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount)))
            {
                if (rule.IdentityReference.Value.Equals(@"BUILTIN\IIS_IUSRS"))
                {
                    return true;
                }
            }
            return false;
        }

        public static void SetFolderRights(string FolderPath)
        {
            var security = new DirectorySecurity();
            security.AddAccessRule(new FileSystemAccessRule("IIS_IUSRS", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
            Directory.SetAccessControl(FolderPath, security);
        }
    }
}
