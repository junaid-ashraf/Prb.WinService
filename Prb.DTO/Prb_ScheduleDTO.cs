using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prb.DTO
{
    public class Prb_ScheduleDTO
    {
        public int ScheduleId { get; set; }
        public int SiteId { get; set; }
        public Nullable<System.DateTime> StartDateTime { get; set; }
        public Nullable<System.DateTime> EndDateTime { get; set; }
        public string Description { get; set; }
        public int? StatusId { get; set; }
        public int OperationId { get; set; }
        public int SettingId { get; set; }
    }
}
