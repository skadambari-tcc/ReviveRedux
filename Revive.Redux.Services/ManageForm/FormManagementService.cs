using Revive.Redux.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revive.Redux.Entities;

namespace Revive.Redux.Services
{
   public class FormManagementService : IFormManagementService
    {
       private IFormManagementRespository _IFormManagementRespository = null;
        

       public FormManagementService()
        {
            _IFormManagementRespository = new FormManagementRespository();
        }


       public List<FormModel> GetCustomerCustomFieldsById(int nCustomerId)
       {
           return _IFormManagementRespository.GetCustomerCustomFieldsById(nCustomerId);
       }

      
       public bool ActivateDeactivateCustomField(FormModel objForm)
       {
           return _IFormManagementRespository.ActivateDeactivateCustomField(objForm);
       }


       public string AddEditCustomForm(FormModel objForm)
       {
           var alreadyExists = CheckDuplicateCustomFieldname(objForm.CustomFieldName, objForm.CustomerId, objForm.CustomFieldId);
           string result="";
           if (alreadyExists == false)
           {
               var success = _IFormManagementRespository.AddEditCustomForm(objForm);
               if (success == true)
                   result = "";
               else
                   result = "Failed!";
           }
               //return 
           else
           {
               result = "Custom field name already exists for selected Customer. Please enter a different Custom field name.";
           }
               return result;
       }


       public FormModel GetCustomFieldsById(int nCustomFieldId)
       {
           return _IFormManagementRespository.GetCustomFieldsById(nCustomFieldId);
       }

       public bool CheckDuplicateCustomFieldname(string customFieldname, int custID, int CustomFieldId)
       {
           return _IFormManagementRespository.CheckDuplicateCustomFieldname(customFieldname, custID, CustomFieldId);
       }
    }
}
