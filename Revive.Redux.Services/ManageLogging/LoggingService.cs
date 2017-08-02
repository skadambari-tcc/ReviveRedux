using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revive.Redux.Repositories;
using Revive.Redux.Entities;
using Kendo.Mvc.UI;

namespace Revive.Redux.Services
{

    public class LoggingService : ILoggingService
    {
        private Logging loggingRepository = null;
        public LoggingService()
        {
            loggingRepository = new Logging();
        }

        public bool Add(LoggingModel LoggingModelObj)
        {
            return loggingRepository.Add(LoggingModelObj);
        }

        public DataSourceResult GetLoggingDetails(DataSourceRequest req, LoggingParameter ObjLoggingParameters)
        {
            return loggingRepository.GetLoggingDetails(req, ObjLoggingParameters);
        }


    }
}
