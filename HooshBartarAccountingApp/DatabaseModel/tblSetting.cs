//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HooshBartarAccountingApp.DatabaseModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblSetting
    {
        public int Id { get; set; }
        public Nullable<decimal> BuyKarmozdRate { get; set; }
        public Nullable<decimal> SellKarmozdRate { get; set; }
        public Nullable<decimal> NaghloEnteghalTaxRate { get; set; }
        public Nullable<decimal> ArzeshAfzoodehTaxRate { get; set; }
    }
}