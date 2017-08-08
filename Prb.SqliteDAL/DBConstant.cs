using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prb.SqliteDAL
{
    public static class DBConstant
    {
        public struct Prb_Setting
        {
            public const string SP_PRB_SETTING = "SP_Prb_Setting";
        }
        public struct Prb_Site
        {
            public const string SP_PRB_SITE = "SP_Prb_Site";
        }
        public struct Prb_Schedule
        {
            public const string SP_PRB_SCHEDULE = "SP_Prb_Schedule";
        }
        public struct Hw_MasterInfo
        {
            public const string SP_HW_MASTERINFO_INSERTDATA = "SP_Hw_MasterInfo_InsertData";
            public const string SP_HW_MASTERINFO = "SP_Hw_MasterInfo";
        }
    }
}
