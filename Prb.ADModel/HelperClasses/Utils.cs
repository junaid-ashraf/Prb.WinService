using Prb.Common;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Prb.ActiveDirectoryOperations.HelperClasses
{
    public class Utils
    {
        /// <summary>
        /// Check wether the Ip Address is Local or not 
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static bool IsLocalIpAddress(string host)
        {
            try
            {
                IPAddress[] hostIPs = Dns.GetHostAddresses(host);
                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

                foreach (IPAddress hostIP in hostIPs)
                {
                    if (IPAddress.IsLoopback(hostIP)) return true;
                    foreach (IPAddress localIP in localIPs)
                    {
                        if (hostIP.Equals(localIP)) return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        public static void ExpectSSH(string address, string login, string password)
        {
            try
            {
                SshClient sshClient = new SshClient(address, 22, login, password);

                sshClient.Connect();
                IDictionary<Renci.SshNet.Common.TerminalModes, uint> termkvp = new Dictionary<Renci.SshNet.Common.TerminalModes, uint>();
                termkvp.Add(Renci.SshNet.Common.TerminalModes.ECHO, 53);

                ShellStream shellStream = sshClient.CreateShellStream("xterm", 80, 24, 800, 600, 1024, termkvp);


                //Get logged in
                string rep = shellStream.Expect(new Regex(@"[$>]")); //expect user prompt
                WriteTextFile.WriteErrorLog(rep);

                //send command
                shellStream.WriteLine("sudo");
                rep = shellStream.Expect(new Regex(@"([$#>:])")); //expect password or user prompt
                WriteTextFile.WriteErrorLog(rep);

                //check to send password
                if (rep.Contains(":"))
                {
                    //send password
                    shellStream.WriteLine(password);
                    rep = shellStream.Expect(new Regex(@"[$#>]")); //expect user or root prompt
                    WriteTextFile.WriteErrorLog(rep);
                }

                sshClient.Disconnect();
            }//try to open connection
            catch (Exception ex)
            {
                WriteTextFile.WriteErrorLog(ex);
            }

        }
        public static void ReadCsvFile(string filePath)
        {
            string currentLine = string.Empty;
            List<string> columns = new List<string>();
            try
            {
                if (filePath != "")
                {
                    DataTable newdt = new DataTable();
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        // currentLine will be null when the StreamReader reaches the end of file
                        int linenumber = 1;
                        DataRow workRow = null;
                        while ((currentLine = sr.ReadLine()) != null)
                        {
                            //Replace statement to remove special characters from current line.
                            currentLine = currentLine.Replace("\"\"\"", "\"");
                            currentLine = currentLine.Replace(",\"\"", ",\"");
                            currentLine = currentLine.Replace("\"\",", "\",");
                            if (currentLine[currentLine.Length - 1].ToString() == "\"")
                                currentLine = currentLine.Remove(currentLine.Length - 1);
                            //Regular expression...
                            Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                            String[] Fields = CSVParser.Split(currentLine);
                            int i = 0;
                            workRow = newdt.NewRow();
                            foreach (var field in Fields)
                            {
                                if (linenumber == 1)
                                {
                                    newdt.Columns.Add(field, typeof(string));
                                    columns.Add(field);
                                }
                                else
                                {
                                    workRow[i] = CleanInput(field);
                                    i++;
                                }
                                // log.writelog(field.ToString());
                            }
                            newdt.Rows.Add(workRow);
                            linenumber++;
                        }
                      //  int count = 0;
                        foreach (DataRow row in newdt.Rows)
                        {
                            foreach (DataColumn dc in newdt.Columns)
                            {
                                WriteTextFile.WriteErrorLog(row[dc].ToString());
                            }
                        }

                    }
                }
                else
                {
                    string msg = "There is no file to upload. Click Browse CSV file button, select file and try again";
                    Console.WriteLine(msg);
                }
            }
            catch (Exception ex)
            {
                WriteTextFile.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// Clean Input
        /// remove /
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static string CleanInput(string strIn)
        {
            // Replace invalid characters with empty strings.
            try
            {
                return strIn.Replace("\"", "");
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }
        public static Chilkat.SshKey RsaKeyPair()
        {
            Chilkat.SshKey key = new Chilkat.SshKey();

            bool success;
            int numBits;
            int exponent;
            numBits = 2048;
            exponent = 65537;
            success = key.GenerateRsaKey(numBits, exponent);
            //var sshKeyPair = new SshKeyPair();

            key.ToPuttyPrivateKey(false);
            if (!success) RsaKeyPair();
            return key;
        }
        /// <summary>
        /// Formate Size 
        /// </summary>
        /// <param name="lSize"></param>
        /// <param name="abc"></param>
        /// <returns></returns>
        public static string formatSize(Int64 lSize, bool abc = false)
        {
            //Format number to KB
            string stringSize = "";
            NumberFormatInfo myNfi = new NumberFormatInfo();
            Int64 lKBSize = 0;
            if (lSize < 1024)
            {
                if (lSize == 0)
                {
                    //zero byte
                    stringSize = "0";
                }
                else
                {
                    //less than 1K but not zero byte
                    stringSize = "1";
                }
            }
            else
            {
                //convert to KB
                lKBSize = lSize / 1024;
                //format number with default format
                stringSize = lKBSize.ToString("n", myNfi);
                //remove decimal
                stringSize = stringSize.Replace(".00", "");
            }
            return stringSize + " KB";
        }
        /// <summary>
        /// Formate Speed 
        /// </summary>
        /// <param name="lSpeed"></param>
        /// <returns></returns>
        public static string formatSpeed(Int64 lSpeed)
        {
            //Format number to Hz
            float floatSpeed = 0;
            string stringSpeed = "";
            NumberFormatInfo myNfi = new NumberFormatInfo();

            if (lSpeed < 1000)
            {
                //less than 1G Hz
                stringSpeed = lSpeed.ToString() + "M Hz";
            }
            else
            {
                //convert to Giga Hz
                floatSpeed = (float)lSpeed / 1000;
                stringSpeed = floatSpeed.ToString() + "G Hz";
            }
            return stringSpeed;
        }
    }
}