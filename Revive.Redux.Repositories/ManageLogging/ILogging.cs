using Kendo.Mvc.UI;
using Revive.Redux.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Repositories
{
    public interface ILogging
    {
        bool Add(LoggingModel LoggingModelObj);
        DataSourceResult GetLoggingDetails(DataSourceRequest req, LoggingParameter ObjLoggingParameters);
    }
}
