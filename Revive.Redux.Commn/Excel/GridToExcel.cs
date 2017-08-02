using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using Revive.Redux.Entities;

namespace Revive.Redux.Commn.Excel
{
    public class GridToExcel
    {
        /// <summary>
        /// user for Formatting Currency fields in Excel
        /// </summary>
        public int CurrencyDecimalPlace { get; set; }
        /// <summary>
        /// User for showing Disclaimer Msg in Excel 
        /// </summary>
        public string ReportsDisclaimerMsg { get; set; }

        public GridToExcel()
        {
            CurrencyDecimalPlace = 2;
        }
        /// <summary>
        /// Actually Generate the Excel with given Data
        /// </summary>
        /// <param name="model"></param>
        /// <param name="data"></param>
        /// <param name="title"></param>
        /// <param name="excelPackage"></param>
        public void GenerateExcelFromGrid(string model, string data, string title, ExcelPackage excelPackage)
        {
            ExcelSetup(title, excelPackage);


            List<int> colToIgnore = new List<int>();
            /* Get the information needed for the worksheet */

            var modelObject = JsonConvert.DeserializeObject<dynamic>(model);
            var dataObject = JsonConvert.DeserializeObject<dynamic>(data);
            Dictionary<string, Dictionary<string, String>> allDropDownVals = new Dictionary<string, Dictionary<string, string>>();
            //Create the worksheet
            ExcelWorksheet ws = excelPackage.Workbook.Worksheets.Add(title);
            SetExcelGeneralFormat(ws);



            /////////////////////////////////////////////////////
            //if (extraHeader != null && extraHeader.Length > 40)
            //{
            //    for (int mdx = 0; mdx < modelObject.Count; mdx++)
            //    {
            //        ws.Cells[1, mdx + 1].Value = extraHeader[mdx];// Here we need to minus the Ignore Col
            //    }
            //}
            /////////////////////////////////////////////////
            // For each column...
            const int headRowNumber = 2; //Start from 2nd Row
            for (int mdx = 0; mdx < modelObject.Count; mdx++)
            {
                if (modelObject[mdx].title == null) //In Case Title is null Ignore that Column
                {
                    colToIgnore.Add(mdx);
                }
                else if ((modelObject[mdx].filterable != null && modelObject[mdx].sortable != null) && (modelObject[mdx].filterable == false && modelObject[mdx].sortable == false))
                {
                    colToIgnore.Add(mdx);
                }
                else if (modelObject[mdx].hidden != null && modelObject[mdx].hidden == true)
                {
                    colToIgnore.Add(mdx);
                }
                else if (modelObject[mdx].editor != null &&
                         (modelObject[mdx].attributes != null && modelObject[mdx].attributes.textVal != null && modelObject[mdx].values != null))
                //in Case in Grid We are using drop down with Value and Text field
                {
                    MaintainDicForDropDown(modelObject, mdx, allDropDownVals);
                    ws.Cells[headRowNumber, mdx + 1 - colToIgnore.Count].Value = modelObject[mdx].title.ToString();
                }
                else
                {
                    ws.Cells[headRowNumber, mdx + 1 - colToIgnore.Count].Value = modelObject[mdx].title.ToString();
                    // Here we need to minus the Ignore Col
                }
            }

            /* Add the data to the worksheet. */
            const int rowNumber = headRowNumber + 1;
            // For each row of data...
            for (int idx = 0; idx < dataObject.Count; idx++)
            {
                int ignoreColCount = 0;
                // For each column...
                for (int mdx = 0; mdx < modelObject.Count; mdx++)
                {
                    if (!colToIgnore.Contains(mdx)) //Check if the Col is not part of ignored columns
                    {
                        var cell = ws.Cells[idx + rowNumber, mdx + 1 - ignoreColCount];

                        if (modelObject[mdx].format != null) // In case in Grid Number formatting is Defined
                        {
                            string colFormat = modelObject[mdx].format.ToString();

                            if (dataObject[idx][modelObject[mdx].field.ToString()].Value is DateTime)
                            {
                                DateTime fetchValue = dataObject[idx][modelObject[mdx].field.ToString()];
                                cell.Value = fetchValue.ToLocalTime().ToString();

                            }
                            else
                            {
                                object returnval;
                                cell.Style.Numberformat.Format = SetNumberFormat(colFormat,
                                                                                 dataObject[idx][
                                                                                     modelObject[mdx].field.ToString()]
                                                                                     .ToString(), out returnval);
                                cell.Value = returnval;
                            }

                        }
                        else if (modelObject[mdx].editor != null && (modelObject[mdx].attributes != null && modelObject[mdx].attributes.textVal != null)) //in Case in Grid We are using drop down with Value and Text field
                        {

                            string selectedIdDropDown =
                                dataObject[idx][modelObject[mdx].field.ToString()].ToString();
                            if (!string.IsNullOrEmpty(selectedIdDropDown))
                            {
                                Dictionary<string, string> selectedDropDown =
                                allDropDownVals[modelObject[mdx].field.ToString()];
                                cell.Value = selectedDropDown[selectedIdDropDown];
                            }
                            else
                            {
                                cell.Value = string.Empty;
                            }

                        }
                        else
                        {
                            cell.Value =
                                dataObject[idx][modelObject[mdx].field.ToString()].ToString();
                        }


                        // Here we need to minus the Ignore Col
                    }
                    else
                    {
                        ignoreColCount++; //Maintain  the count of Columns to ignore.
                    }
                }

            }

            ////////////////////////////////////////////////////////

            //if (extraHeader != null && extraHeader.Length > 40)
            //{
            //    using (ExcelRange rng = ws.Cells[1, 1, 1, modelObject.Count - colToIgnore.Count])
            //    //Get the Range for header 
            //    {
            //        rng.AutoFitColumns();
            //        rng.Style.Font.Bold = true;
            //        //Set Pattern for the background to Solid
            //        rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
            //        //Set color to dark blue
            //        rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
            //        rng.Style.Font.Color.SetColor(Color.White);
            //    }
            //}
            ////////////////////////////
            using (ExcelRange rng = ws.Cells[headRowNumber, 1, headRowNumber, modelObject.Count - colToIgnore.Count])
            //Get the Range for header 
            {
                rng.AutoFitColumns();
                rng.Style.Font.Bold = true;
                //Set Pattern for the background to Solid
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                //Set color to dark blue
                rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
                rng.Style.Font.Color.SetColor(Color.White);
            }

            SetBorderAndPrint(ws, headRowNumber, dataObject, rowNumber, modelObject, colToIgnore);
            SetReportsDisclaimerMsg(ws, dataObject, rowNumber, modelObject, colToIgnore);
        }

