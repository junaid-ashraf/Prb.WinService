//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Prb.SqliteDAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Hw_Sw_Installed
    {
        public int SwInstalledId { get; set; }
        public int HwMasterInfoId { get; set; }
        public string SwName { get; set; }
        public string SwDescription { get; set; }
        public string SwType { get; set; }
        public string SwVersion { get; set; }
        public string ProductName { get; set; }
        public string ProductVersion { get; set; }
        public string CopyRight { get; set; }
        public string Size { get; set; }
        public Nullable<System.DateTime> SwDateModified { get; set; }
        public string Language { get; set; }
        public string SerialNo { get; set; }
        public string LicenceNo { get; set; }
        public string LicenceType { get; set; }
        public Nullable<System.DateTime> InstalledDate { get; set; }
        public string PathName { get; set; }
        public string Status { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual Hw_MasterInfo Hw_MasterInfo { get; set; }
    }
}
