using Revive.Redux.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Repositories
{
    public interface IFormManagementRespository
    {
      List<FormModel> GetCustomerCustomFieldsById(int nCustomerId);
      bool AddEditCustomForm(FormModel objForm);
      bool ActivateDeactivateCustomField(FormModel objForm);
      FormModel GetCustomFieldsById(int nCustomFieldId);
      bool CheckDuplicateCustomFieldname(string customFieldName, int custID, int CustomFieldId);
    }
}
