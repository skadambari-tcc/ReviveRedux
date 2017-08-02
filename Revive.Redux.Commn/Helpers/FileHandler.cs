using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revive.Redux.Entities;
using System.Data.OleDb;
using OfficeOpenXml;

namespace Revive.Redux.Commn
{

    /// <summary>
    /// A data-reader style interface for reading Csv (and otherwise-char-separated) files.
    /// </summary>
    public class FileHandler : IDisposable
    {

        #region "Private variables"

        private Stream stream;
        private StreamReader reader;
        private char m_separator;

        #endregion

        #region "Constructors"

        /// <summary>
        /// Creates a new Csv reader for the given stream.
        /// </summary>
        /// <param name="s">The stream to read the CSV from.</param>
        public FileHandler(Stream s)
            : this(s, null, ',')
        {
        }

        /// <summary>
        /// Creates a new reader for the given stream and separator.
        /// </summary>
        /// <param name="s">The stream to read the separator from.</param>
        /// <param name="separator">The field separator character</param>
        public FileHandler(Stream s, char separator)
            : this(s, null, separator)
        {
        }

        /// <summary>
        /// Creates a new Csv reader for the given stream and encoding.
        /// </summary>
        /// <param name="s">The stream to read the CSV from.</param>
        /// <param name="enc">The encoding used.</param>
        public FileHandler(Stream s, Encoding enc)
            : this(s, enc, ',')
        {
        }

        /// <summary>
        /// Creates a new reader for the given stream, encoding and separator character.
        /// </summary>
        /// <param name="s">The stream to read the data from.</param>
        /// <param name="enc">The encoding used.</param>
        /// <param name="separator">The separator character between the fields</param>
        public FileHandler(Stream s, Encoding enc, char separator)
        {

            this.m_separator = separator;
            this.stream = s;
            if (!s.CanRead)
            {
                throw new CSVHandlerException("Could not read the given data stream!");
            }
            reader = (enc != null) ? new StreamReader(s, enc) : new StreamReader(s);
        }

        /// <summary>
        /// Creates a new Csv reader for the given text file path.
        /// </summary>
        /// <param name="filename">The name of the file to be read.</param>
        public FileHandler(string filename)
            : this(filename, null, ',')
        {
        }

        /// <summary>
        /// Creates a new reader for the given text file path and separator character.
        /// </summary>
        /// <param name="filename">The name of the file to be read.</param>
        /// <param name="separator">The field separator character</param>
        public FileHandler(string filename, char separator)
            : this(filename, null, separator)
        {
        }

        /// <summary>
        /// Creates a new Csv reader for the given text file path and encoding.
        /// </summary>
        /// <param name="filename">The name of the file to be read.</param>
        /// <param name="enc">The encoding used.</param>
        public FileHandler(string filename, Encoding enc)
            : this(filename, enc, ',')
        {
        }

        /// <summary>
        /// Creates a new reader for the given text file path, encoding and field separator.
        /// </summary>
        /// <param name="filename">The name of the file to be read.</param>
        /// <param name="enc">The encoding used.</param>
        /// <param name="separator">The field separator character.</param>
        public FileHandler(string filename, Encoding enc, char separator)
            : this(new FileStream(filename, FileMode.Open), enc, separator)
        {
        }

        #endregion

        #region "Properties"

        /// <summary>
        /// The separator character for the fields. Comma for normal CSV.
        /// </summary>
        public char Separator
        {
            get { return m_separator; }
            set { m_separator = value; }
        }

        #endregion

        #region "Parsing"

        /// <summary>
        /// Returns the fields for the next row of data (or null if at eof)
        /// </summary>
        /// <returns>A string array of fields or null if at the end of file.</returns>
        public string[] GetCsvLine()
        {

            string data = reader.ReadLine();
            if (data == null)
            {
                return null;
            }
            if (data.Length == 0)
            {
                return new string[-1 + 1];
            }

            ArrayList result = new ArrayList();

            ParseCsvFields(result, data);

            return (string[])result.ToArray(typeof(string));
        }

        // Parses the fields and pushes the fields into the result arraylist
        private void ParseCsvFields(ArrayList result, string data)
        {
            int pos = -1;
            while (pos < data.Length)
            {
                result.Add(ParseCsvField(data, ref pos));
            }
        }

