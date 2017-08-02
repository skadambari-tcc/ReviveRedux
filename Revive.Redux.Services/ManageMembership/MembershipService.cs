using Revive.Redux.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revive.Redux.Repositories;
using Revive.Redux.Entities.Common;
using Revive.Redux.Common;
using Revive.Redux.Commn;
using System.ComponentModel.DataAnnotations;
using Kendo.Mvc.UI;
namespace Revive.Redux.Services
{
    public class MembershipService : IMembershipService
    {
        IMembershipRepository _membershipRepository = null;
        IGeneralRepository _generalRepository = null;
        public MembershipService()
        {
            _membershipRepository = new MembershipRepository();
            _generalRepository = new GeneralRepository();
        }
        public RepositoryResult AddEditMembership(MembersShipModel objmembership)
        {
            int _machineId = _membershipRepository.GetMachineIdByMachineIdVal(objmembership.machine_Id_Val);
            objmembership.machine_Id = _machineId;


            return _membershipRepository.AddEditMembership(objmembership);
        }
        public IEnumerable<MemberShipDetailsModel> GetMembmershipDetails(MembersShipModel objMembership)
        {
            return _membershipRepository.GetMembmershipDetails(objMembership);
        }
        public Boolean CheckEmailAddressExists(String emailAddress, string membershipId, Guid user_Id)
        {
            return _membershipRepository.CheckEmailAddressExists(emailAddress, membershipId, user_Id);
        }
        public bool updateMembership(MembersShipModel objMembership)
        {
            return _membershipRepository.updateMembership(objMembership);
        }
        public RepositoryResult VoidMembership(MembersShipModel objmembership)
        {
            return _membershipRepository.VoidMembership(objmembership);

        }
        public RepositoryResult renewMembership(MembersShipModel objMembership)
        {
            return _membershipRepository.renewMembership(objMembership);
        }
        public IEnumerable<DtoList> GetDevivemanufacturer()
        {
            return _generalRepository.GetDevivemanufacturer();
        }
        public IEnumerable<DtoList> GetHowlongago()
        {
            return _generalRepository.GetHowlongago();
        }
        public IEnumerable<DtoList> GetModes(Guid user_id)
        {
            return _generalRepository.GetModes(user_id);
        }
        public int GetCustomizedTemplateIdByMachineId(string machine_id_val)
        {
            int _machineId = _membershipRepository.GetMachineIdByMachineIdVal(machine_id_val);
            return _membershipRepository.GetCustomizedTemplateIdByMachineId(_machineId);
        }
        public IEnumerable<CustomizedParameterModel> GetCustomizedDebugParameter(int Template_Id)
        {
            return _membershipRepository.GetCustomizedDebugParameter(Template_Id);
        }
        public RepositoryResult InitiateCheckinProcess(MemberActivityModel objMembershipActivity)
        {
            RepositoryResult objresult = new RepositoryResult();
            // Data validation  check
            int _machineId = _membershipRepository.GetMachineIdByMachineIdVal(objMembershipActivity.machine_Id_Val);
            objMembershipActivity.machine_Id = _machineId;
            if (objMembershipActivity.isMember == true)
            {
                if (objMembershipActivity.machine_Id == null || objMembershipActivity.machine_Id == 0 || objMembershipActivity.membership_Id == null)
                {
                    objresult.resultId = APIStatusValues.FailureIdmsgs;
                    objresult.resultVal = "Not able to start revive as machine is not registered on ES or membership is not valid";
                }

                if (objMembershipActivity.membership_Id != null)
                {
                    var ActiveMember = _membershipRepository.CheckMembershipActive(objMembershipActivity.membership_Id, (Guid)objMembershipActivity.created_by);
                    if (ActiveMember == false)
                    {
                        objresult.resultId = APIStatusValues.FailureIdmsgs;
                        objresult.resultVal = "Member not activate.";
                    }
                }
                //int GetMachineIdValidateForReviveByMachineId(int machine_Id);
                if (objMembershipActivity.machine_Id != null || objMembershipActivity.machine_Id > 0)
                {
                    var MappedMachineIdwithLocation = _membershipRepository.GetMachineIdValidateForReviveByMachineId((int)objMembershipActivity.machine_Id);
                    if (MappedMachineIdwithLocation == 0)
                    {
                        objresult.resultId = APIStatusValues.FailureIdmsgs;
                        objresult.resultVal = "Machine Id : " + objMembershipActivity.machine_Id_Val + " Not mapped with location.";
                    }
                }


            }
            else
            {
                if (objMembershipActivity.machine_Id == null || objMembershipActivity.machine_Id == 0)
                {
                    objresult.resultId = objresult.resultId = APIStatusValues.FailureIdmsgs;
                    objresult.resultVal = "Not able to start revive as machine is not registered on ES";
                }
                if (objMembershipActivity.machine_Id != null || objMembershipActivity.machine_Id > 0)
                {
                    var MappedMachineIdwithLocation = _membershipRepository.GetMachineIdValidateForReviveByMachineId((int)objMembershipActivity.machine_Id);
                    if (MappedMachineIdwithLocation == 0)
                    {
                        objresult.resultId = APIStatusValues.FailureIdmsgs;
                        objresult.resultVal = "Machine Id : " + objMembershipActivity.machine_Id_Val + " Not mapped with location.";
                    }
                }
            }

            // Check machine Killed or Not?
            if (objresult.resultVal == null)
            {
                string _machineActive = _membershipRepository.CheckMachineActive((int)objMembershipActivity.machine_Id);
                if (_machineActive != "")
                {
                    _machineActive = "Machine Id " + objMembershipActivity.machine_Id_Val + " Has been Killed.";
                    objresult.resultId = APIStatusValues.FailureIdmsgs;
                    objresult.resultVal = _machineActive;
                }
            }
            if (objresult.resultVal == null)
            {
                if (objMembershipActivity.isMember == false)
                {
                    string sKey = System.Configuration.ConfigurationSettings.AppSettings["Encrption"].ToString();

                    objMembershipActivity.member_name = (objMembershipActivity.member_name != null && objMembershipActivity.member_name != string.Empty) ? Cryptography.EncryptText(objMembershipActivity.member_name + "@", sKey) : string.Empty;
                    objMembershipActivity.email_Id = (objMembershipActivity.email_Id != null && objMembershipActivity.email_Id != string.Empty) ? Cryptography.EncryptText(objMembershipActivity.email_Id + "@", sKey) : string.Empty;
                    objMembershipActivity.phonenumber = (objMembershipActivity.phonenumber != null && objMembershipActivity.phonenumber != string.Empty) ? Cryptography.EncryptText(objMembershipActivity.phonenumber + "@", sKey) : string.Empty;
                }
                objresult = _membershipRepository.InitiateCheckinProcess(objMembershipActivity);
            }

            return objresult;
        }


