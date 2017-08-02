using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revive.Redux.Repositories;
using Revive.Redux;
using Revive.Redux.Entities;
using Revive.Redux.Commn;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace Revive.Redux.Services
{
    public class LocationManagement : ILocationManagement
    {

        private ILocationManagementRespository _ILocationManagementRespository = null;
        public LocationManagement()
        {
            _ILocationManagementRespository = new LocationManagementRespository();
        }
        public bool AddEditLocation(LocationModel objLocation)
        {
            return _ILocationManagementRespository.AddEditLocation(objLocation);
        }

        public List<CustomerLocationModel> GetCustomerLocationsById(int CustomerId)
        {
            return _ILocationManagementRespository.GetCustomerLocationsById(CustomerId);
        }
        public List<CustomerLocationModel> GetCustomerLocationsById(int CustomerId, Guid AccountManagerId)
        {
            return _ILocationManagementRespository.GetCustomerLocationsById(CustomerId, AccountManagerId);
        }

        public List<CustomerLocationModel> GetCustomerLocationsBySubsidiaryAdminUser(Guid SubsidiaryAdminUserId)
        {
            return _ILocationManagementRespository.GetCustomerLocationsBySubsidiaryAdminUser(SubsidiaryAdminUserId);
        }
        public List<CustomerLocationModel> GetCustomerLocationsBySubAgentAdminUser(Guid SubAgentAdminUserId)
        {
            return _ILocationManagementRespository.GetCustomerLocationsBySubAgentAdminUser(SubAgentAdminUserId);
        }


        public CustomerLocationModel GetCustomerLocation(int nLocationId)
        {
            return _ILocationManagementRespository.GetCustomerLocation(nLocationId);
        }

        public bool CheckDuplicateStoreNo(string storeNo, int custID, int locID)
        {
            return _ILocationManagementRespository.CheckDuplicateStoreNo(storeNo, custID, locID);
        }

        public bool CheckDuplicateCustID(string custRefCode, String custID)
        {
            return _ILocationManagementRespository.CheckDuplicateCustID(custRefCode, custID);
        }
        public int GetLocationIdByLocationName(string _locationName, int CustomerId)
        {
            return _ILocationManagementRespository.GetLocationIdByLocationName(_locationName, CustomerId);
        }

        public int GetLocationIdByStoreNumber(string _StoreNumber, int CustomerId)
        {
            return _ILocationManagementRespository.GetLocationIdByStoreNumber(_StoreNumber, CustomerId);
        }

        public bool CustomerFileUpload(LocationModel objCustomerLocation, out LocationModel objCustomerLocationResult)
        {
            bool bResult = false;
            List<LocationResultModel> lstLocationResult = new List<LocationResultModel>();
            objCustomerLocationResult = new LocationModel();
            int nLineNumber = 1;
            string sMessage = string.Empty;
            string errorMesg = string.Empty;
            List<LocationModel> lstLocationModel = FileHandler.ValidateExcel(objCustomerLocation.FileName, out errorMesg);

            foreach (var objLocation in lstLocationModel)
            {
                objLocation.CustomerId = objCustomerLocation.CustomerId;
                objLocation.Modified_by = objCustomerLocation.Modified_by;
                objLocation.Modified_Date = Common.CommonMethods.GetCurrentDate();
                objLocation.Created_by = objCustomerLocation.Created_by;
                objLocation.Created_Date = Common.CommonMethods.GetCurrentDate();
                objLocation.State = _ILocationManagementRespository.GetStateIdByStateName(objLocation.StateName);
                //  objLocation.City = objCustomerLocation.CityName;//_ILocationManagementRespository.GetCityIdByCityName(objLocation.CityName, objLocation.State);
                //objLocation.CityName = objCustomerLocation.CityName;
                objLocation.SubsidiaryId = objCustomerLocation.SubsidiaryId;
                objLocation.SubAgentId = objCustomerLocation.SubAgentId;
                objLocation.GroupId = objCustomerLocation.GroupId;

                var validationResults = new List<ValidationResult>();
                bResult = false;
                if (ValidateCustomerFile(objLocation, out validationResults))
                {
                    var storeDuplicate = CheckDuplicateStoreNo(objLocation.StoreNumber, objLocation.CustomerId, 0); // Added by KD for Store Duplicate check
                    if (!storeDuplicate)
                    {
                        bResult = _ILocationManagementRespository.AddEditLocation(objLocation);

                        if (bResult)
                        {
                            sMessage = "Successfully Inserted.";
                        }
                    }
                    else
                    {
                        sMessage = "Failed to Inserted. ";
                        sMessage += "; " + "Store Number :" + objLocation.StoreNumber.ToString() + " Already Exists.";
                    }
                }
                else
                {
                    sMessage = "Failed to Inserted. ";
                    foreach (var val in validationResults)
                    {
                        var errorKeyLst = val.MemberNames;
                        string keystr = "";

                        foreach (var errorstr in errorKeyLst)
                        {
                            switch (errorstr)
                            {
                                case "LocationName":
                                    {
                                        keystr += "Location Name";
                                        break;
                                    }
                                case "AddressLine1":
                                    {
                                        keystr += "AddressLine Line 1";
                                        break;
                                    }
                                case "CityName":
                                    {
                                        keystr += "City Name";
                                        break;
                                    }
                                case "PrimaryPhone":
                                    {
                                        keystr += "Primary Phone Number";
                                        break;
                                    }
                                case "StoreNumber":
                                    {
                                        keystr += "Store Number";
                                        break;
                                    }
                                case "StateId":
                                    {
                                        keystr += "State Code";
                                        break;
                                    }

                                default:
                                    {
                                        keystr += errorstr;
                                        break;
                                    }
                            }
                        }
                        sMessage += "; " + keystr + " not valid";

                    }

                }

                var LocationResult = new LocationResultModel()
                {
                    LineNumber = "Line Number " + nLineNumber,
                    Message = sMessage,
                    Status = bResult

                };

                lstLocationResult.Add(LocationResult);
                nLineNumber++;
            }
            if (lstLocationModel.Count == 0)
            {
                var LocationResult = new LocationResultModel()
                {
                    LineNumber = "Line Number 1",
                    //Message = "Invalid file format.",
                    Message = errorMesg,
                    Status = bResult

                };
                lstLocationResult.Add(LocationResult);
            }

            objCustomerLocationResult.LocationResult = lstLocationResult;

            return bResult;
        }


        private bool ValidateCustomerFile(LocationModel objCustomerLocation, out List<ValidationResult> validationResults)
        {
            bool bResult = false;
            if (objCustomerLocation.AddressLine2 == "")
            {
                objCustomerLocation.AddressLine2 = null;
            }

            LocationModelBulk objLocBulk = new LocationModelBulk();

            objLocBulk.StoreNumber = objCustomerLocation.StoreNumber;
            objLocBulk.LocationName = objCustomerLocation.LocationName;
            objLocBulk.AddressLine1 = objCustomerLocation.AddressLine1;
            objLocBulk.AddressLine2 = objCustomerLocation.AddressLine2;
            objLocBulk.CityName = objCustomerLocation.CityName;
            objLocBulk.StateId = objCustomerLocation.State;
            objLocBulk.ZipCode = objCustomerLocation.ZipCode;
            objLocBulk.PrimaryPhone = objCustomerLocation.PrimaryPhone;
            objLocBulk.StoreOpeningTime = objCustomerLocation.StoreOpeningTime;
            objLocBulk.StoreClosingTime = objCustomerLocation.StoreClosingTime;


            validationResults = new List<ValidationResult>();

            var context = new ValidationContext(objLocBulk, serviceProvider: null, items: null);

            bResult = Validator.TryValidateObject(objLocBulk, context, validationResults, true);

            return bResult;
        }

        public List<CustomerLocationModel> GetCustomerLocations(CustomerLocationModel objCustomerLocation, Guid UserId, string pageAccessCode)
        {
            return _ILocationManagementRespository.GetCustomerLocations(objCustomerLocation, UserId, pageAccessCode);
        }
       
        
    }
}
