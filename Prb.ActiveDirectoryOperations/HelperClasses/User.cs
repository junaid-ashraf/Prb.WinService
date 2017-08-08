using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prb.ActiveDirectoryOperations.HelperClasses
{
    public class User
    {
        public String SamAccountName { get; set; }
        public String DisplayName { get; set; }
        public String Name { get; set; }
        public String GivenName { get; set; }
        public String Surname { get; set; }
        public String Description { get; set; }
        public Boolean? Enabled { get; set; }
        public String EmployeeId { get; set; }
        public DateTime? LastPasswordSet { get; set; }
        public DateTime? LastBadPasswordAttempt { get; set; }

        public DateTime? LastLogon { get; set; }

        public int DomainUsersInfoId { get; set; }
        public int SiteId { get; set; }
        public int ScheduleId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }

        public User(String SamAccountName, String DisplayName, String Name, String GivenName, String Surname, String Description,
            Boolean? Enabled, String EmployeeId, DateTime? LastPasswordSet, DateTime? LastBadPasswordAttempt, DateTime? LastLogon)
        {
            this.SamAccountName = SamAccountName;
            this.DisplayName = DisplayName;
            this.Name = Name;
            this.GivenName = GivenName;
            this.Surname = Surname;
            this.Description = Description;
            this.Enabled = Enabled;
            this.EmployeeId = EmployeeId;
            this.LastPasswordSet = LastPasswordSet;
            this.LastBadPasswordAttempt = LastBadPasswordAttempt;
            this.LastLogon = LastLogon;
        }
        public List<string> Properties()
        {
            return new List<string> { SamAccountName, DisplayName, Name, GivenName, Surname, Description, Enabled.ToString(), LastLogon.ToString() };
        }
        public int UserPropertiesTotal = 8;
        public static string[] StringArrayUesrProperties = { "SamAccountName", "DisplayName", "Name", "GivenName", "Surname", "Description", "Enabled", "LastLogon" };
    }
}
