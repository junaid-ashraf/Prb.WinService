using Prb.ActiveDirectoryOperations.HelperClasses;
using Prb.DTO;
using Prb.DTO.Request;
using Prb.DTO.Response;
using Prb.SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Prb.ADReadRemoteRegisteryWithWMI
{
    public class ReadRemoteRegistryUsingWmi
    {
        public static Hw_DetailInfoDTO ReadRemoteRegistryusingWmi(string machineName, string username, string pass)
        {
            // Network Discovery Info
            var LstHw_DetailInfoDTO = new List<Hw_DetailInfoDTO>();
            Hw_DetailInfoDTO oHw_DetailInfoDTO = new Hw_DetailInfoDTO();
            var oHw_DetailInfoResponse = new Hw_DetailInfoResponse();
            var oHw_DetailInfoRequest = new Hw_DetailInfoRequest();

            #region  Connection 

            var programs = new List<string>();
            var connectionOptions = new ConnectionOptions
            {
                Username = username,
                Password = pass,
                Impersonation = ImpersonationLevel.Impersonate
            };
            var MachineName = machineName;

            var IsLocal = false;
            IsLocal = Utils.IsLocalIpAddress(MachineName);

            ManagementScope ms;
            if ((IsLocal == false) && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(pass))
            {
                ms = new ManagementScope("\\\\" + MachineName + "\\root\\cimv2", connectionOptions);
            }
            else if (IsLocal == true && machineName == Environment.MachineName)
            {
                // Local Machine
                ms = new ManagementScope("\\\\" + Environment.MachineName + "\\root\\cimv2");
            }
            else if (IsLocal == true)
            {
                ms = new ManagementScope("\\\\" + Environment.MachineName + "\\root\\cimv2");
            }
            else
            {
                return null;
            }

            bool result = false;
            Ping ping = new Ping();
            PingReply pingReply;
            string pingMsg = "";
            try
            {
                pingReply = ping.Send(MachineName, 1000);
                if (pingReply.Status == IPStatus.Success)
                    result = true;

                pingMsg = "IP Ping Status: " + pingReply.Status;
            }
            catch
            {
                result = false;
                pingMsg = "You IP have Some issue";
            }


            try
            {
                if (result)
                {

                }
                else
                {
                    WriteTextFile.WriteErrorLog("Not Connected due to: Machine " + pingMsg);
                    oHw_DetailInfoDTO.isConnected = false;
                    oHw_DetailInfoDTO.HwDescription = "Connection Exception: " + pingMsg;
                    return oHw_DetailInfoDTO;
                }
                ms.Connect();
                WriteTextFile.WriteErrorLog("Connected Successfully");
                oHw_DetailInfoDTO.HwDescription = "Connection: Machine Connected Successfully";
                oHw_DetailInfoDTO.isConnected = true;
            }
            catch (Exception ex)
            {
                WriteTextFile.WriteErrorLog("Not Connected due to: " + ex.Message + ex.StackTrace);
                oHw_DetailInfoDTO.isConnected = false;
                oHw_DetailInfoDTO.HwDescription = "Connection Exception: " + ex.Message;
                return oHw_DetailInfoDTO;
            }

            #endregion

            #region Fetching Operating System Information

            WriteTextFile.WriteErrorLog("====================Operating System Started=====================");
            var oq = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            var query = new ManagementObjectSearcher(ms, oq);
            try
            {
                var queryCollection = query.Get();
                foreach (var o in queryCollection)
                {
                    var mo = (ManagementObject)o;
                    string text = mo.GetText(TextFormat.Mof);
                    WriteTextFile.WriteErrorLog("Operating System: " + mo["Caption"]);
                    oHw_DetailInfoDTO.OS = mo["Caption"].ToString();

                    WriteTextFile.WriteErrorLog("Version: " + mo["Version"]);
                    oHw_DetailInfoDTO.BiosVersion = mo["Version"].ToString();

                    WriteTextFile.WriteErrorLog("BuildType: " + mo["BuildType"]);
                    WriteTextFile.WriteErrorLog("Manufacturer : " + mo["Manufacturer"]);

                    WriteTextFile.WriteErrorLog("Computer Name : " + mo["csname"]);
                    oHw_DetailInfoDTO.HwCaption = mo["csname"].ToString();

                    WriteTextFile.WriteErrorLog("Operating System Architecture : " + mo["OSArchitecture"]);
                    oHw_DetailInfoDTO.HwSystemType = mo["OSArchitecture"].ToString();

                    WriteTextFile.WriteErrorLog("InstallDate: " + mo["InstallDate"]);
                    WriteTextFile.WriteErrorLog("Windows Serial Number : " + mo["SerialNumber"]);
                    //oHw_DetailInfoDTO.BiosSerialNo = mo["SerialNumber"].ToString();

                    WriteTextFile.WriteErrorLog("Windows Directory : " + mo["WindowsDirectory"]);

                    WriteTextFile.WriteErrorLog("Status: " + mo["Status"]);
                    oHw_DetailInfoDTO.BiosVersion = mo["Version"].ToString();

                    WriteTextFile.WriteErrorLog("NumberOfUsers: " + mo["NumberOfUsers"]);
                    WriteTextFile.WriteErrorLog("RegisteredUser: " + mo["RegisteredUser"]);
                    WriteTextFile.WriteErrorLog("NumberOfLicensedUsers: " + mo["NumberOfLicensedUsers"]);
                    WriteTextFile.WriteErrorLog("NumberOfProcesses: " + mo["NumberOfProcesses"]);
                    WriteTextFile.WriteErrorLog("ServicePackMajorVersion: " + mo["ServicePackMajorVersion"]);
                    WriteTextFile.WriteErrorLog("ServicePackMinorVersion: " + mo["ServicePackMinorVersion"]);
                }
                WriteTextFile.WriteErrorLog("====================Operating System Ended=====================");

                #endregion



                #region objects Running software
                /// Running Softwares List
                ///  Dataabse column must be identity increment
                ///  

                var LstHw_Sw_RunningDTO = new List<Hw_Sw_RunningDTO>();
                var oHw_Sw_RunningResponse = new Hw_Sw_RunningResponse();
                var oHw_Sw_RunningRequest = new Hw_Sw_RunningRequest();

                #endregion objects Running software

                #region Process List Information
                try
                {
                    WriteTextFile.WriteErrorLog("====================Process List Started=====================");
                    oq = new ObjectQuery("SELECT * FROM Win32_Process");
                    query = new ManagementObjectSearcher(ms, oq);
                    queryCollection = query.Get();
                    foreach (var o in queryCollection)
                    {
                        try
                        {
                            var mo = (ManagementObject)o;
                            //create child node for operating system

                            Console.WriteLine("Process Name: " + mo["Name"] +
                            " | Caption: " + mo["Caption"] +
                            " | Description: " + mo["Description"] +
                            " | Priority: " + mo["Priority"] +
                            " | PeakVirtualSize: " + mo["PeakVirtualSize"] +
                            " | ProcessId: " + mo["ProcessId"] +
                            " | ParentProcessId: " + mo["ParentProcessId"] +
                            " | CreationDate: " + mo["CreationDate"] +
                            " | ExecutablePath: " + mo["ExecutablePath"]);

                            WriteTextFile.WriteErrorLog("Process Name: " + mo["Caption"] +
                            " | Caption: " + mo["Caption"] +
                            " | Description: " + mo["Description"] +
                            " | Priority: " + mo["Priority"] +
                            " | PeakVirtualSize: " + mo["PeakVirtualSize"] +
                            " | ProcessId: " + mo["ProcessId"] +
                            " | ParentProcessId: " + mo["ParentProcessId"] +
                            " | CreationDate: " + mo["CreationDate"] +
                            " | ExecutablePath: " + mo["ExecutablePath"]);


                            var oHw_Sw_RunningDTO = new Hw_Sw_RunningDTO();
                            oHw_Sw_RunningDTO.HwMasterInfoId = oHw_DetailInfoDTO.HwMasterInfoId;

                            oHw_Sw_RunningDTO.SwName = mo["Caption"].ToString();
                            oHw_Sw_RunningDTO.SwDescription = mo["Description"].ToString();
                            oHw_Sw_RunningDTO.LicenceNo = mo["ProcessId"].ToString();
                            try
                            {
                                string dateString = mo["CreationDate"].ToString().Substring(0, 8); // Modified from MSDN
                                oHw_Sw_RunningDTO.InstalledDate = DateTime.ParseExact(dateString, "yyyyMMdd", null);
                                Console.WriteLine(oHw_Sw_RunningDTO.InstalledDate);
                            }
                            catch (Exception)
                            {
                            }
                            try
                            {
                                oHw_Sw_RunningDTO.PathName = mo["ExecutablePath"].ToString();
                            }
                            catch (Exception)
                            {
                            }
                            oHw_Sw_RunningDTO.LicenceNo = mo["ThreadCount"].ToString();
                            oHw_Sw_RunningDTO.SerialNo = mo["ProcessId"].ToString();
                            oHw_Sw_RunningDTO.Status = mo["Priority"].ToString();

                            LstHw_Sw_RunningDTO.Add(oHw_Sw_RunningDTO);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteTextFile.WriteErrorLog("---Process List Exception---\n" + ex.Message + ex.StackTrace);
                    // ignored
                }
                WriteTextFile.WriteErrorLog("====================Process List Ended=====================");
                oHw_DetailInfoDTO.LstHw_Sw_RunningDTO = LstHw_Sw_RunningDTO;

                #endregion

                #region Computer System


                WriteTextFile.WriteErrorLog("====================Computer System Started=====================");
                oq = new ObjectQuery("SELECT * FROM Win32_ComputerSystem");
                query = new ManagementObjectSearcher(ms, oq);
                queryCollection = query.Get();
                foreach (var o in queryCollection)
                {
                    try
                    {
                        var mo = (ManagementObject)o;
                        string text = mo.GetText(TextFormat.Mof);
                        WriteTextFile.WriteErrorLog("Computer Manufacturer Name: " + mo["Manufacturer"]);
                        oHw_DetailInfoDTO.HwManufacturer = mo["Manufacturer"].ToString();
                        Console.WriteLine(oHw_DetailInfoDTO.HwManufacturer);

                        if (mo["Manufacturer"].ToString().ToUpper().Contains("VMWARE"))
                        {
                            oHw_DetailInfoDTO.IsVirtual = true;
                            oHw_DetailInfoDTO.VmInstanceName = mo["Manufacturer"].ToString();
                            oHw_DetailInfoDTO.VmInstanceStatus = mo["Manufacturer"].ToString();
                        }
                        else
                        {
                            oHw_DetailInfoDTO.IsServerRole = false;
                            oHw_DetailInfoDTO.IsVirtual = false;
                        }


                        WriteTextFile.WriteErrorLog("Caption: " + mo["Caption"]);
                        oHw_DetailInfoDTO.HwCaption = mo["Caption"].ToString();

                        WriteTextFile.WriteErrorLog("Model: " + mo["Model"]);
                        oHw_DetailInfoDTO.HwModelNo = mo["Model"].ToString();

                        WriteTextFile.WriteErrorLog("Name: " + mo["Name"]);

                        WriteTextFile.WriteErrorLog("System Type: " + mo["SystemType"]);
                        oHw_DetailInfoDTO.HwSystemType = mo["SystemType"].ToString();

                        WriteTextFile.WriteErrorLog("System Type: " + mo["SystemType"]);

                        try
                        {
                            WriteTextFile.WriteErrorLog("Part Number (SystemSKUNumber): " + mo["SystemSKUNumber"]);
                            oHw_DetailInfoDTO.HwPartNo = mo["SystemSKUNumber"].ToString();
                        }
                        catch (Exception ex)
                        {
                            oHw_DetailInfoDTO.HwPartNo = ex.Message;
                            Console.WriteLine("PartNo(SystemSKUNumber) not found due to:" + ex.Message + ex.StackTrace);
                        }

                        WriteTextFile.WriteErrorLog("Number Of Processors: " + mo["NumberOfProcessors"]);
                        oHw_DetailInfoDTO.NoOfProcessors = int.Parse(mo["NumberOfProcessors"].ToString());

                        WriteTextFile.WriteErrorLog("Total Physical Memory: " + Utils.formatSize(Int64.Parse(mo["totalphysicalmemory"].ToString()), false));
                        WriteTextFile.WriteErrorLog("Part Of Domain: " + mo["PartOfDomain"]);

                        WriteTextFile.WriteErrorLog("DNSHostName: " + mo["DNSHostName"]);
                        oHw_DetailInfoDTO.DNSHostName = mo["DNSHostName"].ToString();

                        WriteTextFile.WriteErrorLog("Domain: " + mo["Domain"]);
                        oHw_DetailInfoDTO.DomainName = mo["Domain"].ToString();

                        WriteTextFile.WriteErrorLog("User Name: " + mo["UserName"]);
                        oHw_DetailInfoDTO.UserName = mo["UserName"].ToString();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("====================Computer System info Exception=====================");
                        Console.WriteLine(ex.Message + ex.StackTrace);
                        Console.WriteLine("====================Computer System info Exception=====================");
                    }

                }
                WriteTextFile.WriteErrorLog("====================Computer System Ended=====================");
                #endregion

                #region System Base Board Information
                WriteTextFile.WriteErrorLog("====================System Base Board Started=====================");
                oq = new ObjectQuery("SELECT * FROM Win32_Baseboard");
                query = new ManagementObjectSearcher(ms, oq);
                queryCollection = query.Get();
                foreach (var o in queryCollection)
                {
                    var mo = (ManagementObject)o;
                    string text = mo.GetText(TextFormat.Mof);
                    // WriteTextFile.WriteErrorLog(text);

                    WriteTextFile.WriteErrorLog("Caption: " + mo["Caption"]);
                    oHw_DetailInfoDTO.BiosName = mo["Caption"].ToString();

                    WriteTextFile.WriteErrorLog("SerialNumber: " + mo["SerialNumber"]);
                    oHw_DetailInfoDTO.HwSerialNo = mo["SerialNumber"].ToString();

                    WriteTextFile.WriteErrorLog("Description: " + mo["Description"]);
                    oHw_DetailInfoDTO.HwDescription = mo["Description"].ToString();

                    WriteTextFile.WriteErrorLog("Product: " + mo["Product"]);
                    WriteTextFile.WriteErrorLog("PoweredOn: " + mo["PoweredOn"]);
                    WriteTextFile.WriteErrorLog("Replaceable: " + mo["Replaceable"]);
                    try
                    {
                        WriteTextFile.WriteErrorLog("Version: " + mo["Version"]);
                        oHw_DetailInfoDTO.HwVersion = mo["Version"].ToString();
                    }
                    catch (Exception)
                    {

                    }

                    WriteTextFile.WriteErrorLog("Replaceable: " + mo["Replaceable"]);
                    WriteTextFile.WriteErrorLog("Status: " + mo["Status"]);
                    oHw_DetailInfoDTO.HwStatus = mo["Status"].ToString();



                    Console.WriteLine("Caption: " + mo["Caption"]);
                    Console.WriteLine("SerialNumber: " + mo["SerialNumber"]);
                    Console.WriteLine("Description: " + mo["Description"]);
                    Console.WriteLine("Product: " + mo["Product"]);
                    Console.WriteLine("PoweredOn: " + mo["PoweredOn"]);
                    Console.WriteLine("Replaceable: " + mo["Replaceable"]);
                    Console.WriteLine("Version: " + mo["Version"]);
                    Console.WriteLine("Replaceable: " + mo["Replaceable"]);
                    Console.WriteLine("Status: " + mo["Status"]);
                }
                WriteTextFile.WriteErrorLog("====================System Base Board Ended=====================");
                #endregion

                #region System Bios Information
                WriteTextFile.WriteErrorLog("====================System Bios Started=====================");
                oq = new ObjectQuery("SELECT * FROM Win32_bios");
                query = new ManagementObjectSearcher(ms, oq);
                queryCollection = query.Get();
                foreach (var o in queryCollection)
                {

                    var mo = (ManagementObject)o;
                    string text = mo.GetText(TextFormat.Mof);
                    WriteTextFile.WriteErrorLog("BIOS: " + mo["Caption"]);
                    oHw_DetailInfoDTO.BiosName = mo["Caption"].ToString();

                    WriteTextFile.WriteErrorLog("Manufacturer: " + mo["Manufacturer"]);
                    WriteTextFile.WriteErrorLog("Bios Serial Number: " + mo["serialnumber"]);
                    oHw_DetailInfoDTO.BiosSerialNo = mo["serialnumber"].ToString();

                    WriteTextFile.WriteErrorLog("BIOS Version: " + mo["version"]);
                    oHw_DetailInfoDTO.BiosVersion = mo["version"].ToString();

                    WriteTextFile.WriteErrorLog("Release Date: " + mo["ReleaseDate"]);

                    try
                    {
                        WriteTextFile.WriteErrorLog("List Of Languages: " + (string[])mo["ListOfLanguages"]);
                        var langList = ((string[])mo["ListOfLanguages"]);
                        oHw_DetailInfoDTO.ListOfAvailableLanguages = string.Join(",", langList);
                    }
                    catch (Exception ex)
                    {
                        WriteTextFile.WriteErrorLog("Error 'Win32_bios': " + ex.Message + ex.StackTrace);
                        Console.WriteLine(ex.Message);
                    }
                }
                WriteTextFile.WriteErrorLog("====================System Bios Ended=====================");
                #endregion System Bios Information

                #region System Processor Information 
                WriteTextFile.WriteErrorLog("====================System Processor Started=====================");
                oq = new ObjectQuery("SELECT * FROM Win32_processor");
                query = new ManagementObjectSearcher(ms, oq);
                queryCollection = query.Get();
                foreach (var o in queryCollection)
                {
                    var mo = (ManagementObject)o;
                    string text = mo.GetText(TextFormat.Mof);
                    WriteTextFile.WriteErrorLog("Manufacturer: " + mo["Manufacturer"]);

                    WriteTextFile.WriteErrorLog("Processor Id: " + mo["ProcessorId"]);
                    WriteTextFile.WriteErrorLog("Processor Name: " + mo["Name"]);
                    WriteTextFile.WriteErrorLog("Computer Processor: " + mo["Caption"]);
                    oHw_DetailInfoDTO.Processor = mo["Caption"].ToString();

                    WriteTextFile.WriteErrorLog("Number Of Logical Processors: " + mo["NumberOfLogicalProcessors"]);
                    oHw_DetailInfoDTO.NoOfLogicalProcessors = int.Parse(mo["NumberOfLogicalProcessors"].ToString());

                    WriteTextFile.WriteErrorLog("Number Of Cores: " + mo["NumberOfCores"]);
                    WriteTextFile.WriteErrorLog("CPU Speed: " + Utils.formatSpeed(Int64.Parse(mo["MaxClockSpeed"].ToString())));
                    WriteTextFile.WriteErrorLog("L2 Cache Size: " + Utils.formatSize(Int64.Parse(mo["L2CacheSize"].ToString()), false));
                    WriteTextFile.WriteErrorLog("L3 Cache Size: " + Utils.formatSize(Int64.Parse(mo["L3CacheSize"].ToString()), false));
                }
                WriteTextFile.WriteErrorLog("====================System Processor Ended=====================");
                #endregion System Processor Information 

                #region System Time Zone Information 
                WriteTextFile.WriteErrorLog("====================System Time Zone Started=====================");
                oq = new ObjectQuery("SELECT * FROM Win32_timezone");
                query = new ManagementObjectSearcher(ms, oq);
                queryCollection = query.Get();
                foreach (var o in queryCollection)
                {
                    var mo = (ManagementObject)o;
                    string text = mo.GetText(TextFormat.Mof);
                    WriteTextFile.WriteErrorLog("Time Zone: " + mo["Caption"]);
                    oHw_DetailInfoDTO.CurrentTimeZone = mo["Caption"].ToString();
                }
                WriteTextFile.WriteErrorLog("====================System Time Zone Ended=====================");
                #endregion System Time Zone Information 

                #region Memory Information 
                //Get memory configuration
                oq = new ObjectQuery("SELECT * FROM Win32_LogicalMemoryConfiguration");
                query = new ManagementObjectSearcher(ms, oq);
                queryCollection = query.Get();

                WriteTextFile.WriteErrorLog("====================Logical Memory Configuration=====================");





                WriteTextFile.WriteErrorLog("====================Physical Ram Configuration Start=====================");
                /// <summary>
                /// Retrieving Physical Ram Memory.
                /// </summary>
                /// <returns></returns>
                oq = new ObjectQuery("SELECT Capacity FROM Win32_PhysicalMemory");
                query = new ManagementObjectSearcher(ms, oq);
                queryCollection = query.Get();
                long MemSize = 0;
                long mCap = 0;
                // In case more than one Memory sticks are installed
                foreach (ManagementObject obj in queryCollection)
                {
                    mCap = Convert.ToInt64(obj["Capacity"]);
                    MemSize += mCap;
                    WriteTextFile.WriteErrorLog("Mem Size: " + MemSize);
                }
                MemSize = (MemSize / 1024) / 1024;
                string memSize = MemSize.ToString() + "MB";
                WriteTextFile.WriteErrorLog("Total Size: " + memSize);
                oHw_DetailInfoDTO.PhysicalMemory = memSize.ToString();


                WriteTextFile.WriteErrorLog("====================Physical Ram Configuration End=====================");

                #endregion Memory Information 

                #region Network Connection Information 




                WriteTextFile.WriteErrorLog("====================Network Connection Started=====================");
                oq = new ObjectQuery("SELECT * FROM Win32_NetworkConnection");
                query = new ManagementObjectSearcher(ms, oq);
                queryCollection = query.Get();
                foreach (var o in queryCollection)
                {
                    var mo = (ManagementObject)o;
                    string text = mo.GetText(TextFormat.Mof);
                    WriteTextFile.WriteErrorLog("Name: " + mo["Name"]);

                }
                WriteTextFile.WriteErrorLog("====================Network Connection Ended=====================");
                #endregion Network Connection Information 

                #region Win32 Network Adapter Configuration
                WriteTextFile.WriteErrorLog("====================Win32_NetworkAdapterConfiguration System Started=====================");
                try
                {
                    ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                    ManagementObjectCollection moc = mc.GetInstances();
                    string MACAddress = String.Empty;

                    //// Installed Software's List
                    var LstHw_IpMacAddressDTO = new List<Hw_IpMacAddressDTO>();
                    var oHw_IpMacAddressResponse = new Hw_DetailInfoResponse();
                    var oHw_IpMacAddressRequest = new Hw_IpMacAddressRequest();
                    Hw_IpMacAddressDTO oHw_IpMacAddressDTO;
                    try
                    {
                        foreach (ManagementObject mo in moc)
                        {
                            oHw_IpMacAddressDTO = new Hw_IpMacAddressDTO();
                            oHw_IpMacAddressDTO.HwMasterInfoId = oHw_DetailInfoDTO.HwMasterInfoId;


                            string text = mo.GetText(TextFormat.Mof);
                            Console.WriteLine(text);
                            oHw_IpMacAddressDTO.IPEnabled = "false";
                            if (MACAddress == String.Empty)
                            {
                                if ((bool)mo["IPEnabled"] == true) MACAddress = mo["MacAddress"].ToString();
                            }
                            // string ipAddress;
                            try
                            {
                                if ((bool)mo["IPEnabled"] == true)
                                {
                                    string[] addresses = (string[])mo["IPAddress"];
                                    Console.WriteLine(addresses[0]);
                                    oHw_IpMacAddressDTO.IpAddress = addresses[0];
                                    oHw_IpMacAddressDTO.IPEnabled = ((bool)mo["IPEnabled"]).ToString();

                                    Console.WriteLine(mo["IPAddress"]);
                                }
                                oHw_IpMacAddressDTO.Caption = mo["Description"].ToString();
                                oHw_IpMacAddressDTO.MacAddress = mo["MacAddress"].ToString();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Exception Message in 'Network Cards Info': " + ex.Message + ex.StackTrace);
                            }
                            mo.Dispose();
                            if (!string.IsNullOrEmpty(oHw_IpMacAddressDTO.MacAddress))
                            {
                                LstHw_IpMacAddressDTO.Add(oHw_IpMacAddressDTO);
                            }
                        }
                        if (LstHw_IpMacAddressDTO.Count > 0)
                        {
                            oHw_DetailInfoDTO.LstHw_IpMacAddressDTO = LstHw_IpMacAddressDTO;
                        }
                    }
                    catch (Exception)
                    {
                        if (LstHw_IpMacAddressDTO.Count > 0)
                            oHw_DetailInfoDTO.LstHw_IpMacAddressDTO = LstHw_IpMacAddressDTO;
                    }
                }
                catch (Exception ex)
                {
                    WriteTextFile.WriteErrorLog("====================Win32_NetworkAdapterConfiguration System Exception: " + ex.Message + "=====================" + ex.StackTrace);
                }
                WriteTextFile.WriteErrorLog("====================Win32_NetworkAdapterConfiguration System Ended=====================");
                #endregion Win32 Network Adapter Configuration

                #region Network Card Information 

                WriteTextFile.WriteErrorLog("====================Network Cards Info Started=====================");
                oq = new ObjectQuery("SELECT * FROM Win32_NetworkAdapterConfiguration");
                query = new ManagementObjectSearcher(ms, oq);
                queryCollection = query.Get();
                string MACAdd = String.Empty;
                foreach (var o in queryCollection)
                {
                    var mo = (ManagementObject)o;
                    string text = mo.GetText(TextFormat.Mof);
                    if (MACAdd == String.Empty)
                    {
                        string[] ipFull;
                        string ipAddress;
                        try
                        {
                            ipFull = mo["IPAddress"].ToString().Split(',');
                            ipAddress = ipFull[0].ToString();
                            Console.WriteLine(mo["IPAddress"].ToString());
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Exception Message in 'Network Cards Info': " + ex.Message + ex.StackTrace);
                        }


                    }
                }
                WriteTextFile.WriteErrorLog("====================Network Cards Info Ended=====================");

                #endregion Network Card Information 

                #region Video Controller Information 

                WriteTextFile.WriteErrorLog("====================Video Controller Started=====================");
                oq = new ObjectQuery("SELECT * FROM Win32_VideoController");
                query = new ManagementObjectSearcher(ms, oq);
                queryCollection = query.Get();
                foreach (var o in queryCollection)
                {
                    try
                    {
                        var mo = (ManagementObject)o;
                        string text = mo.GetText(TextFormat.Mof);
                        WriteTextFile.WriteErrorLog("Name: " + mo["Name"]);
                        WriteTextFile.WriteErrorLog("Processor: " + mo["VideoProcessor"]);
                        WriteTextFile.WriteErrorLog("Mode: " + mo["VideoModeDescription"]);
                        oHw_DetailInfoDTO.BiosDisplayConfiguration = mo["VideoModeDescription"].ToString();

                        if (mo["AdapterRAM"] == null)
                        {
                            WriteTextFile.WriteErrorLog("Video Ram: " + mo["AdapterRAM"]);
                        }
                        else
                        {
                            WriteTextFile.WriteErrorLog("Video Ram: " + Utils.formatSize(Int64.Parse(mo["AdapterRAM"].ToString()), false));
                        }
                        WriteTextFile.WriteErrorLog("PNP Device ID: " + mo["PNPDeviceID"]);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine("Exception Message in 'Video Controller Cards Info': " + ex.Message + ex.StackTrace);
                    }
                }
                WriteTextFile.WriteErrorLog("====================Video Controller Ended=====================");
                #endregion Video Controller Information 

                #region Netwrok IIS Web Information 

                try
                {
                    // SELECT* FROM IIsWebInfo
                    WriteTextFile.WriteErrorLog("====================Network IIsWebInfo Started=====================");
                    oq = new ObjectQuery("SELECT * FROM IIsWebInfo");
                    query = new ManagementObjectSearcher(ms, oq);
                    queryCollection = query.Get();
                    foreach (var o in queryCollection)
                    {
                        var mo = (ManagementObject)o;
                        string text = mo.GetText(TextFormat.Mof);
                        WriteTextFile.WriteErrorLog(text);
                    }
                    WriteTextFile.WriteErrorLog("====================Network IIsWebInfo Ended=====================");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                try
                {
                    WriteTextFile.WriteErrorLog("====================Network IIsWebService Started=====================");
                    oq = new ObjectQuery("SELECT * FROM IIsWebService");
                    query = new ManagementObjectSearcher(ms, oq);
                    queryCollection = query.Get();
                    foreach (var o in queryCollection)
                    {
                        var mo = (ManagementObject)o;
                        string text = mo.GetText(TextFormat.Mof);
                        WriteTextFile.WriteErrorLog(text);
                    }
                    WriteTextFile.WriteErrorLog("====================Network IIsWebService Ended=====================");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                #endregion Netwrok IIS Web Information 

                #region Network IIS Smtp Service
                try
                {
                    WriteTextFile.WriteErrorLog("====================Network IIsSmtpService Started=====================");
                    oq = new ObjectQuery("SELECT * FROM IIsSmtpService");
                    query = new ManagementObjectSearcher(ms, oq);
                    queryCollection = query.Get();
                    foreach (var o in queryCollection)
                    {
                        var mo = (ManagementObject)o;
                        string text = mo.GetText(TextFormat.Mof);
                        WriteTextFile.WriteErrorLog(text);
                    }
                    WriteTextFile.WriteErrorLog("====================Network IIsSmtpService Ended=====================");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                #endregion Network IIS Smtp Service

                #region Network IIS Pop3 Service 
                try
                { //  SELECT* FROM IIsPop3Service
                    WriteTextFile.WriteErrorLog("====================Network IIsPop3Service Started=====================");
                    oq = new ObjectQuery("SELECT * FROM IIsPop3Service");
                    query = new ManagementObjectSearcher(ms, oq);
                    queryCollection = query.Get();
                    foreach (var o in queryCollection)
                    {
                        var mo = (ManagementObject)o;
                        string text = mo.GetText(TextFormat.Mof);
                        WriteTextFile.WriteErrorLog(text);
                    }
                    WriteTextFile.WriteErrorLog("====================Network IIsPop3Service Ended=====================");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                #endregion Network IIS Pop3 Service 

                #region Installed Software and services List

                //// Services List
                //// Installed Software's List
                var LstHw_Sw_ServicesDTO = new List<Hw_Sw_ServicesDTO>();
                var oHw_Sw_ServicesResponse = new Hw_Sw_ServicesResponse();
                var oHw_Sw_ServicesRequest = new Hw_Sw_ServicesRequest();

                WriteTextFile.WriteErrorLog("====================Services List Started=====================");
                string[] lvData = new string[12];
                try
                {
                    oq = new ObjectQuery("SELECT * FROM Win32_Service");
                    query = new ManagementObjectSearcher(ms, oq);
                    queryCollection = query.Get();
                    foreach (var o in queryCollection)
                    {
                        try
                        {
                            var mo = (ManagementObject)o;
                            //create child node for operating system

                            var oHw_Sw_ServicesDTO = new Hw_Sw_ServicesDTO();
                            oHw_Sw_ServicesDTO.HwMasterInfoId = oHw_DetailInfoDTO.HwMasterInfoId;

                            lvData[0] = mo["Name"].ToString();
                            oHw_Sw_ServicesDTO.Caption = mo["Name"].ToString();
                            //lvData[1] = mo["Caption"].ToString();
                            lvData[2] = mo["DisplayName"].ToString();
                            oHw_Sw_ServicesDTO.DisplayName = mo["DisplayName"].ToString();

                            lvData[3] = mo["Description"].ToString();
                            oHw_Sw_ServicesDTO.Description = mo["Description"].ToString();

                            //lvData[4] = mo["PathName"].ToString();
                            lvData[5] = mo["ServiceType"].ToString();
                            oHw_Sw_ServicesDTO.ServiceType = mo["ServiceType"].ToString();

                            lvData[6] = mo["StartMode"].ToString();
                            oHw_Sw_ServicesDTO.StartMode = mo["StartMode"].ToString();

                            if (mo["Started"].Equals(true))
                                lvData[7] = "Started";
                            else
                                lvData[7] = "Stop";
                            oHw_Sw_ServicesDTO.Started = mo["Started"].Equals(true);

                            lvData[8] = mo["StartName"].ToString();
                            oHw_Sw_ServicesDTO.StartName = mo["StartName"].ToString();

                            lvData[9] = mo["State"].ToString();
                            oHw_Sw_ServicesDTO.State = mo["State"].ToString();

                            lvData[10] = mo["Status"].ToString();
                            oHw_Sw_ServicesDTO.Status = mo["Status"].ToString();

                            WriteTextFile.WriteErrorLog(string.Join("','", lvData));

                            LstHw_Sw_ServicesDTO.Add(oHw_Sw_ServicesDTO);
                        }
                        catch (Exception ex)
                        {
                            WriteTextFile.WriteErrorLog(ex);
                        }
                    }

                }
                catch (Exception ex)
                {
                    oHw_DetailInfoDTO.LstHw_Sw_ServicesDTO = LstHw_Sw_ServicesDTO;

                    // ignored
                }
                oHw_DetailInfoDTO.LstHw_Sw_ServicesDTO = LstHw_Sw_ServicesDTO;
                WriteTextFile.WriteErrorLog("====================Services List Ended=====================");



                //// Installed Software's List
                var LstHw_Sw_InstalledDTO = new List<Hw_Sw_InstalledDTO>();
                var oHw_Sw_InstalledResponse = new Hw_Sw_InstalledResponse();
                var oHw_Sw_InstalledRequest = new Hw_Sw_InstalledRequest();

                WriteTextFile.WriteErrorLog("====================Installed Softwares List Started=====================");
                int count = 0;

                WriteTextFile.WriteErrorLog(MachineName + " Installed Softwares List From Win32_Product as:-");
                oq = new ObjectQuery("SELECT * FROM Win32_Product");
                query = new ManagementObjectSearcher(ms, oq);
                queryCollection = query.Get();
                foreach (var o in queryCollection)
                {
                    try
                    {

                        var mo = (ManagementObject)o;
                        count++;
                        WriteTextFile.WriteErrorLog("Software No: " + count + "  |  " + mo["Name"] + " | " + mo["InstallDate"].ToString() + " | " + mo["Vendor"].ToString() + " | " + mo["Version"].ToString());

                        Hw_Sw_InstalledDTO oHw_Sw_InstalledDTO = new Hw_Sw_InstalledDTO();
                        oHw_Sw_InstalledDTO.HwMasterInfoId = oHw_DetailInfoDTO.HwMasterInfoId;

                        oHw_Sw_InstalledDTO.SwName = mo["Name"].ToString();

                        //  Console.WriteLine(mo["InstallDate"].ToString());

                        string dateString = mo["InstallDate"].ToString(); // Modified from MSDN
                        try
                        {
                            oHw_Sw_InstalledDTO.InstalledDate = DateTime.ParseExact(dateString, "yyyyMMdd", null);

                            Console.WriteLine(oHw_Sw_InstalledDTO.InstalledDate);
                        }
                        catch (Exception)
                        {

                        }

                        oHw_Sw_InstalledDTO.CopyRight = mo["Vendor"].ToString();
                        oHw_Sw_InstalledDTO.SwDescription = mo["Vendor"].ToString();
                        oHw_Sw_InstalledDTO.SwVersion = mo["Version"].ToString();

                        LstHw_Sw_InstalledDTO.Add(oHw_Sw_InstalledDTO);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        WriteTextFile.WriteErrorLog("Exception Message From Installesd Softwares List: " + ex.Message);
                        WriteTextFile.WriteErrorLog("====================Installed Softwares List End=====================");
                    }
                }
                oHw_DetailInfoDTO.LstHw_Sw_InstalledDTO = LstHw_Sw_InstalledDTO;



                ///////////////////////////////// Softwares list Compression

                count = 0;
                try
                {
                    WriteTextFile.WriteErrorLog(MachineName + " Installed Softwares List From Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall as:-");

                    string softwareRegLoc = @"Software\Microsoft\Windows\CurrentVersion\Uninstall";

                    ManagementClass registry = new ManagementClass(ms, new ManagementPath("StdRegProv"), null);
                    ManagementBaseObject inParams = registry.GetMethodParameters("EnumKey");
                    inParams["hDefKey"] = 0x80000002; //HKEY_LOCAL_MACHINE
                    inParams["sSubKeyName"] = softwareRegLoc;

                    // Read Registry Key Names 
                    ManagementBaseObject outParams = registry.InvokeMethod("EnumKey", inParams, null);
                    string[] programGuids = outParams["sNames"] as string[];

                    foreach (string subKeyName in programGuids)
                    {
                        inParams = registry.GetMethodParameters("GetStringValue");
                        inParams["hDefKey"] = 0x80000002;//HKEY_LOCAL_MACHINE
                        inParams["sSubKeyName"] = softwareRegLoc + @"\" + subKeyName;
                        inParams["sValueName"] = "DisplayName";
                        // Read Registry Value 
                        outParams = registry.InvokeMethod("GetStringValue", inParams, null);

                        if (outParams.Properties["sValue"].Value != null)
                        {
                            string softwareName = outParams.Properties["sValue"].Value.ToString();
                            programs.Add(softwareName);
                        }
                    }
                    //// share folder path
                    programs.Sort();
                    foreach (string softname in programs)
                    {
                        count++;
                        WriteTextFile.WriteErrorLog("Software No: " + count + "  |  " + softname);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    WriteTextFile.WriteErrorLog("Exception Message From Installesd Softwares List: " + ex.Message);
                    WriteTextFile.WriteErrorLog("====================Installed Softwares List End=====================");
                }
                WriteTextFile.WriteErrorLog("====================Installed Softwares List Ended=====================");

                return oHw_DetailInfoDTO;
            }
            catch (Exception ex)
            {
                WriteTextFile.WriteErrorLog("Hardware Detailed Information Outer ERROR");
                WriteTextFile.WriteErrorLog("==========================================");
                WriteTextFile.WriteErrorLog(ex);

                return oHw_DetailInfoDTO;
            }
            #endregion Installed Software and services List
        }
    }
}
