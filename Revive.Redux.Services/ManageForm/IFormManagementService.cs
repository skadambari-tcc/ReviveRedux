using Revive.Redux.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Services
{
    public interface IFormManagementService
    {
        List<FormModel> GetCustomerCustomFieldsById(int nCustomerId);
        string AddEditCustomForm(FormModel objForm);
        bool ActivateDeactivateCustomField(FormModel objForm);
        FormModel GetCustomFieldsById(int nCustomFieldId);
        bool CheckDuplicateCustomFieldname(string customFieldname, int custID, int CustomFieldId);
    }
}
