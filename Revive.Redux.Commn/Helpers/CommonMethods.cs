using Revive.Redux.Entities.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Common
{
    public static class CommonMethods
    {
        /// <summary>
        /// GetRandomPassword
        /// </summary>
        /// <returns></returns>
        public static String GetRandomPassword()
        {
            string allowedChars = "";
            allowedChars = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";
            allowedChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";
            allowedChars += "1,2,3,4,5,6,7,8,9,0,!,@,#,$,%,&,?";
            char[] sep = { ',' };
            string[] arr = allowedChars.Split(sep);
            string passwordString = "";
            string temp = "";
            Random rand = new Random();
            for (int i = 0; i < 7; i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                passwordString += temp;
            }
            passwordString += GetRandomInteger();
            return passwordString;
        }


        /// <summary>
        /// GetCurrentDate
        /// </summary>
        /// <returns></returns>
        public static DateTime GetCurrentDate()
        {
            DateTime dCurrentDate = DateTime.Now;

            return dCurrentDate;
        }

        /// <summary>
        /// GetRandomPassword
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        public static String GetRandomPassword(string firstName, string lastName)
        {
            string passwordString = string.Empty;
            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            {


                var firstTwo = firstName.Substring(0, 2);
                var lastTwo = lastName.Substring(0, 2);
                var ranChars = GetRandomString(2);
                var ranNum = GetRandomNumber();

                passwordString = firstTwo + lastTwo + ranChars + Convert.ToInt32(ranNum);
            }
            return passwordString;
        }


        /// <summary>
        /// GetRandomString
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private static String GetRandomString(int length)
        {
            var allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ";
            var len = length;

            var chars = new char[len];
            var rd = new Random();

            for (var i = 0; i < len; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new String(chars);
        }

        /// <summary>
        /// GetRandomNumber
        /// </summary>
        /// <returns></returns>
        private static int GetRandomNumber()
        {
            int min = 10;
            int max = 99;
            Random random = new Random((int)DateTime.Now.Ticks);

            return random.Next(min, max);
        }

        /// <summary>
        /// CombineUrlPath
        /// </summary>
        /// <param name="uri1"></param>
        /// <param name="uri2"></param>
        /// <returns></returns>
        public static string CombineUrlPath(string uri1, string uri2)
        {
            uri1 = uri1.TrimEnd('/');
            uri2 = uri2.TrimStart('/');
            return string.Format("{0}/{1}", uri1, uri2);
        }
        private static int GetRandomInteger()
        {
            int seed = 345;
            int min = 1;
            int max = 9;
            Random random = new Random((int)DateTime.Now.Ticks);
            return random.Next(min, max);
        }

        public static int CompareTime(String OpeningTime, string ClosingTime, int nBuffer = 8)
        {
            int nResult = 1;

            if (!string.IsNullOrEmpty(OpeningTime) && !string.IsNullOrEmpty(OpeningTime))
            { 
                DateTime dOpeningTime = Convert.ToDateTime(OpeningTime);
                DateTime dClosingTime = Convert.ToDateTime(ClosingTime);

                var timeSpan = dClosingTime.TimeOfDay - dOpeningTime.TimeOfDay;

                nResult = timeSpan.Minutes + timeSpan.Hours * 60 - nBuffer * 60;
            }

            return nResult;
        }

        public static string Encode(string encodeVal)
        {
            byte[] encoded = System.Text.Encoding.UTF8.GetBytes(encodeVal);
            return Convert.ToBase64String(encoded);

            //string EncryptionKey = "REVIVEREDUX";
            //byte[] clearBytes = Encoding.Unicode.GetBytes(encodeVal);
            //using (Aes encryptor = Aes.Create())
            //{
            //    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            //    encryptor.Key = pdb.GetBytes(32);
            //    encryptor.IV = pdb.GetBytes(16);
            //    using (MemoryStream ms = new MemoryStream())
            //    {
            //        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
            //        {
            //            cs.Write(clearBytes, 0, clearBytes.Length);
            //            cs.Close();
            //        }
            //        encodeVal = Convert.ToBase64String(ms.ToArray());
            //    }
            //}
            //return encodeVal;
        }

        public static string Decode(string decodeVal)
        {
            byte[] encoded = Convert.FromBase64String(decodeVal);
            return System.Text.Encoding.UTF8.GetString(encoded);

            //string EncryptionKey = "REVIVEREDUX";
            //byte[] cipherBytes = Convert.FromBase64String(decodeVal);
            //using (Aes encryptor = Aes.Create())
            //{
            //    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            //    encryptor.Key = pdb.GetBytes(32);
            //    encryptor.IV = pdb.GetBytes(16);
            //    using (MemoryStream ms = new MemoryStream())
            //    {
            //        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
            //        {
            //            cs.Write(cipherBytes, 0, cipherBytes.Length);
            //            cs.Close();
            //        }
            //        decodeVal = Encoding.Unicode.GetString(ms.ToArray());
            //    }
            //}
            //return decodeVal;
        }
        
        /// <summary>
        /// Write to Log file using log4net service
        /// </summary>
        /// <param name="writeLogObj">class object</param>
        public static void WriteLog(WriteLog writeLogObj)
        {
            LogService _log4netService = new LogService();
            string strLog = string.Empty;
            if (!string.IsNullOrEmpty(writeLogObj.funcName))
                strLog += writeLogObj.funcName;
            if (writeLogObj.keyValPair != null && writeLogObj.keyValPair.Count > 0)
            {
                for (int i = 0; i < writeLogObj.keyValPair.Count; i++)
                {
                    // If key isn't empty
                    if (!string.IsNullOrEmpty(writeLogObj.keyValPair[i].Item1))
                    {
                        // Write Key.
                        strLog += writeLogObj.keyValPair[i].Item1;

                        // if value is empty just write key.
                        if (!string.IsNullOrEmpty(writeLogObj.keyValPair[i].Item2))
                            strLog += writeLogObj.keyValPair[i].Item2;
                    }
                }
            }
            if (!string.IsNullOrEmpty(strLog))
            {
               
                _log4netService.LogInformationFormat(strLog);

               // string path = String.Format("{0}{1}{2}{3}{4}", "ReduxAPI_", DateTime.Now.Month.ToString()
                //                 , DateTime.Now.Day.ToString()
                //                 , DateTime.Now.Year.ToString(), ".log");


               // _log4netService.ChangeFilePath("RollingFileAppender", path);/
            }

           



               
               
        }
    }
}
