using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public partial class MenuModel
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public int ParentMenuId { get; set; }
        public string MenuType { get; set; }
        public string MenuName { get; set; }
        public bool IsSelected { get; set; }
        public string MenuItemsHtml { get; set; }
        public int UserLevelID { get; set; }
        public int? menuSeqno { get; set; }
    }
}
