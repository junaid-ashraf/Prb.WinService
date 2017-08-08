
using Prb.DTO;
using Prb.DTO.Request;
using Prb.DTO.Response;
using Prb.SharedLibrary;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Prb.ADLinux
{
    public class LinuxInformation
    {
        /// <summary>
        /// Get Information from LInux using SSH
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="username"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static Hw_DetailInfoDTO SSHCommandForLinux(string ip, string username, string pass)
        {
            #region Varaibles and connectivity status
            // Network Discovery Info
            var LstHw_DetailInfoDTO = new List<Hw_DetailInfoDTO>();
            Hw_DetailInfoDTO oHw_DetailInfoDTO = new Hw_DetailInfoDTO();
            var oHw_DetailInfoResponse = new Hw_DetailInfoResponse();
            var oHw_DetailInfoRequest = new Hw_DetailInfoRequest();


            bool PingResult = false;
            Ping ping = new Ping();
            PingReply pingReply;
            string pingMsg = "";
            try
            {
                pingReply = ping.Send(ip, 1000);
                if (pingReply.Status == IPStatus.Success)
                    PingResult = true;

                pingMsg = "IP Ping Status: " + pingReply.Status;
            }
            catch
            {
                PingResult = false;
                pingMsg = "You IP have Some issue";
            }

            if (PingResult)
            {

            }
            else
            {
                WriteTextFile.WriteErrorLog("Not Connected due to: Machine " + pingMsg);
                oHw_DetailInfoDTO.isConnected = false;
                oHw_DetailInfoDTO.HwDescription = "Connection Exception: " + pingMsg;
                return oHw_DetailInfoDTO;
            }
            #endregion Varaibles and connectivity status


           
            try
            {
                Console.WriteLine("\n -------------------- Linux Process Start--------------------");
                WriteTextFile.WriteErrorLog("\n -------------------- Linux Process Start--------------------");
                Console.WriteLine("Enter Linux Machine IP");
                //  string ip = "192.168.22.122";
                WriteTextFile.WriteErrorLog(ip);
                Console.WriteLine("192.168.22.122");
                Console.WriteLine("Enter Username");
                Console.WriteLine("root");
                //   string username = "root";
                WriteTextFile.WriteErrorLog(username);
                Console.WriteLine("Enter Password");
                Console.WriteLine("*******");
                //  string pass = "Tech123";



                // ip = " ssh -t " + username + "@" +ip;
                SshClient client;
                client = new SshClient(ip, username, pass);
                try
                {
                    client.Connect();
                    Console.WriteLine("--- Linux Connected --- \n");
                    oHw_DetailInfoDTO.HwDescription = "Connection: Machine Connected Successfully";
                    oHw_DetailInfoDTO.isConnected = true;
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("--- Linux Not Connected --- \n " + ex.Message);
                    Console.WriteLine("--- UnExpected Error, Please Try Again--- \n" + ex.Message);
                    Console.WriteLine("\n --------------------Linux Process End--------------------");
                    oHw_DetailInfoDTO.HwDescription = "Connection Exception: " + ex.Message;
                    oHw_DetailInfoDTO.isConnected = false;
                    return oHw_DetailInfoDTO;
                    // SSHCommandForLinux();
                }

                if (client.IsConnected)
                {
                    Console.WriteLine("Command Date sets a system's date and time: date");
                    Console.WriteLine("Command user/login name associated with the current user ID: whoami");
                    Console.WriteLine("Command Display who is logged on: who");
                    Console.WriteLine("Command Display Ram Info: free -hg");
                    Console.WriteLine("Command Display System info: lscpu");
                    Console.WriteLine("Command Display Info: less /proc/meminfo");
                    Console.WriteLine("Command Display physical Stroage info: fdisk - l");

                    Console.WriteLine("Command Displays the resources being used on your system. Press q to exit: top");
                    Console.WriteLine("Command displays the name of the current operating system: uname");
                    Console.WriteLine("Command Nslookup allows a user to enter a host name and find the corresponding IP address: nslookup");
                    Console.WriteLine("Command Find searches the directory tree to find particular groups of files that meet specified conditions, including --name and --type, -exec and --size and --mtime and --user: find");
                    Console.WriteLine("Command Display the pathname for the current directory.: pwd");
                    Console.WriteLine("Command Create a new directory: mkdir");
                    Console.WriteLine("Command Clear a command line screen/window for a fresh start: clear");
                    Console.WriteLine("Command list the programs: compgen - c");
                    Console.WriteLine("Command Linux Flavour: cat /etc/*-release");
                    Console.WriteLine("Command Linux Flavour: lsb_release - a ");
                    Console.WriteLine("Command to display all running process: ps aux | less ");
                    Console.WriteLine("Command pstree shows running processes as a tree. The tree is rooted at either pid or init if pid is omitted: pstree");


                    Console.WriteLine("\n");
                    List<string> queryCommand = new List<string>();
                    queryCommand.Add("date");
                    queryCommand.Add("cat /etc/*-release");
                    queryCommand.Add("lsb_release -a");
                    queryCommand.Add("whoami");
                    queryCommand.Add("who");
                    queryCommand.Add("cat /proc/sys/kernel/{ostype,osrelease,version}");
                    queryCommand.Add("free -hg");
                    queryCommand.Add("lscpu");
                    queryCommand.Add("less /proc/meminfo");
                    queryCommand.Add("fdisk -l");

                    queryCommand.Add("service httpd status");
                    queryCommand.Add("httpd -v");
                    queryCommand.Add("rpm -qa | grep mysql");

                    queryCommand.Add("ps aux | less");
                    queryCommand.Add("compgen -c");
                    queryCommand.Add("pstree");




                    /// OS
                    string command = "";
                    SshCommand result = null;




                    /// lscpu | grep -i Architecture
                    command = "lscpu | grep -i Architecture"; //  lscpu | grep -i Architecture
                    result = client.RunCommand(command);
                    //client.Disconnect();
                    if (!string.IsNullOrEmpty(result.Result))
                    {
                        //Split the string by newlines.
                        //var lines = result.Result.Split(new string[] { "\n\t" }, StringSplitOptions.RemoveEmptyEntries);
                        try
                        {
                            oHw_DetailInfoDTO.HwSystemType = result.Result;
                            Console.WriteLine(result.Result);
                            WriteTextFile.WriteErrorLog(result.Result);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        Console.WriteLine("------------");
                        WriteTextFile.WriteErrorLog("------------");
                    }
                    else
                    {
                        Console.WriteLine("Command Responce not found");
                        WriteTextFile.WriteErrorLog("Command Responce not found");
                    }


                    command = "gcc --version";
                    //  command = "cat /proc/sys/kernel/{ostype}"; // 
                    result = client.RunCommand(command);
                    //client.Disconnect();
                    if (!string.IsNullOrEmpty(result.Result))
                    {
                        //Split the string by newlines.
                        var lines = result.Result.Split(new string[] { "\n\t" }, StringSplitOptions.RemoveEmptyEntries);
                        try
                        {
                            oHw_DetailInfoDTO.OS = lines[0];
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        Console.WriteLine(result.Result);
                        WriteTextFile.WriteErrorLog(result.Result);
                        Console.WriteLine("------------");
                        WriteTextFile.WriteErrorLog("------------");
                    }
                    else
                    {
                        Console.WriteLine("Command Responce not found");
                        WriteTextFile.WriteErrorLog("Command Responce not found");
                    }

                    //dmidecode -t 1

                    /// dmidecode -s bios-vendor
                    command = "dmidecode -s bios-vendor"; //  dmidecode -s bios-vendor
                    result = client.RunCommand(command);
                    //client.Disconnect();
                    if (!string.IsNullOrEmpty(result.Result))
                    {
                        //Split the string by newlines.
                        //var lines = result.Result.Split(new string[] { "\n\t" }, StringSplitOptions.RemoveEmptyEntries);
                        try
                        {
                            oHw_DetailInfoDTO.HwStatus = "Ok";
                            oHw_DetailInfoDTO.BiosStatus = "Ok";
                            Console.WriteLine(result.Result);
                            WriteTextFile.WriteErrorLog(result.Result);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        Console.WriteLine("------------");
                        WriteTextFile.WriteErrorLog("------------");
                    }
                    else
                    {
                        Console.WriteLine("Command Responce not found");
                        WriteTextFile.WriteErrorLog("Command Responce not found");
                    }




                    /// dmidecode -s bios-version
                    command = "dmidecode -s bios-version"; //  dmidecode -s bios-version
                    result = client.RunCommand(command);
                    //client.Disconnect();
                    if (!string.IsNullOrEmpty(result.Result))
                    {
                        //Split the string by newlines.
                        //var lines = result.Result.Split(new string[] { "\n\t" }, StringSplitOptions.RemoveEmptyEntries);
                        try
                        {
                            oHw_DetailInfoDTO.BiosVersion = result.Result;
                            Console.WriteLine(result.Result);
                            WriteTextFile.WriteErrorLog(result.Result);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        Console.WriteLine("------------");
                        WriteTextFile.WriteErrorLog("------------");
                    }
                    else
                    {
                        Console.WriteLine("Command Responce not found");
                        WriteTextFile.WriteErrorLog("Command Responce not found");
                    }
                    ///// OS Version
                    //command = "cat /proc/sys/kernel/{osrelease}"; // 
                    //oHw_DetailInfoDTO.BiosVersion = result.Result;




                    /// dmidecode -s system-manufacturer
                    command = "dmidecode -s system-manufacturer"; //  dmidecode -s system-manufacturer
                    result = client.RunCommand(command);
                    //client.Disconnect();
                    if (!string.IsNullOrEmpty(result.Result))
                    {
                        //Split the string by newlines.
                        //var lines = result.Result.Split(new string[] { "\n\t" }, StringSplitOptions.RemoveEmptyEntries);
                        try
                        {
                            oHw_DetailInfoDTO.HwManufacturer = result.Result;
                            if (result.Result.Contains("VMware"))
                            {
                                oHw_DetailInfoDTO.HwCaption = result.Result;

                                oHw_DetailInfoDTO.IsVirtual = true;
                                oHw_DetailInfoDTO.VmInstanceName = oHw_DetailInfoDTO.HwCaption;
                                oHw_DetailInfoDTO.VmInstanceStatus = oHw_DetailInfoDTO.HwStatus;
                            }
                            else
                            {
                                oHw_DetailInfoDTO.IsVirtual = false;
                            }

                            Console.WriteLine(result.Result);
                            WriteTextFile.WriteErrorLog(result.Result);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        Console.WriteLine("------------");
                        WriteTextFile.WriteErrorLog("------------");
                    }
                    else
                    {
                        Console.WriteLine("Command Responce not found");
                        WriteTextFile.WriteErrorLog("Command Responce not found");
                    }



                    /// dmidecode -s system-product-name
                    command = "dmidecode -s system-product-name"; //  dmidecode -s system-product-name
                    result = client.RunCommand(command);
                    //client.Disconnect();
                    if (!string.IsNullOrEmpty(result.Result))
                    {
                        //Split the string by newlines.
                        //var lines = result.Result.Split(new string[] { "\n\t" }, StringSplitOptions.RemoveEmptyEntries);
                        try
                        {
                            oHw_DetailInfoDTO.HwCaption = result.Result;
                            //oHw_DetailInfoDTO.HwDescription = result.Result;
                            Console.WriteLine(result.Result);
                            WriteTextFile.WriteErrorLog(result.Result);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        Console.WriteLine("------------");
                        WriteTextFile.WriteErrorLog("------------");
                    }
                    else
                    {
                        Console.WriteLine("Command Responce not found");
                        WriteTextFile.WriteErrorLog("Command Responce not found");
                    }




                    /// dmidecode -s system-version
                    command = "dmidecode -s system-version"; //  dmidecode -s system-version
                    result = client.RunCommand(command);
                    //client.Disconnect();
                    if (!string.IsNullOrEmpty(result.Result))
                    {
                        //Split the string by newlines.
                        //var lines = result.Result.Split(new string[] { "\n\t" }, StringSplitOptions.RemoveEmptyEntries);
                        try
                        {
                            oHw_DetailInfoDTO.HwModelNo = result.Result;
                            Console.WriteLine(result.Result);
                            WriteTextFile.WriteErrorLog(result.Result);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        Console.WriteLine("------------");
                        WriteTextFile.WriteErrorLog("------------");
                    }
                    else
                    {
                        Console.WriteLine("Command Responce not found");
                        WriteTextFile.WriteErrorLog("Command Responce not found");
                    }




                    /// dmidecode -s chassis-Serial-Number
                    command = "dmidecode -s chassis-Serial-Number"; //  dmidecode -s chassis-Serial-Number
                    result = client.RunCommand(command);
                    //client.Disconnect();
                    if (!string.IsNullOrEmpty(result.Result))
                    {
                        //Split the string by newlines.
                        //var lines = result.Result.Split(new string[] { "\n\t" }, StringSplitOptions.RemoveEmptyEntries);
                        try
                        {
                            oHw_DetailInfoDTO.HwSerialNo = result.Result;
                            Console.WriteLine(result.Result);
                            WriteTextFile.WriteErrorLog(result.Result);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        Console.WriteLine("------------");
                        WriteTextFile.WriteErrorLog("------------");
                    }
                    else
                    {
                        Console.WriteLine("Command Responce not found");
                        WriteTextFile.WriteErrorLog("Command Responce not found");
                    }



                    /// dmidecode -s system-serial-number
                    command = "dmidecode -s system-serial-number"; //  dmidecode -s system-serial-number
                    result = client.RunCommand(command);
                    //client.Disconnect();
                    if (!string.IsNullOrEmpty(result.Result))
                    {
                        //Split the string by newlines.
                        //var lines = result.Result.Split(new string[] { "\n\t" }, StringSplitOptions.RemoveEmptyEntries);
                        try
                        {
                            oHw_DetailInfoDTO.BiosSerialNo = result.Result;
                            Console.WriteLine(result.Result);
                            WriteTextFile.WriteErrorLog(result.Result);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        Console.WriteLine("------------");
                        WriteTextFile.WriteErrorLog("------------");
                    }
                    else
                    {
                        Console.WriteLine("Command Responce not found");
                        WriteTextFile.WriteErrorLog("Command Responce not found");
                    }




                    /// dmidecode -s system-uuid
                    command = "dmidecode -s system-uuid"; //  dmidecode -s system-uuid
                    result = client.RunCommand(command);
                    //client.Disconnect();
                    if (!string.IsNullOrEmpty(result.Result))
                    {
                        //Split the string by newlines.
                        //var lines = result.Result.Split(new string[] { "\n\t" }, StringSplitOptions.RemoveEmptyEntries);
                        try
                        {
                            oHw_DetailInfoDTO.HwDescription = result.Result;
                            Console.WriteLine(result.Result);
                            WriteTextFile.WriteErrorLog(result.Result);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        Console.WriteLine("------------");
                        WriteTextFile.WriteErrorLog("------------");
                    }
                    else
                    {
                        Console.WriteLine("Command Responce not found");
                        WriteTextFile.WriteErrorLog("Command Responce not found");
                    }


                    /// dmidecode -s baseboard-manufacturer
                    command = "dmidecode -s baseboard-manufacturer"; //  dmidecode -s baseboard-manufacturer
                    result = client.RunCommand(command);
                    //client.Disconnect();
                    if (!string.IsNullOrEmpty(result.Result))
                    {
                        //Split the string by newlines.
                        //var lines = result.Result.Split(new string[] { "\n\t" }, StringSplitOptions.RemoveEmptyEntries);
                        try
                        {
                            oHw_DetailInfoDTO.BiosName = oHw_DetailInfoDTO.BiosName + " - " + result.Result;
                            Console.WriteLine(result.Result);
                            WriteTextFile.WriteErrorLog(result.Result);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        Console.WriteLine("------------");
                        WriteTextFile.WriteErrorLog("------------");
                    }
                    else
                    {
                        Console.WriteLine("Command Responce not found");
                        WriteTextFile.WriteErrorLog("Command Responce not found");
                    }



                    /// dmidecode -s baseboard-product-name
                    command = "dmidecode -s baseboard-product-name"; //  dmidecode -s baseboard-product-name
                    result = client.RunCommand(command);
                    //client.Disconnect();
                    if (!string.IsNullOrEmpty(result.Result))
                    {
                        //Split the string by newlines.
                        //var lines = result.Result.Split(new string[] { "\n\t" }, StringSplitOptions.RemoveEmptyEntries);
                        try
                        {
                            oHw_DetailInfoDTO.BiosName = oHw_DetailInfoDTO.BiosName + " - " + result.Result;
                            Console.WriteLine(result.Result);
                            WriteTextFile.WriteErrorLog(result.Result);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        Console.WriteLine("------------");
                        WriteTextFile.WriteErrorLog("------------");
                    }
                    else
                    {
                        Console.WriteLine("Command Responce not found");
                        WriteTextFile.WriteErrorLog("Command Responce not found");
                    }



                    /// dmidecode -s baseboard-version
                    command = "dmidecode -s baseboard-version"; //  dmidecode -s baseboard-version
                    result = client.RunCommand(command);
                    //client.Disconnect();
                    if (!string.IsNullOrEmpty(result.Result))
                    {
                        //Split the string by newlines.
                        //var lines = result.Result.Split(new string[] { "\n\t" }, StringSplitOptions.RemoveEmptyEntries);
                        try
                        {
                            oHw_DetailInfoDTO.BiosVersion = result.Result;
                            Console.WriteLine(result.Result);
                            WriteTextFile.WriteErrorLog(result.Result);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        Console.WriteLine("------------");
                        WriteTextFile.WriteErrorLog("------------");
                    }
                    else
                    {
                        Console.WriteLine("Command Responce not found");
                        WriteTextFile.WriteErrorLog("Command Responce not found");
                    }


                    /// dmidecode -s chassis-asset-tag
                    command = "dmidecode -s chassis-asset-tag"; //  dmidecode -s chassis-asset-tag
                    result = client.RunCommand(command);
                    //client.Disconnect();
                    if (!string.IsNullOrEmpty(result.Result))
                    {
                        //Split the string by newlines.
                        //var lines = result.Result.Split(new string[] { "\n\t" }, StringSplitOptions.RemoveEmptyEntries);
                        try
                        {
                            oHw_DetailInfoDTO.HwPartNo = result.Result;
                            Console.WriteLine(result.Result);
                            WriteTextFile.WriteErrorLog(result.Result);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        Console.WriteLine("------------");
                        WriteTextFile.WriteErrorLog("------------");
                    }
                    else
                    {
                        Console.WriteLine("Command Responce not found");
                        WriteTextFile.WriteErrorLog("Command Responce not found");
                    }



                    ///// dmidecode -s processor-family
                    //command = "dmidecode -s processor-family"; //  dmidecode -s processor-family
                    //result = client.RunCommand(command);
                    ////client.Disconnect();
                    //if (!string.IsNullOrEmpty(result.Result))
                    //{
                    //    //Split the string by newlines.
                    //    //var lines = result.Result.Split(new string[] { "\n\t" }, StringSplitOptions.RemoveEmptyEntries);
                    //    try
                    //    {
                    //        oHw_DetailInfoDTO.pro = result.Result;
                    //        Console.WriteLine(result.Result);
                    //        WriteTextFile.WriteErrorLog(result.Result);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Console.WriteLine(ex.Message);
                    //    }
                    //    Console.WriteLine("------------");
                    //    WriteTextFile.WriteErrorLog("------------");
                    //}
                    //else
                    //{
                    //    Console.WriteLine("Command Responce not found");
                    //    WriteTextFile.WriteErrorLog("Command Responce not found");
                    //}




                    /// dmidecode -s processor-manufacturer
                    command = "dmidecode -s processor-manufacturer"; //  dmidecode -s processor-manufacturer
                    result = client.RunCommand(command);
                    //client.Disconnect();
                    if (!string.IsNullOrEmpty(result.Result))
                    {
                        //Split the string by newlines.
                        var lines = result.Result.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                        try
                        {
                            oHw_DetailInfoDTO.Processor = lines[0];
                            Console.WriteLine(result.Result);
                            WriteTextFile.WriteErrorLog(result.Result);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        Console.WriteLine("------------");
                        WriteTextFile.WriteErrorLog("------------");
                    }
                    else
                    {
                        Console.WriteLine("Command Responce not found");
                        WriteTextFile.WriteErrorLog("Command Responce not found");
                    }



                    /// dmidecode -s processor-version
                    command = "dmidecode -s processor-version"; //  dmidecode -s processor-version
                    result = client.RunCommand(command);
                    //client.Disconnect();
                    if (!string.IsNullOrEmpty(result.Result))
                    {
                        //Split the string by newlines.
                        var lines = result.Result.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                        try
                        {
                            oHw_DetailInfoDTO.Processor = oHw_DetailInfoDTO.Processor + " - " + lines[0];
                            Console.WriteLine(result.Result);
                            WriteTextFile.WriteErrorLog(result.Result);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        Console.WriteLine("------------");
                        WriteTextFile.WriteErrorLog("------------");
                    }
                    else
                    {
                        Console.WriteLine("Command Responce not found");
                        WriteTextFile.WriteErrorLog("Command Responce not found");
                    }


                    /// dmidecode -s processor-version
                    command = "dmidecode -s nproc --all"; //  dmidecode -s processor-version
                    result = client.RunCommand(command);
                    //client.Disconnect();
                    if (!string.IsNullOrEmpty(result.Result))
                    {
                        //Split the string by newlines.
                        //   var lines = result.Result.Split(new string[] { "\n\t" }, StringSplitOptions.RemoveEmptyEntries);
                        try
                        {
                            oHw_DetailInfoDTO.NoOfProcessors = int.Parse(result.Result);
                            Console.WriteLine(result.Result);
                            WriteTextFile.WriteErrorLog(result.Result);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        Console.WriteLine("------------");
                        WriteTextFile.WriteErrorLog("------------");
                    }
                    else
                    {
                        Console.WriteLine("Command Responce not found");
                        WriteTextFile.WriteErrorLog("Command Responce not found");
                    } 


                    /// Version
                    command = "dmidecode - t 1 | grep 'Version'"; //Version: 6.00
                    result = client.RunCommand(command);
                    //client.Disconnect();
                    if (!string.IsNullOrEmpty(result.Result))
                    {
                        //Split the string by newlines.
                        var lines = result.Result.Split(new string[] { "\n\t" }, StringSplitOptions.RemoveEmptyEntries);
                        try
                        {
                            oHw_DetailInfoDTO.BiosVersion = lines[0] + " - " + oHw_DetailInfoDTO.BiosVersion;
                            //oHw_DetailInfoDTO.Processor = lines[4];
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        Console.WriteLine(result.Result);
                        WriteTextFile.WriteErrorLog(result.Result);
                        Console.WriteLine("------------");
                        WriteTextFile.WriteErrorLog("------------");
                    }
                    else
                    {
                        Console.WriteLine("Command Responce not found");
                        WriteTextFile.WriteErrorLog("Command Responce not found");
                    }



                   
                    Console.WriteLine("Command Date sets a system's date and time: dmidecode - t 1 | grep 'SKU Number'");

                    /// PartNo SKU Number
                    command = "dmidecode - t 1 | grep 'SKU Number'"; // PartNo  SKU Number: Not Specified
                    result = client.RunCommand(command);
                    //client.Disconnect();
                    if (!string.IsNullOrEmpty(result.Result))
                    {
                        //Split the string by newlines.
                        try
                        {
                            oHw_DetailInfoDTO.HwPartNo = oHw_DetailInfoDTO.HwPartNo + " - " + result.Result;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        Console.WriteLine(result.Result);
                        WriteTextFile.WriteErrorLog(result.Result);
                        Console.WriteLine("------------");
                        WriteTextFile.WriteErrorLog("------------");
                    }
                    else
                    {
                        Console.WriteLine("Command Responce not found");
                        WriteTextFile.WriteErrorLog("Command Responce not found");
                    }


                    ///// UUID
                    //command = "dmidecode - t 1 | grep 'UUID'"; //UUID: C9C24D56-4905-17CE-E131-B6B1EBE4AEA9
                    //result = client.RunCommand(command);
                    ////client.Disconnect();
                    //if (!string.IsNullOrEmpty(result.Result))
                    //{
                    //    //Split the string by newlines.
                    //    var lines = result.Result.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    //    try
                    //    {
                    //        oHw_DetailInfoDTO.HwDescription = lines[0];
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Console.WriteLine(ex.Message);
                    //    }
                    //    Console.WriteLine(result.Result);
                    //    WriteTextFile.WriteErrorLog(result.Result);
                    //    Console.WriteLine("------------");
                    //    WriteTextFile.WriteErrorLog("------------");
                    //}
                    //else
                    //{
                    //    Console.WriteLine("Command Responce not found");
                    //    WriteTextFile.WriteErrorLog("Command Responce not found");
                    //}

                    /// UserName
                    command = "whoami"; // UserName
                    result = client.RunCommand(command);
                    //client.Disconnect();
                    if (!string.IsNullOrEmpty(result.Result))
                    {
                        try
                        {
                            oHw_DetailInfoDTO.UserName = result.Result;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        Console.WriteLine(result.Result);
                        WriteTextFile.WriteErrorLog(result.Result);
                        Console.WriteLine("------------");
                        WriteTextFile.WriteErrorLog("------------");
                    }
                    else
                    {
                        oHw_DetailInfoDTO.UserName = "User Not Found";
                        Console.WriteLine("------------Command Responce not found- " + command + "-----------");
                        WriteTextFile.WriteErrorLog("Command Responce not found");
                    }

                    /// hostname
                    command = "hostname"; // hostname:	
                    result = client.RunCommand(command);
                    //client.Disconnect();
                    if (!string.IsNullOrEmpty(result.Result))
                    {
                        try
                        {
                            oHw_DetailInfoDTO.DNSHostName = result.Result;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        Console.WriteLine(result.Result);
                        WriteTextFile.WriteErrorLog(result.Result);
                        Console.WriteLine("------------");
                        WriteTextFile.WriteErrorLog("------------");
                    }
                    else
                    {
                        Console.WriteLine("------------Command Responce not found- " + command + "-----------");
                        WriteTextFile.WriteErrorLog("Command Responce not found");
                    }


                    /// domianname
                    command = "domianname"; // domianname:	 ZPSLAB.LOCAL
                    result = client.RunCommand(command);
                    //client.Disconnect();
                    if (!string.IsNullOrEmpty(result.Result))
                    {
                        try
                        {
                            oHw_DetailInfoDTO.DomainName = result.Result;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        Console.WriteLine(result.Result);
                        WriteTextFile.WriteErrorLog(result.Result);
                        Console.WriteLine("------------");
                        WriteTextFile.WriteErrorLog("------------");
                    }
                    else
                    {
                        Console.WriteLine("------------Command Responce not found- " + command + "-----------");
                        WriteTextFile.WriteErrorLog("Command Responce not found");
                    }



                    /// date
                    command = "date"; // date Sat Feb 11 03:34:38 PST 2017
                    result = client.RunCommand(command);
                    //client.Disconnect();
                    if (!string.IsNullOrEmpty(result.Result))
                    {
                        try
                        {
                            oHw_DetailInfoDTO.CurrentTimeZone = result.Result;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        Console.WriteLine(result.Result);
                        WriteTextFile.WriteErrorLog(result.Result);
                        Console.WriteLine("------------");
                        WriteTextFile.WriteErrorLog("------------");
                    }
                    else
                    {
                        Console.WriteLine("------------Command Responce not found- " + command + "-----------");
                        WriteTextFile.WriteErrorLog("Command Responce not found");
                    }



                    /// Running SoftwaresList
                    /// ps aux | less
                    command = "ps aux | less"; //ps aux | less
                    result = client.RunCommand(command);
                    Hw_Sw_RunningDTO objRunningSoftwaresList;
                    var LstHw_Sw_RunningDTO = new List<Hw_Sw_RunningDTO>();
                    if (!string.IsNullOrEmpty(result.Result))
                    {
                        //Split the string by newlines.
                        var lines = result.Result.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                        try
                        {
                            for (int i = 0; i < lines.Count(); i++)
                            {
                                var rowCol = Regex.Split(lines[i], @"\s+").Where(s => s != string.Empty).ToArray();

                                objRunningSoftwaresList = new Hw_Sw_RunningDTO();
                                objRunningSoftwaresList.LicenceNo = rowCol[6];
                                objRunningSoftwaresList.Status = "Running";
                                objRunningSoftwaresList.SwDescription = rowCol[8];
                                objRunningSoftwaresList.LicenceNo = rowCol[7];
                                objRunningSoftwaresList.SwName = rowCol[10];
                                objRunningSoftwaresList.ProductName = rowCol[10];

                                LstHw_Sw_RunningDTO.Add(objRunningSoftwaresList);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        Console.WriteLine(result.Result);
                        WriteTextFile.WriteErrorLog(result.Result);
                        Console.WriteLine("------------");
                        WriteTextFile.WriteErrorLog("------------");
                    }
                    else
                    {
                        Console.WriteLine("Command Responce not found");
                        WriteTextFile.WriteErrorLog("Command Responce not found");
                    }
                    if (LstHw_Sw_RunningDTO.Count > 0)
                    {
                        oHw_DetailInfoDTO.LstHw_Sw_RunningDTO = LstHw_Sw_RunningDTO;
                    }



                    /// ps aux | less   
                    command = "ps aux | less"; //ps aux | less
                    result = client.RunCommand(command);
                    Hw_Sw_ServicesDTO objServices;
                    var LstHw_Sw_ServicesDTO = new List<Hw_Sw_ServicesDTO>();
                    if (!string.IsNullOrEmpty(result.Result))
                    {
                        //Split the string by newlines.
                        var lines = result.Result.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                        try
                        {
                            for (int i = 0; i < lines.Count(); i++)
                            {
                                var rowCol = Regex.Split(lines[i], @"\s+").Where(s => s != string.Empty).ToArray();

                                objServices = new Hw_Sw_ServicesDTO();
                                objServices.ServiceType = rowCol[6];
                                objServices.State = rowCol[7];
                                objServices.Status = "Ok";
                                objServices.Description = rowCol[8];
                                objServices.StartMode = "";
                                objServices.Caption = rowCol[10];
                                objServices.Name = rowCol[10];

                                LstHw_Sw_ServicesDTO.Add(objServices);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        Console.WriteLine(result.Result);
                        WriteTextFile.WriteErrorLog(result.Result);
                        Console.WriteLine("------------");
                        WriteTextFile.WriteErrorLog("------------");
                    }
                    else
                    {
                        Console.WriteLine("Command Responce not found");
                        WriteTextFile.WriteErrorLog("Command Responce not found");
                    }
                    if (LstHw_Sw_ServicesDTO.Count > 0)
                    {
                        oHw_DetailInfoDTO.LstHw_Sw_ServicesDTO = LstHw_Sw_ServicesDTO;
                    }


                    //If you are using a Debian or Ubuntu Linux, use the dpkg command to list installed software
                    //dpkg--get -selections
                    //nstalled software on a RHEL / Fedora / Suse / CentOS Linux
                    //  rpm -qa

                    //// List all Installed Packages using RPM  .... Redhat/CentOS/Suse/Fedora Linux, enter:
                    /// rpm -qa
                    command = "rpm -qa"; // rpm -qa
                    command = "rpm -qa --last"; // rpm - qa--last

                    result = client.RunCommand(command);
                    Hw_Sw_InstalledDTO objHw_Sw_InstalledDTO;
                    var LstobjHw_Sw_InstalledDTO = new List<Hw_Sw_InstalledDTO>();
                    if (!string.IsNullOrEmpty(result.Result))
                    {
                        //Split the string by newlines.
                        var lines = result.Result.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                        string[] ssizes = lines[0].Split(' ');
                        char[] whitespace = new char[] { ' ', '\t' };
                        var abc = lines[0].Split((whitespace));

                        try
                        {
                            for (int i = 0; i < lines.Count(); i++)
                            {
                                string string1 = lines[i].Substring(0, lines[i].IndexOf(' ') + 1).Trim();
                                string string2 = lines[i].Substring(lines[i].IndexOf(' ') + 1).Trim();

                                var rowCol = Regex.Split(lines[i], @"\s+").Where(s => s != string.Empty).ToArray();
                                objHw_Sw_InstalledDTO = new Hw_Sw_InstalledDTO();
                                objHw_Sw_InstalledDTO.SwName = string1;
                                objHw_Sw_InstalledDTO.ProductName = string1;
                                //string2=String.Format("{0:}", string2);
                                DateTime dtTemp;
                                string2 = string2.Substring(0, string2.LastIndexOf(' '));
                                if (DateTime.TryParseExact(string2, "ddd dd MMM yyyy hh:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out dtTemp))
                                    objHw_Sw_InstalledDTO.InstalledDate = dtTemp;

                                LstobjHw_Sw_InstalledDTO.Add(objHw_Sw_InstalledDTO);

                                Console.WriteLine(result.Result);
                                WriteTextFile.WriteErrorLog(result.Result);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.WriteLine(result.Result);
                            WriteTextFile.WriteErrorLog(result.Result);
                        }
                        Console.WriteLine("------------");
                        WriteTextFile.WriteErrorLog("------------");
                    }
                    else
                    {
                        try
                        {

                            //List all installed packages
                            //With version and architecture information, and description, in a table:
                            // dpkg-query -l
                            // dpkg --get
                            command = "dpkg-query -l"; // dpkg --get

                            result = client.RunCommand(command);
                            if (!string.IsNullOrEmpty(result.Result))
                            {
                                try
                                {
                                    var lines = result.Result.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                                    string[] ssizes = lines[0].Split(' ');
                                    char[] whitespace = new char[] { ' ', '\t' };
                                    var abc = lines[0].Split((whitespace));
                                    for (int i = 4; i < lines.Count(); i++)
                                    {
                                        try
                                        {
                                            string string1 = lines[i].Substring(0, lines[i].IndexOf(' ') + 1).Trim();
                                            string string2 = lines[i].Substring(lines[i].IndexOf(' ') + 1).Trim();

                                            var rowCol = Regex.Split(lines[i], @"\s+").Where(s => s != string.Empty).ToArray();

                                            var myString = Regex.Replace(lines[i], @"\s+", "  ");

                                            RegexOptions options = RegexOptions.None;
                                            Regex regex = new Regex("[ ]{2,}", options);
                                            var tempo = regex.Replace(lines[i], " ");
                                            var remainingStrings = rowCol.Skip(4);
                                            string strDesc = string.Join(" ", remainingStrings);

                                            objHw_Sw_InstalledDTO = new Hw_Sw_InstalledDTO();
                                            objHw_Sw_InstalledDTO.SwVersion = rowCol[2];
                                            objHw_Sw_InstalledDTO.ProductName = rowCol[1];
                                            objHw_Sw_InstalledDTO.SwName = rowCol[1];
                                            objHw_Sw_InstalledDTO.SwType = rowCol[3];
                                            objHw_Sw_InstalledDTO.SwDescription = strDesc;


                                            LstobjHw_Sw_InstalledDTO.Add(objHw_Sw_InstalledDTO);

                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }

                                Console.WriteLine(result.Result);
                                WriteTextFile.WriteErrorLog(result.Result);

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Command Responce not found");
                            WriteTextFile.WriteErrorLog("Command Responce not found");
                        }
                        Console.WriteLine("Command Responce not found");
                        WriteTextFile.WriteErrorLog("Command Responce not found");
                    }
                    if (LstobjHw_Sw_InstalledDTO.Count > 0)
                    {
                        oHw_DetailInfoDTO.LstHw_Sw_InstalledDTO = LstobjHw_Sw_InstalledDTO;
                    }


                    //// List all Installed Packages using apt based distro such as .... Debian/Ubuntu Linux, enter:
                    Hw_Sw_InstalledDTO objHw_Sw_InstalledDTO2;
                    var LstobjHw_Sw_InstalledDTO2 = new List<Hw_Sw_InstalledDTO>();

                    if (LstobjHw_Sw_InstalledDTO.Count == 0)
                    {
                        /// yum list installed | less
                        command = "yum list installed | less"; // yum list installed | less
                        result = client.RunCommand(command);
                        if (!string.IsNullOrEmpty(result.Result))
                        {
                            //Split the string by newlines.
                            var lines = result.Result.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                            try
                            {
                                for (int i = 0; i < lines.Count(); i++)
                                {
                                    objHw_Sw_InstalledDTO2 = new Hw_Sw_InstalledDTO();
                                    objHw_Sw_InstalledDTO2.SwName = lines[0];
                                    objHw_Sw_InstalledDTO2.ProductName = lines[0];
                                    LstobjHw_Sw_InstalledDTO2.Add(objHw_Sw_InstalledDTO2);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            Console.WriteLine(result.Result);
                            WriteTextFile.WriteErrorLog(result.Result);
                            Console.WriteLine("------------");
                            WriteTextFile.WriteErrorLog("------------");
                        }
                        else
                        {
                            Console.WriteLine("Command Responce not found");
                            WriteTextFile.WriteErrorLog("Command Responce not found");
                        }
                    }

                    if (LstobjHw_Sw_InstalledDTO2.Count > 0)
                    {
                        oHw_DetailInfoDTO.LstHw_Sw_InstalledDTO = LstobjHw_Sw_InstalledDTO2;
                    }


                    /// httpd - v
                    command = "httpd -v"; //httpd - v
                    result = client.RunCommand(command);
                    //client.Disconnect();
                    if (!string.IsNullOrEmpty(result.Result))
                    {
                        try
                        {
                            oHw_DetailInfoDTO.IsServerRole = true;
                            oHw_DetailInfoDTO.WebServer = result.Result;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        Console.WriteLine(result.Result);
                        WriteTextFile.WriteErrorLog(result.Result);
                        Console.WriteLine("------------");
                        WriteTextFile.WriteErrorLog("------------");
                    }
                    else
                    {
                        command = "apachectl -v"; //httpd - v
                        result = client.RunCommand(command);
                        //client.Disconnect();
                        if (!string.IsNullOrEmpty(result.Result))
                        {
                            try
                            {
                                oHw_DetailInfoDTO.IsServerRole = true;
                                oHw_DetailInfoDTO.WebServer = result.Result;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            Console.WriteLine(result.Result);
                            WriteTextFile.WriteErrorLog(result.Result);
                            Console.WriteLine("------------");
                            WriteTextFile.WriteErrorLog("------------");
                        }
                        else
                        {
                            oHw_DetailInfoDTO.WebServer = "Appachi Server Not Found";
                            Console.WriteLine("Command Responce not found");
                            WriteTextFile.WriteErrorLog("Command Responce not found");
                        }
                    }

                    /// rpm - qa | grep mysql
                    command = "rpm -qa | grep mysql"; //rpm - qya | grep mysql
                    result = client.RunCommand(command);
                    //client.Disconnect();
                    if (!string.IsNullOrEmpty(result.Result))
                    {
                        try
                        {
                            oHw_DetailInfoDTO.IsServerRole = true;
                            oHw_DetailInfoDTO.DatabaseServer = result.Result;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        Console.WriteLine(result.Result);
                        WriteTextFile.WriteErrorLog(result.Result);
                        Console.WriteLine("------------");
                        WriteTextFile.WriteErrorLog("------------");
                    }
                    else
                    {
                        // /etc/init.d/mysql status
                        command = "/etc/init.d/mysql status"; // /etc/init.d/mysql status
                        result = client.RunCommand(command);
                        //client.Disconnect();
                        if (!string.IsNullOrEmpty(result.Result))
                        {
                            try
                            {
                                oHw_DetailInfoDTO.IsServerRole = true;
                                oHw_DetailInfoDTO.DatabaseServer = result.Result;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            Console.WriteLine(result.Result);
                            WriteTextFile.WriteErrorLog(result.Result);
                            Console.WriteLine("------------");
                            WriteTextFile.WriteErrorLog("------------");
                        }
                        else
                        {
                            oHw_DetailInfoDTO.DatabaseServer = "Not Found";
                            Console.WriteLine("Command Responce not found");
                            WriteTextFile.WriteErrorLog("Command Responce not found");
                        }
                    }






                    //foreach (var dataRowCommand in queryCommand)
                    //{
                    //    commandTemp = dataRowCommand;
                    //    var resultTemp = client.RunCommand(commandTemp);
                    //    //client.Disconnect();
                    //    Console.WriteLine(" --- Linux Running Command: " + commandTemp + " ---");
                    //    WriteTextFile.WriteErrorLog(" --- Linux Running Command: " + commandTemp + " ---");

                    //    if (!string.IsNullOrEmpty(resultTemp.Result))
                    //    {
                    //        if (resultTemp.Result.Contains("Debian") || resultTemp.Result.Contains("Ubuntu Linux"))
                    //        {
                    //            isDebianOrUbuntu = true;
                    //            isDebianOrUbuntuCount++;
                    //        }
                    //        if (isDebianOrUbuntu == true && isDebianOrUbuntuCount == 1)
                    //        {
                    //            isDebianOrUbuntu = false;
                    //            isDebianOrUbuntuCount++;

                    //            //  queryCommand.Add("dpkg--get - selections");
                    //            Console.WriteLine(resultTemp.Result);
                    //            WriteTextFile.WriteErrorLog(resultTemp.Result);
                    //            Console.WriteLine("------------");
                    //            WriteTextFile.WriteErrorLog("------------");
                    //            // queryCommand.Add("rpm - qa");
                    //            WriteTextFile.WriteErrorLog("------------");

                    //            commandTemp = "dpkg--get - selections";
                    //            Console.WriteLine(" --- Linux Running Command: " + commandTemp + " ---");
                    //            WriteTextFile.WriteErrorLog(" --- Linux Running Command: " + commandTemp + " ---");
                    //            resultTemp = client.RunCommand(commandTemp);
                    //        }
                    //        else if (resultTemp.Result.Contains("RHEL") || resultTemp.Result.Contains("Fedora") || resultTemp.Result.Contains("Suse") || resultTemp.Result.Contains("CentOS Linux"))
                    //        {
                    //            isRhelFedoraSuseCentOSLinux = true;
                    //            isRhelFedoraSuseCentOSLinuxCount++;
                    //        }
                    //        if (isRhelFedoraSuseCentOSLinux == true && isRhelFedoraSuseCentOSLinuxCount == 1)
                    //        {
                    //            isRhelFedoraSuseCentOSLinux = false;
                    //            isRhelFedoraSuseCentOSLinuxCount++;

                    //            Console.WriteLine(resultTemp.Result);
                    //            WriteTextFile.WriteErrorLog(resultTemp.Result);
                    //            Console.WriteLine("------------");
                    //            WriteTextFile.WriteErrorLog("------------");
                    //            // queryCommand.Add("rpm - qa");
                    //            WriteTextFile.WriteErrorLog("------------");

                    //            commandTemp = "rpm - qa";
                    //            Console.WriteLine(" --- Linux Running Command: " + commandTemp + " ---");
                    //            resultTemp = client.RunCommand(commandTemp);
                    //        }
                    //        Console.WriteLine(resultTemp.Result);
                    //        WriteTextFile.WriteErrorLog(resultTemp.Result);
                    //        Console.WriteLine("------------");
                    //        WriteTextFile.WriteErrorLog("------------");
                    //    }
                    //    else
                    //    {
                    //        WriteTextFile.WriteErrorLog(" --- MAC Running Command: " + commandTemp + " | Response not found---");
                    //        Console.WriteLine(" --- MAC Running Command: " + commandTemp + " | Response not found---");
                    //    }
                    //}

                    //Console.WriteLine("\n Enter Linux Command:");
                    //string command = Console.ReadLine();
                    //var result = client.RunCommand(command);
                    //Console.WriteLine(" --- Linux Running Command: " + command + " ---");
                    //WriteTextFile.WriteErrorLog(" --- Linux Running Command: " + command + " ---");
                    ////client.Disconnect();
                    //if (!string.IsNullOrEmpty(result.Result))
                    //{
                    //    Console.WriteLine(result.Result);
                    //    WriteTextFile.WriteErrorLog(result.Result);
                    //    Console.WriteLine("------------");
                    //    WriteTextFile.WriteErrorLog("------------");
                    //}
                    //else
                    //{
                    //    Console.WriteLine("Command Responce not found");
                    //    WriteTextFile.WriteErrorLog("Command Responce not found");

                    //}
                    //Console.WriteLine("\n Press Y to Continue and N to exit");
                    //option = Console.ReadLine();
                    //option = option.ToUpper();
                    //if (option != "Y")
                    //{
                    //    WriteTextFile.WriteErrorLog("Command Responce not found");
                    //    Console.WriteLine("Command Responce not found");
                    //}
                    client.Disconnect();
                }
                else
                {
                    Console.WriteLine("--- Linux Not Connected ---");
                }
                Console.WriteLine("\n -------------------- Linux Process End-------------------- \n");
                WriteTextFile.WriteErrorLog("\n -------------------- Linux Process End--------------------");
            }
            catch (Exception)
            {
                Console.WriteLine("--- UnExpected Error, Please Try Again---");
                // SSHCommandForLinux();
            }
            return oHw_DetailInfoDTO;
        }
    }
}