        public RepositoryResult UpdateReviveStartResult(MemberActivityModel objMembershipActivity)
        {
            int _machineId = _membershipRepository.GetMachineIdByMachineIdVal(objMembershipActivity.machine_Id_Val);
            objMembershipActivity.machine_Id = _machineId;

            RepositoryResult objresult = new RepositoryResult();
            if (_machineId == null || _machineId == 0)
            {
                objresult.resultId = APIStatusValues.FailureIdmsgs;
                objresult.resultVal = "Error in getting Machine Id";
            }
            else
            {
                objresult = _membershipRepository.UpdateReviveStartResult(objMembershipActivity);
            }
            return objresult;
        }
        public RepositoryResult UpdateReviveEndResult(MemberActivityModel objMembershipActivity)
        {
            int _machineId = _membershipRepository.GetMachineIdByMachineIdVal(objMembershipActivity.machine_Id_Val);
            objMembershipActivity.machine_Id = _machineId;

            RepositoryResult objresult = new RepositoryResult();

            if (_machineId == null || _machineId == 0)
            {
                objresult.resultId = APIStatusValues.FailureIdmsgs;
                objresult.resultVal = "Error in getting Machine Id";
            }
            else
            {
                if (objMembershipActivity.resultCategoryCode == TabletMachineActivityStages.ReviveStopedWithFault) // Fault during Revive Process New functionality added 
                {
                    var result = _membershipRepository.IsReviveStopped(objMembershipActivity, TabletMachineActivityStages.ReviveStopedWithFault);
                    if (result == true)
                    {
                        objresult.resultId = APIStatusValues.SuccessId;
                        objresult.resultVal = "Fault data saved successfully.";
                    }
                    else
                    {
                        objresult = _membershipRepository.UpdateReviveFaultResult(objMembershipActivity);
                    }
                }
                else
                {
                    var result = _membershipRepository.IsReviveStopped(objMembershipActivity, TabletMachineActivityStages.ReviveEndResult);
                    if (result == true)
                    {
                        objresult.resultId = APIStatusValues.SuccessId;
                        objresult.resultVal = "Revive End Result saved successfully.";
                    }
                    else
                    {

                        objresult = _membershipRepository.UpdateReviveEndResult(objMembershipActivity); // Revive End Result 
                    }
                }

            }
            return objresult;
        }
        public RepositoryResult SendDebugInformation(MemberActivityModel objMembershipActivity)
        {
            int _machineId = _membershipRepository.GetMachineIdByMachineIdVal(objMembershipActivity.machine_Id_Val);
            objMembershipActivity.machine_Id = _machineId;

            RepositoryResult objresult = new RepositoryResult();
            if (_machineId == null || _machineId == 0)
            {
                objresult.resultId = APIStatusValues.FailureIdmsgs;
                objresult.resultVal = "Error in getting Machine Id";
            }
            else
            {
                objresult = _membershipRepository.SendDebugInformation(objMembershipActivity);
            }
            return objresult;
        }