        public void GenerateExcelFromGrid_ReportRawData(string model, string data, string title, ExcelPackage excelPackage, ReportConfigModel objReportModel)
        {
            ExcelSetup(title, excelPackage);
            string sKey = System.Configuration.ConfigurationSettings.AppSettings["Encrption"].ToString();

            List<int> colToIgnore = new List<int>();
            /* Get the information needed for the worksheet */

            var modelObject = JsonConvert.DeserializeObject<dynamic>(model);
            var dataObject = JsonConvert.DeserializeObject<dynamic>(data);
            Dictionary<string, Dictionary<string, String>> allDropDownVals = new Dictionary<string, Dictionary<string, string>>();
            //Create the worksheet
            ExcelWorksheet ws = excelPackage.Workbook.Worksheets.Add(title);
            SetExcelGeneralFormat(ws);



            /////////////////////////////////////////////////////
            //if (extraHeader != null && extraHeader.Length > 40)
            //{
            //    for (int mdx = 0; mdx < modelObject.Count; mdx++)
            //    {
            //        ws.Cells[1, mdx + 1].Value = extraHeader[mdx];// Here we need to minus the Ignore Col
            //    }
            //}
            /////////////////////////////////////////////////
            // For each column...
            // Added Report fields Array
            //if (objReportModel.ReportMasterList.ReportId == 18)
            //{
            //    objReportModel.ReportMasterList.FieldName += ",Activity Type,Membership Type,Membership Id,Member,Email Id,Device Manufacturer,HowLong Ago,plugged in,Phone Number,Accumulated RH Value,Total Cycles,Remaining Cycles,KPI Maximum Vacuum,KPI time to Maximum Vacuum,KPI Initial Injection ROR,KPI Platen ROR,KPI Minimum Chamber Humidity,KPI Maximum Chamber Humidity,KPI Maximum Charge,KPI Number of Cycles,KPI Duration,KPI Process ID,KPI Maximum Ambient Temperatur,Process State,Checkout Details".ToString();
            //}
            //string[] ReportFieldName = objReportModel.ReportMasterList.FieldName.Split(',');
            int totalCountField = 17;

            const int headRowNumber = 2; //Start from 2nd Row
            for (int mdx = 0; mdx < modelObject.Count; mdx++)
            {
                /// Added by KD on Dated 29 JUN 2015 , FOR CHANGING THE  Dynamic column name of reports 
                //if (modelObject[mdx].title != null && mdx < totalCountField)
                //{
                //    modelObject[mdx].title = "Report Raw Data";
                //}

                /// End

                if (modelObject[mdx].title == null) //In Case Title is null Ignore that Column
                {
                    colToIgnore.Add(mdx);
                }
                else if ((modelObject[mdx].filterable != null && modelObject[mdx].sortable != null) && (modelObject[mdx].filterable == false && modelObject[mdx].sortable == false))
                {
                    colToIgnore.Add(mdx);
                }
                else if (modelObject[mdx].hidden != null && modelObject[mdx].hidden == true)
                {
                    colToIgnore.Add(mdx);
                }
                else if (modelObject[mdx].editor != null &&
                         (modelObject[mdx].attributes != null && modelObject[mdx].attributes.textVal != null && modelObject[mdx].values != null))
                //in Case in Grid We are using drop down with Value and Text field
                {
                    MaintainDicForDropDown(modelObject, mdx, allDropDownVals);
                    ws.Cells[headRowNumber, mdx + 1 - colToIgnore.Count].Value = modelObject[mdx].title.ToString();
                }
                else
                {
                    ws.Cells[headRowNumber, mdx + 1 - colToIgnore.Count].Value = modelObject[mdx].title.ToString();
                    // Here we need to minus the Ignore Col
                }
            }

            /* Add the data to the worksheet. */
            const int rowNumber = headRowNumber + 1;
            // For each row of data...
            for (int idx = 0; idx < dataObject.Count; idx++)
            {
                int ignoreColCount = 0;
                // For each column...
                for (int mdx = 0; mdx < modelObject.Count; mdx++)
                {
                    if (!colToIgnore.Contains(mdx)) //Check if the Col is not part of ignored columns
                    {
                        var cell = ws.Cells[idx + rowNumber, mdx + 1 - ignoreColCount];

                        if (modelObject[mdx].format != null) // In case in Grid Number formatting is Defined
                        {
                            string colFormat = modelObject[mdx].format.ToString();

                            if (dataObject[idx][modelObject[mdx].field.ToString()].Value is DateTime)
                            {
                                DateTime fetchValue = dataObject[idx][modelObject[mdx].field.ToString()];
                                cell.Value = fetchValue.ToLocalTime().ToString();

                            }
                            else
                            {
                                object returnval;
                                cell.Style.Numberformat.Format = SetNumberFormat(colFormat,
                                                                                 dataObject[idx][
                                                                                     modelObject[mdx].field.ToString()]
                                                                                     .ToString(), out returnval);
                                cell.Value = returnval;
                            }

                        }
                        else if (modelObject[mdx].editor != null && (modelObject[mdx].attributes != null && modelObject[mdx].attributes.textVal != null)) //in Case in Grid We are using drop down with Value and Text field
                        {

                            string selectedIdDropDown =
                                dataObject[idx][modelObject[mdx].field.ToString()].ToString();
                            if (!string.IsNullOrEmpty(selectedIdDropDown))
                            {
                                Dictionary<string, string> selectedDropDown =
                                allDropDownVals[modelObject[mdx].field.ToString()];
                                cell.Value = selectedDropDown[selectedIdDropDown];
                            }
                            else
                            {
                                cell.Value = string.Empty;
                            }

                        }
                        else
                        {
                            //string val = dataObject[idx][modelObject[mdx].field.ToString()].ToString();
                            //cell.Value = val != null && val.Trim() != "" && val.Trim() != string.Empty ? (val.LastIndexOf("Non-Member", StringComparison.Ordinal) > 0 ? Cryptography.SplitText(Cryptography.DecryptText(val.Substring(0, val.LastIndexOf("Non-Member", StringComparison.Ordinal)), sKey), "@") : val) : val;
                            if (dataObject[idx][modelObject[mdx].field.ToString()] != null)
                            {
                                cell.Value = dataObject[idx][modelObject[mdx].field.ToString()].ToString();
                            }

                        }


                        // Here we need to minus the Ignore Col
                    }
                    else
                    {
                        ignoreColCount++; //Maintain  the count of Columns to ignore.
                    }
                }

            }

            ////////////////////////////////////////////////////////

            //if (extraHeader != null && extraHeader.Length > 40)
            //{
            //    using (ExcelRange rng = ws.Cells[1, 1, 1, modelObject.Count - colToIgnore.Count])
            //    //Get the Range for header 
            //    {
            //        rng.AutoFitColumns();
            //        rng.Style.Font.Bold = true;
            //        //Set Pattern for the background to Solid
            //        rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
            //        //Set color to dark blue
            //        rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
            //        rng.Style.Font.Color.SetColor(Color.White);
            //    }
            //}
            ////////////////////////////
            using (ExcelRange rng = ws.Cells[headRowNumber, 1, headRowNumber, modelObject.Count - colToIgnore.Count])
            //Get the Range for header 
            {
                rng.AutoFitColumns();
                rng.Style.Font.Bold = true;
                //Set Pattern for the background to Solid
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                //Set color to dark blue
                rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
                rng.Style.Font.Color.SetColor(Color.White);
            }

            SetBorderAndPrint(ws, headRowNumber, dataObject, rowNumber, modelObject, colToIgnore);
            SetReportsDisclaimerMsg(ws, dataObject, rowNumber, modelObject, colToIgnore);
        }


