using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prb.DTO
{
    public class Hw_IpMacAddressDTO
    {
        public int SwServiceId { get; set; }
        public int HwMasterInfoId { get; set; }
        public string Caption { get; set; }
        public string IpAddress { get; set; }
        public string IPEnabled { get; set; }
        public string MacAddress { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }

        //public  Hw_MasterInfo Hw_MasterInfo { get; set; }
    }
}
