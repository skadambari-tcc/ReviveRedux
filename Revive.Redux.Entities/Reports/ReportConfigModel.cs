using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public class ReportConfigModel : PageBasic 
    {
        public int LocationId { get; set; }
        public int CustomerId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public DateTime DateExpire { get; set; }
        public string MembershipStatus { get; set; }
        public int PhoneManufacturerId { get; set; }
        public int OrderId { get; set; }
        public int MachineId { get; set; }
        public int ReportId { get; set; }
        public int ModeId { get; set; }
        public int ManufacturerId { get; set; }
        public int SubsidiaryID { get; set; }
        public int SubAgentID { get; set; }
        public int GroupId { get; set; }

        public string[] SubAgentIDMulti { get; set; }
        public string SubAgentIDMultiValue { get; set; }
        //Membership Modal Start
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email_ID { get; set; }
        public string Membership_ID { get; set; }
        public string MemberName { get; set; }
        public string Phone_Number { get; set; }
        public string DOB { get; set; }
        public string LocationName { get; set; }
        public Guid user_Id { get; set; }
        public List<MembershipResultModel> LocationResult { get; set; }
        public string ErrorMsgs { get; set; }
        public string FileName { get; set; }  // file Name
        public List<MembershipResultModel> UserResult { get; set; }
        public bool IsLegacy { get; set; }
        public string InvoiceNumber { get; set; }
        public int reviveDeviceId { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public string strEndDate { get; set; }
        public string strStartDate { get; set; }
        public bool flag { get; set; }
        public int no_of_FreeDries { get; set; }
        public string MultiDeviceTypeCode { get; set; }
        public string ReportStatusCode { get; set; }
        //Membership Modal End

        public bool IsReportAvailable { get; set; }

        private IEnumerable<DtoList> _CustomerNameList;

        public IEnumerable<DtoList> CustomerNameList
        {
            get { return _CustomerNameList; }
            set { _CustomerNameList = value; }
        }

        private IEnumerable<DtoList> _LocationList;

        public IEnumerable<DtoList> LocationList
        {
            get { return _LocationList; }
            set { _LocationList = value; }
        }
        private IEnumerable<DtoList> _ModeList;
        public IEnumerable<DtoList> ModeList
        {
            get { return _ModeList; }
            set { _ModeList = value; }
        }


        private IEnumerable<ReportFilterType> _ReportFilterTypeList;

        public IEnumerable<ReportFilterType> ReportFilterTypeList
        {
            get { return _ReportFilterTypeList; }
            set { _ReportFilterTypeList = value; }
        }

        private ReportMasterModel _ReportMasterList;

        public ReportMasterModel ReportMasterList
        {
            get { return _ReportMasterList; }
            set { _ReportMasterList = value; }
        }

        private List<ReportModel> _ReportModelList;

        public List<ReportModel> ReportModelList
        {
            get { return _ReportModelList; }
            set { _ReportModelList = value; }
        }

        private List<ReportModel> _ReportModelResultList;

        public List<ReportModel> ReportModelResultList
        {
            get { return _ReportModelResultList; }
            set { _ReportModelResultList = value; }
        }

        private IEnumerable<DtoList> _PhoneManufacturerList;

        public IEnumerable<DtoList> PhoneManufacturerList
        {
            get { return _PhoneManufacturerList; }
            set { _PhoneManufacturerList = value; }
        }

        private IEnumerable<DtoList> _OrderList;

        public IEnumerable<DtoList> OrderList
        {
            get { return _OrderList; }
            set { _OrderList = value; }
        }

        private IEnumerable<DtoList> _MachineList;

        public IEnumerable<DtoList> MachineList
        {
            get { return _MachineList; }
            set { _MachineList = value; }
        }

        private IEnumerable<DtoList> _ManufacturerList;

        public IEnumerable<DtoList> ManufacturerList
        {
            get { return _ManufacturerList; }
            set { _ManufacturerList = value; }
        }


        private IEnumerable<DtoList> _SubsidiaryList;

        public IEnumerable<DtoList> SubsidiaryList
        {
            get { return _SubsidiaryList; }
            set { _SubsidiaryList = value; }
        }

        private IEnumerable<DtoList> _SubAgentList;

        public IEnumerable<DtoList> SubAgentList
        {
            get { return _SubAgentList; }
            set { _SubAgentList = value; }
        }
        private IEnumerable<DtoList> _LocationGroupList;
        public IEnumerable<DtoList> LocationGroupList
        {
            get { return _LocationGroupList; }
            set { _LocationGroupList = value; }
        }

        private IEnumerable<DtoList> _MultiDeviceList;
        public IEnumerable<DtoList> MultiDeviceList
        {
            get { return _MultiDeviceList; }
            set { _MultiDeviceList = value; }
        }
        private IEnumerable<DtoList> _ReportStatusList;
        public IEnumerable<DtoList> ReportStatusList
        {
            get { return _ReportStatusList; }
            set { _ReportStatusList = value; }
        }



        public string GridModel { get; set; }
        public string GridTitle { get; set; }
        public string GridReportId { get; set; }
    
    }
    public class MembershipResultModel
    {
        public string LineNumber { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }
    }
}
