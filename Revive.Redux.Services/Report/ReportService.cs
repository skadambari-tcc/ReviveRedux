using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revive.Redux.Entities;
using Revive.Redux.Repositories;
using Kendo.Mvc.UI;

namespace Revive.Redux.Services
{
    public class ReportService : IReportService
    {

        private IReportRepository _IReportRepository = null;

        public ReportService()
        {
            _IReportRepository = new ReportRepository();
        }



        public List<ReportMasterModel> GetReports()
        {
            return _IReportRepository.GetReports();
        }

        public List<ReportModel> ShowReport(int nReportId, string sParameter, Guid loginUserId, string pageAccessCode)
        {
            return _IReportRepository.ShowReport(nReportId, sParameter, loginUserId, pageAccessCode);
        }

        public DataSourceResult LoadReport(DataSourceRequest request, int nReportId, string sParameter, Guid loginUserId, string pageAccessCode)
        {
            return _IReportRepository.LoadReport(request, nReportId, sParameter, loginUserId, pageAccessCode);
        }
        public DataSourceResult LoadActivityReport(DataSourceRequest request, int customerId, int subsidiaryId, int subAgentId, int locationId,
            int modeId, string dateFrom, string dateto)
        {
            return _IReportRepository.LoadActivityReport(request, customerId, subsidiaryId, subAgentId, locationId, modeId, dateFrom, dateto);
        }
        public ReportConfigModel GetReportById(int nReportId)
        {
            return _IReportRepository.GetReportById(nReportId);
        }
        public DataSourceResult LoadActivityReportDetails(DataSourceRequest request, int customerId, int subsidiaryId, int subAgentId, int locationId, int modeId, string dateFrom, string dateto)
        {
            return _IReportRepository.LoadActivityReportDetails(request, customerId, subsidiaryId, subAgentId, locationId, modeId, dateFrom, dateto);
        }
        public List<ActivityReport> ExportActivityRawData(int customerId, int subsidiaryId, int subAgentId, int locationId, int modeId, string dateFrom, string dateto)
        {

            return _IReportRepository.ExportActivityRawData(customerId, subsidiaryId, subAgentId, locationId, modeId, dateFrom, dateto);
        }
    }
}