        // Parses the field at the given position of the data, modified pos to match
        // the first unparsed position and returns the parsed field
        private string ParseCsvField(string data, ref int startSeparatorPosition)
        {

            if (startSeparatorPosition == data.Length - 1)
            {
                startSeparatorPosition += 1;
                // The last field is empty
                return "";
            }

            int fromPos = startSeparatorPosition + 1;

            // Determine if this is a quoted field
            if (data[fromPos] == '"')
            {
                // If we're at the end of the string, let's consider this a field that
                // only contains the quote
                if (fromPos == data.Length - 1)
                {
                    fromPos += 1;
                    return "\"";
                }

                // Otherwise, return a string of appropriate length with double quotes collapsed
                // Note that FSQ returns data.Length if no single quote was found
                int nextSingleQuote = FindSingleQuote(data, fromPos + 1);
                startSeparatorPosition = nextSingleQuote + 1;
                return data.Substring(fromPos + 1, nextSingleQuote - fromPos - 1).Replace("\"\"", "\"");
            }

            // The field ends in the next separator or EOL
            int nextSeparator = data.IndexOf(m_separator, fromPos);
            if (nextSeparator == -1)
            {
                startSeparatorPosition = data.Length;
                return data.Substring(fromPos);
            }
            else
            {
                startSeparatorPosition = nextSeparator;
                return data.Substring(fromPos, nextSeparator - fromPos);
            }
        }

        // Returns the index of the next single quote mark in the string 
        // (starting from startFrom)
        private static int FindSingleQuote(string data, int startFrom)
        {

            int i = startFrom - 1;
            while (System.Threading.Interlocked.Increment(ref i) < data.Length)
            {
                if (data[i] == '"')
                {
                    // If this is a double quote, bypass the chars
                    if (i < data.Length - 1 && data[i + 1] == '"')
                    {
                        i += 1;
                        continue;
                    }
                    else
                    {
                        return i;
                    }
                }
            }
            // If no quote found, return the end value of i (data.Length)
            return i;
        }



        private static T InlineAssignHelper<T>(ref T target, T value)
        {
            target = value;
            return value;
        }

        #endregion

        #region "Dispose"

        /// <summary>
        /// Disposes the reader. The underlying stream is closed.
        /// </summary>
        public void Dispose()
        {
            // Closing the reader closes the underlying stream, too
            if (reader != null)
            {
                reader.Close();
            }
            else if (stream != null)
            {
                stream.Close();
            }
            // In case we failed before the reader was constructed
            GC.SuppressFinalize(this);
        }

        #endregion

        #region "Conversion"

        public static DataSet DataTableFromCSV(string Filename)
        {
            try
            {
                string sLine = File.ReadAllText(Filename);
                DataTable dtData = new DataTable();
                char separator = Convert.ToChar(",");
                MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(sLine));
                using (FileHandler csv = new FileHandler(ms, Encoding.Unicode, separator))
                {
                    string[] fields = null;
                    int lines = 0;
                    while ((InlineAssignHelper(ref fields, csv.GetCsvLine())) != null)
                    {
                        if (lines == 0)
                        {
                            for (int i = 0; i < fields.Length; i++)
                            {
                                string field = fields[i];
                                DataColumn dcCol = new DataColumn();
                                dcCol.ColumnName = field;
                                dtData.Columns.Add(dcCol);
                            }
                        }
                        else
                        {
                            dtData.LoadDataRow(fields, LoadOption.Upsert);
                        }
                        lines += 1;
                    }
                }
                DataSet ds = new DataSet();
                ds.Tables.Add(dtData);
                return ds;
            }
            catch (Exception ex)
            {
                throw new CSVHandlerException(ex.Message);
            }
        }

