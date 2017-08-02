using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revive.Redux.Entities;
//using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Revive.Redux.Commn;
namespace Revive.Redux.Repositories
{
    public class ReportRepository : IReportRepository
    {
        public List<ReportMasterModel> GetReports()
        {
            try
            {
                var lstReports = new List<ReportMasterModel>();
                using (var dbContext = new ReviveDBEntities())
                {

                    var lstReportCollection = (from d in dbContext.Report_Master
                                               select new ReportMasterModel
                                                {
                                                    ReportId = d.Report_Id,
                                                    Name = d.Name,
                                                    Description = d.Description,
                                                    Filters = d.Filters,
                                                    Fields = d.Fields,
                                                    Authorization = d.Authorization,
                                                    FieldName = d.Field_Name
                                                }).ToList();

                    lstReports.AddRange(lstReportCollection);
                }
                return lstReports;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<ReportModel> ShowReport(int nReportId, string sParameter, Guid loginUserId, string pageAccessCode)
        {
            try
            {
                string sKey = System.Configuration.ConfigurationSettings.AppSettings["Encrption"].ToString();
                var lstReport = new List<ReportModel>();
                using (var dbContext = new ReviveDBEntities())
                {
                    var lstReportDetails = dbContext.usp_GetReports(nReportId, sParameter, loginUserId, pageAccessCode).Select(d => new ReportModel
                    {
                        custom1 = d.custom1,
                        custom2 = d.custom2 != null && d.custom2.Trim() != "" && d.custom2.Trim() != string.Empty ? (d.custom2.LastIndexOf("Non-Member", StringComparison.Ordinal) > 0 ? Cryptography.SplitText(Cryptography.DecryptText(d.custom2.Substring(0, d.custom2.LastIndexOf("Non-Member", StringComparison.Ordinal)), sKey), "@") : d.custom2) : d.custom2,
                        custom3 = d.custom3,
                        custom4 = d.custom4,
                        custom5 = d.custom5,
                        custom6 = d.custom6,
                        custom7 = d.custom7,
                        custom8 = d.custom8,
                        custom9 = d.custom9,
                        custom10 = d.custom10

                    }).ToList();

                    lstReport.AddRange(lstReportDetails);
                }
                return lstReport;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private dynamic CheckStringToNumeric(string value)
        {

            double resultVal = 0.0;
            string s = value;
            dynamic resultDynamic;

            bool result = double.TryParse(s, out resultVal);
            if (result == false)
            {
                resultDynamic = value;

            }
            else
            {
                resultDynamic = resultVal;
            }
            return resultDynamic;


        }
        public DataSourceResult LoadActivityReport(DataSourceRequest request, int customerId, int subsidiaryId, int subAgentId, int locationId,
            int modeId, string dateFrom, string dateto)
        {
            string sKey = System.Configuration.ConfigurationSettings.AppSettings["Encrption"].ToString();
            using (var dbContext = new ReviveDBEntities())
            {
                var lstReportDetails = dbContext.usp_vzMachineActivity(dateFrom, dateto, customerId, subsidiaryId, subAgentId, locationId, modeId).Select(d => new ActivityReport
                {
                    ActivityDate = d.Activity_Date,
                    MachineId = d.Machine_Id,
                    LocationName = d.Location_Name,
                    MemberName = d.MemberName != null && d.MemberName.Trim() != "" && d.MemberName.Trim() != string.Empty ? (d.Membership.LastIndexOf("Non-Member") == 0 ? Cryptography.SplitText(Cryptography.DecryptText(d.MemberName, sKey), "@") : d.MemberName) : d.MemberName,
                    Email = d.Email != null && d.Email.Trim() != "" && d.Email.Trim() != string.Empty ? (d.Membership.LastIndexOf("Non-Member") == 0 ? Cryptography.SplitText(Cryptography.DecryptText(d.Email, sKey), "@") : d.Email) : d.Email,
                    PhoneNumber = d.PhoneNumber != null && d.PhoneNumber.Trim() != "" && d.PhoneNumber.Trim() != string.Empty ? (d.Membership.LastIndexOf("Non-Member") == 0 ? Cryptography.SplitText(Cryptography.DecryptText(d.PhoneNumber, sKey), "@") : d.PhoneNumber) : d.PhoneNumber,
                    IsTmp = d.IsTMP,
                    InvoiceNo = d.Invoice_no,
                    ProcessId = d.process_id,
                    ModeName = d.ModeName,
                    DryTime = d.Dry_time,
                    MfrType = d.MFRType,
                    TimeSincePeril = d.TimeSincePeril,
                    PluginYes = d.PluginYes,
                    ReviveSuccessStatus = d.ReviveSuccessStatus,
                    WaterRemoved = d.Water_removed,
                    UserName = d.UserName,
                    Membership = d.Membership,
                    MachineActivityId = d.Machine_Activity_ID
                }).ToList();

                DataSourceResult result = lstReportDetails.ToDataSourceResult(request);
                return result;
            }
        }

        public DataSourceResult LoadActivityReportDetails(DataSourceRequest request, int customerId, int subsidiaryId, int subAgentId, int locationId, int modeId, string dateFrom, string dateto)
        {
            try
            {
                string sKey = System.Configuration.ConfigurationSettings.AppSettings["Encrption"].ToString();
                using (var dbContext = new ReviveDBEntities())
                {
                    var lstReportDetails = dbContext.usp_MachineActivityDetails(dateFrom, dateto, customerId, subsidiaryId, subAgentId, locationId, modeId).Select(d => new ActivityReport
                    {

                        ProcessId = d.process_id,
                        ActivityDate = d.Activity_Date,
                        MachineActivityId = d.Machine_Activity_ID,
                        MachineId = d.Machine_Id,
                        UserName = d.UserName,
                        MemberName = d.MemberName != null && d.MemberName.Trim() != "" && d.MemberName.Trim() != string.Empty ? (d.Membership.LastIndexOf("Non-Member") == 0 ? Cryptography.SplitText(Cryptography.DecryptText(d.MemberName, sKey), "@") : d.MemberName) : d.MemberName,
                        Email = d.Email != null && d.Email.Trim() != "" && d.Email.Trim() != string.Empty ? (d.Membership.LastIndexOf("Non-Member") == 0 ? Cryptography.SplitText(Cryptography.DecryptText(d.Email, sKey), "@") : d.Email) : d.Email,
                        PhoneNumber = d.PhoneNumber != null && d.PhoneNumber.Trim() != "" && d.PhoneNumber.Trim() != string.Empty ? (d.Membership.LastIndexOf("Non-Member") == 0 ? Cryptography.SplitText(Cryptography.DecryptText(d.PhoneNumber, sKey), "@") : d.PhoneNumber) : d.PhoneNumber,
                        Membership = d.Membership,
                        InvoiceNo = d.Invoice_no,
                        MfrType = d.MFRType,
                        PluginYes = d.PluginYes,
                        ReviveSuccessStatus = d.ReviveSuccessStatus,
                        ModeName = d.ModeName,
                        TimeSincePeril = d.TimeSincePeril,
                        CheckOutResults = d.CheckOutResults,
                        ActivityResults = d.ActivityResults

                    }).ToList();

                    DataSourceResult result = lstReportDetails.ToDataSourceResult(request);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        /// <summary>
        /// Added new Repository method on dated 29-05-2017 for exporting activity raw data
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="subsidiaryId"></param>
        /// <param name="subAgentId"></param>
        /// <param name="locationId"></param>
        /// <param name="modeId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateto"></param>
        /// <returns></returns>
        public List<ActivityReport> ExportActivityRawData( int customerId, int subsidiaryId, int subAgentId, int locationId, int modeId, string dateFrom, string dateto)
        {
            try
            {
                string sKey = System.Configuration.ConfigurationSettings.AppSettings["Encrption"].ToString();
                using (var dbContext = new ReviveDBEntities())
                {
                    var lstReportDetails = dbContext.usp_MachineActivityDetails(dateFrom, dateto, customerId, subsidiaryId, subAgentId, locationId, modeId).Select(d => new ActivityReport
                    {

                        ProcessId = d.process_id,
                        ActivityDate = d.Activity_Date,
                        MachineActivityId = d.Machine_Activity_ID,
                        MachineId = d.Machine_Id,
                        UserName = d.UserName,
                        MemberName = d.MemberName != null && d.MemberName.Trim() != "" && d.MemberName.Trim() != string.Empty ? (d.Membership.LastIndexOf("Non-Member") == 0 ? Cryptography.SplitText(Cryptography.DecryptText(d.MemberName, sKey), "@") : d.MemberName) : d.MemberName,
                        Email = d.Email != null && d.Email.Trim() != "" && d.Email.Trim() != string.Empty ? (d.Membership.LastIndexOf("Non-Member") == 0 ? Cryptography.SplitText(Cryptography.DecryptText(d.Email, sKey), "@") : d.Email) : d.Email,
                        PhoneNumber = d.PhoneNumber != null && d.PhoneNumber.Trim() != "" && d.PhoneNumber.Trim() != string.Empty ? (d.Membership.LastIndexOf("Non-Member") == 0 ? Cryptography.SplitText(Cryptography.DecryptText(d.PhoneNumber, sKey), "@") : d.PhoneNumber) : d.PhoneNumber,
                        Membership = d.Membership,
                        InvoiceNo = d.Invoice_no,
                        MfrType = d.MFRType,
                        PluginYes = d.PluginYes,
                        ReviveSuccessStatus = d.ReviveSuccessStatus,
                        ModeName = d.ModeName,
                        TimeSincePeril = d.TimeSincePeril,
                        CheckOutResults = d.CheckOutResults,
                        ActivityResults = d.ActivityResults

                    }).ToList();


                    return lstReportDetails;
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }


        public DataSourceResult LoadReport([DataSourceRequest]DataSourceRequest request, int nReportId, string sParameter, Guid loginUserId, string pageAccessCode)
        {
            try
            {

                if (nReportId == 1 || nReportId == 12)
                {
                    using (var dbContext = new ReviveDBEntities())
                    {
                        var lstReportDetails = dbContext.usp_GetReports(nReportId, sParameter, loginUserId, pageAccessCode).Select(d => new ReportModel
                        {
                            custom1 = d.custom1,
                            custom2 = d.custom2,
                            custom3 = d.custom3,
                            custom4 = d.custom4,
                            custom5 = d.custom5,
                            custom6 = d.custom6,
                            custom7 = d.custom7,
                            custom8 = d.custom8,
                            custom9 = d.custom9,
                            custom10 = d.custom10
                        }).ToList();

                        DataSourceResult result = lstReportDetails.ToDataSourceResult(request);
                        return result;
                    }
                }
                else if (nReportId == 2)
                {
                    using (var dbContext = new ReviveDBEntities())
                    {
                        var lstReportDetails = dbContext.usp_GetReports(nReportId, sParameter, loginUserId, pageAccessCode).Select(d => new MembershipAggregate
                        {
                            custom1 = d.custom1,
                            custom2 = d.custom2,
                            custom3 = d.custom3,
                            custom4 = Convert.ToInt32(d.custom4),
                            custom5 = d.custom5,
                            custom6 = d.custom6,
                            custom7 = d.custom7,
                            custom8 = d.custom8,
                            custom9 = Convert.ToDateTime(d.custom9),
                            custom10 = Convert.ToDateTime(d.custom10)

                        }).ToList();

                        DataSourceResult result = lstReportDetails.ToDataSourceResult(request);
                        return result;
                    }
                }
                else if (nReportId == 3)
                {
                    using (var dbContext = new ReviveDBEntities())
                    {
                        var lstReportDetails = dbContext.usp_GetReports(nReportId, sParameter, loginUserId, pageAccessCode).Select(d => new MembershipExpiring
                        {
                            custom1 = d.custom1,
                            custom2 = d.custom2,
                            custom3 = d.custom3,
                            custom4 = d.custom4,
                            custom5 = Convert.ToDateTime(d.custom5),
                            custom6 = d.custom6,
                            custom7 = d.custom7,
                            custom8 = d.custom8,
                            custom9 = d.custom9,
                            custom10 = d.custom10

                        }).ToList();

                        DataSourceResult result = lstReportDetails.ToDataSourceResult(request);
                        return result;
                    }
                }
                else if (nReportId == 4)
                {
                    using (var dbContext = new ReviveDBEntities())
                    {
                        var lstReportDetails = dbContext.usp_GetReports(nReportId, sParameter, loginUserId, pageAccessCode).Select(d => new MachineStartStop
                        {
                            custom1 = Convert.ToDateTime(d.custom1),
                            custom2 = d.custom2,
                            custom3 = d.custom3,
                            custom4 = Convert.ToInt32(d.custom4),
                            custom5 = d.custom5,
                            custom6 = d.custom6,
                            custom7 = d.custom7,
                            custom8 = d.custom8,
                            custom9 = d.custom9,
                            custom10 = d.custom10

                        }).ToList();

                        DataSourceResult result = lstReportDetails.ToDataSourceResult(request);
                        return result;
                    }
                }
                else if (nReportId == 5)
                {
                    using (var dbContext = new ReviveDBEntities())
                    {
                        var lstReportDetails = dbContext.usp_GetReports(nReportId, sParameter, loginUserId, pageAccessCode).Select(d => new SuccessRateAggregate
                        {
                            custom1 = d.custom1,
                            custom2 = d.custom2,
                            custom3 = Convert.ToInt32(d.custom3),
                            custom4 = Convert.ToDouble(d.custom4),
                            custom5 = Convert.ToDouble(d.custom5),
                            custom6 = Convert.ToDouble(d.custom6),
                            custom7 = Convert.ToDouble(d.custom7),
                            custom8 = d.custom8,
                            custom9 = d.custom9,
                            custom10 = d.custom10

                        }).ToList();

                        DataSourceResult result = lstReportDetails.ToDataSourceResult(request);
                        return result;
                    }
                }
                else if (nReportId == 6)
                {
                    using (var dbContext = new ReviveDBEntities())
                    {
                        var lstReportDetails = dbContext.usp_GetReports(nReportId, sParameter, loginUserId, pageAccessCode).Select(d => new SuccessRateMember
                        {
                            custom1 = d.custom1,
                            custom2 = d.custom2,
                            custom3 = Convert.ToInt32(d.custom3),
                            custom4 = Convert.ToDouble(d.custom4),
                            custom5 = Convert.ToDouble(d.custom5),
                            custom6 = Convert.ToDouble(d.custom6),
                            custom7 = d.custom7,
                            custom8 = d.custom8,
                            custom9 = d.custom9,
                            custom10 = d.custom10

                        }).ToList();

                        DataSourceResult result = lstReportDetails.ToDataSourceResult(request);
                        return result;
                    }
                }
                else if (nReportId == 7)
                {
                    using (var dbContext = new ReviveDBEntities())
                    {
                        var lstReportDetails = dbContext.usp_GetReports(nReportId, sParameter, loginUserId, pageAccessCode).Select(d => new SuccessRateManufacturer
                        {
                            custom1 = d.custom1,
                            custom2 = d.custom2 != null ? Convert.ToInt32(d.custom2) : 0,
                            custom3 = d.custom3 != null ? Convert.ToDouble(d.custom3) : 0,
                            custom4 = d.custom4 != null ? Convert.ToDouble(d.custom4) : 0,
                            custom5 = d.custom5 != null ? Convert.ToDouble(d.custom5) : 0,
                            custom6 = d.custom6 != null ? Convert.ToDouble(d.custom6) : 0,
                            custom7 = d.custom7,
                            custom8 = d.custom8,
                            custom9 = d.custom9,
                            custom10 = d.custom10

                        }).ToList();

                        DataSourceResult result = lstReportDetails.ToDataSourceResult(request);
                        return result;
                    }
                }
                else if (nReportId == 8)
                {
                    using (var dbContext = new ReviveDBEntities())
                    {
                        var lstReportDetails = dbContext.usp_GetReports(nReportId, sParameter, loginUserId, pageAccessCode).Select(d => new KillMachines
                        {
                            custom1 = d.custom1,
                            custom2 = d.custom2,
                            custom3 = d.custom3,
                            custom4 = Convert.ToDateTime(d.custom4),
                            custom5 = d.custom5,
                            custom6 = d.custom6,
                            custom7 = d.custom7,
                            custom8 = d.custom8,
                            custom9 = d.custom9,
                            custom10 = d.custom10

                        }).ToList();

                        DataSourceResult result = lstReportDetails.ToDataSourceResult(request);
                        return result;
                    }
                }

                else if (nReportId == 9 || nReportId == 10)
                {
                    using (var dbContext = new ReviveDBEntities())
                    {
                        var lstReportDetails = dbContext.usp_GetReports(nReportId, sParameter, loginUserId, pageAccessCode).Select(d => new LeastUsageStore
                        {
                            custom1 = d.custom1,
                            custom2 = d.custom2,
                            custom3 = d.custom3,
                            custom4 = Convert.ToInt32(d.custom4),
                            custom5 = d.custom5,
                            custom6 = d.custom6,
                            custom7 = d.custom7,
                            custom8 = d.custom8,
                            custom9 = d.custom9,
                            custom10 = d.custom10

                        }).ToList();

                        DataSourceResult result = lstReportDetails.ToDataSourceResult(request);
                        return result;
                    }
                }
                else if (nReportId == 11)
                {
                    using (var dbContext = new ReviveDBEntities())
                    {
                        var lstReportDetails = dbContext.usp_GetReports(nReportId, sParameter, loginUserId, pageAccessCode).Select(d => new ReportModel
                        {
                            custom1 = d.custom1,
                            custom2 = d.custom2,
                            custom3 = d.custom3,
                            custom4 = d.custom4,
                            custom5 = d.custom5,
                            custom6 = d.custom6,
                            custom7 = d.custom7,
                            custom8 = d.custom8,
                            custom9 = d.custom9,
                            custom10 = d.custom10

                        }).ToList();

                        DataSourceResult result = lstReportDetails.ToDataSourceResult(request);
                        return result;
                    }
                }

                else if (nReportId == 13)
                {
                    using (var dbContext = new ReviveDBEntities())
                    {
                        var lstReportDetails = dbContext.usp_GetReports(nReportId, sParameter, loginUserId, pageAccessCode).Select(d => new getreportofmembershipexpired
                        {
                            custom1 = Convert.ToDouble(d.custom1),
                            custom2 = d.custom2,
                            custom3 = d.custom3,
                            custom4 = d.custom4,
                            custom5 = Convert.ToDateTime(d.custom5),
                            custom6 = d.custom6,
                            custom7 = d.custom7,
                            custom8 = d.custom8,
                            custom9 = d.custom9,
                            custom10 = d.custom10

                        }).ToList();

                        DataSourceResult result = lstReportDetails.ToDataSourceResult(request);
                        return result;
                    }
                }
                else if (nReportId == 15 || nReportId == 18)
                {

                    string sKey = System.Configuration.ConfigurationSettings.AppSettings["Encrption"].ToString();

                    using (var dbContext = new ReviveDBEntities())
                    {

                        var lstReportDetails = dbContext.usp_GetReports(nReportId, sParameter, loginUserId, pageAccessCode).Select(d => new MachineFault
                        {
                            custom1 = Convert.ToDateTime(d.custom1),
                            custom2 = d.custom2,
                            custom3 = d.custom3,
                            custom4 = d.custom4,
                            custom5 = d.custom5,
                            custom6 = d.custom6 != null && d.custom6.Trim() != "" && d.custom6.Trim() != string.Empty ? (d.custom6.LastIndexOf("Non-Member") > 0 ? Cryptography.SplitText(Cryptography.DecryptText(d.custom6.Substring(0, d.custom6.LastIndexOf("Non-Member", StringComparison.Ordinal)), sKey), "@") : d.custom6) : d.custom6,
                            //custom6=d.custom6,
                            custom7 = d.custom7,
                            custom8 = d.custom8,
                            custom9 = d.custom9,
                            custom10 = d.custom10

                        }).ToList();

                        DataSourceResult result = lstReportDetails.ToDataSourceResult(request);
                        return result;
                    }
                }
                else if (nReportId == 19)
                {
                    using (var dbContext = new ReviveDBEntities())
                    {
                        var lstReportDetails = dbContext.usp_GetReports(nReportId, sParameter, loginUserId, pageAccessCode).Select(d => new Reconciliation
                        {
                            custom1 = d.custom1,
                            custom2 = d.custom2,
                            custom3 = d.custom3,
                            custom4 = Convert.ToDateTime(d.custom4),
                            custom5 = Convert.ToDateTime(d.custom5),
                            custom6 = d.custom6,
                            custom7 = d.custom7,
                            custom8 = d.custom8,
                            custom9 = d.custom9,
                            custom10 = d.custom10

                        }).ToList();

                        DataSourceResult result = lstReportDetails.ToDataSourceResult(request);
                        return result;
                    }
                }

                else
                {
                    using (var dbContext = new ReviveDBEntities())
                    {
                        var lstReportDetails = dbContext.usp_GetReports(nReportId, sParameter, loginUserId, pageAccessCode).Select(d => new ReportModel
                        {
                            custom1 = d.custom1,
                            custom2 = d.custom2,
                            custom3 = d.custom3,
                            custom4 = d.custom4,
                            custom5 = d.custom5,
                            custom6 = d.custom6,
                            custom7 = d.custom7,
                            custom8 = d.custom8,
                            custom9 = d.custom9,
                            custom10 = d.custom10

                        }).ToList();

                        DataSourceResult result = lstReportDetails.ToDataSourceResult(request);
                        return result;


                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public ReportConfigModel GetReportById(int nReportId)
        {
            try
            {
                var objReport = new ReportConfigModel();
                using (var dbContext = new ReviveDBEntities())
                {
                    var objReportDetails = dbContext.Report_Master.Where(cond => cond.Report_Id == nReportId).FirstOrDefault();

                    if (objReportDetails != null)
                    {
                        var objReportReportMasterList = new ReportMasterModel();
                        objReportReportMasterList.Name = objReportDetails.Name;
                        objReportReportMasterList.Description = objReportDetails.Description;
                        objReportReportMasterList.Filters = objReportDetails.Filters;
                        objReportReportMasterList.ReportId = objReportDetails.Report_Id;
                        objReportReportMasterList.Fields = objReportDetails.Fields;
                        objReportReportMasterList.Authorization = objReportDetails.Authorization;
                        objReportReportMasterList.FieldName = objReportDetails.Field_Name;

                        objReport.ReportMasterList = objReportReportMasterList;


                        var lstReportFilterType = new List<ReportFilterType>();

                        foreach (var objFilterType in objReportDetails.Report_Filter_Type.OrderBy(d => d.Priority))
                        {
                            var objReportFilterType = new ReportFilterType();
                            objReportFilterType.FilterTypeId = objFilterType.Filter_Type_Id;
                            objReportFilterType.FilterType = objFilterType.Filter_Type;
                            objReportFilterType.FilterDataType = objFilterType.Filter_Data_Type;
                            objReportFilterType.FitlerText = objFilterType.Fitler_Text;
                            objReportFilterType.ReportId = objFilterType.Report_Id;
                            objReportFilterType.Priority = objFilterType.Priority;

                            lstReportFilterType.Add(objReportFilterType);
                        }

                        objReport.ReportFilterTypeList = lstReportFilterType;

                    }
                }
                return objReport;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
