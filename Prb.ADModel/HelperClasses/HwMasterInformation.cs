using System;
using System.Net;
using System.Runtime.InteropServices;
using Prb.Common;

namespace Prb.ActiveDirectoryOperations.HelperClasses
{
    public class HwMasterInformation
    {
        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        public static extern int SendARP(int DestIP, int SrcIP, [Out] byte[] pMacAddr, ref int PhyAddrLen);
        /// <summary>
        /// Getting Hardware Master information against IP address
        /// </summary>
        /// <param name="Tempaddr"></param>
        /// <returns></returns>
        public static string[] GetHwMasterInfo(System.Net.IPHostEntry Tempaddr)
        {
            string[] Ipaddr = new string[2];
            try
            {
                System.Net.IPAddress[] TempAd = Tempaddr.AddressList;
                foreach (IPAddress TempA in TempAd)
                {
                    Ipaddr[0] = TempA.ToString();

                    byte[] ab = new byte[6];
                    int len = ab.Length;

                    // This Function Used to Get The Physical Address
                    int r = SendARP((int)TempA.Address, 0, ab, ref len);
                    string mac = BitConverter.ToString(ab, 0, 6);

                    Ipaddr[1] = mac;
                }
            }
            catch(Exception ex)
            {
                WriteTextFile.WriteErrorLog("Hardware Master Information ERROR:");
                WriteTextFile.WriteErrorLog("==========================================");
                WriteTextFile.WriteErrorLog(ex);
            }
            return Ipaddr;
        }
    }
}