        public static List<LocationModel> getInsertedLocationStatus(string filename)
        {

            List<LocationModel> lstLocationModel = null;
            try
            {
                string sLine = File.ReadAllText(filename);

                char separator = Convert.ToChar(",");
                MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(sLine));
                using (FileHandler csv = new FileHandler(ms, Encoding.Unicode, separator))
                {
                    string[] fields = null;

                    int lines = 0;
                    lstLocationModel = new List<LocationModel>();
                    while ((InlineAssignHelper(ref fields, csv.GetCsvLine())) != null)
                    {
                        if (lines != 0)
                        {
                            var location = new LocationModel
                            {
                                StoreNumber = fields[0],
                                LocationName = fields[1],
                                AddressLine1 = fields[2],
                                AddressLine2 = fields[3],
                                CityName = fields[4],
                                StateName = fields[5],
                                ZipCode = fields[6],
                                PrimaryPhone = fields[7],
                                StoreClosingTime = fields[8],
                                StoreOpeningTime = fields[9],
                            };
                            lstLocationModel.Add(location);
                        }
                        lines += 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CSVHandlerException(ex.Message);
            }

            return lstLocationModel;
        }

        //private static Dictionary<string, string> GetCenterDictionary()
        //{
        //    var repository = new CenterInformationRepository();
        //    var centerCollection = repository.GetAllCenterForDataUpload();
        //    var centers = new Dictionary<string, string>();
        //    foreach (var cetner in centerCollection)
        //    {
        //        if (!centers.ContainsKey(cetner.CenterCode))
        //            centers.Add(cetner.CenterCode, cetner.CenterId.ToString());
        //    }
        //    return centers;
        //}

        //private static Dictionary<string, string> GetQualificationDictionary()
        //{
        //    var qualifications = new Dictionary<string, string>();
        //    using (var dbContext = new AAMFEntities())
        //    {
        //        var qualificationCollectoin = (from l in dbContext.TblQualifications select l).ToList();
        //        foreach (var qualification in qualificationCollectoin)
        //        {
        //            if (!qualifications.ContainsKey(qualification.EducationDetails))
        //                qualifications.Add(qualification.EducationDetails, qualification.EducationId.ToString());
        //        }
        //    }



        //    return qualifications;
        //}



        public static string DataTableToCSV(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataColumn dc in dt.Columns)
            {
                sb.Append("\"");
                sb.Append(dc.ColumnName);
                sb.Append("\",");
            }
            sb.Remove(sb.ToString().Length - 1, 1);
            sb.Append(Environment.NewLine);
            foreach (DataRow dr in dt.Rows)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    sb.Append("\"");
                    sb.Append(dr[dc.ColumnName]);
                    sb.Append("\",");
                }
                sb.Remove(sb.ToString().Length - 1, 1);
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        #endregion

        #region

