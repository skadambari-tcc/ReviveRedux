using Kendo.Mvc.UI;
using Revive.Redux.Controllers.Common;
using Revive.Redux.Entities;
using Revive.Redux.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;

namespace Revive.Redux.UI.Controllers
{
    [Authorize]
    [ReviveAuth]
    public class ManageFormController : Controller
    {
        #region Private Variables

        private ICustomerManagement _ICustomerManagement = null;
        private IGeneralService _IGeneralService = null;
        private FormModel objFormModel = null;
        private IFormManagementService _IFormManagementService = null;
        List<FormModel> objFormCustomerModel = null;

        #endregion

        #region Construtors

        public ManageFormController()
        {
            _ICustomerManagement = new CustomerManagement();
            _IGeneralService = new GeneralService();
            _IFormManagementService = new FormManagementService();
        }

        #endregion

        #region Actions

        // GET: ManageCustomers
        public ActionResult Form(int? id)
        {
            id = 0; // to show all customer's Custom fields.

            objFormModel = new FormModel();
            objFormModel = GetFormData("Manage Custom Fields", "Add Custom Field", (int)id);
            objFormModel.FieldNameList = _IFormManagementService.GetCustomerCustomFieldsById((int)id);
            objFormModel.CustomFieldCount = objFormModel.FieldNameList.Count();
            objFormModel.FieldCountValid = (objFormModel.CustomFieldCount > 5) ? false : true;

            return View("Form", objFormModel);
        }

        // GET: Form/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Form/Create
        public ActionResult Create(FormModel objSubmitFormModel)
        {
            bool bIsAdded = false;
            string sErrorMsg = string.Empty;
            // If PageMode=1 (Edit Page) use Edit title, else Save title.
            var objFormModel = objSubmitFormModel.PageMode == 1 ? GetFormData("Edit Custom Field", "Save", objSubmitFormModel.CustomerId) :
                GetFormData("Add Custom Field", "Add Field", objSubmitFormModel.CustomerId);
            if (ModelState.IsValid)
            {
                var objCurrentUserDetail = (CurrentUserDetail)Session["CurrentUser"];

                objSubmitFormModel.Status = true;
                objFormModel.FieldCountValid = true;
                if (objSubmitFormModel.PageMode == 0)
                {
                    objSubmitFormModel.CreatedBy = objCurrentUserDetail.User_Id;
                    objSubmitFormModel.CreatedDate = Common.CommonMethods.GetCurrentDate();
                }
                else
                {
                    objSubmitFormModel.ModifiedBy = objCurrentUserDetail.User_Id;
                    objSubmitFormModel.ModifiedDate = Common.CommonMethods.GetCurrentDate();
                }
                objFormModel.FieldNameList = _IFormManagementService.GetCustomerCustomFieldsById(objSubmitFormModel.CustomerId);
                var fieldNameListWithoutCurrentEdit = objFormModel.FieldNameList.ToList();
                objFormModel.CustomFieldCount = objFormModel.FieldNameList.Count();
                //In case of Add New Custom Field, objSubmitFormModel.CustomFieldId = 0 (Bug ID 6834 resolved)
                //if (objSubmitFormModel.CustomFieldId != 0)
                //{
                //    var removeElement = fieldNameListWithoutCurrentEdit.FindIndex(element => element.CustomFieldId.ToString().Trim() == objSubmitFormModel.CustomFieldId.ToString().Trim());
                //    if (removeElement != 0 && removeElement != null && removeElement != -1) //Will be "-1" in the case when Customer Name itself is changed.
                //    {
                //        fieldNameListWithoutCurrentEdit.RemoveAt(removeElement);
                //    }
                //    objFormModel.CustomFieldCount = fieldNameListWithoutCurrentEdit.Count();

                //}
                objFormModel.CustomFieldCount = fieldNameListWithoutCurrentEdit.Count();

                if (objSubmitFormModel.PageMode == 0)
                {
                    objFormModel.FieldCountValid = (objFormModel.CustomFieldCount >= 5) ? false : true;
                }
                else
                {
                    objFormModel.FieldCountValid = (objFormModel.CustomFieldCount-1 >= (5)) ? false : true; // -1 deducted 1 item for edit case.
                }
                objSubmitFormModel.CustomerId = objSubmitFormModel.CustomerId;

                if (objFormModel.FieldCountValid)
                {
                    sErrorMsg = _IFormManagementService.AddEditCustomForm(objSubmitFormModel);
                    if (sErrorMsg == "")
                        bIsAdded = true;
                    else
                        bIsAdded = false;
                }
                else
                {
                    sErrorMsg = "Max 5 fields can be added.";
                }


                if (!bIsAdded)
                {
                    ModelState.AddModelError("Name", sErrorMsg);
                    return View("AddEditForm", objFormModel);
                }
                else
                {

                    if (objSubmitFormModel.PageMode == 0)
                    {
                        TempData["isCreatedSuccess"] = true;
                    }
                    else
                    {
                        TempData["isUpdatedSuccess"] = true;
                    }

                    return RedirectToAction("Form", new { id = objSubmitFormModel.CustomerId });
                }

            }
            else
            {
                return RedirectToAction("View", objFormModel);
            }
        }


