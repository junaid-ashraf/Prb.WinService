using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prb.DTO
{
    public class PrbSetting
    {
        public List<Prb_SettingDTO> Prb_SettingListDTO { get; set; }
        public Prb_SettingDTO Prb_SettingDTO { get; set; }
    }

    public class Prb_SettingDTO
    {
        public int SettingId { get; set; }
        public int SiteId { get; set; }
        public string DomainType { get; set; }
        public Nullable<bool> WindowsAccess { get; set; }
        public Nullable<bool> MacAccess { get; set; }
        public Nullable<bool> LinuxAccess { get; set; }
        public Nullable<bool> PrintersAccess { get; set; }
        public Nullable<bool> RoutersSwitchesAccess { get; set; }
        public Nullable<bool> OthersAccess { get; set; }
        public Nullable<bool> HardwareDetail { get; set; }
        public Nullable<bool> SoftwareDetail { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }

        // Site
        public string CustomerName { get; set; }

        // Settings
        public string SiteName { get; set; }
        public string DomainName { get; set; }
        public string DomainAdminUser { get; set; }
        public string Password { get; set; }

        public int IsRead { get; set; }
    }
}
