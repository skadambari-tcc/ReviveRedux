using Revive.Redux.Common;
using Revive.Redux.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Repositories
{
  public  class FormManagementRespository : IFormManagementRespository
    {

      public List<FormModel> GetCustomerCustomFieldsById(int nCustomerId)     
      {
          try
          {
              var objForms = new List<FormModel>();
              using (var dbContext = new ReviveDBEntities())
              {
                  if (nCustomerId > 0)
                  {
                      var lstFormCollection = (from d in dbContext.Customer_CustomField
                                               where d.Customer_ID == nCustomerId && d.Status == true
                                               orderby d.Custom_Field_ID
                                               select new FormModel
                                               {
                                                   CustomFieldId = d.Custom_Field_ID,
                                                   CustomFieldName = d.Custom_Field_Name,
                                                   CustomerId = (d.Customer_ID != null) ? (int)d.Customer_ID : 0,
                                                   CreatedBy = (d.Created_by != null) ? (Guid)d.Created_by : new Guid(),
                                                   CreatedDate = (d.Created_Date != null) ? (DateTime)d.Created_Date : DateTime.Now,
                                                   ModifiedBy = (d.Created_by != null) ? (Guid)d.Created_by : new Guid(),
                                                   ModifiedDate = (d.Modified_Date != null) ? (DateTime)d.Modified_Date : DateTime.Now,
                                                   Status = d.Status,
                                                   CustomerName = dbContext.Customers.Where(a => a.Customer_ID == d.Customer_ID).Select(a => a.Customer_Name).FirstOrDefault()
                                               }).ToList();
                      objForms.AddRange(lstFormCollection);   
                  }
                  else
                  {
                      var lstFormCollection = (from d in dbContext.Customer_CustomField
                                               where d.Customer_ID >= 0 && d.Status == true
                                               orderby d.Custom_Field_ID
                                               select new FormModel
                                               {
                                                   CustomFieldId = d.Custom_Field_ID,
                                                   CustomFieldName = d.Custom_Field_Name,
                                                   CustomerId = (d.Customer_ID != null) ? (int)d.Customer_ID : 0,
                                                   CreatedBy = (d.Created_by != null) ? (Guid)d.Created_by : new Guid(),
                                                   CreatedDate = (d.Created_Date != null) ? (DateTime)d.Created_Date : DateTime.Now,
                                                   ModifiedBy = (d.Created_by != null) ? (Guid)d.Created_by : new Guid(),
                                                   ModifiedDate = (d.Modified_Date != null) ? (DateTime)d.Modified_Date : DateTime.Now,
                                                   Status = d.Status,
                                                   CustomerName = dbContext.Customers.Where(a => a.Customer_ID == d.Customer_ID).Select(a => a.Customer_Name).FirstOrDefault()
                                               }).ToList();
                      objForms.AddRange(lstFormCollection);   
                  }
                 
              }
              return objForms;
          }
          catch (Exception ex)
          {
              throw;
          }
      }

      public bool ActivateDeactivateCustomField(FormModel objForm)
      {
          var bResult = false;
          try
          {
              using (var dbContext = new ReviveDBEntities())
              {
                  if (objForm.CustomFieldId > 0)
                  {
                      var objCustomerFormModel = dbContext.Customer_CustomField.FirstOrDefault(cond => cond.Custom_Field_ID == objForm.CustomFieldId);
                      if (objCustomerFormModel != null)
                      {
                          objCustomerFormModel.Status = !objForm.Status;
                      }
                      dbContext.SaveChanges();
                      bResult = true;
                  }
              }
          }
          catch (Exception ex)
          {
              throw;
          }
          return bResult;
      }

      public bool AddEditCustomForm(FormModel objForms)
      {
          bool result = false;
          try
          {
              
              var objFormCollection = new Customer_CustomField();
              using (var dbContext = new ReviveDBEntities())
              {

                  objFormCollection = MapDataModels(objForms, objFormCollection);

                  if (objForms.PageMode == 0)
                  {
                      dbContext.Customer_CustomField.Add(objFormCollection);
                  }
                  else if (objForms.PageMode == 1)
                  {
                      objFormCollection = dbContext.Customer_CustomField.FirstOrDefault(cond => cond.Custom_Field_ID == objForms.CustomFieldId);

                      if (objFormCollection != null)
                      {
                          objFormCollection = MapDataModels(objForms, objFormCollection);
                      }
                  }

                  dbContext.SaveChanges();
                  result = true;

              }
              
          }
          
          catch (Exception ex)
          {
              result = false;
              throw ex;

          }
          return result;
          
      }


      public FormModel GetCustomFieldsById(int nCustomFieldId)
      {
          try
          {
              var objForms = new FormModel();
              using (var dbContext = new ReviveDBEntities())
              {
               var objCustomerFormModel = dbContext.Customer_CustomField.FirstOrDefault(cond => cond.Custom_Field_ID == nCustomFieldId);
               if (objCustomerFormModel != null)
                   {
                       objForms.CustomFieldId = objCustomerFormModel.Custom_Field_ID;
                       objForms.CustomFieldName = objCustomerFormModel.Custom_Field_Name;
                       objForms.CustomerId = (objCustomerFormModel.Customer_ID != null) ? (int)objCustomerFormModel.Customer_ID : 0;
                       objForms.ModifiedDate = (objCustomerFormModel.Modified_Date !=null) ? (DateTime)objCustomerFormModel.Modified_Date : DateTime.Today;
                       objForms.ModifiedBy = (objCustomerFormModel.Modified_by !=null) ? (Guid) objCustomerFormModel.Modified_by : new Guid();
                       objForms.CreatedDate = (objCustomerFormModel.Created_Date != null) ? (DateTime)objCustomerFormModel.Created_Date : DateTime.Today;
                       objForms.CreatedBy = (objCustomerFormModel.Created_by != null) ? (Guid)objCustomerFormModel.Created_by : new Guid();
                       objForms.Status = objCustomerFormModel.Status;
                   }
              }
              return objForms;
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }

      private Customer_CustomField MapDataModels(FormModel objForms, Customer_CustomField objFormCollection)
      {
          objFormCollection.Custom_Field_ID = objForms.CustomFieldId;
          objFormCollection.Customer_ID = objForms.CustomerId;
          objFormCollection.Custom_Field_Name = objForms.CustomFieldName;
          objFormCollection.Modified_by = objForms.ModifiedBy;
          objFormCollection.Modified_Date = objForms.ModifiedDate;
          objFormCollection.Status = objForms.Status;

          if (objForms.PageMode == 0)
          {
              objFormCollection.Created_Date = objForms.CreatedDate;
              objFormCollection.Created_by = objForms.CreatedBy;
          }

          return objFormCollection;
      }

      public bool CheckDuplicateCustomFieldname(string customFieldName, int custID, int CustomFieldId)
      {
          try
          {
              bool isExists = false;
              using (var dbContext = new ReviveDBEntities())
              {
                  var allCustCustomFields = new List<string>();
                  if (CustomFieldId != 0 || CustomFieldId != null)
                  {
                      allCustCustomFields = (from c in dbContext.Customer_CustomField
                                             where c.Customer_ID == custID && c.Custom_Field_ID != CustomFieldId
                                             && c.Custom_Field_Name.ToLower() == customFieldName.ToLower()
                                             select c.Custom_Field_Name).ToList();
                  }
                  else
                  allCustCustomFields = (from c in dbContext.Customer_CustomField where c.Customer_ID == custID &&
                                         c.Custom_Field_Name.ToLower() == customFieldName.ToLower()
                                         select c.Custom_Field_Name).ToList();
                  if (allCustCustomFields.Any())
                      isExists = true;
              }
              return isExists;
          }
          catch(Exception ex)
          {
              throw ex;
          }
      }

      
    }
}
