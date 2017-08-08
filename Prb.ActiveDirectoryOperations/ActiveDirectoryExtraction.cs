using System;
using System.Collections.Generic;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
using System.Configuration;
using System.Data;
//using System.Diagnostics;
//using System.Linq;

//using System.Text;
//using System.Threading.Tasks;
using Prb.DTO;
using Prb.DTO.Request;
using Prb.DTO.Response;
using Prb.SqliteDAL;

//using System.IO;
using System.Threading;
using System.DirectoryServices;
using System.Net;
using System.Xml;
using Prb.ActiveDirectoryOperations.HelperClasses;
using Prb.Common;
using Prb.ADLinux;
using Prb.ADReadRemoteRegisteryWithWMI;
using System.DirectoryServices.AccountManagement;

namespace Prb.ActiveDirectoryOperations
{
    public class ActiveDirectoryExtraction
    {

        #region Varaibales 
        public static int ScheduleId = 0;
        private static List<string> ipListWindows = null;
        private static Hw_MasterInfo ipListWindowsObj = null;
        private static List<Hw_MasterInfo> ipListWindowsList = null;
        private static List<Hw_MasterInfo> ipListWindowsListRemainScaning = null;
        private static List<string> ipListWindowsServer = null;
        private static List<string> ipListWindowsClient = null;
        private static List<string> ipListLinux = null;
        private static List<string> ipListMAC = null;
        private static List<string> ipListPrinters = null;
        private static List<string> ipListOthers = null;

        static Prb_SettingDTO _prb_settingObj = new Prb_SettingDTO();
        static Prb_SettingsDAL _Prb_SettingsDAL = new Prb_SettingsDAL();
        static Prb_ScheduleRequest oPrb_ScheduleRequestStart = new Prb_ScheduleRequest();
        #endregion

