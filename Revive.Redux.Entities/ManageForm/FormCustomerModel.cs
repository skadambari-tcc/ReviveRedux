using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Web.Mvc;

namespace Revive.Redux.Entities
{
   
    public class FormCustomerModel : PageBasic
    {
        private int _nCustomFieldId;

        public int CustomFieldId
        {
            get { return _nCustomFieldId; }
            set { _nCustomFieldId = value; }
        }

        private string _sCustomFieldName;

        [Display(Name = "Custom Field Name")]
        [Required(ErrorMessage = "Required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Custom Field Name should be between 2 to 100 characters")]
        [RegularExpression(@"^([a-zA-Z0-9 \.\&\'\-]+)$", ErrorMessage = "Invalid Custom Field Name")]
        //[Remote("CheckDuplicateCustomFieldname", "ManageForm", AdditionalFields = "CustomFieldName, CustomerId, CustomFieldId", HttpMethod = "POST", ErrorMessage = "This Custom Field Name already exists for the given Customer. Please enter a different Custom Field Name")]
        public string CustomFieldName
        {
            get { return _sCustomFieldName; }
            set { _sCustomFieldName = value; }
        }
        
        private bool _bStatus;

        public bool Status
        {
            get { return _bStatus; }
            set { _bStatus = value; }
        }

        private int _nCustomFieldCount;

        public int CustomFieldCount
        {
            get { return _nCustomFieldCount; }
            set { _nCustomFieldCount = value; }
        }

        private bool _bFieldCountValid;

        public bool FieldCountValid
        {
            get { return _bFieldCountValid; }
            set { _bFieldCountValid = value; }
        }

        private string _sCustomerName;

        public string CustomerName
        {
            get { return _sCustomerName; }
            set { _sCustomerName = value; }
        }
    }
}
