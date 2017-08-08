using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prb.DTO
{
    public class DomainUsersInfoDTO
    {
        public int DomainUsersInfoId { get; set; }
        public int SiteId { get; set; }
        public int ScheduleId { get; set; }
        public string SamAccountName { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string Description { get; set; }
        public Nullable<bool> Enabled { get; set; }
        public string EmployeeId { get; set; }
        public Nullable<System.DateTime> LastPasswordSet { get; set; }
        public Nullable<System.DateTime> LastBadPasswordAttempt { get; set; }
        public Nullable<System.DateTime> LastLogon { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
}