        public ActionResult View(int? Id)
        {
            if (Id == null) { Id = 0; }
            var objFormModel = GetFormData("Add Custom Field", "Add Field", (int)Id);
            objFormModel.PageMode = 0;
            return View("AddEditForm", objFormModel);
        }

        //public JsonResult CheckDuplicateCustomFieldname(string CustomFieldName, int CustomerId, int CustomFieldId)
        //{
        //    try
        //    {
        //        var data = !_IFormManagementService.CheckDuplicateCustomFieldname(CustomFieldName, CustomerId, CustomFieldId);
        //        return Json(data);
        //    }
        //    catch(Exception ex)
        //    {
        //        throw;
        //    }
        //}

        // GET: Form/Edit/5
        public ActionResult Edit(int id)
        {
            var objCustomFields = _IFormManagementService.GetCustomFieldsById(id);
            var objFormModel = GetFormData("Edit Custom Field", "Save", objCustomFields.CustomerId);
            objFormModel.CustomFieldName = objCustomFields.CustomFieldName;
            objFormModel.PageMode = 1;
            objFormModel.CustomFieldId = objCustomFields.CustomFieldId;
            ViewBag.isUpdatedMode = true;


            return View("AddEditForm", objFormModel);
        }

        // POST: Form/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                // return View();
                throw;
            }
        }

        // GET: Form/Delete/5
        public ActionResult Delete(int id)
        {
            return View("AddEditForm");
        }

        [AjaxHandleException]
        public ActionResult FormAjax([DataSourceRequest] DataSourceRequest request, int id)
        {
            objFormCustomerModel = _IFormManagementService.GetCustomerCustomFieldsById(id);
            DataSourceResult result = objFormCustomerModel.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ActivateDeactivateField(FormModel objSubmitFormMode)
        {
            if (objSubmitFormMode.CustomFieldId > 0)
            {

                bool bResult = _IFormManagementService.ActivateDeactivateCustomField(objSubmitFormMode);
                if (bResult)
                {
                    return Json(bResult, JsonRequestBehavior.AllowGet);
                }

            }
            return View("Form");
        }


        #endregion

        #region Private Methods

        private FormModel GetFormData(string sPageHeader, string sButtonText, int nCustomerId)
        {
            var objFormModel = new FormModel();
            objFormModel.PageHeader = sPageHeader;
            objFormModel.PageButtonSubmit = sButtonText;
            objFormModel.CustomerNameList = _IGeneralService.GetCustomerService();
            objFormModel.CustomerId = nCustomerId;

            return objFormModel;
        }


        #endregion
    }
}
