//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Revive.Redux.Repositories
{
    using System;
    using System.Collections.Generic;
    
    public partial class City
    {
        public int CityId { get; set; }
        public string city1 { get; set; }
        public string state_code { get; set; }
        public Nullable<System.DateTime> aud_date { get; set; }
        public string aud_user { get; set; }
        public Nullable<int> StateId { get; set; }
    
        public virtual State State { get; set; }
    }
}