        public static List<LocationModel> getInsertedLocationStatusEx(string filename)
        {
            try
            {
                var existingFile = new FileInfo(filename);
                List<LocationModel> lstLocationModel = new List<LocationModel>();
                using (var package = new ExcelPackage(existingFile))
                {
                    // Get the work book in the file
                    var workBook = package.Workbook;
                    if (workBook != null)
                    {
                        if (workBook.Worksheets.Count > 0)
                        {
                            // Get the first worksheet
                            var oSheet = workBook.Worksheets.First();
                            var totalRows = oSheet.Dimension.End.Row;
                            var totalCols = oSheet.Dimension.End.Column;
                            var dt = new DataTable(oSheet.Name);
                            DataRow dr = null;
                            for (int i = 1; i <= totalRows; i++)
                            {
                                if (i > 1) dr = dt.Rows.Add();
                                for (int j = 1; j <= totalCols; j++)
                                {
                                    if (i == 1)
                                    {
                                        dt.Columns.Add(Convert.ToString(oSheet.Cells[i, j].Value).Trim());
                                    }

                                    else if (dr != null) dr[j - 1] = oSheet.Cells[i, j].Value == null ? string.Empty : oSheet.Cells[i, j].Value.ToString();
                                }
                            }

                            if (dt.Columns.Count >= 10)
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    if (dt.Rows[i][0] != string.Empty && dt.Rows[i][0] != null)
                                    {
                                        var location = new LocationModel
                                        {
                                            StoreNumber = Convert.ToString(dt.Rows[i]["Store Number"]),
                                            LocationName = Convert.ToString(dt.Rows[i]["Location Name"]),
                                            AddressLine1 = Convert.ToString(dt.Rows[i]["Address Line 1"]),
                                            AddressLine2 = Convert.ToString(dt.Rows[i]["Address Line 2"]),
                                            CityName = Convert.ToString(dt.Rows[i]["City Name"]),
                                            StateName = Convert.ToString(dt.Rows[i]["State Name"]),
                                            ZipCode = Convert.ToString(dt.Rows[i]["Zip Code"]),
                                            PrimaryPhone = GetPhoneFormatted(Convert.ToString(dt.Rows[i]["Primary Phone"])),
                                            StoreOpeningTime = GetTimeFormatted(Convert.ToString(dt.Rows[i]["Store Opening Time"])),
                                            StoreClosingTime = GetTimeFormatted(Convert.ToString(dt.Rows[i]["Store Closing Time"])),
                                        };
                                        lstLocationModel.Add(location);
                                    }
                                }
                            }
                        }
                    }
                }
                return lstLocationModel;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static List<UserModels> getInsertedUser(string filename)
        {
            try
            {
                var existingFile = new FileInfo(filename);
                List<UserModels> lstUserModel = new List<UserModels>();
                using (var package = new ExcelPackage(existingFile))
                {
                    // Get the work book in the file
                    var workBook = package.Workbook;
                    if (workBook != null)
                    {
                        if (workBook.Worksheets.Count > 0)
                        {
                            // Get the first worksheet
                            var oSheet = workBook.Worksheets.First();
                            var totalRows = oSheet.Dimension.End.Row;
                            var totalCols = oSheet.Dimension.End.Column;
                            var dt = new DataTable(oSheet.Name);
                            DataRow dr = null;
                            for (int i = 1; i <= totalRows; i++)
                            {
                                if (i > 1) dr = dt.Rows.Add();
                                for (int j = 1; j <= totalCols; j++)
                                {
                                    if (i == 1)
                                        dt.Columns.Add(Convert.ToString(oSheet.Cells[i, j].Value).Trim());
                                    else if (dr != null) dr[j - 1] = oSheet.Cells[i, j].Value == null ? string.Empty : oSheet.Cells[i, j].Value.ToString();
                                }
                            }
                            if (dt.Columns.Count >= 5)
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    if (dt.Rows[i][0] != string.Empty && dt.Rows[i][0] != null)
                                    {
                                        var user = new UserModels
                                        {
                                            FirstName = Convert.ToString(dt.Rows[i]["First Name"]),
                                            LastName = Convert.ToString(dt.Rows[i]["Last Name"]),
                                            Email = Convert.ToString(dt.Rows[i]["User Email Address"]),
                                            UserMobile = Convert.ToString(dt.Rows[i]["User Mobile"]),
                                            LocationName = Convert.ToString(dt.Rows[i]["Location"])
                                            //Location_Id 
                                        };
                                        lstUserModel.Add(user);
                                    }
                                }
                            }
                        }
                    }
                }
                return lstUserModel;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static List<ManageMember> getInsertedMember(string filename)
        {
            try
            {
                var existingFile = new FileInfo(filename);
                List<ManageMember> lstMembershipWebModel = new List<ManageMember>();
                using (var package = new ExcelPackage(existingFile))
                {
                    // Get the work book in the file
                    var workBook = package.Workbook;
                    if (workBook != null)
                    {
                        if (workBook.Worksheets.Count > 0)
                        {
                            // Get the first worksheet
                            var oSheet = workBook.Worksheets.First();
                            var totalRows = oSheet.Dimension.End.Row;
                            var totalCols = oSheet.Dimension.End.Column;
                            var dt = new DataTable(oSheet.Name);
                            DataRow dr = null;
                            for (int i = 1; i <= totalRows; i++)
                            {
                                if (i > 1) dr = dt.Rows.Add();
                                for (int j = 1; j <= totalCols; j++)
                                {
                                    if (i == 1)
                                        dt.Columns.Add(Convert.ToString(oSheet.Cells[i, j].Value).Trim());
                                    else if (dr != null) dr[j - 1] = oSheet.Cells[i, j].Value == null ? string.Empty : oSheet.Cells[i, j].Value.ToString();
                                }
                            }
                            if (dt.Columns.Count >= 7)
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    if (Convert.ToString(dt.Rows[i]["Member Name".Trim()]) != string.Empty || Convert.ToString(dt.Rows[i]["Start Date".Trim()]) != string.Empty || Convert.ToString(dt.Rows[i]["End Date".Trim()]) != string.Empty)
                                    {
                                        var member = new ManageMember
                                        {
                                            MemberName = Convert.ToString(dt.Rows[i]["Member Name".Trim()]),
                                            Phone_Number = Convert.ToString(dt.Rows[i]["Phone Number".Trim()]),
                                            no_of_FreeDries = Convert.ToString(dt.Rows[i]["Revives Remaining".Trim()]) == "" ? 0 : Convert.ToInt32(dt.Rows[i]["Revives Remaining".Trim()]),
                                            StoreNumber = Convert.ToString(dt.Rows[i]["Store Number".Trim()]),
                                            strStartDate = Convert.ToString((dt.Rows[i]["Start Date".Trim()])),
                                            strEndDate = Convert.ToString((dt.Rows[i]["End Date".Trim()])),
                                            InvoiceNumber = Convert.ToString(dt.Rows[i]["Invoice Number".Trim()]),
                                            IsMultiDevice = dt.Rows[i]["MultiDevice".Trim()] == null ? false : (dt.Rows[i]["MultiDevice"].ToString().ToUpper() == "YES" ? true : false)//Convert.ToString(dt.Rows[i]["MultiDevice".Trim()])
                                        };
                                        lstMembershipWebModel.Add(member);
                                    }
                                }
                            }
                        }
                    }
                }
                return lstMembershipWebModel;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static List<ManageMember> getInsertedMemberAW(string filename)
        {
            try
            {
                var existingFile = new FileInfo(filename);
                List<ManageMember> lstMembershipWebModel = new List<ManageMember>();
                using (var package = new ExcelPackage(existingFile))
                {
                    // Get the work book in the file
                    var workBook = package.Workbook;
                    if (workBook != null)
                    {
                        if (workBook.Worksheets.Count > 0)
                        {
                            // Get the first worksheet
                            var oSheet = workBook.Worksheets.First();
                            var totalRows = oSheet.Dimension.End.Row;
                            var totalCols = oSheet.Dimension.End.Column;
                            var dt = new DataTable(oSheet.Name);
                            DataRow dr = null;
                            for (int i = 1; i <= totalRows; i++)
                            {
                                if (i > 1) dr = dt.Rows.Add();
                                for (int j = 1; j <= totalCols; j++)
                                {
                                    if (i == 1)
                                        dt.Columns.Add(Convert.ToString(oSheet.Cells[i, j].Value).Trim());
                                    else if (dr != null) dr[j - 1] = oSheet.Cells[i, j].Value == null ? string.Empty : oSheet.Cells[i, j].Value.ToString();
                                }
                            }
                            if (dt.Columns.Count >= 5)
                            {

                                //DataView dv = dt.DefaultView;
                                //dv.Sort = "Customer asc";
                                // DataTable sortedDT = dv.ToTable();
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    if (Convert.ToString(dt.Rows[i]["Customer".Trim()]) != string.Empty || Convert.ToString(dt.Rows[i]["Sold On".Trim()]) != string.Empty)
                                    {
                                        var member = new ManageMember
                                        {
                                            MemberName = Convert.ToString(dt.Rows[i]["Customer".Trim()]),
                                            Phone_Number = Convert.ToString(dt.Rows[i]["Tracking #".Trim()]),
                                            //no_of_FreeDries = Convert.ToString(dt.Rows[i]["Revives Remaining".Trim()]) == "" ? 0 : Convert.ToInt32(dt.Rows[i]["Revives Remaining".Trim()]),


                                            strStartDate = Convert.ToString((dt.Rows[i]["Sold On".Trim()])),
                                            // strEndDate = Convert.ToString((dt.Rows[i]["End Date".Trim()])),
                                            InvoiceNumber = Convert.ToString(dt.Rows[i]["Invoice #".Trim()]),
                                            // IsMultiDevice = dt.Rows[i]["MultiDevice".Trim()] == null ? false : (dt.Rows[i]["MultiDevice"].ToString().ToUpper() == "YES" ? true : false)//Convert.ToString(dt.Rows[i]["MultiDevice".Trim()])
                                        };

                                        if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Invoice #".Trim()])) && Convert.ToString(dt.Rows[i]["Invoice #".Trim()]).Length > 4)
                                        {
                                            member.StoreNumber = Convert.ToString(dt.Rows[i]["Invoice #".Trim()]).Substring(0, 5);
                                        }



                                        var MultiDeviceFound = Convert.ToString(dt.Rows[i]["Product Name".Trim()]).IndexOf("Multi-Device");


                                        if (MultiDeviceFound>0)
                                        {
                                            var searchMember = lstMembershipWebModel.Where(x => x.InvoiceNumber == Convert.ToString(dt.Rows[i]["Invoice #".Trim()])).FirstOrDefault();
                                            if (searchMember != null)
                                            {
                                                searchMember.Phone_Number = searchMember.Phone_Number + "," + Convert.ToString(dt.Rows[i]["Tracking #".Trim()]);

                                            }
                                            else
                                            {
                                                member.IsMultiDevice = true;
                                                lstMembershipWebModel.Add(member);
                                            }
                                        }
                                        else
                                        {
                                            lstMembershipWebModel.Add(member);

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return lstMembershipWebModel;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static List<MachineModel> getInsertedShippingDetails(string filename)
        {
            try
            {
                var existingFile = new FileInfo(filename);
                List<MachineModel> lstMachineModel = new List<MachineModel>();
                using (var package = new ExcelPackage(existingFile))
                {
                    // Get the work book in the file
                    var workBook = package.Workbook;
                    if (workBook != null)
                    {
                        if (workBook.Worksheets.Count > 0)
                        {
                            // Get the first worksheet
                            var oSheet = workBook.Worksheets.First();
                            var totalRows = oSheet.Dimension.End.Row;
                            var totalCols = oSheet.Dimension.End.Column;
                            var dt = new DataTable(oSheet.Name);
                            DataRow dr = null;
                            for (int i = 1; i <= totalRows; i++)
                            {
                                if (i > 1) dr = dt.Rows.Add();
                                for (int j = 1; j <= totalCols; j++)
                                {
                                    if (i == 1)
                                        dt.Columns.Add(oSheet.Cells[i, j].Value.ToString());
                                    else if (dr != null) dr[j - 1] = oSheet.Cells[i, j].Value == null ? string.Empty : oSheet.Cells[i, j].Value.ToString();
                                }
                            }
                            if (dt.Columns.Count == 3)
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    var ShippingDetails = new MachineModel
                                    {
                                        MachineId_Val = dt.Rows[i][0].ToString(),
                                        Location = dt.Rows[i][1].ToString(),
                                        ShippingId = dt.Rows[i][2].ToString()
                                    };
                                    lstMachineModel.Add(ShippingDetails);
                                }
                            }
                        }
                    }
                }
                return lstMachineModel;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //public static List<LocationModel> getInsertedLocationStatusEx(ExcelPackage workBook)
        //{
        //    List<LocationModel> lstLocationModel = new List<LocationModel>();

        //                // Get the first worksheet
        //                var oSheet = workBook.Worksheets.First();
        //                var totalRows = oSheet.Dimension.End.Row;
        //                var totalCols = oSheet.Dimension.End.Column;
        //                var dt = new DataTable(oSheet.Name);
        //                DataRow dr = null;
        //                for (int i = 1; i <= totalRows; i++)
        //                {
        //                    if (i > 1) dr = dt.Rows.Add();
        //                    for (int j = 1; j <= totalCols; j++)
        //                    {
        //                        if (i == 1)
        //                            dt.Columns.Add(oSheet.Cells[i, j].Value.ToString());
        //                        else if (dr != null) dr[j - 1] = oSheet.Cells[i, j].Value == null ? string.Empty : oSheet.Cells[i, j].Value.ToString();
        //                    }
        //                }
        //                if (dt.Columns.Count == 10)
        //                {
        //                    for (int i = 0; i < dt.Rows.Count; i++)
        //                    {
        //                        var location = new LocationModel
        //                        {
        //                            StoreNumber = dt.Rows[i][0].ToString(),
        //                            LocationName = dt.Rows[i][1].ToString(),
        //                            AddressLine1 = dt.Rows[i][2].ToString(),
        //                            AddressLine2 = dt.Rows[i][3].ToString(),
        //                            CityName = dt.Rows[i][4].ToString(),
        //                            StateName = dt.Rows[i][5].ToString(),
        //                            ZipCode = dt.Rows[i][6].ToString(),
        //                            PrimaryPhone = GetPhoneFormatted(dt.Rows[i][7].ToString()),
        //                            StoreClosingTime = GetTimeFormatted(dt.Rows[i][8].ToString()),
        //                            StoreOpeningTime = GetTimeFormatted(dt.Rows[i][9].ToString()),
        //                        };
        //                        lstLocationModel.Add(location);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return lstLocationModel;
        //}

        public static List<LocationModel> ValidateExcel(string filename, out string errorMesg)
        {
            List<LocationModel> lstLocationModel = new List<LocationModel>();
            try
            {
                var existingFile = new FileInfo(filename);
                bool isValid = true;

                //string errorMesg;
                using (var package = new ExcelPackage(existingFile))
                {
                    // Get the work book in the file
                    var workBook = package.Workbook;
                    if (workBook != null)
                    {
                        if (workBook.Worksheets.Count > 0)
                        {
                            var oSheet = workBook.Worksheets.First();
                            if (oSheet.Dimension == null)
                            {
                                isValid = false;
                                errorMesg = "No rows found!";
                            }
                            else
                            {
                                lstLocationModel = getInsertedLocationStatusEx(filename);
                                if (lstLocationModel.Any())
                                    errorMesg = "Success!";
                                else
                                    errorMesg = "Invalid Excel!";
                            }
                        }
                        else
                        {
                            isValid = false;
                            errorMesg = "Workbook sheet not found!";
                        }
                    }
                    else
                    {
                        isValid = false;
                        errorMesg = "Workbook not found!";
                    }
                }
                return lstLocationModel;
            }

            catch (Exception ex)
            {
                errorMesg = "Phone No./Store Opening/Closing Time is/are in incorrect format!";
                return lstLocationModel;
            }
        }

        public static List<UserModels> ValidateExcelBulkUser(string filename, out string errorMesg)
        {
            List<UserModels> lstLocationModel = new List<UserModels>();
            try
            {
                var existingFile = new FileInfo(filename);
                bool isValid = true;

                //string errorMesg;
                using (var package = new ExcelPackage(existingFile))
                {
                    // Get the work book in the file
                    var workBook = package.Workbook;
                    if (workBook != null)
                    {
                        if (workBook.Worksheets.Count > 0)
                        {
                            var oSheet = workBook.Worksheets.First();
                            if (oSheet.Dimension == null)
                            {
                                isValid = false;
                                errorMesg = "No rows found!";
                            }
                            else
                            {
                                lstLocationModel = getInsertedUser(filename);
                                if (lstLocationModel.Any())
                                    errorMesg = "Success!";
                                else
                                    errorMesg = "Invalid Excel!";
                            }
                        }
                        else
                        {
                            isValid = false;
                            errorMesg = "Workbook sheet not found!";
                        }
                    }
                    else
                    {
                        isValid = false;
                        errorMesg = "Workbook not found!";
                    }
                }
                return lstLocationModel;
            }

            catch (Exception ex)
            {
                errorMesg = "Error in Uploading Store User.";
                return lstLocationModel;
            }
        }

        public static List<ManageMember> ValidateExcelBulkMember(string filename, out string errorMesg)
        {
            List<ManageMember> lstMembershipModel = new List<ManageMember>();
            try
            {
                var existingFile = new FileInfo(filename);
                bool isValid = true;

                //string errorMesg;
                using (var package = new ExcelPackage(existingFile))
                {
                    // Get the work book in the file
                    var workBook = package.Workbook;
                    if (workBook != null)
                    {
                        if (workBook.Worksheets.Count > 0)
                        {
                            var oSheet = workBook.Worksheets.First();
                            if (oSheet.Dimension == null)
                            {
                                isValid = false;
                                errorMesg = "No rows found!";
                            }
                            else
                            {
                                lstMembershipModel = getInsertedMember(filename);
                                if (lstMembershipModel.Any())
                                    errorMesg = "Success!";
                                else
                                    errorMesg = "Invalid Excel!";
                            }
                        }
                        else
                        {
                            isValid = false;
                            errorMesg = "Workbook sheet not found!";
                        }
                    }
                    else
                    {
                        isValid = false;
                        errorMesg = "Workbook not found!";
                    }
                }
                return lstMembershipModel;
            }

            catch (Exception ex)
            {
                errorMesg = "First Name/Phone Number/Revives Remaining/Location Name/Start Date/End Date/ are in incorrect format!";
                return lstMembershipModel;
            }
        }
        public static List<ManageMember> ValidateExcelBulkMemberAW(string filename, out string errorMesg)
        {
            List<ManageMember> lstMembershipModel = new List<ManageMember>();
            try
            {
                var existingFile = new FileInfo(filename);
                bool isValid = true;

                //string errorMesg;
                using (var package = new ExcelPackage(existingFile))
                {
                    // Get the work book in the file
                    var workBook = package.Workbook;
                    if (workBook != null)
                    {
                        if (workBook.Worksheets.Count > 0)
                        {
                            var oSheet = workBook.Worksheets.First();
                            if (oSheet.Dimension == null)
                            {
                                isValid = false;
                                errorMesg = "No rows found!";
                            }
                            else
                            {
                                lstMembershipModel = getInsertedMemberAW(filename);
                                if (lstMembershipModel.Any())
                                    errorMesg = "Success!";
                                else
                                    errorMesg = "Invalid Excel!";
                            }
                        }
                        else
                        {
                            isValid = false;
                            errorMesg = "Workbook sheet not found!";
                        }
                    }
                    else
                    {
                        isValid = false;
                        errorMesg = "Workbook not found!";
                    }
                }
                return lstMembershipModel;
            }

            catch (Exception ex)
            {
                errorMesg = "First Name/Phone Number/Revives Remaining/Location Name/Start Date/End Date/ are in incorrect format!";
                return lstMembershipModel;
            }
        }



        //public static List<LocationModel> getInsertedLocationStatusEx_Old(string filename)
        //{
        //    DataSet ds = new DataSet();
        //    List<LocationModel> lstLocationModel = new List<LocationModel>(); ;
        //    string fileExtension = Path.GetExtension(filename);
        //    string excelConnectionString = string.Empty;
        //    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
        //    filename + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
        //    //connection String for xls file format.
        //    if (fileExtension == ".xls")
        //    {
        //        excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
        //        filename + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
        //    }
        //    //connection String for xlsx file format.
        //    else if (fileExtension == ".xlsx")
        //    {
        //        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
        //        filename + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
        //    }
        //    //Create Connection to Excel work book and add oledb namespace
        //    OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
        //    excelConnection.Open();
        //    DataTable dt = new DataTable();

        //    dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        //    if (dt == null)
        //    {
        //        return null;
        //    }

        //    String[] excelSheets = new String[dt.Rows.Count];
        //    int t = 0;
        //    //excel data saves in temp file here.
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        excelSheets[t] = row["TABLE_NAME"].ToString();
        //        t++;
        //    }
        //    OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);


        //    string query = string.Format("Select * from [{0}]", excelSheets[0]);
        //    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
        //    {
        //        dataAdapter.Fill(ds);
        //    }

        //    if (ds.Tables[0].Columns.Count == 10)
        //    {
        //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //        {
        //            var location = new LocationModel
        //            {
        //                StoreNumber = ds.Tables[0].Rows[i][0].ToString(),
        //                LocationName = ds.Tables[0].Rows[i][1].ToString(),
        //                AddressLine1 = ds.Tables[0].Rows[i][2].ToString(),
        //                AddressLine2 = ds.Tables[0].Rows[i][3].ToString(),
        //                CityName = ds.Tables[0].Rows[i][4].ToString(),
        //                StateName = ds.Tables[0].Rows[i][5].ToString(),
        //                ZipCode = ds.Tables[0].Rows[i][6].ToString(),
        //                PrimaryPhone = GetPhoneFormatted(ds.Tables[0].Rows[i][7].ToString()),
        //                StoreClosingTime = GetTimeFormatted(ds.Tables[0].Rows[i][8].ToString()),
        //                StoreOpeningTime = GetTimeFormatted(ds.Tables[0].Rows[i][9].ToString()),
        //            };

        //            lstLocationModel.Add(location);
        //        }
        //    }
        //    return lstLocationModel;
        //}

        public static List<MachineModel> ValidateExcelBulkShippingDetails(string filename, out string errorMesg)
        {
            List<MachineModel> lstMachineModel = new List<MachineModel>();
            try
            {
                var existingFile = new FileInfo(filename);
                bool isValid = true;

                //string errorMesg;
                using (var package = new ExcelPackage(existingFile))
                {
                    // Get the work book in the file
                    var workBook = package.Workbook;
                    if (workBook != null)
                    {
                        if (workBook.Worksheets.Count > 0)
                        {
                            var oSheet = workBook.Worksheets.First();
                            if (oSheet.Dimension == null)
                            {
                                isValid = false;
                                errorMesg = "No rows found!";
                            }
                            else
                            {
                                lstMachineModel = getInsertedShippingDetails(filename);
                                if (lstMachineModel.Any())
                                    errorMesg = "Success!";
                                else
                                    errorMesg = "Invalid Excel!";
                            }
                        }
                        else
                        {
                            isValid = false;
                            errorMesg = "Workbook sheet not found!";
                        }
                    }
                    else
                    {
                        isValid = false;
                        errorMesg = "Workbook not found!";
                    }
                }
                return lstMachineModel;
            }

            catch (Exception ex)
            {
                errorMesg = "Machine ID/Shipping ID/Location Name are in incorrect format!";
                return lstMachineModel;
            }
        }

        private static string GetPhoneFormatted(string sPhone)
        {
            try
            {
                if (!string.IsNullOrEmpty(sPhone) && (!sPhone.Contains("(") || !sPhone.Contains(")") || !sPhone.Contains("-")))
                {
                    sPhone = String.Format("{0:(###) ###-####}", double.Parse(sPhone));
                }
                return sPhone;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string GetTimeFormatted(string sTime)
        {
            try
            {
                if (!string.IsNullOrEmpty(sTime))
                {
                    sTime = Convert.ToDateTime(sTime).ToString("HH:mm");
                }

            }
            catch (Exception ex)
            {
                sTime = Convert.ToDateTime("00:00").ToString("HH:mm");
                //return sTime;
                //throw ex;
            }
            return sTime;
        }

        #endregion
    }


    /// <summary>
    /// Exception class for FileHandler exceptions.
    /// </summary>
    [Serializable()]
    public class CSVHandlerException : ApplicationException
    {

        /// <summary>
        /// Constructs a new CSVHandlerException.
        /// </summary>
        public CSVHandlerException()
            : this("The CSV Handler encountered an error.")
        {
        }

        /// <summary>
        /// Constructs a new exception with the given message.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public CSVHandlerException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructs a new exception with the given message and the inner exception.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="inner">Inner exception that caused this issue.</param>
        public CSVHandlerException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Constructs a new exception with the given serialization information.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected CSVHandlerException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }

    }
}