        public void GenerateExcelFromGrid_Report(string model, string data, string title, ExcelPackage excelPackage, ReportConfigModel objReportModel)
        {
            ExcelSetup(title, excelPackage);
            string sKey = System.Configuration.ConfigurationSettings.AppSettings["Encrption"].ToString();

            List<int> colToIgnore = new List<int>();
            /* Get the information needed for the worksheet */

            var modelObject = JsonConvert.DeserializeObject<dynamic>(model);
            var dataObject = JsonConvert.DeserializeObject<dynamic>(data);
            Dictionary<string, Dictionary<string, String>> allDropDownVals = new Dictionary<string, Dictionary<string, string>>();
            //Create the worksheet
            ExcelWorksheet ws = excelPackage.Workbook.Worksheets.Add(title);
            SetExcelGeneralFormat(ws);



            /////////////////////////////////////////////////////
            //if (extraHeader != null && extraHeader.Length > 40)
            //{
            //    for (int mdx = 0; mdx < modelObject.Count; mdx++)
            //    {
            //        ws.Cells[1, mdx + 1].Value = extraHeader[mdx];// Here we need to minus the Ignore Col
            //    }
            //}
            /////////////////////////////////////////////////
            // For each column...
            // Added Report fields Array
            if (objReportModel.ReportMasterList.ReportId == 18)
            {
                objReportModel.ReportMasterList.FieldName += ",Activity Type,Membership Type,Membership Id,Member,Email Id,Device Manufacturer,HowLong Ago,plugged in,Phone Number,Accumulated RH Value,Total Cycles,Remaining Cycles,KPI Maximum Vacuum,KPI time to Maximum Vacuum,KPI Initial Injection ROR,KPI Platen ROR,KPI Minimum Chamber Humidity,KPI Maximum Chamber Humidity,KPI Maximum Charge,KPI Number of Cycles,KPI Duration,KPI Process ID,KPI Maximum Ambient Temperatur,Process State,Checkout Details".ToString();
            }
            string[] ReportFieldName = objReportModel.ReportMasterList.FieldName.Split(',');
            int totalCountField = ReportFieldName.Length;

            const int headRowNumber = 2; //Start from 2nd Row
            for (int mdx = 0; mdx < modelObject.Count; mdx++)
            {
                /// Added by KD on Dated 29 JUN 2015 , FOR CHANGING THE  Dynamic column name of reports 
                if (modelObject[mdx].title != null && mdx < totalCountField)
                {
                    modelObject[mdx].title = ReportFieldName[mdx];
                }

                /// End

                if (modelObject[mdx].title == null) //In Case Title is null Ignore that Column
                {
                    colToIgnore.Add(mdx);
                }
                else if ((modelObject[mdx].filterable != null && modelObject[mdx].sortable != null) && (modelObject[mdx].filterable == false && modelObject[mdx].sortable == false))
                {
                    colToIgnore.Add(mdx);
                }
                else if (modelObject[mdx].hidden != null && modelObject[mdx].hidden == true)
                {
                    colToIgnore.Add(mdx);
                }
                else if (modelObject[mdx].editor != null &&
                         (modelObject[mdx].attributes != null && modelObject[mdx].attributes.textVal != null && modelObject[mdx].values != null))
                //in Case in Grid We are using drop down with Value and Text field
                {
                    MaintainDicForDropDown(modelObject, mdx, allDropDownVals);
                    ws.Cells[headRowNumber, mdx + 1 - colToIgnore.Count].Value = modelObject[mdx].title.ToString();
                }
                else
                {
                    ws.Cells[headRowNumber, mdx + 1 - colToIgnore.Count].Value = modelObject[mdx].title.ToString();
                    // Here we need to minus the Ignore Col
                }
            }

            /* Add the data to the worksheet. */
            const int rowNumber = headRowNumber + 1;
            // For each row of data...
            for (int idx = 0; idx < dataObject.Count; idx++)
            {
                int ignoreColCount = 0;
                // For each column...
                for (int mdx = 0; mdx < modelObject.Count; mdx++)
                {
                    if (!colToIgnore.Contains(mdx)) //Check if the Col is not part of ignored columns
                    {
                        var cell = ws.Cells[idx + rowNumber, mdx + 1 - ignoreColCount];

                        if (modelObject[mdx].format != null) // In case in Grid Number formatting is Defined
                        {
                            string colFormat = modelObject[mdx].format.ToString();

                            if (dataObject[idx][modelObject[mdx].field.ToString()].Value is DateTime)
                            {
                                DateTime fetchValue = dataObject[idx][modelObject[mdx].field.ToString()];
                                cell.Value = fetchValue.ToLocalTime().ToString();

                            }
                            else
                            {
                                object returnval;
                                cell.Style.Numberformat.Format = SetNumberFormat(colFormat,
                                                                                 dataObject[idx][
                                                                                     modelObject[mdx].field.ToString()]
                                                                                     .ToString(), out returnval);
                                cell.Value = returnval;
                            }

                        }
                        else if (modelObject[mdx].editor != null && (modelObject[mdx].attributes != null && modelObject[mdx].attributes.textVal != null)) //in Case in Grid We are using drop down with Value and Text field
                        {

                            string selectedIdDropDown =
                                dataObject[idx][modelObject[mdx].field.ToString()].ToString();
                            if (!string.IsNullOrEmpty(selectedIdDropDown))
                            {
                                Dictionary<string, string> selectedDropDown =
                                allDropDownVals[modelObject[mdx].field.ToString()];
                                cell.Value = selectedDropDown[selectedIdDropDown];
                            }
                            else
                            {
                                cell.Value = string.Empty;
                            }

                        }
                        else
                        {
                            string val = dataObject[idx][modelObject[mdx].field.ToString()].ToString();
                            cell.Value = val != null && val.Trim() != "" && val.Trim() != string.Empty ? (val.LastIndexOf("Non-Member", StringComparison.Ordinal) > 0 ? Cryptography.SplitText(Cryptography.DecryptText(val.Substring(0, val.LastIndexOf("Non-Member", StringComparison.Ordinal)), sKey), "@") : val) : val;
                            //cell.Value =
                            //    dataObject[idx][modelObject[mdx].field.ToString()].ToString();
                        }


                        // Here we need to minus the Ignore Col
                    }
                    else
                    {
                        ignoreColCount++; //Maintain  the count of Columns to ignore.
                    }
                }

            }

            ////////////////////////////////////////////////////////

            //if (extraHeader != null && extraHeader.Length > 40)
            //{
            //    using (ExcelRange rng = ws.Cells[1, 1, 1, modelObject.Count - colToIgnore.Count])
            //    //Get the Range for header 
            //    {
            //        rng.AutoFitColumns();
            //        rng.Style.Font.Bold = true;
            //        //Set Pattern for the background to Solid
            //        rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
            //        //Set color to dark blue
            //        rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
            //        rng.Style.Font.Color.SetColor(Color.White);
            //    }
            //}
            ////////////////////////////
            using (ExcelRange rng = ws.Cells[headRowNumber, 1, headRowNumber, modelObject.Count - colToIgnore.Count])
            //Get the Range for header 
            {
                rng.AutoFitColumns();
                rng.Style.Font.Bold = true;
                //Set Pattern for the background to Solid
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                //Set color to dark blue
                rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
                rng.Style.Font.Color.SetColor(Color.White);
            }

            SetBorderAndPrint(ws, headRowNumber, dataObject, rowNumber, modelObject, colToIgnore);
            SetReportsDisclaimerMsg(ws, dataObject, rowNumber, modelObject, colToIgnore);
        }




