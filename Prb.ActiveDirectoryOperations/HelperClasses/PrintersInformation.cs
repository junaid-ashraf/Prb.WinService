using Prb.Common;
using Prb.DTO;
using SnmpSharpNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prb.ActiveDirectoryOperations.HelperClasses
{
    public class PrintersInformation
    {
        private static List<string> listPrintersInformation = new List<string>();
        public static string[] GetPrinters(string IP)
        {
            string[] Ipaddr = new string[5]; ;
            String snmpAgent = IP.ToString();// printer IP
            String snmpCommunity = "public";//printer community
            SimpleSnmp snmp = new SimpleSnmp(snmpAgent, snmpCommunity);
            Dictionary<Oid, AsnType> result = snmp.Get(SnmpVersion.Ver2, new string[] { "1.3.6.1.2.1.43.16.5.1.2.1.1" });
            if (result == null)
            {
            }
            else
            {
                List<string> printerInfo = SNMP.GetName(result, IP);
                Ipaddr[0] = printerInfo[0];
                Ipaddr[1] = printerInfo[1];
                Ipaddr[2] = printerInfo[2];
                Ipaddr[3] = "Devices";
                Ipaddr[4] = "Printers";

                // Found Printer Detail
                //listPrintersInformation = new List<string>();
                //listPrintersInformation.Add("Printer IP: " + IP);
                //listPrintersInformation.Add("Printer Name: " + printerInfo[0]);
                //SNMPPrinterInfo(snmp, ip.ToString());

                return Ipaddr;
            }
            return Ipaddr;
        }

        static Hw_DetailInfoDTO oHw_DetailInfoDTO;
        public static Hw_DetailInfoDTO getprinterDetail(string ip)
        {
            oHw_DetailInfoDTO = new Hw_DetailInfoDTO();

            string[] Ipaddr = new string[7]; ;
            String snmpAgent = ip.ToString();// printer IP
            String snmpCommunity = "public";//printer community
                                            // int snmpPeerPort = 9100;

            SimpleSnmp snmp = new SimpleSnmp(snmpAgent, snmpCommunity);

            Dictionary<Oid, AsnType> ResultPrinterDetail = snmp.Get(SnmpVersion.Ver1, new string[] { "1.3.6.1.4.1.11.2.3.9.1.1.7.0" });
            Dictionary<Oid, AsnType> resultPrinterMode = snmp.Get(SnmpVersion.Ver1, new string[] { "1.3.6.1.2.1.43.16.5.1.2.1.1" });
            Dictionary<Oid, AsnType> resultMAC = snmp.Get(SnmpVersion.Ver1, new string[] { "1.3.6.1.1.1.1.22" });

            // System info
            Dictionary<Oid, AsnType> resultSystemInfo = snmp.Get(SnmpVersion.Ver1, new string[] { "1.3.6.1.2.1.1.5.0" });
            Dictionary<Oid, AsnType> resultSysDesc = snmp.Get(SnmpVersion.Ver1, new string[] { "1.3.6.1.2.1.2.2.1.2.1" });
            

            // Serial No
            Dictionary<Oid, AsnType> resultSerialNo = snmp.Get(SnmpVersion.Ver1, new string[] { "1.3.6.1.2.1.43.5.1.1.17.1" });
            Dictionary<Oid, AsnType> resultPhyAddress= snmp.Get(SnmpVersion.Ver1, new string[] { "1.3.6.1.2.1.2.2.1.6.1" });
            

            //  Dictionary<Oid, AsnType> result2 = snmp.Get(SnmpVersion.Ver2, new string[] { "1.3.6.1.4.1.11.2.3.9.1.1.7.0" });
            //  Dictionary<Oid, AsnType> result = snmp.Get(SnmpVersion.Ver2, "1.3.6.1.2.1.43.16.5.1.2.1.1");
            //  1.3.6.1.4.1.11.2.3.9.1
            if (resultPrinterMode == null)
            {
                return null;
            }
            else
            {
                oHw_DetailInfoDTO.isConnected = true;

                List<string> printerInfo = SNMP.GetName(ResultPrinterDetail, ip);
                List<string> PrinterMode = SNMP.GetName(resultPrinterMode, ip);
                List<string> _resultSystemInfo = SNMP.GetName(resultSystemInfo, ip);
                List<string> _resultSystemInfoSerial = SNMP.GetName(resultSerialNo, ip);

                Ipaddr[0] = printerInfo[0];
                //      oHw_DetailInfoDTO.HwCaption = printerInfo[0]; // Printer Name

                //   Ipaddr[1] = printerInfo[1];
                oHw_DetailInfoDTO.HwCaption = _resultSystemInfo[0]; // Domain Name
                oHw_DetailInfoDTO.HwCaption = "";

                oHw_DetailInfoDTO.HwDescription = PrinterMode[0];
                oHw_DetailInfoDTO.HwSerialNo = _resultSystemInfoSerial[0];

                //    Ipaddr[2] = printerInfo[2]; // Printer MAC
                Ipaddr[3] = "Devices";
                Ipaddr[4] = "Printers";
                Ipaddr[5] = "  ";
                Ipaddr[6] = "  ";
                // items = new ListViewItem(Ipaddr);

                // System.Windows.Forms.ListViewItem TempItem = new ListViewItem(Ipaddr);

                // Found Printer Detail
                listPrintersInformation.Add("Printer IP: " + ip);
                WriteTextFile.WriteErrorLog("Printer IP: " + ip);

                listPrintersInformation.Add("Printer Name: " + printerInfo[0]);
                WriteTextFile.WriteErrorLog("Printer Name: " + printerInfo[0]);

                string[] data = SNMPPrinterInfo(snmp, ip.ToString());



                return oHw_DetailInfoDTO;
                // return Ipaddr;
            }
            // return items;
            // return oHw_DetailInfoDTO;
            //return Ipaddr;
        }
        public static string[] SNMPPrinterInfo(SimpleSnmp snmp, string i)
        {
            string SerialNo = "";
            //Dictionary<Oid, AsnType> result = snmp.Get(SnmpVersion.Ver2, new string[] { "1.3.6.1.2.1.43.16.5.1.2.1.1" });
            // Printer Detail
            Dictionary<Oid, AsnType> result = snmp.Get(SnmpVersion.Ver1, new string[] { "1.3.6.1.4.1.11.2.3.9.1.1.7.0" });
            //Dictionary<Oid, AsnType> result2 = snmp.Get(SnmpVersion.Ver2, new string[] { "1.3.6.1.4.1.11.2.3.9.1" });
            if (result == null)
            {
            }
            else
            {
                string[] IpaddrDet = new string[20];
                int c = 0;
                foreach (KeyValuePair<Oid, AsnType> kvp in result)
                {
                    IpaddrDet[c] = string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
                    try
                    {
                        string[] outputWithEqual = IpaddrDet[c].Split('=');
                        string[] outputWithComma = outputWithEqual[2].Split(';');


                        //                    snmp Version 1
                        if (outputWithComma.Length > 7)
                        {
                            if (IpaddrDet[c].Contains("MFG"))
                            {
                                var MFG = outputWithComma[0];
                                oHw_DetailInfoDTO.HwManufacturer = MFG; // Add MFG info into HW_Detailinfo

                                listPrintersInformation.Add(MFG);
                                WriteTextFile.WriteErrorLog(MFG);
                            }
                            if (IpaddrDet[c].Contains("CMD"))
                            {
                                var CMD = outputWithComma[2];
                                oHw_DetailInfoDTO.ListOfAvailableLanguages = CMD; // Add CMD info into HW_Detailinfo

                                listPrintersInformation.Add(CMD);
                                WriteTextFile.WriteErrorLog(CMD);
                            }
                            //if (IpaddrDet[c].Contains("CLS"))
                            //{
                            //    var MFG = outputWithComma[2];
                            //}
                            //if (IpaddrDet[c].Contains("CMD"))
                            //{
                            //    var CMD = outputWithComma[2];
                            //    oHw_DetailInfoDTO.ListOfAvailableLanguages = CMD; // Add Desc info into HW_Detailinfo

                            //    listPrintersInformation.Add(CMD);
                            //    WriteTextFile.WriteErrorLog(CMD);
                            //}
                            if (IpaddrDet[c].Contains("MDL"))
                            {
                                var MDL = outputWithComma[1];
                                oHw_DetailInfoDTO.HwModelNo = MDL; // Add Model info into HW_Detailinfo
                                oHw_DetailInfoDTO.HwCaption = MDL; // Add Model info into HW_Detailinfo

                                listPrintersInformation.Add(MDL);
                                WriteTextFile.WriteErrorLog(MDL);
                            }
                            if (IpaddrDet[c].Contains("CLS"))
                            {
                                var data = outputWithComma[3];
                                oHw_DetailInfoDTO.HwDescription = data; // Add Class info into HW_Detailinfo

                                listPrintersInformation.Add(data);
                                WriteTextFile.WriteErrorLog(data);
                            }
                            if (IpaddrDet[c].Contains("DES"))
                            {
                                var data = outputWithComma[4];
                                oHw_DetailInfoDTO.HwDescription = data; // Add HwDescription info into HW_Detailinfo

                                listPrintersInformation.Add(data);
                                WriteTextFile.WriteErrorLog(data);
                            }

                            if (IpaddrDet[c].Contains("LEDMDIS"))
                            {
                                var data = outputWithComma[6];
                                oHw_DetailInfoDTO.HwDescription = data; // Add HwDescription info into HW_Detailinfo
                                SerialNo = data;
                                listPrintersInformation.Add(data);
                                WriteTextFile.WriteErrorLog(data);
                            }
                            if (IpaddrDet[c].Contains("SN"))
                            {
                                var data = outputWithComma[7];
                                oHw_DetailInfoDTO.HwSerialNo = data; // Add HwDescription info into HW_Detailinfo
                                SerialNo = data;
                                listPrintersInformation.Add(data);
                                WriteTextFile.WriteErrorLog(data);
                            }
                        }




                        //                    snmp Version 2
                        //if (outputWithComma.Length > 5)
                        //{
                        //    if (IpaddrDet[c].Contains("MFG"))
                        //    {
                        //        var MFG = outputWithComma[0];
                        //        oHw_DetailInfoDTO.HwManufacturer = MFG; // Add MFG info into HW_Detailinfo

                        //        listPrintersInformation.Add(MFG);
                        //        WriteTextFile.WriteErrorLog(MFG);
                        //    }
                        //    if (IpaddrDet[c].Contains("CMD"))
                        //    {
                        //        var CMD = outputWithComma[1];
                        //        oHw_DetailInfoDTO.ListOfAvailableLanguages = CMD; // Add CMD info into HW_Detailinfo

                        //        listPrintersInformation.Add(CMD);
                        //        WriteTextFile.WriteErrorLog(CMD);
                        //    }
                        //    //if (IpaddrDet[c].Contains("CLS"))
                        //    //{
                        //    //    var MFG = outputWithComma[2];
                        //    //}
                        //    //if (IpaddrDet[c].Contains("CMD"))
                        //    //{
                        //    //    var CMD = outputWithComma[2];
                        //    //    oHw_DetailInfoDTO.ListOfAvailableLanguages = CMD; // Add Desc info into HW_Detailinfo

                        //    //    listPrintersInformation.Add(CMD);
                        //    //    WriteTextFile.WriteErrorLog(CMD);
                        //    //}
                        //    if (IpaddrDet[c].Contains("MDL"))
                        //    {
                        //        var MDL = outputWithComma[3];
                        //        oHw_DetailInfoDTO.HwModelNo = MDL; // Add Model info into HW_Detailinfo
                        //        oHw_DetailInfoDTO.HwCaption = MDL; // Add Model info into HW_Detailinfo

                        //        listPrintersInformation.Add(MDL);
                        //        WriteTextFile.WriteErrorLog(MDL);
                        //    }
                        //    if (IpaddrDet[c].Contains("CLS"))
                        //    {
                        //        var data = outputWithComma[4];
                        //        oHw_DetailInfoDTO.HwDescription = data; // Add Class info into HW_Detailinfo

                        //        listPrintersInformation.Add(data);
                        //        WriteTextFile.WriteErrorLog(data);
                        //    }
                        //    if (IpaddrDet[c].Contains("DES"))
                        //    {
                        //        var data = outputWithComma[5];
                        //        oHw_DetailInfoDTO.HwDescription = data; // Add HwDescription info into HW_Detailinfo

                        //        listPrintersInformation.Add(data);
                        //        WriteTextFile.WriteErrorLog(data);
                        //    }
                        //}







                        //if (IpaddrDet[c].Contains("DES"))
                        //{
                        //    var MFG = outputWithComma[6];
                        //    IpaddrDet[c].Contains("DES");
                        //}

                        //string[] columns = IpaddrDet[c].Split(';');
                        //List<string> list = new List<string>();
                        //for (int ig = 0; ig < columns.Length; ig++)
                        //{
                        //    string[] data = columns[ig].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                        //    list.Add(data[1]); //this will add middle data 
                        //}
                        //string newText = String.Join(",", list.ToArray());
                        //var outputWithCollon = outputWithComma[c].Split(':');

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    c++;
                }
                List<string> printerInfo = SNMP.GetName(result, i);
                string[] Ipaddr = new string[7];
                Ipaddr[0] = printerInfo[0];
                //Ipaddr[1] = printerInfo[1];
                Ipaddr[1] = printerInfo[0];
                //Ipaddr[2] = printerInfo[2];
                Ipaddr[2] = printerInfo[0];
                Ipaddr[3] = "Printers";
                //Ipaddr[4] = printerInfo[2];
                Ipaddr[4] = printerInfo[0];
                Ipaddr[5] = "";
                Ipaddr[6] = "";
                return Ipaddr;
            }
            return null;
        }
    }
}