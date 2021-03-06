﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prb.DTO
{
    public class Hw_DetailInfoDTO
    {
        public bool isConnected { get; set; }

        public int HwDetailInfoId { get; set; }
        public int HwMasterInfoId { get; set; }
        public string IPAddress { get; set; }
        public string HwManufacturer { get; set; }
        public string HwCaption { get; set; }
        public string HwDescription { get; set; }
        public string HwSerialNo { get; set; }
        public string HwStatus { get; set; }
        public string HwVersion { get; set; }
        public string HwModelNo { get; set; }
        public string HwPartNo { get; set; }
        public string HwSystemType { get; set; }
        public string OS { get; set; }
        public string Processor { get; set; }
        public string PhysicalMemory { get; set; }
        public Nullable<int> NoOfProcessors { get; set; }
        public Nullable<int> NoOfLogicalProcessors { get; set; }
        public string DNSHostName { get; set; }
        public string DomainName { get; set; }
        public string CurrentTimeZone { get; set; }
        public string CurrentLanguages { get; set; }
        public string ListOfAvailableLanguages { get; set; }
        public Nullable<int> InstalableLanguage { get; set; }
        public string BiosName { get; set; }
        public string BiosVersion { get; set; }
        public string BiosSerialNo { get; set; }
        public string BiosStatus { get; set; }
        public string BiosDisplayConfiguration { get; set; }
        public string UserName { get; set; }
        public string VmInstanceName { get; set; }
        public string VmInstanceStatus { get; set; }


        public bool IsVirtual { get; set; }
        public bool IsServerRole { get; set; }
        public string WebServer { get; set; }
        public string MailServer { get; set; }
        public string CMS_SharePoint { get; set; }
        public string DatabaseServer { get; set; }


        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }



        public Hw_MasterInfoDTO Hw_MasterInfo { get; set; }


        // Custome Attached with DetailInfo (Installed Softwares and Services)
        public List<Hw_Sw_InstalledDTO> LstHw_Sw_InstalledDTO { get; set; }
        public List<Hw_Sw_ServicesDTO> LstHw_Sw_ServicesDTO { get; set; }
        public List<Hw_IpMacAddressDTO> LstHw_IpMacAddressDTO { get; set; }
        public List<Hw_Sw_RunningDTO> LstHw_Sw_RunningDTO { get; set; }
    }
}