        public RepositoryResult ReviveCheckOut(MemberActivityModel objMembershipActivity)
        {

            RepositoryResult objresult = new RepositoryResult();
            int _machineId = _membershipRepository.GetMachineIdByMachineIdVal(objMembershipActivity.machine_Id_Val);
            objMembershipActivity.machine_Id = _machineId;
            if (_machineId == null || _machineId == 0)
            {
                objresult.resultId = APIStatusValues.FailureIdmsgs;
                objresult.resultVal = "Error in getting Machine Id";
            }
            else
            {

                objresult = _membershipRepository.ReviveCheckOut(objMembershipActivity);


            }
            return objresult;
        }

        public RepositoryResult MachineFault(MemberActivityModel objMemberActivityModel)
        {
            RepositoryResult objresult = new RepositoryResult();
            int _machineId = _membershipRepository.GetMachineIdByMachineIdVal(objMemberActivityModel.machine_Id_Val);
            objMemberActivityModel.machine_Id = _machineId;
            if (_machineId > 0)
            {
                objresult = _membershipRepository.MachineFault(objMemberActivityModel);
            }
            else
            {
                objresult.resultId = objresult.resultId = APIStatusValues.FailureIdmsgs;
                objresult.resultVal = "Machine Id is not valid";
            }
            return objresult;

        }
        public RepositoryResult updateSoftwareVersion(MemberActivityModel objMemberActivityModel)
        {

            RepositoryResult objresult = new RepositoryResult();
            int _machineId = _membershipRepository.GetMachineIdByMachineIdVal(objMemberActivityModel.machine_Id_Val);
            objMemberActivityModel.machine_Id = _machineId;

            if (_machineId > 0)
            {
                objresult = _membershipRepository.updateSoftwareVersion(objMemberActivityModel);
            }
            else
            {
                objresult.resultId = objresult.resultId = APIStatusValues.FailureIdmsgs;
                objresult.resultVal = "Machine Id is not valid";
            }
            return objresult;



        }
        public SoftwareVersionModel GetSoftwareVersionByMachineId(int machine_Id)
        {
            return _membershipRepository.GetSoftwareVersionByMachineId(machine_Id);
        }

