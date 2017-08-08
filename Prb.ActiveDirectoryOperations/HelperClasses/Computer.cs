using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prb.ActiveDirectoryOperations.HelperClasses
{
    public class Computer
    {
        public String SamAccountName { get; set; }
        public String DisplayName { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public Boolean? Enabled { get; set; }
        public DateTime? LastLogon { get; set; }

        /// <summary>
        /// Constructor assgin paramters to this object's properties
        /// </summary>
        /// <param name="SamAccountName"></param>
        /// <param name="DisplayName"></param>
        /// <param name="Name"></param>
        /// <param name="Description"></param>
        /// <param name="Enabled"></param>
        /// <param name="LastLogon"></param>
        public Computer(String SamAccountName, String DisplayName, String Name, String Description,
            Boolean? Enabled, DateTime? LastLogon)
        {
            this.SamAccountName = SamAccountName;
            this.DisplayName = DisplayName;
            this.Name = Name;
            this.Description = Description;
            this.Enabled = Enabled;
            this.LastLogon = LastLogon;
        }
      
        
        //public static string[] StringArrayComputerProperties = { "SamAccountName", "DisplayName", "Name", "Description", "Enabled", "LastLogon" };
    }
}
