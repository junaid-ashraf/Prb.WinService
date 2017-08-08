
using Prb.DTO;
using Prb.DTO.Request;
using Prb.DTO.Response;
using Prb.SharedLibrary;
using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Prb.ActiveDirectoryOperations.HelperClasses
{
    public class MacInformation
    {
        private static string MacPassword = "";
        public static void HandleKeyEvent(Object sender, AuthenticationPromptEventArgs e)
        {
            foreach (AuthenticationPrompt prompt in e.Prompts)
            {
                if (prompt.Request.IndexOf("Password:", StringComparison.InvariantCultureIgnoreCase) != -1)
                {
                    prompt.Response = MacPassword;
                }
            }
        }
        public static Hw_DetailInfoDTO SSHCommandForMAC(string ip, string username, string pass)
        {
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


            try
            {
                Console.WriteLine("\n -------------------- MAC Process Start--------------------");

                // LogTitle.Add("\n -------------------- MAC Process Start--------------------");
                // string option = "";
                Console.WriteLine("Enter MAC Machine IP:");
                // string ip = Console.ReadLine();
                Console.WriteLine("Enter UserName:");
                // string username = Console.ReadLine();
                Console.WriteLine("Enter Password:");
                // string pass = Console.ReadLine();
                MacPassword = pass;
                
                KeyboardInteractiveAuthenticationMethod kauth = new KeyboardInteractiveAuthenticationMethod(username);
                PasswordAuthenticationMethod pauth = new PasswordAuthenticationMethod(username, pass);
                kauth.AuthenticationPrompt += new EventHandler<AuthenticationPromptEventArgs>(HandleKeyEvent);
                ConnectionInfo connectionInfo = new ConnectionInfo(ip, 22, username, pauth, kauth);
                using (var client = new SshClient(connectionInfo))
                {
                    try
                    {
                        client.Connect();
                        //Console.WriteLine("--- MAC Connected --- \n");
                        WriteTextFile.WriteErrorLog("--- MAC Connected --- ");
                        oHw_DetailInfoDTO.HwDescription = oHw_DetailInfoDTO.HwDescription + "Connection: Machine Connected Successfully";
                        oHw_DetailInfoDTO.isConnected = true;
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine("--- Linux Not Connected --- \n " + ex.Message);
                        //Console.WriteLine("--- UnExpected Error, Please Try Again--- \n" + ex.Message);
                        WriteTextFile.WriteErrorLog("--- UnExpected Error, Please Try Again--- \n" + ex.Message);
                        WriteTextFile.WriteErrorLog("-------------------- MAC info Missing--------------------");
                        oHw_DetailInfoDTO.HwDescription = "Connection Exception: " + ex.Message;
                        oHw_DetailInfoDTO.isConnected = false;
                        return oHw_DetailInfoDTO;
                    }

                    if (client.IsConnected)
                    {
                        WriteTextFile.WriteErrorLog("");
                        WriteTextFile.WriteErrorLog("Command Date sets a system's date and time: date");
                        WriteTextFile.WriteErrorLog("Command Show Hostname: hostname");
                        WriteTextFile.WriteErrorLog("Command Show Mac OS X operating system version: sw_vers");
                        WriteTextFile.WriteErrorLog("Command Show operating system name and more: uname");
                        WriteTextFile.WriteErrorLog("displays disk usage information based on file system (ie: entire drives, attached media");
                        WriteTextFile.WriteErrorLog("Command Apple hardware and software configuration: system_profiler");
                        WriteTextFile.WriteErrorLog("Command Apple hardware and software configuration: system_profiler | less");
                        WriteTextFile.WriteErrorLog("Command Apple hardware and software configuration: system_profiler SPSoftwareDataType");
                        WriteTextFile.WriteErrorLog("Command user/login name associated with the current user ID: whoami");
                        WriteTextFile.WriteErrorLog("Command Display who is logged on: who");
                        WriteTextFile.WriteErrorLog("Command displays the name of the current operating system: uname");
                        WriteTextFile.WriteErrorLog("Command Clear a command line screen/window for a fresh start: clear");
                        WriteTextFile.WriteErrorLog("Command to display all running process: ps aux | less ");
                        WriteTextFile.WriteErrorLog("Command To find the Apache version: httpd -v");
                        WriteTextFile.WriteErrorLog("\n");


                        Console.WriteLine("Command Date sets a system's date and time: date");
                        Console.WriteLine("Command Show Hostname: hostname");
                        Console.WriteLine("Command Show Mac OS X operating system version: sw_vers");
                        Console.WriteLine("Command Show operating system name and more: uname");
                        Console.WriteLine("displays disk usage information based on file system (ie: entire drives, attached media");
                        Console.WriteLine("Command Apple hardware and software configuration: system_profiler");
                        Console.WriteLine("Command Apple hardware and software configuration: system_profiler | less");
                        Console.WriteLine("Command Apple hardware and software configuration: system_profiler SPSoftwareDataType");
                        Console.WriteLine("Command user/login name associated with the current user ID: whoami");
                        Console.WriteLine("Command Display who is logged on: who");
                        Console.WriteLine("Command displays the name of the current operating system: uname");
                        Console.WriteLine("Command Clear a command line screen/window for a fresh start: clear");
                        Console.WriteLine("Command to display all running process: ps aux | less ");
                        Console.WriteLine("Command To find the Apache version: httpd -v");


                        Console.WriteLine("\n");
                        List<string> queryCommand = new List<string>();
                        queryCommand.Add("date");
                        queryCommand.Add("hostname");
                        queryCommand.Add("sw_vers");
                        //   sw_vers-- -
                        // ProductName:	Mac OS X
                        // ProductVersion:	10.10.5
                        // BuildVersion: 14F27
                        queryCommand.Add("sw_vers -productName");
                        queryCommand.Add("sw_vers -productVersion");
                        queryCommand.Add("sw_vers -BuildVersion");
                        queryCommand.Add("uname");
                        queryCommand.Add("system_profiler | grep 'Serial Number (system)'");
                        queryCommand.Add("system_profiler | grep 'Model Name'");
                        queryCommand.Add("system_profiler | grep 'Model Identifier'");
                        queryCommand.Add("system_profiler | grep 'Processor Name'");
                        queryCommand.Add("system_profiler | grep 'Processor Speed'");
                        queryCommand.Add("system_profiler | grep 'Number of Processors'");
                        queryCommand.Add("system_profiler | grep 'Total Number of Cores'");
                        queryCommand.Add("system_profiler | grep 'L2 Cache'");
                        queryCommand.Add("system_profiler | grep 'Bus Speed'");
                        queryCommand.Add("system_profiler | grep 'Hardware UUID'");



                        queryCommand.Add("df - h");
                        queryCommand.Add("y");
                        queryCommand.Add("diskutil info");

                        //queryCommand.Add("system_profile");
                        queryCommand.Add("system_profiler Graphics/Displays");
                        queryCommand.Add("system_profiler | grep Processor");
                        queryCommand.Add("system_profiler | less");
                        queryCommand.Add("system_profiler SPSoftwareDataType");

                        queryCommand.Add("sysctl -a | grep cpu");
                        queryCommand.Add("hwprefs cpu_count");
                        queryCommand.Add("hwprefs memory_size");

                        queryCommand.Add("whoami");
                        queryCommand.Add("who");

                        queryCommand.Add("service httpd status");
                        queryCommand.Add("httpd -v");
                        queryCommand.Add("rpm -qa | grep mysql");

                        // Running Processy
                        queryCommand.Add("ps aux | less");
                        // View All Running Processes with Terminal
                        // To sort by cpu usage
                        queryCommand.Add("top -o cpu");
                        //  Sort top by memory usage:
                        //  queryCommand.Add("top -o rsize");


                        Console.WriteLine("Command to display all running Services: launchctl list");
                        queryCommand.Add("launchctl list");

                        //
                        //To search for a specific process (or application name, for that matter), you can use grep like so:
                        //ps aux|grep process
                        //Or to look for applications:
                        //ps aux|grep "Application Name"
                        //  queryCommand.Add("y");



                        //If you are using a Debian or Ubuntu Linux, use the dpkg command to list installed software
                        //dpkg--get - selections
                        //nstalled software on a RHEL / Fedora / Suse / CentOS Linux
                        //  rpm - qa





                        /// hostname
                        string command = "hostname"; // hostname:	
                        var result = client.RunCommand(command);
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
                            Console.WriteLine("------------");
                            Console.WriteLine("------------Command Responce not found- " + command + "-----------");
                            WriteTextFile.WriteErrorLog("Command Responce not found");
                        }



                        try
                        {
                            // Services
                            /// ps aux | less
                            command = "launchctl list"; //ps aux | less
                            result = client.RunCommand(command);
                            Hw_Sw_ServicesDTO objServices;
                            var LstHw_Sw_ServicesDTO = new List<Hw_Sw_ServicesDTO>();
                            if (!string.IsNullOrEmpty(result.Result))
                            {
                                try
                                {
                                    //Split the string by newlines.
                                    var Resultlines = result.Result.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                                    try
                                    {
                                        for (int i = 0; i < Resultlines.Count(); i++)
                                        {
                                            objServices = new Hw_Sw_ServicesDTO();
                                            var rowCol = Regex.Split(Resultlines[i], @"\t").Where(s => s != string.Empty).ToArray();

                                            objServices = new Hw_Sw_ServicesDTO();
                                            objServices.StartName = rowCol[0];
                                            objServices.State = rowCol[1];
                                            objServices.Status = rowCol[1];
                                            objServices.Description = "";
                                            objServices.Caption = rowCol[2];
                                            objServices.Name = rowCol[2];

                                            LstHw_Sw_ServicesDTO.Add(objServices);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
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
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Ip Address Exception: " + ex.Message);
                            WriteTextFile.WriteErrorLog("Ip Address Exception: " + ex.Message);
                        }












                        /// SerialNumber (system)
                        command = "system_profiler | grep 'Serial Number (system)'"; // SerialNumber (system): W89520H85PC
                        result = client.RunCommand(command);
                        //client.Disconnect();
                        if (!string.IsNullOrEmpty(result.Result))
                        {
                            try
                            {
                                oHw_DetailInfoDTO.HwSerialNo = result.Result;
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
                            Console.WriteLine("------------");
                            Console.WriteLine("------------Command Responce not found- " + command + "-----------");
                            WriteTextFile.WriteErrorLog("Command Responce not found");
                        }


                        /// SerialNumber (system)
                        command = "system_profiler | grep 'Model Identifier'"; // SerialNumber (system): W89520H85PC
                        result = client.RunCommand(command);
                        //client.Disconnect();
                        if (!string.IsNullOrEmpty(result.Result))
                        {
                            try
                            {
                                oHw_DetailInfoDTO.HwModelNo = result.Result;
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
                            Console.WriteLine("------------");
                            Console.WriteLine("------------Command Responce not found- " + command + "-----------");
                            WriteTextFile.WriteErrorLog("Command Responce not found");
                        }




                        /// OS
                        command = ("system_profiler SPSoftwareDataType | grep 'System Version'"); //                         /// OS
                        result = client.RunCommand(command);
                        //client.Disconnect();
                        if (!string.IsNullOrEmpty(result.Result))
                        {
                            try
                            {
                                var re = result.Result.Split(':');
                                oHw_DetailInfoDTO.OS = re[1];
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
                            Console.WriteLine("------------");
                            Console.WriteLine("------------Command Responce not found- " + command + "-----------");
                            WriteTextFile.WriteErrorLog("Command Responce not found");
                        }



                        /// OS
                        //command = ("sw_vers -productVersion"); // 


                        ///// OS
                        //command = ("defaults read loginwindow SystemVersionStampAsString"); // OS
                        //result = client.RunCommand(command);
                        ////client.Disconnect();
                        //if (!string.IsNullOrEmpty(result.Result))
                        //{
                        //    try
                        //    {
                        //        oHw_DetailInfoDTO.OS = result.Result;
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
                        //    Console.WriteLine("------------");
                        //    Console.WriteLine("------------Command Responce not found- " + command + "-----------");
                        //    WriteTextFile.WriteErrorLog("Command Responce not found");
                        //}


                        //command = ("sw_vers -productName"); // 
                        //result = client.RunCommand(command);
                        ////client.Disconnect();
                        //if (!string.IsNullOrEmpty(result.Result))
                        //{
                        //    try
                        //    {
                        //        oHw_DetailInfoDTO.OS = result.Result;
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
                        //    Console.WriteLine("------------");
                        //    Console.WriteLine("------------Command Responce not found- " + command + "-----------");
                        //    WriteTextFile.WriteErrorLog("Command Responce not found");
                        //}



                        /// OS
                        command = ("sw_vers -buildVersion"); // 
                        result = client.RunCommand(command);
                        //client.Disconnect();
                        if (!string.IsNullOrEmpty(result.Result))
                        {
                            try
                            {
                                oHw_DetailInfoDTO.BiosVersion = result.Result;
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
                            Console.WriteLine("------------");
                            Console.WriteLine("------------Command Responce not found- " + command + "-----------");
                            WriteTextFile.WriteErrorLog("Command Responce not found");
                        }


                        /// OS
                        command = ("sw_vers -productName"); // 
                        result = client.RunCommand(command);
                        //client.Disconnect();
                        if (!string.IsNullOrEmpty(result.Result))
                        {
                            try
                            {
                                oHw_DetailInfoDTO.HwCaption = result.Result;
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
                            Console.WriteLine("------------");
                            Console.WriteLine("------------Command Responce not found- " + command + "-----------");
                            WriteTextFile.WriteErrorLog("Command Responce not found");
                        }


                        /// SerialNumber (system)
                        command = ("system_profiler | grep 'Hardware UUID'"); // SerialNumber (system): W89520H85PC
                        result = client.RunCommand(command);
                        //client.Disconnect();
                        if (!string.IsNullOrEmpty(result.Result))
                        {
                            try
                            {
                                oHw_DetailInfoDTO.HwSerialNo = result.Result;
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
                            Console.WriteLine("------------");
                            Console.WriteLine("------------Command Responce not found- " + command + "-----------");
                            WriteTextFile.WriteErrorLog("Command Responce not found");
                        }



                        /// Processor Name
                        command = "system_profiler | grep 'Processor Name'"; // Processor Name:	10.10.5
                        result = client.RunCommand(command);
                        //client.Disconnect();
                        if (!string.IsNullOrEmpty(result.Result))
                        {
                            try
                            {
                                oHw_DetailInfoDTO.Processor = result.Result;
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




                        /// Number of Processors
                        command = "system_profiler | grep 'Number of Processors'"; //Number of Processors: 1
                        result = client.RunCommand(command);
                        //client.Disconnect();
                        if (!string.IsNullOrEmpty(result.Result))
                        {
                            var re = result.Result.Split(',');
                            try
                            {
                                oHw_DetailInfoDTO.NoOfProcessors = int.Parse(re[1]);
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




                        // Hardware UUID
                        command = "system_profiler | grep 'Hardware UUID'"; // Hardware UUID
                        result = client.RunCommand(command);
                        //client.Disconnect();
                        if (!string.IsNullOrEmpty(result.Result))
                        {
                            try
                            {
                                oHw_DetailInfoDTO.BiosSerialNo = result.Result;
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




                        try
                        {
                            ////  pkgutil --pkgs    will list all the packages installed with Apple's installer
                            /// pkgutil --pkgs
                            command = "pkgutil --pkgs"; // pkgutil--pkgs
                            result = client.RunCommand(command);
                            Hw_Sw_InstalledDTO ObjHw_Sw_InstalledDTO;
                            var LstHw_Sw_InstalledDTO = new List<Hw_Sw_InstalledDTO>();
                            if (!string.IsNullOrEmpty(result.Result))
                            {
                                try
                                {
                                    var resString = result.Result.Split('\n');
                                    for (int i = 0; i < resString.Count(); i++)
                                    {
                                        try
                                        {
                                            ObjHw_Sw_InstalledDTO = new Hw_Sw_InstalledDTO();
                                            ObjHw_Sw_InstalledDTO.SwName = resString[i];
                                            LstHw_Sw_InstalledDTO.Add(ObjHw_Sw_InstalledDTO);
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
                                Console.WriteLine("------------");
                                WriteTextFile.WriteErrorLog("------------");
                            }
                            else
                            {
                                Console.WriteLine("Command Responce not found");
                                WriteTextFile.WriteErrorLog("Command Responce not found");
                            }
                            if (LstHw_Sw_InstalledDTO.Count > 0)
                            {
                                oHw_DetailInfoDTO.LstHw_Sw_InstalledDTO = LstHw_Sw_InstalledDTO;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Ip Address Exception: " + ex.Message);
                            WriteTextFile.WriteErrorLog("Ip Address Exception: " + ex.Message);
                        }




                        try
                        {
                            /// Process and Application
                            /// // ps -ef
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
                                        try
                                        {


                                            string string0 = lines[i].Substring(lines[i].IndexOf(' ') + 1).Trim();
                                            string string1 = string0.Substring(string0.IndexOf(' ') + 1).Trim();
                                            string string2 = string1.Substring(string1.IndexOf(' ') + 1).Trim();
                                            string string3 = string2.Substring(string2.IndexOf(' ') + 1).Trim();
                                            string string4 = string3.Substring(string3.IndexOf(' ') + 1).Trim();
                                            string string5 = string4.Substring(string4.IndexOf(' ') + 1).Trim();
                                            string string6 = string5.Substring(string5.IndexOf(' ') + 1).Trim();
                                            string string7 = string6.Substring(string6.IndexOf(' ') + 1).Trim();
                                            string string8 = string7.Substring(string7.IndexOf(' ') + 1).Trim();
                                            string string9 = string8.Substring(string8.IndexOf(' ') + 1).Trim();
                                            //string string10 = string9.Substring(string9.IndexOf(' ') + 1).Trim();
                                            var name = string9.Split('/');
                                            var lastname = name.Last();
                                            string[] actuallastnameCol;
                                            var actuallastname = "";
                                            try
                                            {

                                                actuallastnameCol = lastname.Split('-');
                                                actuallastname = actuallastnameCol.First();

                                            }
                                            catch (Exception ex)
                                            {

                                            }


                                            objRunningSoftwaresList = new Hw_Sw_RunningDTO();
                                            //objRunningSoftwaresList.LicenceNo = rowCol[6];
                                            objRunningSoftwaresList.Status = rowCol[7];
                                            objRunningSoftwaresList.SwDescription = string9;
                                            objRunningSoftwaresList.SwName = actuallastname;
                                            objRunningSoftwaresList.ProductName = actuallastname;

                                            try
                                            {

                                                DateTime temp;
                                                if (DateTime.TryParse(rowCol[8], out temp))
                                                {
                                                    objRunningSoftwaresList.InstalledDate = temp;
                                                }


                                            }
                                            catch (Exception ex)
                                            {

                                            }

                                            LstHw_Sw_RunningDTO.Add(objRunningSoftwaresList);
                                        }
                                        catch (Exception ex)
                                        {

                                        }
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
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Exception: " + ex.Message);
                            WriteTextFile.WriteErrorLog("Exception: " + ex.Message);
                        }




                        /// Services Tag Here





                        try
                        {
                            Console.WriteLine("Command to display all IP's : ifconfig | grep inet");
                            queryCommand.Add("ifconfig | grep inet");
                            // Ip Address
                            /// ifconfig | grep inet
                            var LstHw_IpMacAddressDTO = new List<Hw_IpMacAddressDTO>();
                            Hw_IpMacAddressDTO objHw_IpMacAddressDTO = new Hw_IpMacAddressDTO();
                            var oHw_IpMacAddressResponse = new Hw_DetailInfoResponse();
                            var oHw_IpMacAddressRequest = new Hw_IpMacAddressRequest();
                            command = "ifconfig | grep inet"; // ifconfig | grep inet
                            result = client.RunCommand(command);
                            if (!string.IsNullOrEmpty(result.Result))
                            {
                                try
                                {
                                    objHw_IpMacAddressDTO.Caption = result.Result;
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

                            LstHw_IpMacAddressDTO.Add(objHw_IpMacAddressDTO);
                            if (LstHw_IpMacAddressDTO.Count > 0)
                                oHw_DetailInfoDTO.LstHw_IpMacAddressDTO = LstHw_IpMacAddressDTO;

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Ip Address Exception: " + ex.Message);
                            WriteTextFile.WriteErrorLog("Ip Address Exception: " + ex.Message);
                        }


                        //command = ""; // 
                        //result = client.RunCommand(command);
                        ////client.Disconnect();
                        //if (!string.IsNullOrEmpty(result.Result))
                        //{
                        //    try
                        //    {
                        //        oHw_DetailInfoDTO.BiosVersion = result.Result;
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
                        //    Console.WriteLine("------------Command Responce not found- " + command + "-----------");
                        //    WriteTextFile.WriteErrorLog("Command Responce not found");
                        //}








                        ///// 
                        //command = ""; // 
                        //result = client.RunCommand(command);
                        ////client.Disconnect();
                        //if (!string.IsNullOrEmpty(result.Result))
                        //{
                        //    try
                        //    {
                        //        oHw_DetailInfoDTO.BiosVersion = result.Result;
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
                        //    Console.WriteLine("------------Command Responce not found- " + command + "-----------");
                        //    WriteTextFile.WriteErrorLog("Command Responce not found");
                        //}


                        //command = ""; // 
                        //result = client.RunCommand(command);
                        ////client.Disconnect();
                        //if (!string.IsNullOrEmpty(result.Result))
                        //{
                        //    try
                        //    {
                        //        oHw_DetailInfoDTO.BiosVersion = result.Result;
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
                        //    Console.WriteLine("------------Command Responce not found- " + command + "-----------");
                        //    WriteTextFile.WriteErrorLog("Command Responce not found");
                        //}




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



                        /// ProductVersion
                        command = "sw_vers -ProductVersion'"; // ProductVersion:	10.10.5
                        result = client.RunCommand(command);
                        //client.Disconnect();
                        if (!string.IsNullOrEmpty(result.Result))
                        {
                            try
                            {
                                oHw_DetailInfoDTO.BiosVersion = result.Result;
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




                        /// BuildVersion
                        command = "sw_vers -BuildVersion'"; // BuildVersion:	14F27
                        result = client.RunCommand(command);
                        //client.Disconnect();
                        if (!string.IsNullOrEmpty(result.Result))
                        {
                            try
                            {
                                oHw_DetailInfoDTO.UserName = result.Result;
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
                            Console.WriteLine("------------Command Responce not found- " + command + "-----------");
                            WriteTextFile.WriteErrorLog("Command Responce not found");
                        }




                        /// Serial Number
                        command = "system_profiler | grep 'Serial Number (system)'"; // Serial Number:	14F27
                        result = client.RunCommand(command);
                        //client.Disconnect();
                        if (!string.IsNullOrEmpty(result.Result))
                        {
                            try
                            {
                                oHw_DetailInfoDTO.HwSerialNo = result.Result;
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



                        /// Processor
                        command = "system_profiler | grep Processor"; // Processor:	14F27
                        result = client.RunCommand(command);
                        //client.Disconnect();
                        if (!string.IsNullOrEmpty(result.Result))
                        {
                            try
                            {
                                oHw_DetailInfoDTO.Processor = result.Result;
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

                        /// Processor
                        command = "system_profiler | grep Processor"; // Processor:	14F27
                        result = client.RunCommand(command);
                        //client.Disconnect();
                        if (!string.IsNullOrEmpty(result.Result))
                        {
                            try
                            {
                                oHw_DetailInfoDTO.Processor = result.Result;
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
                            Console.WriteLine("------------Command Responce not found- " + command + "-----------");
                            WriteTextFile.WriteErrorLog("Command Responce not found");
                        }


                        /// BiosType
                        command = "uname"; // BiosType : Darwin
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
                            Console.WriteLine("------------Command Responce not found- " + command + "-----------");
                            WriteTextFile.WriteErrorLog("Command Responce not found");
                        }



                        /// HTTP Server
                        command = "httpd - v"; //  HTTP Server
                        result = client.RunCommand(command);
                        //client.Disconnect();
                        if (!string.IsNullOrEmpty(result.Result))
                        {
                            try
                            {
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
                            Console.WriteLine("------------Command Responce not found- " + command + "-----------");
                            WriteTextFile.WriteErrorLog("Command Responce not found");
                        }


                        /// Database Server
                        command = "rpm -qa | grep mysql"; // Database Server
                        result = client.RunCommand(command);
                        //client.Disconnect();
                        if (!string.IsNullOrEmpty(result.Result))
                        {
                            try
                            {
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
                            Console.WriteLine("------------Command Responce not found- " + command + "-----------");
                            WriteTextFile.WriteErrorLog("Command Responce not found");
                        }


                        /// Database Server
                        command = "rpm -qa | grep mysql"; // Database Server
                        result = client.RunCommand(command);
                        //client.Disconnect();
                        if (!string.IsNullOrEmpty(result.Result))
                        {
                            try
                            {
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
                            Console.WriteLine("------------Command Responce not found- " + command + "-----------");
                            WriteTextFile.WriteErrorLog("Command Responce not found");
                        }























                        //    option = "Y";
                        //    string commandTemp = "";

                        //    // One Time Response
                        //    foreach (var dataRowCommand in queryCommand)
                        //    {
                        //        commandTemp = dataRowCommand;
                        //        var resultTemp = client.RunCommand(commandTemp);
                        //        //client.Disconnect();
                        //        Console.WriteLine(" --- MAC Running Command: " + commandTemp + " ---");
                        //        WriteTextFile.WriteErrorLog(" --- MAC Running Command: " + commandTemp + " ---");

                        //        //   LogTitle.Add(" --- MAC Running Command: " + commandTemp + " ---");

                        //        if (!string.IsNullOrEmpty(resultTemp.Result))
                        //        {
                        //            Console.WriteLine(resultTemp.Result);
                        //            WriteTextFile.WriteErrorLog(resultTemp.Result);
                        //            WriteTextFile.WriteErrorLog("");
                        //            // LogTitle.Add(resultTemp.Result);
                        //            Console.WriteLine("------------");
                        //            // LogTitle.Add("------------");
                        //        }
                        //        else
                        //        {
                        //            WriteTextFile.WriteErrorLog(" --- MAC Running Command: " + commandTemp + " | Response not found---");
                        //            Console.WriteLine("Command Response not found");
                        //            // LogTitle.Add("Command Response not found");
                        //        }
                        //    }


                        //    //while (option == "Y")
                        //    //{
                        //    //    Console.WriteLine("\n Enter MAC Command:");
                        //    //    string command = Console.ReadLine();
                        //    //    var result = client.RunCommand(command);
                        //    //    Console.WriteLine(" --- MAC Running Command: " + command + " ---");
                        //    //    // LogTitle.Add(" --- MAC Running Command: " + command + " ---");
                        //    //    //client.Disconnect();
                        //    //    if (!string.IsNullOrEmpty(result.Result))
                        //    //    {
                        //    //        Console.WriteLine(result.Result);
                        //    //        //   LogTitle.Add(result.Result);
                        //    //        Console.WriteLine("------------");
                        //    //        //   LogTitle.Add("------------");
                        //    //    }
                        //    //    else
                        //    //    {
                        //    //        Console.WriteLine("Command Response not found");
                        //    //        // LogTitle.Add("Command Response not found");

                        //    //    }
                        //    //  Console.WriteLine("\n Press Y to Continue and N to exit");
                        //    //  option = Console.ReadLine();
                        //    //  option = option.ToUpper();
                        //    //  if (option != "Y")
                        //    // {
                        //    //    // LogTitle.Add("Command Response not found");
                        //    //   Console.WriteLine("Command Response not found");
                        //    // }
                        //    // }
                        //    client.Disconnect();
                    }
                    else
                    {
                        WriteTextFile.WriteErrorLog("--- MAC Not Connected ---");
                        Console.WriteLine("--- MAC Not Connected ---");
                    }
                    //  LogTitle.Add("\n -------------------- MAC Process End--------------------");
                    // }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("--- UnExpected Error, Please Try Again--- \n" + ex.Message);
                WriteTextFile.WriteErrorLog("--- UnExpected Error, Please Try Again--- \n" + ex.Message);
                // SSHCommandForLinux();
            }
            //  Export("RemoteMacInfo");
            return oHw_DetailInfoDTO;
        }
    }
}