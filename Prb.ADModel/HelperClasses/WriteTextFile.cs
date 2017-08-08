using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prb.Common
{
    public class WriteTextFile
    {
        /// <summary>
        /// Write to Log File with full exception 
        /// </summary>
        /// <param name="ex"></param>
        public static void WriteErrorLog(Exception ex)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                //sw = new StreamWriter("C:\\Mansoor\\WithSqlite\\Prb.WinService\\Prb.ADWinService\\bin\\Debug\\LogFile.txt", true);

                sw.WriteLine(DateTime.Now.ToString() + ": " + ex.Source.ToString().Trim() + "; " + ex.Message.ToString().Trim());
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }

        public static void EmptyErrorLog()
        {
            try
            {
                 //File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", string.Empty);
                 File.WriteAllText("C:\\Mansoor\\WithSqlite\\Prb.WinService\\Prb.ADWinService\\bin\\Debug\\LogFile.txt", string.Empty);
                //sw = new StreamWriter("C:\\Mansoor\\WithSqlite\\Prb.WinService\\Prb.ADWinService\\bin\\Debug\\LogFile.txt", true);
            }
            catch
            {
            }
        }
        /// <summary>
        /// Write to Log File with message
        /// </summary>
        /// <param name="Message"></param>
        public static void WriteErrorLog(string Message)
        {
            StreamWriter sw = null;
            try
            {
                //sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw = new StreamWriter("C:\\Mansoor\\WithSqlite\\Prb.WinService\\Prb.ADWinService\\bin\\Debug\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + Message + "\n");
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }
        public static void WriteErrorLogWithoutDate(string Message)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(Message);
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }
    }
}
