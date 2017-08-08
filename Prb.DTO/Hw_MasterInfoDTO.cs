using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prb.DTO
{
    public class Hw_MasterInfoDTO
    {
        public int HwMasterInfoId { get; set; }
        public int SiteId { get; set; }
        public int ScheduleId { get; set; }
        public int HwTypeId { get; set; }
        public string DevType { get; set; }
        public string DevCategory { get; set; }
        public string ComType { get; set; }
        public string HwName { get; set; }
        public string Description { get; set; }
        public string IPAddress { get; set; }
        public string MacAddress { get; set; }
        public int StatusId { get; set; }
      
    }
}
