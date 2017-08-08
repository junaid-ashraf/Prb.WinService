using System;
using System.DirectoryServices;
using System.Collections.Generic;
using System.Collections;
using System.DirectoryServices.AccountManagement;
using System.Text;
using System.IO;
using Prb.SqliteDAL;
using Prb.Common;

namespace Prb.ActiveDirectoryOperations.HelperClasses
{
    public class ADLDAP
    {
        /// <summary>
        /// Fetch and Return Active Directory Device information List against given Domain name
        /// </summary>
        /// <param name="domainName"></param>
        /// <returns></returns>
        public static List<DeviceInfo> DeviceInfoList(string domainName)
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
        /// <summary>
        /// Fetch and Return Active Directory Users information List against given Domain name
        /// </summary>
        /// <param name="stringDomainName"></param>
        /// <returns></returns>
        public static List<DomainUsersInfo> FindUsersInformation(string stringDomainName, string username, string pass)
        {
            List<DomainUsersInfo> LstUsers = new List<DomainUsersInfo>();
            DomainUsersInfo objDomainUsersInfo;
            if (string.IsNullOrWhiteSpace(stringDomainName))
            {
                stringDomainName = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
            }

            if (stringDomainName != null)
            {
                PrincipalContext PrincipalContext1 = new PrincipalContext(ContextType.Domain, stringDomainName, username, pass);
                WriteTextFile.WriteErrorLog("DomainName:" + stringDomainName);
                UserPrincipal UserPrincipal1 = new UserPrincipal(PrincipalContext1);
                PrincipalSearcher searchUser = new PrincipalSearcher(UserPrincipal1);

                foreach (UserPrincipal result in searchUser.FindAll())
                {
                    objDomainUsersInfo = new DomainUsersInfo();

                    //objDomainUsersInfo.SiteId = siteId;
                    //objDomainUsersInfo.ScheduleId = scheduleId;
                    objDomainUsersInfo.SamAccountName = result.SamAccountName;
                    objDomainUsersInfo.DisplayName = result.DisplayName;
                    objDomainUsersInfo.Name = result.Name;
                    objDomainUsersInfo.GivenName = result.GivenName;
                    objDomainUsersInfo.Surname = result.Surname;
                    objDomainUsersInfo.Description = result.Description;
                    objDomainUsersInfo.Enabled = result.Enabled;
                    objDomainUsersInfo.EmployeeId = result.EmployeeId;
                    objDomainUsersInfo.LastPasswordSet = result.LastPasswordSet;
                    objDomainUsersInfo.LastBadPasswordAttempt = result.LastBadPasswordAttempt;
                    objDomainUsersInfo.LastLogon = result.LastLogon;

                    User User1 = new User(result.SamAccountName, result.DisplayName, result.Name, result.GivenName, result.Surname,
                      result.Description, result.Enabled, result.EmployeeId, result.LastPasswordSet, result.LastBadPasswordAttempt, result.LastLogon);
                    Users1.Add(User1);

                    LstUsers.Add(objDomainUsersInfo);
                }
                searchUser.Dispose();
                //      ExporttoCSV();
            }
            else
            {
                //  File.WriteAllText(string.Format(System.Environment.CurrentDirectory + "\\Error.csv"), "This computer is not a member of domain");
            }
            return LstUsers;
        }
        /// <summary>
        ///  Utilities
        /// </summary>
        public static Users Users1 = new Users();
        public class Users : List<User> { }
        /// <summary>
        /// Export to CSV file
        /// </summary>
        public static void ExporttoCSV()
        {
            var dir = @"C:\Zones\";  // folder location
            if (!Directory.Exists(dir))  // if it doesn't exist, create
            {
                Directory.CreateDirectory(dir);
            }
            string stringFileName = dir + "UsersLog_" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + "_" + DateTime.Now.Hour + DateTime.Now.Minute + ".csv";

            //Save Users
            StringBuilder StringBuilder1 = new StringBuilder(null);
            foreach (string string1 in User.StringArrayUesrProperties)
            {
                if (StringBuilder1.Length == 0)
                    StringBuilder1.Append(string1);
                StringBuilder1.Append(',' + string1);
            }
            StringBuilder1.AppendLine();
            foreach (User User1 in Users1)
            {
                StringBuilder StringBuilderTemp = new StringBuilder(null);
                foreach (string string1 in User1.Properties())
                {
                    if (StringBuilderTemp.Length == 0)
                        StringBuilderTemp.Append(string1);
                    StringBuilderTemp.Append(',' + string1);
                }
                //  WriteErrorLog(StringBuilderTemp.ToString());
                StringBuilder1.AppendLine(StringBuilderTemp.ToString());
            }
            File.WriteAllText(stringFileName, StringBuilder1.ToString(), Encoding.UTF8);
        }
        /// <summary>
        /// Fetch and Return Active Directory computer List List against given Domain name
        /// </summary>
        /// <param name="stringDomainName"></param>
        /// <returns></returns>
        public static List<Computer> FindComputersList(string stringDomainName)
        {
            List<Computer> LstComputers = new List<Computer>();
            if (string.IsNullOrWhiteSpace(stringDomainName))
            {
                stringDomainName = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
            }
            if (stringDomainName != null)
            {
                PrincipalContext PrincipalContext1 = new PrincipalContext(ContextType.Domain, stringDomainName);

                ComputerPrincipal ComputerPrincipal1 = new ComputerPrincipal(PrincipalContext1);
                PrincipalSearcher searchComputer = new PrincipalSearcher(ComputerPrincipal1);
                foreach (ComputerPrincipal result in searchComputer.FindAll())
                {
                    Computer Computer1 = new Computer(result.SamAccountName, result.DisplayName, result.Name, result.Description, result.Enabled, result.LastLogon);
                    LstComputers.Add(Computer1);
                }
                searchComputer.Dispose();
            }
            else
            {
                //  File.WriteAllText(string.Format(System.Environment.CurrentDirectory + "\\Error.csv"), "This computer is not a member of domain");
            }
            return LstComputers;
        }
        /// <summary>
        /// Fetch and Return Active Directory Printer information List against given Domain name
        /// </summary>
        /// <param name="domainName"></param>
        /// <returns></returns>
        public static List<DeviceInfo> PrintersInfoList(string domainName)
        {
            var deviceInfoList = new List<DeviceInfo>();
            try
            {
                using (var ds = new DirectorySearcher())
                {
                    ds.SearchRoot = new DirectoryEntry("LDAP://" + domainName);
                    ds.Filter = "(|(&(objectCategory=printQueue)(name=*)))";

                    ds.PropertiesToLoad.Add("printername");
                    ds.PropertiesToLoad.Add("portname");
                    //ds.PropertiesToLoad.Add("servername");
                    //ds.PropertiesToLoad.Add("cn");
                    //ds.PropertiesToLoad.Add("name");
                    //ds.PropertiesToLoad.Add("printsharename");
                    ds.ReferralChasing = ReferralChasingOption.None;
                    ds.Sort = new SortOption("name", SortDirection.Descending);

                    using (var src = ds.FindAll())
                    {
                        foreach (SearchResult sr in src)
                        {
                            var deviceInfo = new DeviceInfo();
                            deviceInfo.DevType = "Devices";
                            deviceInfo.DevCategory = "Printer";
                            deviceInfo.ComType = "Printer";
                            //WriteTextFile.WriteErrorLog(sr.GetDirectoryEntry().Name);
                            foreach (DictionaryEntry p in sr.Properties)
                            {
                                var propName = p.Key;
                                var propCollection = (ResultPropertyValueCollection)p.Value;
                                var propValue = propCollection.Count > 0 ? propCollection[0] : "";
                                if (propName.ToString() == "printername")
                                {
                                    deviceInfo.ComName = propValue.ToString();
                                }
                                else if (propName.ToString() == "portname")
                                {
                                    deviceInfo.IP = propValue.ToString();
                                }
                                //WriteTextFile.WriteErrorLog(propName.ToString());
                                //WriteTextFile.WriteErrorLog(propValue.ToString());
                            }
                            deviceInfoList.Add(deviceInfo);
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                WriteTextFile.WriteErrorLog("Printers Info List ERROR");
                WriteTextFile.WriteErrorLog("==========================================");
                WriteTextFile.WriteErrorLog(ex);
                deviceInfoList = null;
            }
            return deviceInfoList;
        }
    }



}
