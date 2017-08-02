using Revive.Redux.Commn;
using Revive.Redux.Common;
using Revive.Redux.Entities;
using Revive.Redux.Entities.Common;
using Revive.Redux.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Revive.Redux.UI.WebApi
{
    public class FileUploadController : ApiController
    {
        LoggingService loggingService = null;

        public FileUploadController()
        {
            loggingService = new LoggingService();
        }
        [HttpPost()]
        public string UploadFiles()
        {
            string retResult = "";
            WriteLog logwrite = new WriteLog();
            try
            {
                int iUploadedCnt = 0;
                //var UserId =  GetUserIdByKey();

                // DEFINE THE PATH WHERE WE WANT TO SAVE THE FILES.
                string sPath = "";
                sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/LoggingFiles/");

                System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;





                // Creating Directory
                string dirPath = System.Web.HttpContext.Current.Server.MapPath("~") + @"\LoggingFiles\";
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);

                }

                // CHECK THE FILE COUNT.
                for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
                {
                    System.Web.HttpPostedFile hpf = hfc[iCnt];

                    if (hpf.ContentLength > 0)
                    {
                        // CHECK IF THE SELECTED FILE(S) ALREADY EXISTS IN FOLDER. (AVOID DUPLICATE)
                        if (!File.Exists(sPath + Path.GetFileName(hpf.FileName)))
                        {
                            // SAVE THE FILES IN THE FOLDER.
                            hpf.SaveAs(sPath + Path.GetFileName(hpf.FileName));
                            iUploadedCnt = iUploadedCnt + 1;

                            // DB calling for storing

                            LoggingModel LoggingModelObj = new LoggingModel();
                            LoggingModelObj.FilePath = hpf.FileName;
                            LoggingModelObj.LogFileTimeStamp = System.DateTime.Now;
                            string[] FileNameVal = hpf.FileName.Split('_');
                            LoggingModelObj.Created_by = new Guid(FileNameVal[0].ToString());

                            var loggingDbInsert_Result = loggingService.Add(LoggingModelObj);

                            // Log Writing
                           
                            logwrite.funcName = "UploadFiles Called-----";
                            var list1 = new List<Tuple<string, string>>{
                        new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                        new Tuple<string,string>(" FileName:", hpf.FileName.ToString()),
                        new Tuple<string,string>(" DbResult:", loggingDbInsert_Result.ToString())

                        };
                            logwrite.keyValPair = list1;
                            CommonMethods.WriteLog(logwrite);

                        }
                    }
                }
                

                // RETURN A MESSAGE (OPTIONAL).
                if (iUploadedCnt > 0)
                {
                    retResult=iUploadedCnt + " Files Uploaded Successfully";
                     
                }
                else
                {

                    logwrite.funcName = "UploadFiles Called-----";
                    var list1 = new List<Tuple<string, string>>{
                        new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                        new Tuple<string,string>(" FileName:", "No FIle Found"),
                        new Tuple<string,string>(" DbResult:", "Data Lenghth is 0")};
                    logwrite.keyValPair = list1;
                    CommonMethods.WriteLog(logwrite);

                    retResult= "Upload Failed From Server";
                }
            }
            catch(Exception ex)
            {

                logwrite.funcName = "UploadFiles Called-----";
                var list1 = new List<Tuple<string, string>>{
                        new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                        new Tuple<string,string>(" FileName:", "No FIle Found"),
                        new Tuple<string,string>(" Error:", ex.Message.ToString())};
                logwrite.keyValPair = list1;
                CommonMethods.WriteLog(logwrite);
                retResult = ex.Message.ToString();
            }
            return retResult;
        }

        private Guid GetUserIdByKey()
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            object keyVal = headerValues.FirstOrDefault();
            string sKey = ConfigurationSettings.AppSettings["Encrption"].ToString();
            string _key = keyVal.ToString().Split(' ')[1].ToString();
            Guid userId = new Guid();
            if (_key != null)
            {
                string parameter = Cryptography.DecryptText(_key, sKey);
                string[] _params = parameter.Split('@');
                userId = new Guid(_params[0]);
            }
            return userId;

        }



    }
}