        public SoftwareVersionModel CheckFirmWareUpdate(string machine_id_Val)
        {
            int _machineId = _membershipRepository.GetMachineIdByMachineIdVal(machine_id_Val);
            return _membershipRepository.GetSoftwareVersionByMachineId(_machineId);
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="objmembership"></param>
        /// <returns></returns>
        public RepositoryResult DeleteMemberShipById(MembersShipModel objmembership)
        {
            RepositoryResult obj = new RepositoryResult();
            if (objmembership.membership_Id == null)
            {
                obj.resultId = APIStatusValues.FailureIdmsgs;
                obj.resultIdval = "Membership Id can not be blank.";
            }
            else
            {
                obj = _membershipRepository.DeleteMemberShipById(objmembership);
            }
            return obj;
        }
        public RepositoryResult CheckValidMemberByEmail(MembersShipModel objmembership)
        {
            return _membershipRepository.CheckValidMemberByEmail(objmembership);
        }
        public IsDOBValid IsValidMemberByDOB(MembersShipModel objmembership)
        {
            return _membershipRepository.IsValidMemberByDOB(objmembership);
        }

        public RepositoryResult UploadMember(MembershipWebModel objmembership)
        {
            return _membershipRepository.UploadMember(objmembership);
        }

        public Boolean CheckDuplicateMailByCustomerID(string emailAddress, int CustomerId)
        {
            return _membershipRepository.CheckDuplicateMailByCustomerID(emailAddress, CustomerId);
        }


        public ModeConfigurationAPIModel GetModeValidation(MemberActivityModel objMembership)
        {
            ModeConfigurationAPIModel objModeConfig = new ModeConfigurationAPIModel();

            int _machineId = _membershipRepository.GetMachineIdByMachineIdVal(objMembership.machine_Id_Val);
            objMembership.machine_Id = _machineId;

            bool validate = true;
            objModeConfig.isModeValidate = false;

            // Check Machine ID Exist or not with ES
            if (validate == true)
            {
                if (_machineId == 0)
                {
                    objModeConfig.machineStatus = "Machine Id " + objMembership.machine_Id_Val + " Does not exist in ES.";
                    validate = false;

                }
            }

            if (validate == true)
            {
                string _machineActive = _membershipRepository.CheckMachineActive((int)objMembership.machine_Id);
                if (_machineActive != "")
                {
                    _machineActive = "Machine Id " + objMembership.machine_Id_Val + " Has been Killed.";
                    objModeConfig.machineStatus = _machineActive;
                    validate = false;
                }
            }

            // End Check Machine Kill or not

            if (validate == true)
            {


                if (objMembership.modeId == Modes.Demo)  // For Demo
                {
                    objModeConfig = _membershipRepository.GetModeValidation(objMembership);
                    if (objModeConfig.isModeValidate == false)
                    {
                        objModeConfig.machineStatus = "Demo mode not allowed.";
                    }

                }
                else if (objMembership.modeId == Modes.Debug) // For Debug
                {
                    int TempId = GetCustomizedTemplateIdByMachineId(objMembership.machine_Id_Val);
                    if (TempId > 0)
                    {
                        objModeConfig.isModeValidate = true;
                    }
                    else
                    {
                        objModeConfig.machineStatus = "Template not set in ES For machine Id : " + objMembership.machine_Id_Val;
                    }

                }
                else // For Other Case
                {
                    objModeConfig.isModeValidate = true;
                }


                // Mode Id Capturing in JobOrder_machine_Id master table.
                if (objModeConfig.isModeValidate == true)
                {
                    int lastModeId = _membershipRepository.GetLastModeByMachineId(_machineId, objMembership.modeId);
                    if (lastModeId > 0)
                    {
                        if (lastModeId == Modes.Debug && objMembership.modeId != Modes.Debug)
                        {
                            bool result = _membershipRepository.DeleteTemplateByMachineId(_machineId);
                        }
                    }
                }
            }



            return objModeConfig;

        }
        public ReviveDetails GetReviveDetails(APIParameter APIParameterObj)
        {
            int _machineId = _membershipRepository.GetMachineIdByMachineIdVal(APIParameterObj.Machine_Id_Val);
            APIParameterObj.MachineId = _machineId;
            return _membershipRepository.GetReviveDetails(APIParameterObj);
        }

        private static string AppendAtPosition(string baseString)
        {
            return String.Format("{0:###-###-####}", Convert.ToInt64(baseString));
        }

        private MembershipWebModel mapMemberKey(ManageMember MembershipWebModelObj)
        {
            MembershipWebModel objMembersShipModel = new MembershipWebModel();
            objMembersShipModel.MemberName = MembershipWebModelObj.MemberName;
            objMembersShipModel.Email_ID = "";

            string _memberPhoneNumber = "";
            if (MembershipWebModelObj.Phone_Number != null)
            {
                _memberPhoneNumber = MembershipWebModelObj.Phone_Number.Replace("-", "");
            }


            long v;
            if (_memberPhoneNumber.Length == 10 && Int64.TryParse(_memberPhoneNumber.Trim(), out v))
            {
                objMembersShipModel.Phone_Number = AppendAtPosition(_memberPhoneNumber);
            }
            else
            {
                objMembersShipModel.Phone_Number = _memberPhoneNumber;
            }

            objMembersShipModel.user_Id = MembershipWebModelObj.user_Id;
            objMembersShipModel.InvoiceNumber = MembershipWebModelObj.InvoiceNumber;
            objMembersShipModel.Machine_ID = 0;
            objMembersShipModel.IsLegacy = true;
            objMembersShipModel.StartDate = MembershipWebModelObj.StartDate;
            objMembersShipModel.EndDate = MembershipWebModelObj.EndDate;
            objMembersShipModel.no_of_FreeDries = MembershipWebModelObj.no_of_FreeDries;
            objMembersShipModel.CustomerId = MembershipWebModelObj.CustomerId;
            objMembersShipModel.SubsidiaryId = MembershipWebModelObj.SubsidiaryID;
            objMembersShipModel.SubAgentId = MembershipWebModelObj.SubAgentID;
            objMembersShipModel.LocationId = MembershipWebModelObj.LocationId;
            objMembersShipModel.IsMultiDevice = MembershipWebModelObj.IsMultiDevice == null ? false : MembershipWebModelObj.IsMultiDevice;

            return objMembersShipModel;
        }

        protected bool CheckDate(String date)
        {
            try
            {
                DateTime dt = DateTime.Parse(date);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool MemberFileUpload(ManageMember objMember, out ManageMember objMemberResult)
        {
            bool bResult = false;
            List<MembershipResultModel> lstMemberResult = new List<MembershipResultModel>();

            objMemberResult = new ManageMember();
            int nLineNumber = 1;
            string sMessage = string.Empty;
            string errorMesg = string.Empty;





            List<ManageMember> lstMemberModel = FileHandler.ValidateExcelBulkMember(objMember.FileName, out errorMesg);
            var LstMemberShip = new List<ManageMember>();
            LocationManagement locationSer = new LocationManagement();


            foreach (var _member in lstMemberModel)
            {
                _member.CustomerId = objMember.CustomerId;
                _member.SubsidiaryID = objMember.SubsidiaryID;
                _member.SubAgentID = objMember.SubAgentID;
                _member.user_Id = objMember.user_Id;



                var _locId = locationSer.GetLocationIdByStoreNumber(_member.StoreNumber, (int)_member.CustomerId);
                //locationSer.GetLocationIdByLocationName(_member.LocationName, (int)_member.CustomerId);
                if (_locId > 0)
                {
                    _member.LocationId = _locId;
                }

                var validationResults = new List<ValidationResult>();
                bResult = false;

                if (ValidateMemberFile(_member, out validationResults))
                {
                    if (CheckDate(_member.strStartDate) && CheckDate(_member.strEndDate))
                    {
                        _member.StartDate = Convert.ToDateTime(_member.strStartDate);
                        _member.EndDate = Convert.ToDateTime(_member.strEndDate);
                        if (_member.StartDate > _member.EndDate)
                        {
                            sMessage = "start date should not greater than end date";
                        }
                        else
                        {
                            RepositoryResult objresult = new RepositoryResult();
                            objresult = _membershipRepository.UploadMember(mapMemberKey(_member));
                            if (objresult.resultVal == "")
                            {
                                sMessage = "Successfully Inserted.";
                            }
                            else
                            {
                                sMessage = "Failed to Inserted.";
                            }
                        }
                    }
                    else
                    {
                        sMessage = "Start date or end date is invalid.";
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
                                case "MemberName":
                                    {
                                        keystr += "Member Name";
                                        break;
                                    }
                                case "Phone_Number":
                                    {
                                        keystr += "Phone Name";
                                        break;
                                    }
                                case "Email_ID":
                                    {
                                        keystr += "Email ID";
                                        break;
                                    }
                                case "LocationId":
                                    {
                                        keystr += "Store Number";
                                        break;
                                    }
                                case "strEndDate":
                                    {
                                        keystr += "End Date";
                                        break;
                                    }
                                case "strStartDate":
                                    {
                                        keystr += "Start Date";
                                        break;
                                    }
                                case "no_of_FreeDries":
                                    {
                                        keystr += "Revive Remaining";
                                        break;
                                    }
                                case "InvoiceNumber":
                                    {
                                        keystr += "Invoice Number";
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


                var UserResult = new MembershipResultModel()
                {
                    LineNumber = "Line Number " + nLineNumber,
                    Message = sMessage,
                    Status = bResult

                };

                lstMemberResult.Add(UserResult);
                nLineNumber++;
            }
            if (lstMemberModel.Count == 0)
            {
                var UserResult = new MembershipResultModel()
                {
                    LineNumber = "Line Number 1",
                    //Message = "Invalid file format.",
                    Message = errorMesg,
                    Status = bResult

                };
                lstMemberResult.Add(UserResult);
            }
            objMemberResult.LocationResult = lstMemberResult;
            return bResult;
        }

        public bool MemberFileUploadAW(ManageMember objMember, out ManageMember objMemberResult)
        {
            bool bResult = false;
            List<MembershipResultModel> lstMemberResult = new List<MembershipResultModel>();

            objMemberResult = new ManageMember();
            int nLineNumber = 1;
            string sMessage = string.Empty;
            string errorMesg = string.Empty;

            List<ManageMember> lstMemberModel = FileHandler.ValidateExcelBulkMemberAW(objMember.FileName, out errorMesg);
            var LstMemberShip = new List<ManageMember>();
            LocationManagement locationSer = new LocationManagement();


            foreach (var _member in lstMemberModel)
            {
                _member.CustomerId = objMember.CustomerId;
                _member.SubsidiaryID = objMember.SubsidiaryID;
                _member.SubAgentID = objMember.SubAgentID;
                _member.user_Id = objMember.user_Id;
                _member.no_of_FreeDries = objMember.no_of_FreeDries;
                _member.custMaxNoOfDevices = objMember.custMaxNoOfDevices;

                if (objMember.membershipDuration > 0)
                {
                    _member.strEndDate = Convert.ToDateTime(_member.strStartDate).AddMonths(objMember.membershipDuration).ToShortDateString();
                }
                else
                {
                    _member.strEndDate = Convert.ToDateTime(_member.strStartDate).AddMonths(24).ToShortDateString();
                }



                var _locId = locationSer.GetLocationIdByStoreNumber(_member.StoreNumber, (int)_member.CustomerId);
                //locationSer.GetLocationIdByLocationName(_member.LocationName, (int)_member.CustomerId);
                if (_locId > 0)
                {
                    _member.LocationId = _locId;
                }

                var validationResults = new List<ValidationResult>();
                bResult = false;

                if (ValidateMemberFile(_member, out validationResults))
                {
                    if (CheckDate(_member.strStartDate) && CheckDate(_member.strEndDate))
                    {
                        _member.StartDate = Convert.ToDateTime(_member.strStartDate);
                        _member.EndDate = Convert.ToDateTime(_member.strEndDate);
                        if (_member.StartDate > _member.EndDate)
                        {
                            sMessage = "start date should not greater than end date";
                        }
                        else
                        {

                            int CountPhoneNumber = 0;
                            if (!String.IsNullOrEmpty(_member.Phone_Number))
                            {
                                CountPhoneNumber = _member.Phone_Number.Split(',').Length;

                            }


                            if (_member.IsMultiDevice == true && CountPhoneNumber > _member.custMaxNoOfDevices)
                            {
                                sMessage = "Maximum " + _member.custMaxNoOfDevices.ToString() + " Phones can configure as per master data.";
                            }

                            else
                            {


                                RepositoryResult objresult = new RepositoryResult();
                                objresult = _membershipRepository.UploadMember(mapMemberKey(_member));
                                if (objresult.resultVal == "")
                                {
                                    sMessage = "Successfully Inserted.";
                                }
                                else
                                {
                                    sMessage = "Failed to Inserted.";
                                }
                            }
                        }

                        // Max Devices Validation




                        // Max Devices Validation

                    }
                    else
                    {
                        sMessage = "Start date or end date is invalid.";
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
                                case "MemberName":
                                    {
                                        keystr += "Member Name";
                                        break;
                                    }
                                case "Phone_Number":
                                    {
                                        keystr += "Phone Name";
                                        break;
                                    }
                                case "Email_ID":
                                    {
                                        keystr += "Email ID";
                                        break;
                                    }
                                case "LocationId":
                                    {
                                        keystr += "Store Number";
                                        break;
                                    }
                                case "strEndDate":
                                    {
                                        keystr += "End Date";
                                        break;
                                    }
                                case "strStartDate":
                                    {
                                        keystr += "Start Date";
                                        break;
                                    }
                                case "no_of_FreeDries":
                                    {
                                        keystr += "Revive Remaining";
                                        break;
                                    }
                                case "InvoiceNumber":
                                    {
                                        keystr += "Invoice Number";
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


                var UserResult = new MembershipResultModel()
                {
                    LineNumber = "Line Number " + nLineNumber,
                    Message = sMessage,
                    Status = bResult

                };

                lstMemberResult.Add(UserResult);
                nLineNumber++;
            }
            if (lstMemberModel.Count == 0)
            {
                var UserResult = new MembershipResultModel()
                {
                    LineNumber = "Line Number 1",
                    //Message = "Invalid file format.",
                    Message = errorMesg,
                    Status = bResult

                };
                lstMemberResult.Add(UserResult);
            }
            objMemberResult.LocationResult = lstMemberResult;
            return bResult;
        }
        public DataSourceResult GetMembershipDetailsES(DataSourceRequest req, MemberShipParameters ObjMemberShipParameters)
        {
            return _membershipRepository.GetMembershipDetailsES(req, ObjMemberShipParameters);

        }
        //public List<MemberShipDetails> GetMembershipDetailsES(DataSourceRequest req, MemberShipParameters ObjMemberShipParameters)
        //{
        //    return _membershipRepository.GetMembershipDetailsES(req, ObjMemberShipParameters);
        //}


        private bool ValidateMemberFile(ManageMember objMemberModel, out List<ValidationResult> validationResults)
        {

            if (objMemberModel.MemberName == "")
            {
                objMemberModel.MemberName = null;
            }
            //if (objMemberModel.Email_ID == "")
            //{
            //    objMemberModel.Email_ID = null;
            //}

            MembershipWebModel objMemberModelBulkModel = new MembershipWebModel();
            objMemberModelBulkModel.MemberName = objMemberModel.MemberName;
            objMemberModelBulkModel.Phone_Number = objMemberModel.Phone_Number;
            // objMemberModelBulkModel.LocationName = objMemberModel.LocationName;
            objMemberModelBulkModel.strStartDate = objMemberModel.strStartDate;
            objMemberModelBulkModel.strEndDate = objMemberModel.strEndDate;
            objMemberModelBulkModel.InvoiceNumber = objMemberModel.InvoiceNumber;
            objMemberModelBulkModel.no_of_FreeDries = objMemberModel.no_of_FreeDries;
            objMemberModelBulkModel.LocationId = objMemberModel.LocationId;

            bool bResult = false;


            validationResults = new List<ValidationResult>();

            var context = new ValidationContext(objMemberModelBulkModel, serviceProvider: null, items: null);

            bResult = Validator.TryValidateObject(objMemberModelBulkModel, context, validationResults, true);

            return bResult;
        }
        public ManageMember GetMemberDetailsByMemberDtlsID(MemberShipParameters ObjMemberShipParameters)
        {
            return _membershipRepository.GetMemberDetailsByMemberDtlsID(ObjMemberShipParameters);
        }

        public bool updateMemberDetailsByMemberDtlsID(ManageMember objMembershipDetails)
        {
            return _membershipRepository.updateMemberDetailsByMemberDtlsID(objMembershipDetails);
        }

        public bool voidMemberShipByMembershipId(MemberShipParameters MembershipPara)
        {
            return _membershipRepository.voidMemberShipByMembershipId(MembershipPara);
        }
        public IEnumerable<ReviveDataByPhoneParam> GetReviveData(MembersShipModel objMembership)
        {
            return _membershipRepository.GetReviveData(objMembership);
        }
        public bool IsAPIAccessByClient(string CustRefCode, Guid APIKey)
        {
            return _membershipRepository.IsAPIAccessByClient(CustRefCode, APIKey);
        }
        public IEnumerable<MemberShipDetailsModel> SearchMemberByCustomerId(MembersShipModel objMembership)
        {
            return _membershipRepository.SearchMemberByCustomerId(objMembership);

        }
        public DateTime GetReviveLastDateByMembershipId(string MembershipID)
        {

            return _membershipRepository.GetReviveLastDateByMembershipId(MembershipID);
        }
        public string UpdateTabletNonMemberFieldAsEncryption(string key)
        {
            return _membershipRepository.UpdateTabletNonMemberFieldAsEncryption(key);

        }
        public IEnumerable<AppMachineHistoryModel> GetAppMachineHistoryByMachineId(MembersShipModel objMembership)
        {

            return _membershipRepository.GetAppMachineHistoryByMachineId(objMembership);
        }
        public AppMachineHistoryModel GetAppMachineHistoryByProcessId(MembersShipModel objMembership)
        {
            return _membershipRepository.GetAppMachineHistoryByProcessId(objMembership);

        }
    }
}
