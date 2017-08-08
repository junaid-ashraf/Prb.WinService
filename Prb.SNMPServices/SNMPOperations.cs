
using Prb.DTO;
using Prb.DTO.Request;
using Prb.SharedLibrary;
using Prb.SqliteDAL;
using SnmpSharpNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;



namespace Prb.SNMPServices
{/// <summary>
 /// this class is responsible for discover SNMP devices in the network
 /// </summary>
    public class SNMPOperations
    {


        // Network Discovery Info
        //// hardware master information
        //List<Hw_MasterInfoDTO> LstHw_MasterInfoDTO = new List<Hw_MasterInfoDTO>();

        //Hw_MasterInfoResponse oHw_MasterInfoResponse = new Hw_MasterInfoResponse();
        //Hw_MasterInfoDataAccess oHw_MasterInfoDataAccess = new Hw_MasterInfoDataAccess();


        #region SNMP Operations 
        /// <summary>
        /// this method will pick basic information of switch 
        /// throug SNMP and OIDs on the basis of IP Address and community
        /// 
        /// </summary>

        public string SNMPGetRequest(string OctCommunity, string IpAddress)
        {
            List<Hw_MasterInfoDTO> LstHw_MasterInfoDTO = new List<Hw_MasterInfoDTO>();
            var oHw_MasterInfoResponse = new Hw_MasterInfoResponse();
            Hw_MasterInfoDataAccess oHw_MasterInfoDataAccess = new Hw_MasterInfoDataAccess();
            string lstResults = "";
            try
            {
                // SNMP community name
                OctetString community = new OctetString(OctCommunity);

                // Define agent parameters class
                AgentParameters param = new AgentParameters(community);
                // Set SNMP version to 1 (or 2)
                param.Version = SnmpVersion.Ver1;
                // Construct the agent address object
                // IpAddress class is easy to use here because
                //  it will try to resolve constructor parameter if it doesn't
                //  parse to an IP address
                IpAddress agent = new IpAddress(IpAddress);
                //IpAddress agent = new IpAddress("65.154.4.21");

                // Construct target
                UdpTarget target = new UdpTarget((IPAddress)agent, 161, 2000, 1);

                // Pdu class used for all requests
                Pdu pdu = new Pdu(PduType.Get);
                pdu.VbList.Add("1.3.6.1.2.1.1.1.0"); //sysDescr
                pdu.VbList.Add("1.3.6.1.2.1.1.2.0"); //sysObjectID
                pdu.VbList.Add("1.3.6.1.2.1.1.3.0"); //sysUpTime
                pdu.VbList.Add("1.3.6.1.2.1.1.4.0"); //sysContact
                pdu.VbList.Add("1.3.6.1.2.1.1.5.0"); //sysName

                //

                pdu.VbList.Add("1.3.6.1.2.1.2.2.1.2.12");
                pdu.VbList.Add("1.3.6.1.2.1.2.2.1.2.13");
                pdu.VbList.Add("1.3.6.1.2.1.2.2.1.2.14");
                pdu.VbList.Add("1.3.6.1.2.1.2.2.1.2.15");
                pdu.VbList.Add("1.3.6.1.2.1.2.2.1.2.16");
                pdu.VbList.Add("1.3.6.1.2.1.2.2.1.2.17");
                pdu.VbList.Add("1.3.6.1.2.1.2.2.1.2.18");
                pdu.VbList.Add("1.3.6.1.2.1.2.2.1.2.19");
                pdu.VbList.Add("1.3.6.1.2.1.2.2.1.2.20");
                pdu.VbList.Add("1.3.6.1.2.1.2.2.1.2.21");
                pdu.VbList.Add("1.3.6.1.2.1.2.2.1.2.22");
                pdu.VbList.Add("1.3.6.1.2.1.2.2.1.2.23");
                pdu.VbList.Add("1.3.6.1.2.1.2.2.1.2.24");
                pdu.VbList.Add("1.3.6.1.2.1.2.2.1.2.25");
                pdu.VbList.Add("1.3.6.1.2.1.2.2.1.2.26");
                pdu.VbList.Add("1.3.6.1.2.1.2.2.1.2.27");
                pdu.VbList.Add("1.3.6.1.2.1.2.2.1.2.28");
                pdu.VbList.Add("1.3.6.1.2.1.2.2.1.2.29");

                pdu.VbList.Add("1.3.6.1.2.1.2.2.1.2.1");
                pdu.VbList.Add("1.3.6.1.2.1.2.2.1.2.2");
                pdu.VbList.Add("1.3.6.1.2.1.2.2.1.2.3");
                pdu.VbList.Add("1.3.6.1.2.1.2.2.1.2.4");
                pdu.VbList.Add("1.3.6.1.2.1.2.2.1.2.5");

                //

                // Make SNMP request
                SnmpV1Packet result = (SnmpV1Packet)target.Request(pdu, param);
                Hw_MasterInfoDTO objHw_Info;
                // If result is null then agent didn't reply or we couldn't parse the reply.
                if (result != null)
                {
                    // ErrorStatus other then 0 is an error returned by 
                    // the Agent - see SnmpConstants for error definitions
                    if (result.Pdu.ErrorStatus != 0)
                    {
                        // agent reported an error with the request
                        Console.WriteLine("Error in SNMP reply. Error {0} index {1}",
                            result.Pdu.ErrorStatus,
                            result.Pdu.ErrorIndex);
                    }
                    else
                    {
                        objHw_Info = new Hw_MasterInfoDTO();
                        objHw_Info.IPAddress = IpAddress;
                        objHw_Info.HwTypeId = 7; // Switch TODO: it will be fetched from DataBase's table hardware type.
                        objHw_Info.Description = result.Pdu.VbList[0].Value.ToString();
                        objHw_Info.HwName = result.Pdu.VbList[4].Value.ToString();
                        objHw_Info.DevType = "Machines";
                        objHw_Info.DevCategory = "IOS";
                        objHw_Info.ComType = "Cisco IOS Software";
                        objHw_Info.StatusId = 2; // // Start 1, Running 2, Completed 3, Stop 4, Exception 5... Status 3 is used for Complete Running Probe
                        objHw_Info.MacAddress = "";//TODO: will be fetched from Switch
                        LstHw_MasterInfoDTO.Add(objHw_Info);

                        if (LstHw_MasterInfoDTO.Count > 0)
                        {
                            // ProbeDBEntities _Db = new ProbeDBEntities();
                            oHw_MasterInfoResponse = oHw_MasterInfoDataAccess.Hw_MasterInfo_InsertData(LstHw_MasterInfoDTO); // By Umar
                        }

                        // Reply variables are returned in the same order as they were added
                        //  to the VbList
                        lstResults = "System Description :" + result.Pdu.VbList[0].Value.ToString() + "\n";
                        Console.WriteLine("sysDescr({0}) ({1}): {2}",
                            result.Pdu.VbList[0].Oid.ToString(),
                            SnmpConstants.GetTypeName(result.Pdu.VbList[0].Value.Type),
                            result.Pdu.VbList[0].Value.ToString());

                        lstResults = lstResults + "System Object ID: " + result.Pdu.VbList[1].Value.ToString() + "\n";
                        Console.WriteLine("sysObjectID({0}) ({1}): {2}",
                            result.Pdu.VbList[1].Oid.ToString(),
                            SnmpConstants.GetTypeName(result.Pdu.VbList[1].Value.Type),
                            result.Pdu.VbList[1].Value.ToString());
                        lstResults = lstResults + "System Up Time: " + result.Pdu.VbList[2].Value.ToString() + "\n";
                        Console.WriteLine("sysUpTime({0}) ({1}): {2}",
                            result.Pdu.VbList[2].Oid.ToString(),
                            SnmpConstants.GetTypeName(result.Pdu.VbList[2].Value.Type),
                            result.Pdu.VbList[2].Value.ToString());
                        lstResults = lstResults + "System Conatct: " + result.Pdu.VbList[3].Value.ToString() + "\n";
                        Console.WriteLine("sysContact({0}) ({1}): {2}",
                            result.Pdu.VbList[3].Oid.ToString(),
                            SnmpConstants.GetTypeName(result.Pdu.VbList[3].Value.Type),
                            result.Pdu.VbList[3].Value.ToString());
                        lstResults = lstResults + "System Name: " + result.Pdu.VbList[4].Value.ToString() + "\n";
                        Console.WriteLine("sysName({0}) ({1}): {2}",
                            result.Pdu.VbList[4].Oid.ToString(),
                            SnmpConstants.GetTypeName(result.Pdu.VbList[4].Value.Type),
                            result.Pdu.VbList[4].Value.ToString());
                    }
                }
                else
                {

                    Console.WriteLine("No response received from SNMP agent.");
                    lstResults = "No response received from SNMP agent.";

                }
                target.Close();
            }
            catch (Exception ex)
            {
                lstResults = "Code Breaks :" + ex.Message;
                return lstResults;
            }

            //WriteTextFile.WriteErrorLog("============================ SWITCH Information ====================================");
            //  WriteTextFile.WriteErrorLog(lstResults);

            //  WriteTextFile.WriteErrorLog("============================ SWITCH Information ====================================");

            return lstResults;
        }


