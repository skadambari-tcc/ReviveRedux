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
    
    public partial class Menu
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public int ParentMenuId { get; set; }
        public string MenuType { get; set; }
        public string MenuName { get; set; }
        public string Category { get; set; }
        public Nullable<int> SeqNo { get; set; }
    }
}
