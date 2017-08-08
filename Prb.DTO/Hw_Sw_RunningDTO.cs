using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prb.DTO
{
    public class Hw_Sw_RunningDTO
    {
        public int SwRunningId { get; set; }
        public int HwMasterInfoId { get; set; }
        public string SwName { get; set; }
        public string SwDescription { get; set; }
        public string SwType { get; set; }
        public string SwVersion { get; set; }
        public string ProductName { get; set; }
        public string ProductVersion { get; set; }
        public string CopyRight { get; set; }
        public string Size { get; set; }
        public DateTime SwDateModified { get; set; }
        public string Language { get; set; }
        public string SerialNo { get; set; }
        public string LicenceNo { get; set; }
        public string LicenceType { get; set; }
        public DateTime InstalledDate { get; set; }
        public string PathName { get; set; }
        public string Status { get; set; }
    }
}