        public string ReceiveSNMPTrapNotifications()
        {
            string strResults = string.Empty;
            try
            {

                // Construct a socket and bind it to the trap manager port 162 
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 162);
                EndPoint ep = (EndPoint)ipep;
                socket.Bind(ep);
                // Disable timeout processing. Just block until packet is received 
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 0);
                bool run = true;
                int inlen = -1;
                while (run)
                {
                    byte[] indata = new byte[16 * 1024];
                    // 16KB receive buffer int inlen = 0;
                    IPEndPoint peer = new IPEndPoint(IPAddress.Any, 0);
                    EndPoint inep = (EndPoint)peer;
                    try
                    {
                        inlen = socket.ReceiveFrom(indata, ref inep);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception {0}", ex.Message);
                        strResults = "Exception : " + ex.Message;
                        inlen = -1;
                    }
                    if (inlen > 0)
                    {
                        // Check protocol version int 
                        int ver = SnmpPacket.GetProtocolVersion(indata, inlen);
                        if (ver == (int)SnmpVersion.Ver1)
                        {
                            // Parse SNMP Version 1 TRAP packet 
                            SnmpV1TrapPacket pkt = new SnmpV1TrapPacket();
                            pkt.decode(indata, inlen);
                            Console.WriteLine("** SNMP Version 1 TRAP received from {0}:", inep.ToString());
                            Console.WriteLine("*** Trap generic: {0}", pkt.Pdu.Generic);
                            Console.WriteLine("*** Trap specific: {0}", pkt.Pdu.Specific);
                            Console.WriteLine("*** Agent address: {0}", pkt.Pdu.AgentAddress.ToString());
                            Console.WriteLine("*** Timestamp: {0}", pkt.Pdu.TimeStamp.ToString());
                            Console.WriteLine("*** VarBind count: {0}", pkt.Pdu.VbList.Count);
                            Console.WriteLine("*** VarBind content:");
                            foreach (Vb v in pkt.Pdu.VbList)
                            {
                                Console.WriteLine("**** {0} {1}: {2}", v.Oid.ToString(), SnmpConstants.GetTypeName(v.Value.Type), v.Value.ToString());
                            }
                            Console.WriteLine("** End of SNMP Version 1 TRAP data.");
                        }
                        else
                        {
                            // Parse SNMP Version 2 TRAP packet 
                            SnmpV2Packet pkt = new SnmpV2Packet();
                            pkt.decode(indata, inlen);
                            Console.WriteLine("** SNMP Version 2 TRAP received from {0}:", inep.ToString());
                            if ((SnmpSharpNet.PduType)pkt.Pdu.Type != PduType.V2Trap)
                            {
                                Console.WriteLine("*** NOT an SNMPv2 trap ****");
                            }
                            else
                            {
                                Console.WriteLine("*** Community: {0}", pkt.Community.ToString());
                                Console.WriteLine("*** VarBind count: {0}", pkt.Pdu.VbList.Count);
                                Console.WriteLine("*** VarBind content:");
                                foreach (Vb v in pkt.Pdu.VbList)
                                {
                                    Console.WriteLine("**** {0} {1}: {2}",
                                       v.Oid.ToString(), SnmpConstants.GetTypeName(v.Value.Type), v.Value.ToString());
                                }
                                Console.WriteLine("** End of SNMP Version 2 TRAP data.");
                            }
                        }
                    }
                    else
                    {
                        if (inlen == 0)
                            Console.WriteLine("Zero length packet received.");
                    }
                }


            }
            catch (Exception ex)
            {

                Console.WriteLine("No response received from SNMP agent.");
                strResults = "No response received from SNMP agent.";
            }

            return strResults;

        }



        #endregion

    }




}

