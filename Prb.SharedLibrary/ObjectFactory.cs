using Prb.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prb.SharedLibrary
{
    public class ObjectFactory
    {

        #region Shared Objects Between Active Directory And SNMP

        public static Prb_SiteDTO CustomerSiteInfomation { get; set; }

        

        #endregion



        #region List of Hardware Master information 


        private List<Hw_MasterInfoDTO> _LstHardwareInfo;
        public List<Hw_MasterInfoDTO> LstHardwareMasterInfo
        {
            get
            {
                if (_LstHardwareInfo == null)
                {
                    _LstHardwareInfo = new List<Hw_MasterInfoDTO>();
                }

                return _LstHardwareInfo;

            }
            set { _LstHardwareInfo = value; }
        }
        #endregion 




    }
}
