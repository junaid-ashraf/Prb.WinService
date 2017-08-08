using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prb.SqliteDAL
{
    public class DBHelper
    {
        private static volatile DBHelper instance;
        private static object syncRoot = new Object();
        public ProbeDBEntities _Db;
        private DBHelper()
        {
            _Db = new ProbeDBEntities();
        }

        public static DBHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DBHelper();
                    }
                }

                return instance;
            }
        }
    }
}
