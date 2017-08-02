using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace Revive.Redux.UI
{
    public static class SSOLog
    {
        public static void WriteLog(string message1, string message2)
        {
            string pathName = ConfigurationManager.AppSettings["logFilePath"].ToString();

            string filePath = pathName;

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("LogMessage :" + message1 + "<br/>" + Environment.NewLine + "Action Name :" + message2 +
                   "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
            }

        }
    }

    
}