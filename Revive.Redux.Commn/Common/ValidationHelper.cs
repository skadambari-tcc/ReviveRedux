using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace Revive.Web.Common.Common
{
    public class MustBeSelectedAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null || (int)value == 0)
                return false;
            else return true;
        }
    }
}
