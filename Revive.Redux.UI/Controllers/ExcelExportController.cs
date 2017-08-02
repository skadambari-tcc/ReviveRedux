using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using Revive.Redux.Commn.Excel;
using Revive.Redux.Services;
using Revive.Redux.Entities;


namespace Revive.Redux.UI.Controllers.ExcelExport
{
    public class ExcelExportController : Controller
    {
        //
        // GET: /ExcelExport/

        /// <summary>
        ///     Create the Excel spreadsheet.
        /// </summary>
        /// <param name="model">Definition of the columns for the spreadsheet.</param>
        /// <param name="data">Grid data.</param>
        /// <param name="title">Title of the spreadsheet.</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        public JsonResult ExportToExcel(string model, string data, string title, string ReportId)
        {
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                GridToExcel gridToExcel = new GridToExcel();
                if(ReportId!="")
                {
                    ReportService objRepservice = new ReportService();
                    ReportConfigModel obj = null;
                    obj = objRepservice.GetReportById(Convert.ToInt32(ReportId));
                    if(obj!=null)
                    {
                        gridToExcel.GenerateExcelFromGrid_Report(model, data, title, excelPackage, obj);
                    }

                    //gridToExcel.GenerateExcelFromGrid_Report(model, data, title, excelPackage, ReportId);
                }
                else
                {
                    gridToExcel.GenerateExcelFromGrid(model, data, title, excelPackage);
                }
                


                Session[title] = excelPackage.GetAsByteArray();
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        //private string[] GetLeaveDetailsReportExtraHeader(string title, string year)
        //{
        //    string[] extraHeader = new string[45];
        //    if (title == "Leave Details")
        //    {
        //        extraHeader[3] = "Yearly Data";
        //        extraHeader[7] = "January";
        //        extraHeader[10] = "February";
        //        extraHeader[13] = "March";
        //        extraHeader[16] = "April";
        //        extraHeader[19] = "May";
        //        extraHeader[22] = "June";
        //        extraHeader[25] = "July";
        //        extraHeader[28] = "August";
        //        extraHeader[31] = "September";
        //        extraHeader[34] = "October";
        //        extraHeader[37] = "November";
        //        extraHeader[40] = "December";
        //    }
        //    else if (title.Contains("Appraisal Summary"))
        //    {
        //        if (!string.IsNullOrEmpty(year))
        //        {
        //            extraHeader[1] = "Year - " + year;
        //        }
        //        extraHeader[5] = "January";
        //        extraHeader[8] = "February";
        //        extraHeader[11] = "March";
        //        extraHeader[14] = "April";
        //        extraHeader[17] = "May";
        //        extraHeader[20] = "June";
        //        extraHeader[23] = "July";
        //        extraHeader[26] = "August";
        //        extraHeader[29] = "September";
        //        extraHeader[32] = "October";
        //        extraHeader[35] = "November";
        //        extraHeader[38] = "December";
        //    }
        //    return extraHeader;

        //}

        /// <summary>
        ///     Download the spreadsheet.
        /// </summary>
        /// <param name="title">Title of the spreadsheet.</param>
        /// <returns></returns>
        public FileResult GetExcelFile(string title)
        {
            // Is there a spreadsheet stored in session?
            if (Session[title.Trim()] != null)
            {
                // Get the spreadsheet from seession.
                byte[] file = Session[title] as byte[];
                string filename = string.Format("{0}.xlsx", title);

                // Remove the spreadsheet from session.
                Session.Remove(title);

                // Return the spreadsheet.
                Response.Buffer = true;

                //Write it back to the client
                return File(file, MediaTypeNames.Application.Octet, filename);
            }
            else
            {
                throw new Exception(string.Format("{0} not found", title));
            }
        }
    }
}
