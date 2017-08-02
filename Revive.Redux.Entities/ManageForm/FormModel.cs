using Revive.Redux.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public class FormModel : FormCustomerModel
    {
        private int _nCustomerId;

        [Display(Name = "CustomerName")]
        [Required(ErrorMessage = "Required")]
        public int CustomerId
        {
            get { return _nCustomerId; }
            set { _nCustomerId = value; }
        }

        private string _sCustomerName;

        public string CustomerName
        {
            get { return _sCustomerName; }
            set { _sCustomerName = value; }
        }

        private IEnumerable<DtoList> _CustomerNameList;

        public IEnumerable<DtoList> CustomerNameList
        {
            get { return _CustomerNameList; }
            set { _CustomerNameList = value; }
        }

        private IEnumerable<FormCustomerModel> _FieldNameList;

        public IEnumerable<FormCustomerModel> FieldNameList
        {
            get { return _FieldNameList; }
            set { _FieldNameList = value; }
        }
    }
}
