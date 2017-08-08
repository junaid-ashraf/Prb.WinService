using Prb.DTO;
using Prb.DTO.Request;
using Prb.DTO.Response;
//using Prb.DTO.Request;
//using Prb.DTO.Response;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Prb.SqliteDAL
{
    public class Hw_DetailInfoDataAccess
    {
        ProbeDBEntities _Db = DBHelper.Instance._Db;
        public Hw_DetailInfoResponse Hw_DetailInfo_InsertData(Hw_DetailInfoDTO datarow)
        {
            Hw_DetailInfoResponse response = new Hw_DetailInfoResponse();
            try
            {
                //foreach (Hw_DetailInfoDTO datarow in LstHw_DetailInfoDTO)
                //{
                Hw_MasterInfo _Hw_MasterInfo = _Db.Hw_MasterInfo.Where(x => x.IPAddress == datarow.IPAddress && x.IsDeleted == false).OrderByDescending(x => x.HwMasterInfoId).FirstOrDefault();

                if (_Hw_MasterInfo != null)
                {
                    _Hw_MasterInfo.StatusId = 4; // Status set for system not connected or Exception
                    if (datarow.isConnected && (datarow.HwCaption != "" || datarow.HwDescription != ""))
                    {
                        _Hw_MasterInfo.StatusId = 3; // Status set for system connected and Scaning Completed
                        datarow.HwMasterInfoId = Convert.ToInt32(_Hw_MasterInfo.HwMasterInfoId.ToString()); // Hardware MasterId

                        Hw_DetailInfo _Hw_DetailInfo = new Hw_DetailInfo()
                        {
                            //HwDetailInfoId = 1,
                            HwMasterInfoId = datarow.HwMasterInfoId, // Get HwMasterInfoId From Hw_MasterInfo
                            HwManufacturer = datarow.HwManufacturer,
                            HwCaption = datarow.HwCaption,
                            HwDescription = datarow.HwDescription,
                            HwSerialNo = datarow.HwSerialNo,
                            HwStatus = datarow.HwStatus,
                            HwVersion = datarow.HwVersion,
                            HwModelNo = datarow.HwModelNo,
                            IPAddress = datarow.IPAddress,
                            HwSystemType = datarow.HwSystemType,
                            HwPartNo = datarow.HwPartNo,
                            OS = datarow.OS,
                            Processor = datarow.Processor,
                            PhysicalMemory = datarow.PhysicalMemory,
                            NoOfProcessors = datarow.NoOfProcessors,
                            NoOfLogicalProcessors = datarow.NoOfLogicalProcessors,
                            DNSHostName = datarow.DNSHostName,
                            DomainName = datarow.DomainName,
                            CurrentTimeZone = datarow.CurrentTimeZone,
                            CurrentLanguages = datarow.CurrentLanguages,
                            ListOfAvailableLanguages = datarow.ListOfAvailableLanguages,
                            InstalableLanguage = datarow.InstalableLanguage,
                            BiosName = datarow.BiosName,
                            BiosVersion = datarow.BiosVersion,
                            BiosSerialNo = datarow.BiosSerialNo,
                            BiosStatus = datarow.BiosStatus,
                            BiosDisplayConfiguration = datarow.BiosDisplayConfiguration,
                            UserName = datarow.UserName,
                            VmInstanceName = datarow.VmInstanceName,
                            VmInstanceStatus = datarow.VmInstanceStatus,
                            IsVirtual = datarow.IsVirtual,
                            IsServerRole = datarow.IsServerRole,
                            WebServer = datarow.WebServer,
                            MailServer = datarow.MailServer,
                            CMS_SharePoint = datarow.CMS_SharePoint,
                            DatabaseServer = datarow.DatabaseServer,
                            CreatedBy = 1,
                            CreatedDate = DateTime.Now,
                            ModifiedBy = 1,
                            ModifiedDate = DateTime.Now,
                            IsDeleted = false,
                        };
                        _Db.Hw_DetailInfo.Add(_Hw_DetailInfo);
                        try
                        {
                            _Db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(". Exception On Data Inserstion for Hw_DetailInfo IpAddress: " + datarow.IPAddress + " Message: " + ex.Message);
                            // WriteTextFile.WriteErrorLog(". Exception On Data Inserstion for Hw_DetailInfo IpAddress: " + datarow.IPAddress + " Message: " + ex.Message);
                        }


                        if (datarow.LstHw_Sw_ServicesDTO != null)
                        {
                            foreach (var RowLstSerivces in datarow.LstHw_Sw_ServicesDTO)
                            {
                                Hw_Sw_Services _Hw_Sw_Services = new Hw_Sw_Services()
                                {
                                    HwMasterInfoId = datarow.HwMasterInfoId, // Get HwMasterInfoId From Hw_MasterInfo
                                                                             //SwServiceId = RowLstSerivces.HwMasterInfoId,
                                    Caption = RowLstSerivces.Caption,
                                    Name = RowLstSerivces.Name,
                                    DisplayName = RowLstSerivces.DisplayName,
                                    Description = RowLstSerivces.Description,
                                    PathName = RowLstSerivces.PathName,
                                    ServiceType = RowLstSerivces.ServiceType,
                                    Started = RowLstSerivces.Started,
                                    StartMode = RowLstSerivces.StartMode,
                                    StartName = RowLstSerivces.StartName,
                                    State = RowLstSerivces.State,
                                    Status = RowLstSerivces.Status,
                                    CreatedBy = 1,
                                    CreatedDate = DateTime.Now,
                                    ModifiedBy = 1,
                                    ModifiedDate = DateTime.Now,
                                    IsDeleted = false,
                                };
                                _Db.Hw_Sw_Services.Add(_Hw_Sw_Services);
                            }
                            try
                            {
                                _Db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(". Exception On Data Inserstion for Hw_Sw_Services IpAddress: " + datarow.IPAddress + " Message: " + ex.Message);
                                //    WriteTextFile.WriteErrorLog(". Exception On Data Inserstion for Hw_Sw_Services IpAddress: " + datarow.IPAddress + " Message: " + ex.Message);
                            }
                        }

                        if (datarow.LstHw_Sw_RunningDTO != null)
                        {
                            foreach (var RowLstInstalledSoftwares in datarow.LstHw_Sw_RunningDTO)
                            {
                                if (RowLstInstalledSoftwares.InstalledDate.Year < 1900)
                                    RowLstInstalledSoftwares.InstalledDate = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture);
                                if (RowLstInstalledSoftwares.SwDateModified.Year < 1900)
                                    RowLstInstalledSoftwares.SwDateModified = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture);

                                Hw_Sw_Running _Hw_Sw_Running = new Hw_Sw_Running()
                                {
                                    // SwInstalledId = datarow.SwInstalledId,
                                    HwMasterInfoId = datarow.HwMasterInfoId, // Get HwMasterInfoId From Hw_MasterInfo
                                    SwName = RowLstInstalledSoftwares.SwName,
                                    SwDescription = RowLstInstalledSoftwares.SwDescription,
                                    SwType = RowLstInstalledSoftwares.SwType,
                                    SwVersion = RowLstInstalledSoftwares.SwVersion,
                                    ProductName = RowLstInstalledSoftwares.ProductName,
                                    ProductVersion = RowLstInstalledSoftwares.ProductVersion,
                                    CopyRight = RowLstInstalledSoftwares.CopyRight,
                                    Size = RowLstInstalledSoftwares.Size,
                                    // SwDateModified = RowLstInstalledSoftwares.SwDateModified, Exception: SqlDateTime overflow. Must be between 1/1/1753 12:00:00 AM and 12/31/9999 11:59:59 PM.
                                    Language = RowLstInstalledSoftwares.Language,
                                    SerialNo = RowLstInstalledSoftwares.SerialNo,
                                    LicenceNo = RowLstInstalledSoftwares.LicenceNo,
                                    LicenceType = RowLstInstalledSoftwares.LicenceType,
                                    InstalledDate = RowLstInstalledSoftwares.InstalledDate,
                                    SwDateModified = RowLstInstalledSoftwares.SwDateModified,
                                    PathName = RowLstInstalledSoftwares.PathName,
                                    Status = RowLstInstalledSoftwares.Status,
                                    CreatedBy = 1,
                                    CreatedDate = DateTime.Now,
                                    ModifiedBy = 1,
                                    ModifiedDate = DateTime.Now,
                                    IsDeleted = false,
                                };
                                _Db.Hw_Sw_Running.Add(_Hw_Sw_Running);
                            }
                            try
                            {
                                _Db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(". Exception On Data Inserstion for Hw_Sw_Running IpAddress: " + datarow.IPAddress + " Message: " + ex.Message);
                                //    WriteTextFile.WriteErrorLog(". Exception On Data Inserstion for Hw_Sw_Running IpAddress: " + datarow.IPAddress + " Message: " + ex.Message);
                            }
                        }

                        if (datarow.LstHw_Sw_InstalledDTO != null)
                        {
                            foreach (var RowLstInstalledSoftwares in datarow.LstHw_Sw_InstalledDTO)
                            {
                                if (RowLstInstalledSoftwares.InstalledDate.Year < 1900)
                                    RowLstInstalledSoftwares.InstalledDate = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture);

                                if (RowLstInstalledSoftwares.SwDateModified.Year < 1900)
                                    RowLstInstalledSoftwares.SwDateModified = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture);

                                Hw_Sw_Installed _Hw_Sw_Installed = new Hw_Sw_Installed()
                                {
                                    // SwInstalledId = datarow.SwInstalledId,
                                    HwMasterInfoId = datarow.HwMasterInfoId, // Get HwMasterInfoId From Hw_MasterInfo
                                    SwName = RowLstInstalledSoftwares.SwName,
                                    SwDescription = RowLstInstalledSoftwares.SwDescription,
                                    SwType = RowLstInstalledSoftwares.SwType,
                                    SwVersion = RowLstInstalledSoftwares.SwVersion,
                                    ProductName = RowLstInstalledSoftwares.ProductName,
                                    ProductVersion = RowLstInstalledSoftwares.ProductVersion,
                                    CopyRight = RowLstInstalledSoftwares.CopyRight,
                                    Size = RowLstInstalledSoftwares.Size,
                                    // SwDateModified = RowLstInstalledSoftwares.SwDateModified, Exception: SqlDateTime overflow. Must be between 1/1/1753 12:00:00 AM and 12/31/9999 11:59:59 PM.
                                    Language = RowLstInstalledSoftwares.Language,
                                    SerialNo = RowLstInstalledSoftwares.SerialNo,
                                    LicenceNo = RowLstInstalledSoftwares.LicenceNo,
                                    LicenceType = RowLstInstalledSoftwares.LicenceType,
                                    InstalledDate = RowLstInstalledSoftwares.InstalledDate,
                                    SwDateModified = RowLstInstalledSoftwares.SwDateModified,
                                    PathName = RowLstInstalledSoftwares.PathName,
                                    Status = RowLstInstalledSoftwares.Status,
                                    CreatedBy = 1,
                                    CreatedDate = DateTime.Now,
                                    ModifiedBy = 1,
                                    ModifiedDate = DateTime.Now,
                                    IsDeleted = false,
                                };
                                _Db.Hw_Sw_Installed.Add(_Hw_Sw_Installed);
                            }
                            try
                            {
                                _Db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Exception On Data Inserstion for Hw_Sw_Installed IpAddress: " + datarow.IPAddress + " Message: " + ex.Message);
                                //   WriteTextFile.WriteErrorLog("Exception On Data Inserstion for Hw_Sw_Installed IpAddress: " + datarow.IPAddress + " Message: " + ex.Message);
                            }
                        }

                        // Ip-Mac List Info
                        if (datarow.LstHw_IpMacAddressDTO != null)
                        {
                            foreach (var RowLstIpMacAddress in datarow.LstHw_IpMacAddressDTO)
                            {
                                Hw_IpMacAddress _Hw_Sw_IpMac = new Hw_IpMacAddress()
                                {
                                    // SwInstalledId = datarow.SwInstalledId,
                                    HwMasterInfoId = datarow.HwMasterInfoId, // Get HwMasterInfoId From Hw_MasterInfo
                                    Caption = RowLstIpMacAddress.Caption,
                                    IpAddress = RowLstIpMacAddress.IpAddress,
                                    IPEnabled = RowLstIpMacAddress.IPEnabled,
                                    MacAddress = RowLstIpMacAddress.MacAddress,
                                    CreatedBy = 1,
                                    CreatedDate = DateTime.Now,
                                    ModifiedBy = 1,
                                    ModifiedDate = DateTime.Now,
                                    IsDeleted = false,
                                };
                                _Db.Hw_IpMacAddress.Add(_Hw_Sw_IpMac);
                            }
                        }
                        try
                        {
                            _Db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Exception On Data Inserstion for _Hw_Sw_IpMac IpAddress: " + datarow.IPAddress + " Message: " + ex.Message);
                            //   WriteTextFile.WriteErrorLog("Exception On Data Inserstion for _Hw_Sw_IpMac IpAddress: " + datarow.IPAddress + " Message: " + ex.Message);
                        }
                    }
                    _Db.SaveChanges();
                }
                //}


                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception On Data Inserstion for IP Address: " + datarow.IPAddress + " Message: " + ex.Message);
                return null;
            }
        }
        public Hw_DetailInfoRequest GetHw_DetailInfoData(int SiteId)
        {
            Hw_DetailInfoRequest response = new Hw_DetailInfoRequest();
            try
            {
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception On Data SiteId for SiteId: " + SiteId + " Message: " + ex.Message);
                //   throw ex;
                return response;
            }
        }

        public void Hw_MasterInfo_UpdateIsConnectedStatus(Hw_DetailInfoDTO _Hw_DetailInfo)
        {
            Hw_MasterInfo _Hw_MasterInfo = _Db.Hw_MasterInfo.Where(x => x.IPAddress == _Hw_DetailInfo.IPAddress && x.IsDeleted == false).OrderByDescending(x => x.HwMasterInfoId).FirstOrDefault();

            if (_Hw_MasterInfo != null)
            {
                _Hw_MasterInfo.Description = _Hw_MasterInfo.Description + ". <Br>" + _Hw_DetailInfo.HwDescription;
                _Hw_MasterInfo.StatusId = 5;
                try
                {
                    _Db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception On Data Hw_MasterInfo_UpdateIsConnectedStatus for IP Address: " + _Hw_DetailInfo.IPAddress + " Message: " + ex.Message);

                    // throw;
                }
            }
        }
    }
}