        /// <summary>
        /// Maintain Dictionary for showing Text item in case of Drop downs
        /// </summary>
        /// <param name="modelObject"></param>
        /// <param name="mdx"></param>
        /// <param name="allDropDownVals"></param>
        private static void MaintainDicForDropDown(dynamic modelObject, int mdx, Dictionary<string, Dictionary<string, string>> allDropDownVals)
        {
            Dictionary<string, String> textValDic = new Dictionary<string, string>();
            foreach (var keyVal in modelObject[mdx].values) //convert dropdown  text value to dictionary
            {
                textValDic.Add(keyVal.value.ToString(), ((string)keyVal.text.ToString()).Trim());
            }
            if (textValDic.Count > 0)
            {
                allDropDownVals.Add(modelObject[mdx].field.ToString(), textValDic);
                //in case grid has multiple drop down we ae maintaining a dic of Dic where fieldID will identity the actual dictionary fo the dropdown 
            }
        }

        /// <summary>
        ///     Set Border and Print Settings
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="headRowNumber"></param>
        /// <param name="dataObject"></param>
        /// <param name="rowNumber"></param>
        /// <param name="modelObject"></param>
        /// <param name="colToIgnore"></param>
        private static void SetBorderAndPrint(ExcelWorksheet ws, int headRowNumber, dynamic dataObject, int rowNumber,
                                              dynamic modelObject, List<int> colToIgnore)
        {
            using (
                ExcelRange rng =
                    ws.Cells[headRowNumber, 1, dataObject.Count + rowNumber, modelObject.Count - colToIgnore.Count]
                )
            {
                var border = rng.Style.Border;
                border.Bottom.Style =
                    border.Top.Style =
                    border.Left.Style =
                    border.Right.Style = ExcelBorderStyle.Thin;
            }

            //Autofit columns for all cells            
            ws.Cells.AutoFitColumns(0);

            ExcelPrinterSettings ps = ws.PrinterSettings;
            //ps.BlackAndWhite = true;
            //ps.Draft = true;
            ps.FitToPage = true;
            ps.PaperSize = ePaperSize.A4;
            //int TotalRow
            //ps.PrintArea = ws.Cells[1, 1, dataObject.Count, modelObject.Count - colToIgnore.Count];
        }

