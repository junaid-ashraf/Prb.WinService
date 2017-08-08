using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prb.DTO
{
    public class Prb_SiteCredentialDTO
    {
        public int SiteCredentialId { get; set; }
        public int SiteId { get; set; }
        public string IPAddress { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int HwTypeId { get; set; }
        public bool IsActive { get; set; }
    }
}
