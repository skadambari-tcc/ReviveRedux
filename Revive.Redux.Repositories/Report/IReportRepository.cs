using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revive.Redux.Entities;
using Kendo.Mvc.UI;

namespace Revive.Redux.Repositories
{
    public interface IReportRepository
    {
        List<ReportMasterModel> GetReports();
        List<ReportModel> ShowReport(int nReportId, string sParameter, Guid loginUserId, string pageAccessCode);
        ReportConfigModel GetReportById(int nReportId);
        DataSourceResult LoadReport(DataSourceRequest request, int nReportId, string sParameter, Guid loginUserId, string pageAccessCode);
        DataSourceResult LoadActivityReport(DataSourceRequest request, int customerId, int subsidiaryId, int subAgentId, int locationId, int modeId, string dateFrom, string dateto);
        DataSourceResult LoadActivityReportDetails(DataSourceRequest request, int customerId, int subsidiaryId, int subAgentId, int locationId, int modeId, string dateFrom, string dateto);
        List<ActivityReport> ExportActivityRawData(int customerId, int subsidiaryId, int subAgentId, int locationId, int modeId, string dateFrom, string dateto);
           
    }
}
