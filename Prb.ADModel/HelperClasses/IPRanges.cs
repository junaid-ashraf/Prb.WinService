﻿using SnmpSharpNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;

namespace Prb.ActiveDirectoryOperations.HelperClasses
{
    public static class IPRanges
    {
        /// <summary>
        /// IP Range with start and end 
        /// </summary>
        /// <param name="startIP"></param>
        /// <param name="endIP"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetIPRange(IPAddress startIP, IPAddress endIP)
        {
            uint sIP = ipToUint(startIP.GetAddressBytes());
            uint eIP = ipToUint(endIP.GetAddressBytes());
            while (sIP <= eIP)
            {
                yield return new IPAddress(reverseBytesArray(sIP)).ToString();
                sIP++;
            }
        }

        /// <summary>
        /// Reverse Byte order in array
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        /* reverse byte order in array */
        public static uint reverseBytesArray(uint ip)
        {
            byte[] bytes = BitConverter.GetBytes(ip);
            bytes = bytes.Reverse().ToArray();
            return (uint)BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// Convert bytes array to 32 bit long value
        /// </summary>
        /// <param name="ipBytes"></param>
        /// <returns></returns>
        /* Convert bytes array to 32 bit long value */
        public static uint ipToUint(byte[] ipBytes)
        {
            ByteConverter bConvert = new ByteConverter();
            uint ipUint = 0;

            int shift = 24; // indicates number of bits left for shifting
            foreach (byte b in ipBytes)
            {
                if (ipUint == 0)
                {
                    ipUint = (uint)bConvert.ConvertTo(b, typeof(uint)) << shift;
                    shift -= 8;
                    continue;
                }

                if (shift >= 8)
                    ipUint += (uint)bConvert.ConvertTo(b, typeof(uint)) << shift;
                else
                    ipUint += (uint)bConvert.ConvertTo(b, typeof(uint));

                shift -= 8;
            }

            return ipUint;
        }
    }

    public static class SNMP
    {
        public static List<string> GetName(Dictionary<Oid, AsnType> result, string ip)
        {
            //List<string> printerInfor = new List<string>();
            //foreach (KeyValuePair<Oid, AsnType> kvp in result)
            //{
            //    printerInfor.Add(kvp.Value.ToString());
            //    break;
            //}
            //return printerInfor;

            //List<string> printerInfor = new List<string> { "HP LaserJet P3005", ip, "" };
            //return printerInfor;

            List<string> printerInfor = new List<string> { "HP5261F2", ip, "" };
            return printerInfor;
        }
    }
}