        /// <summary>
        /// Set the ReportsDisclaimerMsg at the end of the Excel file
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="dataObject"></param>
        /// <param name="rowNumber"></param>
        /// <param name="modelObject"></param>
        /// <param name="colToIgnore"></param>
        private void SetReportsDisclaimerMsg(ExcelWorksheet ws, dynamic dataObject,
                                                    int rowNumber,
                                                    dynamic modelObject, List<int> colToIgnore)
        {
            int rowForDisclaimer = dataObject.Count + rowNumber + 2;// Last Row + 2 
            int coltoMergeForDisclaimer = 5;
            if (modelObject.Count - colToIgnore.Count > 5)
            {
                coltoMergeForDisclaimer = modelObject.Count - colToIgnore.Count;
            }

            ws.Cells[rowForDisclaimer, 1, rowForDisclaimer, coltoMergeForDisclaimer].Merge = true;
            var cell = ws.Cells[rowForDisclaimer, 1];
            cell.Value = ReportsDisclaimerMsg;
        }

        /// <summary>
        ///     Excel Font, Alignment Header , footer
        /// </summary>
        /// <param name="ws"></param>
        private void SetExcelGeneralFormat(ExcelWorksheet ws)
        {
            #region ExcelWorksheetSetup

            //Default style for whole sheet
            //ws.Cells.Style.Font.SetFromFont(new Font("Calibri", 11, FontStyle.Regular));
            ws.Cells.Style.Font.Size = 11; //Default font size for whole sheet
            ws.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet
            ws.Cells.Style.Font.Color.SetColor(Color.Black);
            ws.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            ws.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            //Setting borders of cell
            ws.Cells.Style.Border.Left.Style = ws.Cells.Style.Border.Right.Style = ExcelBorderStyle.None;
            ws.Cells.Style.Numberformat.Format = "General";
            /* Add the column titles to the worksheet. */
            //Step 5 : Save all changes to ExcelPackage object which will create Excel 2007 file.
            // lets set the header text 
            ws.HeaderFooter.OddHeader.CenteredText = "&" + 11 + "&U&\"" + "Calibri" + ",Regular Bold\" " + ws.Name;
            // add the page number to the footer plus the total number of pages
            ws.HeaderFooter.OddHeader.RightAlignedText = string.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber,
                                                                       ExcelHeaderFooter.NumberOfPages);
            // add the sheet name to the footer
            ws.HeaderFooter.OddFooter.CenteredText = ReportsDisclaimerMsg;

            #endregion
        }

        /// <summary>
        ///     General setup like Title , Auth, Company
        /// </summary>
        /// <param name="title"></param>
        /// <param name="excelPackage"></param>
        private static void ExcelSetup(string title, ExcelPackage excelPackage)
        {
            #region ExcelPackageSetup

            //Step 4 : (Optional) Set the file properties like title, author and subject
            excelPackage.Workbook.Properties.Title = title;
            excelPackage.Workbook.Properties.Author = "Redux";
            excelPackage.Workbook.Properties.Subject = title;
            excelPackage.Workbook.Properties.Company = "Redux";

            #endregion
        }

        private string SetNumberFormat(string givenFormat, string orgVal, out object returnval)
        {
            string result = "@";
            returnval = orgVal;
            if (givenFormat.Contains("{0:C"))
            {
                result = @"$#,##0.";
                for (int i = 0; i < CurrencyDecimalPlace; i++)
                {
                    result = result + "0";
                }

                returnval = Convert.ToDecimal(orgVal);
            }


            return result;
        }
    }
}
