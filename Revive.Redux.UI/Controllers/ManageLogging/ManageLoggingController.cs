using Kendo.Mvc.UI;
using Revive.Redux.Controllers.Common;
using Revive.Redux.Entities;
using Revive.Redux.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;


namespace Revive.Redux.UI.Controllers
{
    [Authorize]
    [ReviveAuth]
    public class ManageLoggingController : Controller
    {

        #region Private Variables
        private IGeneralService generalService = null;
        private ILoggingService manageLoggingService = null;
        static string LoggingFiles = System.Web.HttpContext.Current.Server.MapPath(@"\LoggingFiles\");
        #endregion

        #region Construtors
        public ManageLoggingController()
        {
            generalService = new GeneralService();
            manageLoggingService = new LoggingService();
        }
        #endregion
        // GET: ManageLogging
        public ActionResult ManageLogging()
        {
            LoggingModel objLogging = new LoggingModel();
            ViewBag.IsModalValid = true;
            IEnumerable<DtoList> Types = null;
            Types = generalService.GetLoggingTypes();
            ViewBag.ExceptionTypes = Types;
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            objLogging.CustomerNameList = generalService.GetCustomerStoreUser(currentUser.User_Id, currentUser.PageAccessCode).ToList();
            objLogging.LocationList = generalService.GetEmptyDDLWithoutSelect();
            objLogging.SubsidiaryList = generalService.GetEmptyDDLWithoutSelect();
            objLogging.SubAgentList = generalService.GetEmptyDDLWithoutSelect();
            return View(objLogging);
        }

        [AjaxHandleException]
        public JsonResult GetSubsidiaryList(int CustomeId, bool bActive = false)
        {
            var customers = generalService.GetSubsidiaryByCustomerId(CustomeId);
            return Json(customers, JsonRequestBehavior.AllowGet);
        }
        [AjaxHandleException]
        public JsonResult GetSubAgentList(int SubsidiaryId, bool bActive = false)
        {
            var customers = generalService.GetAgentsBySubsidiaryId(SubsidiaryId);
            return Json(customers, JsonRequestBehavior.AllowGet);
        }

        [AjaxHandleException]
        public JsonResult GetLocationList(int CustomerId, int SubsidiaryId, int SubAgentId, bool bActive = false)
        {
            var customers = generalService.GetCustomerLocation(CustomerId, SubsidiaryId, SubAgentId, bActive);
            return Json(customers, JsonRequestBehavior.AllowGet);
        }

        [AjaxHandleException]
        public ActionResult SearchLoggingDetailsAjax([DataSourceRequest] DataSourceRequest request, LoggingParameter objLoggingParameter)
        {
            var _data = manageLoggingService.GetLoggingDetails(request, objLoggingParameter);
            return Json(_data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewLogFile()
        {
            string fileName = Request.QueryString["fname"];
            LoggingModel ObjLogging = new LoggingModel();
            var _data = ReadLogFile(fileName);

            if (_data != null && _data.Count > 0)
            {
                IEnumerable<DtoList> Types = null;
                Types = generalService.GetLoggingTypes();
                ViewBag.ExceptionTypes = Types;
                ObjLogging.FilePath = fileName.ToString();
                return View(ObjLogging);
            }
            else
            {
                TempData["IsFileExists"] = false;
                return RedirectToAction("ManageLogging", "ManageLogging");
            }
        }
        public FileResult DownloadLogFile(string FileName)
        {
            //string dirPath = @"c:\MyDir\";
            //string CurrentFileName = dirPath + @"\" + FileName;

            string CurrentFileName = LoggingFiles + FileName;

            string contentType = string.Empty;
            if (CurrentFileName.ToLower().Contains(".txt"))
            {
                contentType = "application/txt";
            }
            else if (CurrentFileName.ToLower().Contains(".pdf"))
            {
                contentType = "application/pdf";
            }

            else if (CurrentFileName.ToLower().Contains(".docx"))
            {
                contentType = "application/docx";
            }

            return File(CurrentFileName, contentType, (FileName));
        }
        public filedata Readfile(string FileName, string Type = null)
        {
            string[] lines;
            filedata obj = new filedata();
            var list = new List<string>();
            var loggingFile = new List<LoggingTextFile>();
            LoggingTextFile objloggingModel = new LoggingTextFile();
            string[] filePaths = Directory.GetFiles(LoggingFiles);
            var fileStream = new FileStream(LoggingFiles + FileName, FileMode.Open, FileAccess.Read);
            System.IO.StreamReader file = new System.IO.StreamReader(LoggingFiles + FileName);
            FileInfo objFileInfo = new FileInfo(filePaths[0]);
            //DateTime dtLastModified;
            // dtLastModified = objFileInfo.LastWriteTime;
            UserModels objMemberList = new UserModels();
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    string[] strFullline = null;
                    strFullline = line.Split(new string[] { "##" }, StringSplitOptions.None);
                    string[] strArr = null;
                    strArr = strFullline[0].Split(new string[] { "||" }, StringSplitOptions.None);
                    if (strArr.Length >= 3)
                    {
                        objloggingModel.Text = strArr[0] != null ? strArr[0].Split(':').Last().Trim().ToString() : "Blank";
                        objloggingModel.Type = strArr[1] != null ? strArr[1].Split(':').Last().Trim().ToString() : "Blank";
                        objloggingModel.TimeStamp = strArr[2] != null ? strArr[2].Split(':').Last().Trim().ToString() : "Blank";
                    }
                    string type = strArr.First().Split(':').Last().Trim().ToString();
                    if (Type != null)
                    {
                        if (Type != LogTypes.info.ToString())
                        {
                            if (type.ToLower() == Type.ToString())
                            {
                                obj.Infodata.Add(line);

                            }
                        }
                        else
                        {
                            obj.Infodata.Add(line);

                        }
                    }
                    loggingFile.Add(objloggingModel);
                    list.Add(line);

                }

            }
            lines = list.ToArray();
            obj.data = lines;
            obj.FileName = fileStream.Name.Split('\\').Last();
            return obj;
        }
        public JsonResult GetLogByType(string FileName, string Type)
        {
            //UserModels objMemberList = new UserModels();
            //objMemberList.FileDetails = ReadLogFile(FileName, Type.ToLower());

            LoggingModel ObjLogging = new LoggingModel();

            var _data = ReadLogFile(FileName, Type.ToLower());
            ObjLogging.objLoggingTextFile = _data;
            ObjLogging.FilePath = FileName.ToString();
            return Json(ObjLogging.objLoggingTextFile, JsonRequestBehavior.AllowGet);
        }
        enum LogTypes
        {
            All,
            info,
            error,
            exception,
            exception_error
        };

        public List<LoggingTextFile> ReadLogFile(string FileName, string Type = null)
        {
            // string[] lines;
            //filedata obj = new filedata();
            //var list = new List<string>();
            var loggingFile = new List<LoggingTextFile>();

            string[] filePaths = Directory.GetFiles(LoggingFiles);
            if (System.IO.File.Exists(LoggingFiles + FileName))
            {
                var fileStream = new FileStream(LoggingFiles + FileName, FileMode.Open, FileAccess.Read);
                System.IO.StreamReader file = new System.IO.StreamReader(LoggingFiles + FileName);
                FileInfo objFileInfo = new FileInfo(filePaths[0]);
                //DateTime dtLastModified;
                // dtLastModified = objFileInfo.LastWriteTime;
                //  UserModels objMemberList = new UserModels();
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] strArr = null;
                        LoggingTextFile objloggingModel = new LoggingTextFile();
                        strArr = line.Split(new string[] { "||" }, StringSplitOptions.None);
                        if (strArr.Length >= 3)
                        {
                            objloggingModel.Type = strArr[0] != null ? strArr[0].Split(':').Last().Trim().ToString() : "Blank";
                            objloggingModel.Text = strArr[1] != null ? strArr[1].Split(':').Last().Trim().ToString() : "Blank";
                            string time = strArr[2] != null ? strArr[2].Split(':').Last().Trim().ToString() : "Blank";
                            if (time.Length > 0)
                            {
                                objloggingModel.TimeStamp = time != null ? time.Replace("##", " ") : "Blank";
                            }
                        }
                        string type = strArr.First().Split(':').Last().Trim().ToString();
                        if (Type != null)
                        {
                            if (Type != LogTypes.All.ToString())
                            {
                                if (type.ToLower() == Type.ToString().ToLower())
                                {
                                    //obj.Infodata.Add(line);
                                    loggingFile.Add(objloggingModel);
                                }
                            }
                            else
                            {
                                // obj.Infodata.Add(line);
                                loggingFile.Add(objloggingModel);
                            }
                        }
                        else { loggingFile.Add(objloggingModel); }
                        // list.Add(line);
                        // loggingFile.Add(objloggingModel);
                    }

                }
                // lines = list.ToArray();
                // obj.data = lines;
                // obj.FileName = fileStream.Name.Split('\\').Last();
            }
            return loggingFile;
        }

        [AjaxHandleException]
        public ActionResult LoggingFileTextAjax([DataSourceRequest] DataSourceRequest request, string FileName, string Type)
        {
            if (Type == null || Type == "")
            {
                Type = null;
            }
            var objFileDetails = ReadLogFile(FileName, Type);
            if (objFileDetails != null && objFileDetails.Count > 0)
            {
                ViewBag.IsModalValid = true;
                IEnumerable<DtoList> Types = null;
                Types = generalService.GetLoggingTypes();
                ViewBag.ExceptionTypes = Types;
                DataSourceResult result = objFileDetails.ToDataSourceResult(request);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["IsFileExists"] = false;
                return RedirectToAction("ManageLogging", "ManageLogging");
            }
        }
    }
}