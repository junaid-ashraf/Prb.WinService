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
    
    public partial class Prb_ADConnectionFailure
    {
        public int FailureId { get; set; }
        public string FailureReason { get; set; }
        public int SettingId { get; set; }
    
        public virtual Prb_Setting Prb_Setting { get; set; }
    }
}