        /// <summary>
        /// Actual Exceution Function of Active Directory Scan
        /// </summary>
        public void StartActiveDirectoryExecution()
        {
            int DevicesCount = 0;

            try
            {
                WriteTextFile.WriteErrorLog("====================Inside Prb.Opration=====================");

                #region Domain Transfer objects
                // Used Schedule For Probe status in Databse 
                Prb_ScheduleRequest oPrb_ScheduleRequestStart;
                oPrb_ScheduleRequestStart = new Prb_ScheduleRequest() { Prb_Schedule = new Prb_ScheduleDTO() };
                Console.WriteLine("Service Process Start");

                // System.Diagnostics.Debugger.Launch();
                var oPrb_SiteDataAccess = new Prb_SiteDataAccess();
                var oPrb_SiteResponse = oPrb_SiteDataAccess.GetSiteData();


                var oPrb_ScheduleDataAccess = new Prb_ScheduleDataAccess();

                // Network Discovery Info
                var LstDomainUsersInfoDTO = new List<DomainUsersInfoDTO>();
                DomainUsersInfoDTO oDomainUsersInfoDTO;
                var oDomainUsersInfoResponse = new DomainUsersInfoResponse();
                var oDomainUsersInfoAccess = new DomainUsersInfoAccess();

                // Network Discovery Info
                // hardware master information
                var LstHw_MasterInfoDTO = new List<Hw_MasterInfoDTO>();
                Hw_MasterInfoDTO oHw_MasterInfoDTO;
                var oHw_MasterInfoResponse = new Hw_MasterInfoResponse();
                var oHw_MasterInfoDataAccess = new Hw_MasterInfoDataAccess();


                //Hardware detail information
                var LstHw_DetailInfoDTO = new List<Hw_DetailInfoDTO>();
                Hw_DetailInfoDTO oHw_DetailInfoDTO;
                var oHw_DetailInfoResponse = new Hw_DetailInfoResponse();
                var oHw_DetailInfoDataAccess = new Hw_DetailInfoDataAccess();

                ipListWindowsList = new List<Hw_MasterInfo>();
                ipListWindowsListRemainScaning = new List<Hw_MasterInfo>();

                #endregion

                while (true)
                {
                    #region ServiceCode

                    try
                    {
                        if (oPrb_SiteResponse.Count() > 0)
                        {
                            // Probe Settings Detail
                            //XmlFileReadProbeSetting();
                            _prb_settingObj = _Prb_SettingsDAL.GetProbeSetting(_Prb_SettingDTO);
                            _Prb_SettingDTO = _prb_settingObj;

                            if (_Prb_SettingDTO != null && _Prb_SettingDTO.SettingId != 0)
                            {

                                foreach (Prb_SiteDTO oSiteDTO in oPrb_SiteResponse)
                                {
                                    //settings scheduler object 
                                    oPrb_ScheduleRequestStart = new Prb_ScheduleRequest() { Prb_Schedule = new Prb_ScheduleDTO() };
                                    oPrb_ScheduleRequestStart.Prb_Schedule.ScheduleId = 0;
                                    oPrb_ScheduleRequestStart.Prb_Schedule.SiteId = oSiteDTO.SiteId;
                                    oPrb_ScheduleRequestStart.Prb_Schedule.SettingId = _Prb_SettingDTO.SettingId;
                                    oPrb_ScheduleRequestStart.Prb_Schedule.StartDateTime = DateTime.Now;
                                    oPrb_ScheduleRequestStart.Prb_Schedule.EndDateTime = DateTime.Now;
                                    oPrb_ScheduleRequestStart.Prb_Schedule.Description = oSiteDTO.Description;
                                    oPrb_ScheduleRequestStart.Prb_Schedule.StatusId = 1; // Start 1, Running 2, Completed 3, Stop 4, Exception 5...
                                    oPrb_ScheduleRequestStart.Prb_Schedule.OperationId = 1;
                                    ScheduleId = oPrb_ScheduleDataAccess.ProcessScheduleDataAdd(oPrb_ScheduleRequestStart);

                                    #region Lists of Devices Objects
                                    ipListWindows = new List<string>();
                                    ipListWindowsServer = new List<string>();
                                    ipListWindowsClient = new List<string>();
                                    ipListLinux = new List<string>();
                                    ipListMAC = new List<string>();
                                    ipListPrinters = new List<string>();
                                    ipListOthers = new List<string>();
                                    #endregion

                                    #region Setting Domain 
                                    var domainNameComplete = "";
                                    if (!(string.IsNullOrEmpty(_prb_settingObj.DomainName)))
                                    {
                                        domainNameComplete = _prb_settingObj.DomainName;
                                    }
                                    else if (!(string.IsNullOrEmpty(_prb_settingObj.DomainName)))
                                    {
                                        domainNameComplete = Environment.UserDomainName;
                                    }

                                    // If domain name not found then get automaticaly
                                    if (string.IsNullOrWhiteSpace(domainNameComplete))
                                    {
                                        domainNameComplete = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
                                    }
                                    #endregion

                                    #region Entry in Log File About User and machine information

                                    // Probe Running Machine Info
                                    WriteTextFile.WriteErrorLog("===================================================================================");
                                    WriteTextFile.WriteErrorLog("====================Probe Running Computer Information Started=====================");
                                    WriteTextFile.WriteErrorLog("-----------------------------------------------------------------------------------");
                                    WriteTextFile.WriteErrorLog("Probe Running Domain Name: " + Environment.UserDomainName);
                                    WriteTextFile.WriteErrorLog("Probe Running Machine Name: " + Environment.MachineName);
                                    WriteTextFile.WriteErrorLog("Probe Running User Name: " + Environment.UserName);
                                    WriteTextFile.WriteErrorLog("Probe Running OS Version: " + Environment.OSVersion);
                                    WriteTextFile.WriteErrorLog("====================Probe Running Computer Information Ended=====================");

                                    WriteTextFile.WriteErrorLog("===================================================================================");
                                    WriteTextFile.WriteErrorLog("");
                                    WriteTextFile.WriteErrorLog("===================================================================================");

                                    #endregion ProbeRunningMachine

                                    #region Devices, Users and Computer Lists Objects
                                    bool valid = false;
                                    try
                                    {
                                        PrincipalContext context = new PrincipalContext(ContextType.Domain, domainNameComplete);
                                        WriteTextFile.WriteErrorLog("Context is valid");
                                        valid = context.ValidateCredentials(_prb_settingObj.DomainAdminUser, _prb_settingObj.Password);
                                        WriteTextFile.WriteErrorLog("Credentials are valid");

                                    }
                                    catch (Exception e)
                                    {
                                        WriteTextFile.WriteErrorLog("Exception:" + e.Message + "; " + e.StackTrace);
                                    }
                                    //#region lalaland
                                    if (valid)
                                    {
                                        var oDeviceInfoList = ADLDAP.DeviceInfoList(domainNameComplete);

                                        var UsersList = ADLDAP.FindUsersInformation(domainNameComplete, _prb_settingObj.DomainAdminUser, _prb_settingObj.Password);

                                        var ComputersList = ADLDAP.FindComputersList(domainNameComplete);

                                        #endregion

                                        #region Insert Network Discovery Domian Users Into DataBase 
                                        if (UsersList.Count > 0)
                                        {
                                            oDomainUsersInfoResponse = oDomainUsersInfoAccess.UsersList_InsertData(UsersList, ScheduleId); // By Umar
                                        }
                                        #endregion  InsertNetworkDiscoveryDomianUsersIntoDataBase 

                                        // Update status for probing
                                        // Update Probe Schedule Status when probe is completed
                                        oPrb_ScheduleRequestStart.Prb_Schedule.ScheduleId = ScheduleId; //  Probe Running ScheduleId From Database
                                                                                                        // Start 1, Running 2, Completed 3, Stop 4, Exception 5... Status 3 is used for Complete Running Probe
                                        oPrb_ScheduleRequestStart.Prb_Schedule.StatusId = 2;
                                        oPrb_ScheduleRequestStart.Prb_Schedule.EndDateTime = DateTime.Now;
                                        oPrb_ScheduleDataAccess.ProcessScheduleDataUpdate(oPrb_ScheduleRequestStart);

                                        string[] domainNameSmall = null;
                                        try
                                        {
                                            domainNameSmall = domainNameComplete.Split('.');
                                        }
                                        catch (Exception ex)
                                        {
                                            WriteTextFile.WriteErrorLog("====================Domian Name Exception Start=====================");
                                            WriteTextFile.WriteErrorLog("Domain Name: " + domainNameComplete);
                                            WriteTextFile.WriteErrorLog(ex.Message);
                                            WriteTextFile.WriteErrorLog("====================Domian Name Exception End=====================");

                                        }


                                        Console.WriteLine("Service Network Discovery");

                                        #region NetworkDiscovery


                                        // Use Your work Group WinNT://&&&&(Work Group Name)

                                        DirectoryEntry domainEntry = new DirectoryEntry("WinNT://" + domainNameSmall[0].Trim());
                                        WriteTextFile.WriteErrorLog("====================Hardware Master information Started=====================");
                                        domainEntry.Children.SchemaFilter.Add("computer");


                                        // To Get all the System names And Display with the Ip Address
                                        foreach (DirectoryEntry machine in domainEntry.Children)
                                        {
                                            if (machine.SchemaClassName == "User")
                                            {
                                                Console.WriteLine(machine.Name + Environment.NewLine);
                                            }

                                            #region Getting Machine Names and Ip Address
                                            IPHostEntry tempaddr;
                                            try
                                            {
#pragma warning disable 618
                                                tempaddr = Dns.GetHostByName(machine.Name);
#pragma warning restore 618
                                            }
                                            catch (Exception)
                                            {
                                                continue;
                                            }
                                            string[] ipaddr = HwMasterInformation.GetHwMasterInfo(tempaddr);

                                            string IpSeries = "";
                                            try
                                            {
                                                IpSeries = ConfigurationManager.AppSettings["ProbeIpSeriesNotInclude"];
                                                if (!string.IsNullOrEmpty(IpSeries))
                                                {
                                                    if (ipaddr[0].Substring(0, 3).ToString() == IpSeries)
                                                    {
                                                        continue;
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                            #endregion

                                            #region Populate Hardware Master DTO
                                            // Checks Adding List And Scanning List and Save Into HW_Master Info
                                            oHw_MasterInfoDTO = new Hw_MasterInfoDTO();
                                            oHw_MasterInfoDTO.SiteId = oSiteDTO.SiteId;
                                            oHw_MasterInfoDTO.ScheduleId = ScheduleId;
                                            oHw_MasterInfoDTO.HwName = machine.Name;
                                            oHw_MasterInfoDTO.HwTypeId = 1;  // HwTypeId = 1 For Machines 
                                            oHw_MasterInfoDTO.IPAddress = ipaddr[0];
                                            oHw_MasterInfoDTO.MacAddress = ipaddr[1];
                                            #endregion

                                            #region Discover Operating systems
                                            var devStatus = false;
                                            if (oDeviceInfoList != null && oDeviceInfoList.Count > 0)
                                            {

                                                foreach (var devInfo in oDeviceInfoList)
                                                {
                                                    if (devInfo.ComName.ToUpper() == machine.Name.ToUpper())
                                                    {
                                                        // Check for Probe Settings for Access Devices
                                                        if (devInfo.DevCategory == "Windows" && _Prb_SettingDTO.WindowsAccess == true || devInfo.DevCategory == "IOS" && _Prb_SettingDTO.MacAccess == true || devInfo.DevCategory == "Linux" && _Prb_SettingDTO.LinuxAccess == true || devInfo.DevCategory == "Printers" && _Prb_SettingDTO.PrintersAccess == true || _Prb_SettingDTO.OthersAccess == true)
                                                        {
                                                            // Network Discover Info
                                                            DevicesCount++;
                                                            WriteTextFile.WriteErrorLog(DevicesCount + ") Device Type: " + devInfo.DevType + " | " + "Category: " + devInfo.DevCategory + " | Machine Type: " + devInfo.ComType + " | Machine Name: " + machine.Name + " | IP: " + ipaddr[0] + " | MAC: " + ipaddr[1] + " | OS: " + devInfo.ComOS);
                                                            Console.WriteLine(DevicesCount + ") Device Type: " + devInfo.DevType + " | " + "Category: " + devInfo.DevCategory + " | Machine Type: " + devInfo.ComType + " | Machine Name: " + machine.Name + " | IP: " + ipaddr[0] + " | MAC: " + ipaddr[1] + " | OS: " + devInfo.ComOS);

                                                            oHw_MasterInfoDTO.DevType = devInfo.DevType;
                                                            oHw_MasterInfoDTO.DevCategory = devInfo.DevCategory;
                                                            oHw_MasterInfoDTO.ComType = devInfo.ComType;
                                                            oHw_MasterInfoDTO.Description = "Scaning Types: HW Check is: " + _Prb_SettingDTO.HardwareDetail + " / Sw Check is: " + _Prb_SettingDTO.SoftwareDetail;

                                                            LstHw_MasterInfoDTO.Add(oHw_MasterInfoDTO);

                                                            // Insert DataToDatabase

                                                            devInfo.IP = ipaddr[0];
                                                            // Log as OS Category List
                                                            if (devInfo.DevCategory == "Windows")
                                                            {
                                                                if (_prb_settingObj.WindowsAccess == true && _Prb_SettingDTO.HardwareDetail == true && _Prb_SettingDTO.SoftwareDetail == true)
                                                                {
                                                                    ipListWindowsObj = new Hw_MasterInfo();
                                                                    ipListWindowsObj.IPAddress = ipaddr[0];
                                                                    ipListWindowsList.Add(ipListWindowsObj);
                                                                    ipListWindows.Add(ipaddr[0]);
                                                                }
                                                            }
                                                            else if (devInfo.DevCategory == "Linux")
                                                            {
                                                                if (_prb_settingObj.LinuxAccess == true && _Prb_SettingDTO.HardwareDetail == true && _Prb_SettingDTO.SoftwareDetail == true)
                                                                {
                                                                    ipListLinux.Add(ipaddr[0]);
                                                                }
                                                            }
                                                            else if (devInfo.DevCategory == "IOS")
                                                            {
                                                                if (_prb_settingObj.MacAccess == true && _Prb_SettingDTO.HardwareDetail == true && _Prb_SettingDTO.SoftwareDetail == true)
                                                                {
                                                                    ipListMAC.Add(ipaddr[0]);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (_prb_settingObj.OthersAccess == true)
                                                                {
                                                                    ipListOthers.Add(ipaddr[0]);
                                                                }
                                                            }

                                                            devStatus = true;
                                                            break;
                                                        }   // Check for Probe Settings 
                                                        if (!devStatus)
                                                        {
                                                            ipListWindowsObj = new Hw_MasterInfo();
                                                            ipListWindowsObj.IPAddress = ipaddr[0];
                                                            ipListWindowsList.Add(ipListWindowsObj);
                                                            ipListWindows.Add(ipaddr[0]);

                                                            WriteTextFile.WriteErrorLog("Machine Type:Unknown Type" + " | Machine Name:" + machine.Name + " | IP:" + ipaddr[0] + " | MAC:" + ipaddr[1]);
                                                            Console.WriteLine("Machine Type:Unknown Type" + " | Machine Name:" + machine.Name + " | IP:" + ipaddr[0] + " | MAC:" + ipaddr[1]);
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                WriteTextFile.WriteErrorLog("====================Hardware Machines Master information list not found=====================");
                                                //   WriteTextFile.WriteErrorLog("Machine Type:Unknown Type" + " | Machine Name:" + machine.Name + " | IP:" + ipaddr[0] + " | MAC:" + ipaddr[1]);
                                            }
                                            #endregion

                                        }

                                        Console.WriteLine("Service Network Discovery Machines End");
                                        WriteTextFile.WriteErrorLog("====================Hardware Master information Ended=====================");
                                        WriteTextFile.WriteErrorLog("===================================================================================");
                                        WriteTextFile.WriteErrorLog("");
                                        WriteTextFile.WriteErrorLog("===================================================================================");

                                        #endregion NetworkDiscovery

                                        // Update Probe Schedule Status when probe is completed
                                        oPrb_ScheduleRequestStart.Prb_Schedule.ScheduleId = ScheduleId; //  Probe Running ScheduleId From Database
                                        oPrb_ScheduleRequestStart.Prb_Schedule.StatusId = 2; // // Start 1, Running 2, Completed 3, Stop 4, Exception 5... Status 3 is used for Complete Running Probe
                                        oPrb_ScheduleRequestStart.Prb_Schedule.EndDateTime = DateTime.Now;
                                        oPrb_ScheduleDataAccess.ProcessScheduleDataUpdate(oPrb_ScheduleRequestStart);


                                        #region NetworkDiscoveryTemp

                                        /// Linux
                                        string machinelinux1 = "192.168.1.8";
                                        ipListLinux.Add(machinelinux1);
                                        // Checks Adding List And Scanning List and Save Into HW_Master Info
                                        oHw_MasterInfoDTO = new Hw_MasterInfoDTO();
                                        oHw_MasterInfoDTO.SiteId = oSiteDTO.SiteId;
                                        oHw_MasterInfoDTO.ScheduleId = ScheduleId;
                                        oHw_MasterInfoDTO.DevType = "Machines";
                                        oHw_MasterInfoDTO.DevCategory = "Linux";
                                        oHw_MasterInfoDTO.ComType = "Linux Client";
                                        oHw_MasterInfoDTO.HwName = "wordpress1";
                                        oHw_MasterInfoDTO.HwTypeId = 2;  // HwTypeId = 2 For Linux 
                                        oHw_MasterInfoDTO.IPAddress = machinelinux1;
                                        oHw_MasterInfoDTO.MacAddress = "66-37-E6-6D-3C-45";
                                        // Check for Probe Settings for Access Devices
                                        oHw_MasterInfoDTO.Description = "TestingLinux info delete from here";
                                        LstHw_MasterInfoDTO.Add(oHw_MasterInfoDTO);


                                        /// Linux
                                        string machinelinux2 = "192.168.1.9";
                                        ipListLinux.Add(machinelinux2);
                                        // Checks Adding List And Scanning List and Save Into HW_Master Info
                                        oHw_MasterInfoDTO = new Hw_MasterInfoDTO();
                                        oHw_MasterInfoDTO.SiteId = oSiteDTO.SiteId;
                                        oHw_MasterInfoDTO.ScheduleId = ScheduleId;
                                        oHw_MasterInfoDTO.DevType = "Machines";
                                        oHw_MasterInfoDTO.DevCategory = "Linux";
                                        oHw_MasterInfoDTO.ComType = "Linux Client";
                                        oHw_MasterInfoDTO.HwName = "wordpress2";
                                        oHw_MasterInfoDTO.HwTypeId = 2;  // HwTypeId = 2 For Linux 
                                        oHw_MasterInfoDTO.IPAddress = machinelinux2;
                                        oHw_MasterInfoDTO.MacAddress = "85-47-25-6D-3C-87";

                                        // Check for Probe Settings for Access Devices
                                        oHw_MasterInfoDTO.Description = "TestingLinux info delete from here";
                                        LstHw_MasterInfoDTO.Add(oHw_MasterInfoDTO);

                                        /// Linux
                                        string machinelinux3 = "192.168.1.10";
                                        ipListLinux.Add(machinelinux3);
                                        // Checks Adding List And Scanning List and Save Into HW_Master Info
                                        oHw_MasterInfoDTO = new Hw_MasterInfoDTO();
                                        oHw_MasterInfoDTO.SiteId = oSiteDTO.SiteId;
                                        oHw_MasterInfoDTO.ScheduleId = ScheduleId;
                                        oHw_MasterInfoDTO.DevType = "Machines";
                                        oHw_MasterInfoDTO.DevCategory = "Linux";
                                        oHw_MasterInfoDTO.ComType = "Linux Client";
                                        oHw_MasterInfoDTO.HwName = "wordpress3.zpslab.local";
                                        oHw_MasterInfoDTO.HwTypeId = 2;  // HwTypeId = 2 For Linux 
                                        oHw_MasterInfoDTO.IPAddress = machinelinux3;
                                        oHw_MasterInfoDTO.MacAddress = "D5-47-25-6D-3C-82";

                                        // Check for Probe Settings for Access Devices
                                        oHw_MasterInfoDTO.Description = "TestingLinux info delete from here";
                                        LstHw_MasterInfoDTO.Add(oHw_MasterInfoDTO);


                                        string mac = "192.168.1.11";
                                        ipListMAC.Add(mac);
                                        // Checks Adding List And Scanning List and Save Into HW_Master Info
                                        oHw_MasterInfoDTO = new Hw_MasterInfoDTO();
                                        oHw_MasterInfoDTO.SiteId = oSiteDTO.SiteId;
                                        oHw_MasterInfoDTO.ScheduleId = ScheduleId;
                                        oHw_MasterInfoDTO.DevType = "Machines";
                                        oHw_MasterInfoDTO.DevCategory = "IOS";
                                        oHw_MasterInfoDTO.ComType = "MAC Client";
                                        oHw_MasterInfoDTO.HwName = "THEMACS-IMAC";
                                        oHw_MasterInfoDTO.HwTypeId = 3;  // HwTypeId = 2 For Linux 
                                        oHw_MasterInfoDTO.IPAddress = mac;
                                        oHw_MasterInfoDTO.MacAddress = "56-1A-A0-B4-A4-58";

                                        // Check for Probe Settings for Access Devices
                                        oHw_MasterInfoDTO.Description = "Mac Mini by menual info here";
                                        LstHw_MasterInfoDTO.Add(oHw_MasterInfoDTO);

                                        string mac2 = "192.168.1.19";
                                        ipListMAC.Add(mac2);
                                        // Checks Adding List And Scanning List and Save Into HW_Master Info
                                        oHw_MasterInfoDTO = new Hw_MasterInfoDTO();
                                        oHw_MasterInfoDTO.SiteId = oSiteDTO.SiteId;
                                        oHw_MasterInfoDTO.ScheduleId = ScheduleId;
                                        oHw_MasterInfoDTO.DevType = "Machines";
                                        oHw_MasterInfoDTO.DevCategory = "IOS";
                                        oHw_MasterInfoDTO.ComType = "MAC Client";
                                        oHw_MasterInfoDTO.HwName = "MACMINI-9EB27C";
                                        oHw_MasterInfoDTO.HwTypeId = 3;  // HwTypeId = 2 For Linux 
                                        oHw_MasterInfoDTO.IPAddress = mac2;
                                        oHw_MasterInfoDTO.MacAddress = "C9-1A-A0-D4-A4-58";

                                        // Check for Probe Settings for Access Devices
                                        oHw_MasterInfoDTO.Description = "Mac Mini by menual info here";
                                        LstHw_MasterInfoDTO.Add(oHw_MasterInfoDTO);


                                        #endregion NetworkDiscoveryTemp


                                        /// Update status for probing
                                        ///   // Update Probe Schedule Status when probe is completed
                                        oPrb_ScheduleRequestStart.Prb_Schedule.ScheduleId = ScheduleId; //  Probe Running ScheduleId From Database
                                        oPrb_ScheduleRequestStart.Prb_Schedule.StatusId = 2; // // Start 1, Running 2, Completed 3, Stop 4, Exception 5... Status 3 is used for Complete Running Probe
                                        oPrb_ScheduleRequestStart.Prb_Schedule.EndDateTime = DateTime.Now;
                                        oPrb_ScheduleDataAccess.ProcessScheduleDataUpdate(oPrb_ScheduleRequestStart);

                                        #region Devices Discovery
                                        // Chk For Printer Access Setting
                                        if (_prb_settingObj.PrintersAccess == true)
                                        {
                                            // Devices Information Start
                                            WriteTextFile.WriteErrorLog("====================Printers information Started=====================");
                                            var oPrinterInfoList = ADLDAP.PrintersInfoList(domainNameComplete);
                                            if (oPrinterInfoList != null && oPrinterInfoList.Count > 0)
                                            {
                                                foreach (DeviceInfo oDeviceInfo in oPrinterInfoList)
                                                {
                                                    // Checks Adding List And Scanning List and Save Into HW_Master Info
                                                    oHw_MasterInfoDTO = new Hw_MasterInfoDTO();
                                                    oHw_MasterInfoDTO.SiteId = oSiteDTO.SiteId;
                                                    oHw_MasterInfoDTO.ScheduleId = ScheduleId;
                                                    oHw_MasterInfoDTO.HwName = oDeviceInfo.ComName;
                                                    oHw_MasterInfoDTO.HwTypeId = 4;  // HwTypeId = 4 For Printers 
                                                    oHw_MasterInfoDTO.IPAddress = oDeviceInfo.IP;
                                                    oHw_MasterInfoDTO.MacAddress = "";

                                                    // Check for Probe Settings for Access Devices
                                                    if (oDeviceInfo.DevCategory == "Printers" && _Prb_SettingDTO.PrintersAccess == true || _Prb_SettingDTO.OthersAccess == true)
                                                    {
                                                        DevicesCount++;
                                                        WriteTextFile.WriteErrorLog(DevicesCount + ") Device Type: " + oDeviceInfo.DevType + " | " + "Category: " + oDeviceInfo.DevCategory + " | Machine Type: " + oDeviceInfo.ComType + " | Machine Name: " + oDeviceInfo.ComName + " | IP: " + oDeviceInfo.IP + " | MAC:" + "PP:00");
                                                        ipListPrinters.Add(oDeviceInfo.IP);

                                                        oHw_MasterInfoDTO.DevType = oDeviceInfo.DevType;
                                                        oHw_MasterInfoDTO.DevCategory = oDeviceInfo.DevCategory;
                                                        oHw_MasterInfoDTO.ComType = oDeviceInfo.ComType;
                                                        oHw_MasterInfoDTO.Description = "This is Master information for the Network Discovery and Marked as: " + oDeviceInfo.DevCategory + " With Check: True and HW Check is: " + _Prb_SettingDTO.HardwareDetail + " / Sw Check is: " + _Prb_SettingDTO.SoftwareDetail;

                                                        LstHw_MasterInfoDTO.Add(oHw_MasterInfoDTO);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                WriteTextFile.WriteErrorLog("====================Printers List Not Found=====================");
                                            }
                                            WriteTextFile.WriteErrorLog("====================Printers information Ended=====================");
                                            // Devices Information END
                                        }



                                        #endregion DevicesDiscovery

                                        #region Insert Network Discovery Into DataBase 
                                        // Should Be MasterInfo Add here
                                        // Insert Network Discovery Data into DataPase
                                        if (LstHw_MasterInfoDTO.Count > 0)
                                        {
                                            oHw_MasterInfoResponse = oHw_MasterInfoDataAccess.Hw_MasterInfo_InsertData(LstHw_MasterInfoDTO); // By Umar
                                        }
                                        #endregion  Insert Network Discovery Into DataBase 

                                        /// Update status for probing
                                        ///   // Update Probe Schedule Status when probe is completed

                                        oPrb_ScheduleRequestStart.Prb_Schedule.ScheduleId = ScheduleId; //  Probe Running ScheduleId From Database
                                        oPrb_ScheduleRequestStart.Prb_Schedule.StatusId = 2; // // Start 1, Running 2, Completed 3, Stop 4, Exception 5... Status 3 is used for Complete Running Probe
                                        oPrb_ScheduleRequestStart.Prb_Schedule.EndDateTime = DateTime.Now;
                                        oPrb_ScheduleDataAccess.ProcessScheduleDataUpdate(oPrb_ScheduleRequestStart);

                                        #region PrintersDetail
                                        // Chk For Printer Access Setting for Get HArdware and Software Detail
                                        if (_prb_settingObj.PrintersAccess == true && _prb_settingObj.HardwareDetail == true && _prb_settingObj.SoftwareDetail == true)
                                        {
                                            /// Printers Detailed Info Start
                                            if (ipListPrinters != null && ipListPrinters.Count() > 0)
                                            {
                                                WriteTextFile.WriteErrorLog("====================Printers Detailed information Started=====================");
                                                WriteTextFile.WriteErrorLog("====================Total No of Printers " + ipListPrinters.Count + " Founded on Network=====================");
                                                foreach (var deviceIp in ipListPrinters)
                                                {
                                                    if (!string.IsNullOrEmpty(deviceIp))
                                                    {
                                                        WriteTextFile.WriteErrorLog("====================Device Detailed info Printer IP: " + deviceIp + " Started=====================");
                                                        // Printer detail info Get and Parsed into Database
                                                        oHw_DetailInfoDTO = PrintersInformation.getprinterDetail(deviceIp);
                                                        /// Insert Data to Database
                                                        if (oHw_DetailInfoDTO != null)
                                                        {
                                                            oHw_DetailInfoDTO.isConnected = true;
                                                            oHw_DetailInfoDTO.IPAddress = deviceIp;
                                                            LstHw_DetailInfoDTO.Add(oHw_DetailInfoDTO);  /// Add Device Info in List LstHw_DetailInfoDTO
                                                            oHw_DetailInfoDataAccess.Hw_DetailInfo_InsertData(oHw_DetailInfoDTO);  /// Insert Device Detail Data into Database
                                                        }

                                                        WriteTextFile.WriteErrorLog("====================Device Detailed info Printer IP: " + deviceIp + " End=====================");
                                                    }
                                                }
                                                WriteTextFile.WriteErrorLog("====================Printers Detailed information Ended=====================");
                                            }
                                            /// Printers Detailed Info End
                                            /// 

                                            WriteTextFile.WriteErrorLog("===================================================================================");
                                            WriteTextFile.WriteErrorLog("");
                                            WriteTextFile.WriteErrorLog("===================================================================================");
                                        }
                                        #endregion PrintersDetail



                                        #region LinuxDetailInfo

                                        /// Linux Start
                                        /// // Chk For Linux Access Setting for Get HArdware and Software Detail
                                        List<Thread> threads = new List<Thread>();
                                        if (_prb_settingObj.LinuxAccess == true && _prb_settingObj.HardwareDetail == true && _prb_settingObj.SoftwareDetail == true)
                                        {

                                            var DomainAdminUserName2 = "root";
                                            var DomainAdminPassword2 = "Zones12345!";

                                            WriteTextFile.WriteErrorLog("====================Linux information Started=====================");
                                            if (ipListLinux != null && ipListLinux.Count() > 0)
                                            {
                                                if (ipListLinux.Count > 1)
                                                {
                                                    WriteTextFile.WriteErrorLog("Implementing threading Linux");
                                                    int noOfThreads = Convert.ToInt32(ConfigurationManager.AppSettings["NoOfThreads"]);
                                                    int total = ipListLinux.Count;
                                                    if (total > noOfThreads)
                                                    {

                                                        int divfactor = total / noOfThreads;
                                                        int taken = 0;
                                                        for (int i = noOfThreads - 1; i >= 0; i--)
                                                        {
                                                            List<string> ipsForThreadLinux;
                                                            if (i == 0)
                                                                ipsForThreadLinux = ipListLinux.Skip(taken).Take(total - taken).ToList();
                                                            else
                                                                ipsForThreadLinux = ipListLinux.Skip(taken).Take(divfactor).ToList();
                                                            WriteTextFile.WriteErrorLog("Implementing threading Linux; thread count :" + i);
                                                            Thread t = new Thread(() => ReadDataLinuxMachines(ipListLinux, LstHw_DetailInfoDTO, oHw_DetailInfoDataAccess, DomainAdminUserName2, DomainAdminPassword2));
                                                            t.Name = "LaLa LAnd Linux" + i;
                                                            threads.Add(t);
                                                            t.Start();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        WriteTextFile.WriteErrorLog("Not Implementing threading Linux total count <2");
                                                        ReadDataLinuxMachines(ipListLinux, LstHw_DetailInfoDTO, oHw_DetailInfoDataAccess, DomainAdminUserName2, DomainAdminPassword2);
                                                    }
                                                }
                                                else
                                                    ReadDataLinuxMachines(ipListLinux, LstHw_DetailInfoDTO, oHw_DetailInfoDataAccess, DomainAdminUserName2, DomainAdminPassword2);
                                            }
                                            WriteTextFile.WriteErrorLog("====================Linux information Ended=====================");

                                            // Linux End
                                            // 
                                            WriteTextFile.WriteErrorLog("===================================================================================");
                                            WriteTextFile.WriteErrorLog("");
                                            WriteTextFile.WriteErrorLog("===================================================================================");
                                        }
                                        #endregion LinuxDetailInfo

                                        /// Update status for probing
                                        ///   // Update Probe Schedule Status when probe is completed
                                        oPrb_ScheduleRequestStart.Prb_Schedule.ScheduleId = ScheduleId; //  Probe Running ScheduleId From Database
                                        oPrb_ScheduleRequestStart.Prb_Schedule.StatusId = 2; // // Start 1, Running 2, Completed 3, Stop 4, Exception 5... Status 3 is used for Complete Running Probe
                                        oPrb_ScheduleRequestStart.Prb_Schedule.EndDateTime = DateTime.Now;
                                        oPrb_ScheduleDataAccess.ProcessScheduleDataUpdate(oPrb_ScheduleRequestStart);

                                        #region Mac Detail Information
                                        /// MAC Start
                                        /// /// // Chk For Linux Access Setting for Get HArdware and Software Detail
                                        if (_prb_settingObj.MacAccess == true && _prb_settingObj.HardwareDetail == true && _prb_settingObj.SoftwareDetail == true)
                                        {

                                            string DomainAdminUserNamePC = "gulzaman";
                                            string DomainAdminPasswordPC = "2525";

                                            DomainAdminUserNamePC = "TheMac";
                                            DomainAdminPasswordPC = "Zones234%";

                                            WriteTextFile.WriteErrorLog("====================MAC information Started=====================");
                                            if (ipListMAC != null && ipListMAC.Count() > 0)
                                            {
                                                if (ipListMAC.Count > 1)
                                                {
                                                    WriteTextFile.WriteErrorLog("Implementing threading MAC");
                                                    int noOfThreads = Convert.ToInt32(ConfigurationManager.AppSettings["NoOfThreads"]);
                                                    int total = ipListMAC.Count;
                                                    if (total > noOfThreads)
                                                    {
                                                        int divfactor = total / noOfThreads;
                                                        int taken = 0;
                                                        for (int i = noOfThreads - 1; i >= 0; i--)
                                                        {
                                                            List<string> ipsForThreadMAC;
                                                            if (i == 0)
                                                                ipsForThreadMAC = ipListMAC.Skip(taken).Take(total - taken).ToList();
                                                            else
                                                                ipsForThreadMAC = ipListMAC.Skip(taken).Take(divfactor).ToList();
                                                            WriteTextFile.WriteErrorLog("Implementing threading MAC; thread count :" + i);
                                                            Thread t = new Thread(() => ReadMachinesDataMac(ipsForThreadMAC, LstHw_DetailInfoDTO, oHw_DetailInfoDataAccess, DomainAdminUserNamePC, DomainAdminPasswordPC));
                                                            t.Name = "LaLa LAnd MAC" + i;
                                                            threads.Add(t);
                                                            t.Start();
                                                        }
                                                    }
                                                    else
                                                        ReadMachinesDataMac(ipListMAC, LstHw_DetailInfoDTO, oHw_DetailInfoDataAccess, DomainAdminUserNamePC, DomainAdminPasswordPC);
                                                }
                                                else
                                                    ReadMachinesDataMac(ipListMAC, LstHw_DetailInfoDTO, oHw_DetailInfoDataAccess, DomainAdminUserNamePC, DomainAdminPasswordPC);
                                            }
                                            WriteTextFile.WriteErrorLog("====================MAC information Ended=====================");

                                            /// MAC END
                                            /// 
                                            WriteTextFile.WriteErrorLog("===================================================================================");
                                            WriteTextFile.WriteErrorLog("");
                                        }
                                        #endregion MacDetailInfo

                                        /// Update status for probing
                                        ///   // Update Probe Schedule Status when probe is completed
                                        oPrb_ScheduleRequestStart.Prb_Schedule.ScheduleId = ScheduleId; //  Probe Running ScheduleId From Database
                                        oPrb_ScheduleRequestStart.Prb_Schedule.StatusId = 2; // // Start 1, Running 2, Completed 3, Stop 4, Exception 5... Status 3 is used for Complete Running Probe
                                        oPrb_ScheduleRequestStart.Prb_Schedule.EndDateTime = DateTime.Now;
                                        oPrb_ScheduleDataAccess.ProcessScheduleDataUpdate(oPrb_ScheduleRequestStart);

                                        #region WindowsDetailInfo

                                        // TestAccount
                                        // string DomainAdminUserName = "mis";
                                        ////string DomainAdminPassword = "SeaPass3";
                                        //   string DomainAdminPassword = "SahPass90!90";
                                        string DomainAdminUserName = _prb_settingObj.DomainAdminUser;
                                        string DomainAdminPassword = _prb_settingObj.Password;

                                        //// Delete This Code when Complete Start
                                        //Hw_DetailInfoDTO FetchDataFromRemote = ReadRemoteRegistryusingWmi(machineWindows1, DomainAdminUserName, DomainAdminPassword);
                                        //if (FetchDataFromRemote != null)
                                        //{
                                        //    FetchDataFromRemote.IPAddress = machineWindows1;
                                        //    LstHw_DetailInfoDTO.Add(FetchDataFromRemote);  /// Add Machine Info in List LstHw_DetailInfoDTO
                                        //    oHw_DetailInfoDataAccess.Hw_DetailInfo_InsertData(FetchDataFromRemote);
                                        //}
                                        // Delete This Code when Complete  End


                                        ////// Zones Lab
                                        //string serverIpName = "172.16.1.160";
                                        //string serverName2 = "172.16.1.199";
                                        //string userName = "administrator";
                                        //string password = "zones12345";

                                        //  ipList.Add(serverName1);

                                        //// Chk For Windows Access Setting for Get HArdware and Software Detail

                                        ipListWindowsList = ipListWindowsList.OrderBy(x => x.IPAddress).ToList();
                                        ipListWindowsListRemainScaning = ipListWindowsList.OrderBy(x => x.IPAddress).ToList();

                                        if (_prb_settingObj.WindowsAccess == true && _prb_settingObj.HardwareDetail == true && _prb_settingObj.SoftwareDetail == true)
                                        {

                                            if (ipListWindows != null && ipListWindowsList.Count > 0)
                                            {
                                                if (ipListWindowsList.Count > 10)
                                                {
                                                    WriteTextFile.WriteErrorLog("Implementing threading");
                                                    int noOfThreads = Convert.ToInt32(ConfigurationManager.AppSettings["NoOfThreads"]);
                                                    int total = ipListWindowsList.Count;
                                                    if (total > noOfThreads)
                                                    {

                                                        int divfactor = total / noOfThreads;
                                                        int taken = 0;
                                                        for (int i = noOfThreads - 1; i >= 0; i--)
                                                        {
                                                            List<string> ipsForThread;
                                                            List<Hw_MasterInfo> MachinesListForThread;
                                                            if (i == 0)
                                                            {
                                                                ipsForThread = ipListWindows.Skip(taken).Take(total - taken).ToList();
                                                                MachinesListForThread = ipListWindowsList.Skip(taken).Take(total - taken).ToList();
                                                            }
                                                            else
                                                            {
                                                                ipsForThread = ipListWindows.Skip(taken).Take(divfactor).ToList();
                                                                MachinesListForThread = ipListWindowsList.Skip(taken).Take(divfactor).ToList();
                                                            }
                                                            WriteTextFile.WriteErrorLog("Implementing threading; thread count :" + i);
                                                            Thread t = new Thread(() => ReadDataWindowaMachines(MachinesListForThread, ipsForThread, LstHw_DetailInfoDTO, oHw_DetailInfoDataAccess, oDeviceInfoList, DomainAdminUserName, DomainAdminPassword));
                                                            t.Name = "LaLa LAnd" + i;
                                                            threads.Add(t);
                                                            t.Start();
                                                        }
                                                    }
                                                    else
                                                        ReadDataWindowaMachines(ipListWindowsList, ipListWindows, LstHw_DetailInfoDTO, oHw_DetailInfoDataAccess, oDeviceInfoList, DomainAdminUserName, DomainAdminPassword);
                                                }
                                                else
                                                    ReadDataWindowaMachines(ipListWindowsList, ipListWindows, LstHw_DetailInfoDTO, oHw_DetailInfoDataAccess, oDeviceInfoList, DomainAdminUserName, DomainAdminPassword);
                                            }
                                            if (threads != null)
                                            {
                                                foreach (Thread thread in threads)
                                                { thread.Join(); }
                                            }
                                        }
                                        var oPrb_ScheduleRequestEnd = new Prb_ScheduleRequest() { Prb_Schedule = new Prb_ScheduleDTO() };
                                        oPrb_ScheduleRequestEnd.Prb_Schedule.ScheduleId = ScheduleId;
                                        oPrb_ScheduleRequestEnd.Prb_Schedule.SiteId = oSiteDTO.SiteId;
                                        oPrb_ScheduleRequestEnd.Prb_Schedule.StartDateTime = DateTime.Now;
                                        oPrb_ScheduleRequestEnd.Prb_Schedule.EndDateTime = DateTime.Now;
                                        oPrb_ScheduleRequestEnd.Prb_Schedule.Description = oSiteDTO.Description;
                                        oPrb_ScheduleRequestEnd.Prb_Schedule.StatusId = 3;
                                        oPrb_ScheduleRequestEnd.Prb_Schedule.OperationId = 2;
                                        ScheduleId = oPrb_ScheduleDataAccess.ProcessScheduleDataUpdate(oPrb_ScheduleRequestEnd);
                                    }

                                    else
                                    {
                                        WriteTextFile.WriteErrorLog("================================AD Name/Credentials are invalid=====================");
                                        WriteTextFile.WriteErrorLog("===================================================================================");
                                        Prb_ADConnectionFailureDTO obj = new Prb_ADConnectionFailureDTO();
                                        obj.Description = "AD Name/Credentials are invalid";
                                        obj.SettingId = _prb_settingObj.SettingId;
                                        _Prb_SettingsDAL.AddProbeFailure(obj);
                                        _Prb_SettingsDAL.UpdateIsRead(_Prb_SettingDTO.SettingId);
                                    }
                                }
                            }
                            #endregion WindowsDetailInfo



                            WriteTextFile.WriteErrorLog("==================================Prb Service End==================================");
                            WriteTextFile.WriteErrorLog("===================================================================================");

                            // Update Probe Schedule Status when probe is completed
                            oPrb_ScheduleRequestStart.Prb_Schedule.ScheduleId = ScheduleId; //  Probe Running ScheduleId From Database
                            oPrb_ScheduleRequestStart.Prb_Schedule.StatusId = 3; // // Start 1, Running 2, Completed 3, Stop 4, Exception 5... Status 3 is used for Complete Running Probe
                            oPrb_ScheduleRequestStart.Prb_Schedule.EndDateTime = DateTime.Now;
                            oPrb_ScheduleDataAccess.ProcessScheduleDataUpdate(oPrb_ScheduleRequestStart);
                            Console.WriteLine("==================================Prb Service End==================================");
                            _Prb_SettingsDAL.UpdateIsRead(_Prb_SettingDTO.SettingId);
                        }
                        else
                        {
                            WriteTextFile.WriteErrorLog("================================No New Probe Setting Found=====================");
                            WriteTextFile.WriteErrorLog("===================================================================================");
                        }

                        System.Threading.Thread.Sleep(1000 * 30);
                    }
                    catch (Exception ex)
                    {
                        // Update Probe Schedule Status when probe Exception
                        oPrb_ScheduleRequestStart.Prb_Schedule.ScheduleId = ScheduleId; //  Probe Running ScheduleId From Database
                        oPrb_ScheduleRequestStart.Prb_Schedule.StatusId = 5; // // Start 1, Running 2, Completed 3, Stop 4, Exception 5... 5 is used\for Exception Running Probe
                        oPrb_ScheduleRequestStart.Prb_Schedule.Description = "Probe Running Exception: " + ex.Message;
                        oPrb_ScheduleRequestStart.Prb_Schedule.EndDateTime = DateTime.Now;
                        oPrb_ScheduleDataAccess.ProcessScheduleDataUpdate(oPrb_ScheduleRequestStart);
                        _Prb_SettingsDAL.UpdateIsRead(_Prb_SettingDTO.SettingId);
                        WriteTextFile.WriteErrorLog("PrbService ERROR:");
                        WriteTextFile.WriteErrorLog("==========================================");
                        WriteTextFile.WriteErrorLog(ex);
                        WriteTextFile.WriteErrorLog(ex.StackTrace);

                    }
                    #endregion
                }

            }
            catch (Exception)
            {

                throw;
            }


        }

        private static void ReadMachinesDataMac(List<string> ipListMAC, List<Hw_DetailInfoDTO> LstHw_DetailInfoDTO, Hw_DetailInfoDataAccess oHw_DetailInfoDataAccess, string DomainAdminUserNamePC, string DomainAdminPasswordPC)
        {
            if (ipListMAC != null && ipListMAC.Count() > 0)
            {
                WriteTextFile.WriteErrorLog("====================Total No of MAC Machines " + ipListMAC.Count + " Founded on Network=====================");
                //TODO: will be a seprate thread  
                foreach (var serverIpName in ipListMAC)
                {
                    WriteTextFile.WriteErrorLog("Thread ID" + Thread.CurrentThread.Name);
                    if (!string.IsNullOrEmpty(serverIpName))
                    {
                        WriteTextFile.WriteErrorLog("====================Hardware Detailed info MAC Machine IP: " + serverIpName + " Started=====================");

                        Hw_DetailInfoDTO FetchDataFromRemotePc = MacInformation.SSHCommandForMAC(serverIpName, DomainAdminUserNamePC, DomainAdminPasswordPC);
                        // Update Status if not connected
                        if (FetchDataFromRemotePc.isConnected == false && FetchDataFromRemotePc.HwDescription != null)
                        {
                            FetchDataFromRemotePc.IPAddress = serverIpName;
                            oHw_DetailInfoDataAccess.Hw_MasterInfo_UpdateIsConnectedStatus(FetchDataFromRemotePc);
                        }
                        /// Insert Data to Database
                        else if (FetchDataFromRemotePc != null)
                        {
                            FetchDataFromRemotePc.IPAddress = serverIpName;
                            LstHw_DetailInfoDTO.Add(FetchDataFromRemotePc);  /// Add Machine Info in List LstHw_DetailInfoDTO
                            oHw_DetailInfoDataAccess.Hw_DetailInfo_InsertData(FetchDataFromRemotePc); // Insert Machine Detail Data into Database
                        }
                        WriteTextFile.WriteErrorLog("====================Hardware Detailed info MAC Machine IP: " + serverIpName + " End=====================");
                    }
                }
            }
            else
            {
                WriteTextFile.WriteErrorLog("====================Hardware MAC Devices List Not Found=====================");
            }
        }

        private static void ReadDataLinuxMachines(List<string> ipListLinux, List<Hw_DetailInfoDTO> LstHw_DetailInfoDTO, Hw_DetailInfoDataAccess oHw_DetailInfoDataAccess, string DomainAdminUserName2, string DomainAdminPassword2)
        {
            if (ipListLinux != null && ipListLinux.Count() > 0)
            {
                WriteTextFile.WriteErrorLog("====================Total No of Linux Machines " + ipListLinux.Count + " Founded on Network=====================");
                //TODO: will be a seprate thread 
                foreach (var serverIpName in ipListLinux)
                {
                    WriteTextFile.WriteErrorLog("Thread ID" + Thread.CurrentThread.Name);
                    WriteTextFile.WriteErrorLog("====================Hardware Detailed info Linux Machine IP: " + serverIpName + " Started=====================");
                    Console.WriteLine("====================Hardware Detailed info Linux Machine IP: " + serverIpName + " Started=====================");

                    Hw_DetailInfoDTO FetchDataFromRemotePc = LinuxInformation.SSHCommandForLinux(serverIpName, DomainAdminUserName2, DomainAdminPassword2);
                    // Update Status if not connected
                    if (FetchDataFromRemotePc.isConnected == false && FetchDataFromRemotePc.HwDescription != null)
                    {
                        FetchDataFromRemotePc.IPAddress = serverIpName;
                        oHw_DetailInfoDataAccess.Hw_MasterInfo_UpdateIsConnectedStatus(FetchDataFromRemotePc);
                    }
                    /// Insert Data to Database
                    else if (FetchDataFromRemotePc != null)
                    {
                        FetchDataFromRemotePc.IPAddress = serverIpName;
                        LstHw_DetailInfoDTO.Add(FetchDataFromRemotePc);   /// Add Machine Info in List LstHw_DetailInfoDTO
                        oHw_DetailInfoDataAccess.Hw_DetailInfo_InsertData(FetchDataFromRemotePc); /// Insert Machine Detail Data into Database
                    }

                    WriteTextFile.WriteErrorLog("====================Hardware Detailed info Linux Machine IP: " + serverIpName + " End=====================");
                    Console.WriteLine("====================Hardware Detailed info Linux Machine IP: " + serverIpName + " End=====================");
                }
            }
            else
            {
                WriteTextFile.WriteErrorLog("====================Hardware Linux Devices List Not Found=====================");
            }
        }

        private static void ReadDataWindowaMachines(List<Hw_MasterInfo> ipListWindowsList, List<String> ipListWindows, List<Hw_DetailInfoDTO> LstHw_DetailInfoDTO, Hw_DetailInfoDataAccess oHw_DetailInfoDataAccess, List<DeviceInfo> oDeviceInfoList, string DomainAdminUserName, string DomainAdminPassword)
        {
            WriteTextFile.WriteErrorLog("====================Hardware Windows Master information Started=====================");
            if (ipListWindows != null && ipListWindowsList.Count() > 0)
            {
                WriteTextFile.WriteErrorLog("====================Total No of Windows Machines " + ipListWindows.Count + " Founded on Network=====================");
                foreach (var serverIpName in ipListWindowsList)
                {
                    WriteTextFile.WriteErrorLog("Thread ID" + Thread.CurrentThread.Name);
                    if (!string.IsNullOrEmpty(serverIpName.IPAddress))
                    {
                        DeviceInfo devInfo = oDeviceInfoList.Where(x => x.IP == serverIpName.IPAddress).FirstOrDefault();
                        WriteTextFile.WriteErrorLog("====================Hardware Detailed " + devInfo.ComType + " IP: " + serverIpName.IPAddress + " info Started=====================");
                        try
                        {
                            //if (devInfo.IP == "172.17.1.222")
                            //{
                            //    Console.WriteLine("Ip Found");
                            //}
                            //if (devInfo.IP == "172.17.1.113")
                            //{
                            //    Console.WriteLine("Ip Found");
                            //}
                            //if (devInfo.IP == "172.17.1.95")
                            //{
                            //    Console.WriteLine("Ip Found");
                            //}
                            //if (devInfo.IP == "192.168.1.253")
                            //{
                            //    Console.WriteLine("Ip Found");
                            //}
                            //if (devInfo.IP == "192.168.1.210")
                            //{
                            //    Console.WriteLine("Ip Found");
                            //}


                            Hw_DetailInfoDTO FetchDataFromRemotePc = ReadRemoteRegistryUsingWmi.ReadRemoteRegistryusingWmi(serverIpName.IPAddress, DomainAdminUserName, DomainAdminPassword);
                            // Update Status if not connected
                            if (FetchDataFromRemotePc.isConnected == false && FetchDataFromRemotePc.HwDescription != null)
                            {
                                FetchDataFromRemotePc.IPAddress = serverIpName.IPAddress;
                                oHw_DetailInfoDataAccess.Hw_MasterInfo_UpdateIsConnectedStatus(FetchDataFromRemotePc);
                            }
                            /// Insert Data to Database
                            else if (FetchDataFromRemotePc != null)
                            {
                                FetchDataFromRemotePc.IPAddress = serverIpName.IPAddress;
                                LstHw_DetailInfoDTO.Add(FetchDataFromRemotePc);  /// Add Machine Info in List LstHw_DetailInfoDTO
                                oHw_DetailInfoDataAccess.Hw_DetailInfo_InsertData(FetchDataFromRemotePc);  /// Insert Machine Detail Data into Database
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteTextFile.WriteErrorLog("==================== Missing info ===================== \n" + ex.Message);
                        }
                        WriteTextFile.WriteErrorLog("====================Hardware Detailed " + ipListWindows.Count + " IP: " + serverIpName.IPAddress + " info End=====================");

                        //      ipListWindowsListRemainScaning.Remove(serverIpName);
                    }
                }
            }
            else
            {
                WriteTextFile.WriteErrorLog("====================Hardware Windows Machines List Not Found=====================");
            }

            //if (ipListWindowsListRemainScaning.Count > 0)
            //{
            //    WriteTextFile.WriteErrorLog("====================Hardware Windows Detailed Machines List Not Scanned:- =====================");
            //    foreach (var serverIpName in ipListWindowsListRemainScaning)
            //    {
            //        WriteTextFile.WriteErrorLog("--- Machine Name: " + serverIpName.HwName + " Machine Ip Address: " + serverIpName.IPAddress + " ---");
            //    }
            //    WriteTextFile.WriteErrorLog("====================Hardware Windows Detailed Machines List End:- =====================");
            //}

            WriteTextFile.WriteErrorLog("====================Hardware Windows Master information Ended=====================");


            //
            WriteTextFile.WriteErrorLog("===================================================================================");
            WriteTextFile.WriteErrorLog("");
            WriteTextFile.WriteErrorLog("===================================================================================");
        }

        public void UpdateProbeSettingStatus()
        {
            // Probe Setting Chk for IsActive To False 
            // Update Probe Schedule Status when probe Stop
            Prb_ScheduleRequest oPrb_ScheduleRequestStart;
            oPrb_ScheduleRequestStart = new Prb_ScheduleRequest() { Prb_Schedule = new Prb_ScheduleDTO() };

            oPrb_ScheduleRequestStart.Prb_Schedule.ScheduleId = ScheduleId; //  Probe Running ScheduleId From Database
            oPrb_ScheduleRequestStart.Prb_Schedule.StatusId = 4; // Start 1, Running 2, Completed 3, Stop 4, Exception 5... 4 is used for  Probe Running Stop
            oPrb_ScheduleRequestStart.Prb_Schedule.Description = "Probe Running Stop";
            oPrb_ScheduleRequestStart.Prb_Schedule.EndDateTime = DateTime.Now;
            _Prb_SettingsDAL.ProbeSettingUpdateStatusAfterComplete(_prb_settingObj);

            WriteTextFile.WriteErrorLog("=================================Prb Service stopped==================================");
        }
        // Menual Methods For Read App Setting From XML ''
        Prb_SettingDTO _Prb_SettingDTO = new Prb_SettingDTO();
        private void XmlFileReadProbeSetting()
        {
            var dir = @"C:\ZonesADSQlite\";  // folder location
                                             //if (!Directory.Exists(dir))  // if it doesn't exist, create
                                             //{
                                             //    Directory.CreateDirectory(dir);
                                             //}

            XmlReader reader = XmlReader.Create(dir + "ProbeSetting.xml");

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Probe")
                {
                    Console.WriteLine("SiteId = " + reader.GetAttribute(0));

                    // Assign To Probe Setting
                    _Prb_SettingDTO.SiteId = int.Parse(reader.GetAttribute(0));

                    Console.WriteLine("DomainType = " + reader.GetAttribute(1));
                    _Prb_SettingDTO.DomainType = reader.GetAttribute(0);

                    while (reader.NodeType != XmlNodeType.EndElement)
                    {
                        reader.Read();
                        if (reader.Name == "SettingId")
                        {
                            while (reader.NodeType != XmlNodeType.EndElement)
                            {
                                reader.Read();
                                if (reader.NodeType == XmlNodeType.Text)
                                {
                                    Console.WriteLine("SettingId = ", Double.Parse(reader.Value));
                                    _Prb_SettingDTO.SettingId = int.Parse(reader.Value);
                                }
                            }
                            reader.Read();

                        } //end if

                        if (reader.Name == "DomainName")
                        {
                            while (reader.NodeType != XmlNodeType.EndElement)
                            {
                                reader.Read();
                                if (reader.NodeType == XmlNodeType.Text)
                                {
                                    Console.WriteLine("DomainName = ", reader.Value);
                                    // _Prb_SettingDTO.DomainName = reader.Value;
                                }
                            }
                            reader.Read();

                            //} //end if

                            if (reader.Name == "OtherDetails")
                            {
                                while (reader.NodeType != XmlNodeType.EndElement)
                                {
                                    reader.Read();
                                    if (reader.Name == "ABC")
                                    {
                                        while (reader.NodeType != XmlNodeType.EndElement)
                                        {
                                            reader.Read();
                                            if (reader.NodeType == XmlNodeType.Text)
                                            {
                                                Console.WriteLine("ABC Value= " + reader.Value);
                                            }
                                        }
                                        reader.Read();
                                    } //end if

                                    if (reader.Name == "XYZ")
                                    {
                                        while (reader.NodeType != XmlNodeType.EndElement)
                                        {
                                            reader.Read();
                                            if (reader.NodeType == XmlNodeType.Text)
                                            {
                                                Console.WriteLine("XYZ Value = " + reader.Value);
                                            }
                                        }

                                    } //end if
                                }
                            } //end if
                        } //end while
                    } //end if

                } //end while
            }
        }




    }
}
