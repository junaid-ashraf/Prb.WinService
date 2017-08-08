using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prb.DTO
{
    public class Hw_Sw_ServicesDTO
    {
        public int SwServiceId { get; set; }
        public int HwMasterInfoId { get; set; }
        public string Caption { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string PathName { get; set; }
        public string ServiceType { get; set; }
        public bool Started { get; set; }
        public string StartMode { get; set; }
        public string StartName { get; set; }
        public string State { get; set; }
        public string Status { get; set; }
    }
}
