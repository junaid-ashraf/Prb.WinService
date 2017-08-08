using Prb.ActiveDirectoryOperations.HelperClasses;
using Prb.SharedLibrary;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prb.ADDeviceInfoList
{
    public class DeviceInfoList
    {
        public List<DeviceInfo> GetDevicesInformationList(string domainName)
        {
            int DevicesCount = 0;
            DirectorySearcher mySearcher;
            var deviceInfoList = new List<DeviceInfo>();
            try
            {
                var entry = new DirectoryEntry("LDAP://" + domainName);

                // For Windows Servers
                mySearcher = new DirectorySearcher(entry) { Filter = ("(objectClass=computer)") };
                foreach (SearchResult resEnt in mySearcher.FindAll())
                {
                    string cmpName = resEnt.GetDirectoryEntry().Name.Substring(3);
                    string sFilter = "(&(objectCategory=computer)(name=" + cmpName + "))";
                    var directorySearch = new DirectorySearcher(entry, sFilter);
                    var directorySearchResult = directorySearch.FindOne();
                    var deComp = directorySearchResult.GetDirectoryEntry();
                    var cmpOs = deComp.Properties["operatingSystem"].Value.ToString();
                    string devType = OSType.GetDeviceType(cmpOs);
                    string devCategory = OSType.GetCMPCategory(cmpOs);
                    string cmpType = OSType.GetCMPType(cmpOs);
                    var deviceInfo = new DeviceInfo
                    {
                        DevType = devType,
                        DevCategory = devCategory,
                        ComType = cmpType,
                        ComName = cmpName,
                        ComOS = cmpOs
                    };
                    deviceInfoList.Add(deviceInfo);
                    DevicesCount++;
                    WriteTextFile.WriteErrorLog(DevicesCount + ") Machine Type: " + devType + " | " + "cat: " + devCategory + " | " + "CMP Type: " + cmpType + " | " + "CmpName: " + cmpName + " | Cmp OS:" + cmpOs);
                }
            }
            catch (Exception ex)
            {
                WriteTextFile.WriteErrorLog("Device Info List ERROR");
                WriteTextFile.WriteErrorLog("==========================================");
                WriteTextFile.WriteErrorLog(ex);
                deviceInfoList = null;
            }
            return deviceInfoList;
        }
    }
}
