using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Revive.Redux.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Revive.Redux.Repositories
{
    public class Logging : ILogging
    {
        public bool Add(LoggingModel LoggingModelObj)
        {
            try
            {
                TabletAppLogFile TabletAppLogFileObj = new TabletAppLogFile();
                using (var dbContext = new ReviveDBEntities())
                {
                    if (LoggingModelObj != null)
                    {
                        var UserDetails = dbContext.Users.FirstOrDefault(us => us.User_ID == LoggingModelObj.Created_by);

                        if (UserDetails != null)
                        {
                            var LocationDetails = dbContext.Customers_Locations.FirstOrDefault(locs => locs.Location_ID == UserDetails.Location_ID);
                            TabletAppLogFileObj.Location_Id = LocationDetails.Location_ID;
                            TabletAppLogFileObj.Customer_ID = LocationDetails.Customer_ID;
                            TabletAppLogFileObj.Subsidiary_ID = LocationDetails.Subsidiary_ID;
                            TabletAppLogFileObj.SubAgent_ID = LocationDetails.SubAgent_ID;
                        }

                        TabletAppLogFileObj.File_Path = LoggingModelObj.FilePath;
                        TabletAppLogFileObj.TimeStamp = LoggingModelObj.LogFileTimeStamp;
                        TabletAppLogFileObj.Created_by = LoggingModelObj.Created_by;


                        TabletAppLogFileObj.Created_Date = System.DateTime.Now;
                        dbContext.TabletAppLogFiles.Add(TabletAppLogFileObj);
                        dbContext.SaveChanges();

                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return true;
        }

        public DataSourceResult GetLoggingDetails(DataSourceRequest req, LoggingParameter ObjLoggingParameters)
        {
            List<LoggingModel> obj=new List<LoggingModel>();
            using (var dbContext = new ReviveDBEntities())
            {
                var lstLoggingDetails = (from loggingDetails in dbContext.TabletAppLogFiles select loggingDetails);

                if (ObjLoggingParameters.DateFrom == Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                {
                    ObjLoggingParameters.DateFrom = DateTime.Now.AddMonths(-1);
                    ObjLoggingParameters.DateTo = DateTime.Now;
                }
                else
                {
                    ObjLoggingParameters.DateFrom = Convert.ToDateTime(ObjLoggingParameters.DateFrom.ToString("MM/dd/yyyy") + " 00:00");
                    ObjLoggingParameters.DateTo = Convert.ToDateTime(ObjLoggingParameters.DateTo.ToString("MM/dd/yyyy") + " 23:59");
                }

                lstLoggingDetails = lstLoggingDetails.Where(d => d.Created_Date >= ObjLoggingParameters.DateFrom && d.Created_Date <= ObjLoggingParameters.DateTo);

                if (ObjLoggingParameters.LocationId > 0)
                {
                    lstLoggingDetails = lstLoggingDetails.Where(d => d.Location_Id == ObjLoggingParameters.LocationId);
                }
                if (ObjLoggingParameters.CustomerId > 0)
                {
                    lstLoggingDetails = lstLoggingDetails.Where(d => d.Customer_ID == ObjLoggingParameters.CustomerId);
                }
                if (ObjLoggingParameters.SubsidiaryId > 0)
                {
                    lstLoggingDetails = lstLoggingDetails.Where(d => d.Subsidiary_ID == ObjLoggingParameters.SubsidiaryId);

                }
                if (ObjLoggingParameters.SubAgentId > 0)
                {
                    lstLoggingDetails = lstLoggingDetails.Where(d => d.SubAgent_ID == ObjLoggingParameters.SubAgentId);

                }
                
                obj = lstLoggingDetails.Select(d => new LoggingModel
                  {
                      FilePath = d.File_Path,
                      LocationName = d.Location_Id != null ? dbContext.Customers_Locations.Where(c => c.Location_ID == d.Location_Id).Select(c => c.Location_Name).FirstOrDefault() : "--",
                      LogFileTimeStamp = d.TimeStamp,
                      UserName = d.Created_by != null ? dbContext.Users.Where(c => c.User_ID == d.Created_by).Select(c => c.FirstName != null ? c.FirstName : "--" + " " + c.LastName != null ? c.FirstName : "--").FirstOrDefault() : "--"
                  }).ToList();

               
            }
            //var data1 = List<LoggingModel>();
            foreach (var FileDetails in obj)
            {
                FileDetails.IsFileExists = IsFileExists(FileDetails.FilePath);
            } 
            
            DataSourceResult result = obj.ToDataSourceResult(req);
                return result;
        }

        private bool IsFileExists(string FilePath)
        {
            string LoggingFilesDir = System.Web.HttpContext.Current.Server.MapPath(@"\LoggingFiles\");
            bool IsFileExists = false;

            if (File.Exists(LoggingFilesDir + FilePath))
            {
                IsFileExists = true;
            }

            return IsFileExists;
        }
    }
}
