namespace Prb.ActiveDirectoryOperations.HelperClasses
{
    public class OSType
    {
        public static string GetDeviceType(string OS)
        {
            string oType = "";
            if (OS.Contains("Windows Server") || OS.Contains("Windows") || OS.Contains("Mac") || OS.Contains("Linux"))
            {
                oType = "Machines";
            }
            else
            {
                oType = "Others";
            }
            return oType;
        }
        public static string GetCMPCategory(string OS)
        {
            string oType = "";
            if (OS.Contains("Windows Server"))
            {
                oType = "Windows";
            }
            else if (OS.Contains("Windows"))
            {
                oType = "Windows";
            }
            else if (OS.Contains("Mac") || OS.Contains("IOS"))
            {
                oType = "IOS";
            }
            else if (OS.Contains("Linux"))
            {
                oType = "Linux";
            }
            else
            {
                oType = "Unknown Type";
            }
            return oType;
        }
        public static string GetCMPType(string OS)
        {
            string oType = "";
            if (OS.Contains("Windows Server"))
            {
                oType = "Windows Server";
            }
            else if (OS.Contains("Windows"))
            {
                oType = "Windows Client";
            }
            else if (OS.Contains("Mac") || OS.Contains("IOS"))
            {
                oType = "MAC";
            }
            else if (OS.Contains("Linux"))
            {
                oType = "Linux";
            }
            else
            {
                oType = "Unknown Type";
            }
            return oType;
        }
    }
}
