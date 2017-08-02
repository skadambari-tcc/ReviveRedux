using Revive.Redux.Common;
using Revive.Redux.Controllers.Common;
using Revive.Redux.Entities;
using Revive.Redux.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Revive.Redux.UI.Controllers
{
    [Authorize]
    [ReviveAuth]
    public class TestingController : Controller
    {
        private IMachineService _IMachineService = null;
        private ITestingService _ITestingService = null;
        private ILogService logService = null;
        public TestingController()
        {
            logService = new LogService();
            _IMachineService = new MachineService();
            _ITestingService = new TestingService();
        }

        public ActionResult Perform(String id)
        {
            TestingModel testModel = new TestingModel();
            if (!string.IsNullOrEmpty(id))
            {
                string decryptedId = CommonMethods.Decode(id);
                int machMapId = Convert.ToInt32(decryptedId);
                try
                {
                    BindPerformAction(machMapId, testModel);
                    testModel.MachineMappingId = machMapId;
                }
                catch (Exception ex)
                {
                }
            }
            return View("Perform", testModel);
        }

        private void BindPerformAction(int machMapId, TestingModel testModel)
        {
            ViewBag.MachineInShipping = false;

            List<DtoList> TestStages = new List<DtoList>();

            // Bind Machine Header Details
            MachineModel machModel = BindMachineHeaderDetails(machMapId);
            bool? midLineTestStatus = machModel.MidLineTestStatus;
            bool? ulTestStatus = machModel.ULTestStatus;
            bool? finalTestStatus = machModel.FinalTestStatus;

            BindTestingResults(machMapId, testModel);

            testModel.MachineMappingId = machMapId;
            testModel.MidLineStageId = (int)machModel.MidLineStageId;
            testModel.ULStageId = (int)machModel.ULStageId;
            testModel.FinalStageId = (int)machModel.FinalStageId;

            testModel.TestingStages = new List<DtoList>();
            TestStages.Add(new DtoList { Id = (int)machModel.MidLineStageId, Text = machModel.MidLineStageName });

            // If Midline status passed, add UL test option in dropdown
            if (midLineTestStatus != null && (bool)midLineTestStatus)
            {
                testModel.MachineTestingStatus = "Mid-Line Testing Passed";
                TestStages.Add(new DtoList { Id = (int)machModel.ULStageId, Text = machModel.ULStageName });
                // If UL status passed, add Final test option in dropdown
                if (ulTestStatus != null && (bool)ulTestStatus)
                {
                    testModel.MachineTestingStatus = "UL Requirements Testing Passed";
                    TestStages.Add(new DtoList { Id = (int)machModel.FinalStageId, Text = machModel.FinalStageName });
                    if (finalTestStatus != null && (bool)finalTestStatus)
                    {
                        testModel.MachineTestingStatus = "Final Assembly Testing Passed";

                        // If Machine has passed all tests, and, Location Id is not null, and, current MachineStatusName is In-Shipping,
                        // User can't change testing results.
                        if (machModel.LocationId != null && (machModel.MachineStatusName == OrderStatusType.InShipping || machModel.MachineStatusName == OrderStatusType.Shipped))
                            ViewBag.MachineInShipping = true;
                    }
                }
            }
            else
            {
                testModel.MachineTestingStatus = "Pending";
            }

            testModel.TestingStages.AddRange(TestStages);
        }
        private MachineModel BindMachineHeaderDetails(int machMapId)
        {
            MachineModel machine = _IMachineService.GetMachineDetailByMachMapId(machMapId);

            ViewBag.JobOrderHeaderID = machine.JobOrderHeaderID;
            ViewBag.JobOrderHeaderIDEncoded = machine.JobOrderHeaderIdEncoded;
            ViewBag.JobOrderLocationId = machine.LocationId;
            TempData["JobOrderHeaderID"] = machine.JobOrderHeaderID;
            TempData["JobOrderHeaderIDEncoded"] = machine.JobOrderHeaderIdEncoded;
            ViewBag.Location = machine.Location;
            ViewBag.CustomerName = machine.CustomerName;
            ViewBag.MachineId = machine.MachineId_Val;
            ViewBag.NoOfMachines = machine.NoOfMachines;            
            return machine;
        }
        [HttpPost]
        public JsonResult LocationValidByMachineId(int jobOrdHeaderId, int jobOrdLocId)
        {
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            bool isValidate = true;

            if (jobOrdLocId > 0)
            {
                isValidate = _IMachineService.isValidMappingFinalSubmitByLocation(jobOrdLocId, jobOrdHeaderId);
            }
            else
            {
                isValidate = _IMachineService.isValidMappingFinalSubmitByLocation(0, jobOrdHeaderId);
            }
            if (isValidate)
            {
                return Json("Valid", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Not Valid", JsonRequestBehavior.AllowGet);
            }




        }

        private void BindTestingResults(int machMapId, TestingModel testModel)
        {
            // BindMidLineResults
            TestingModel dbResults = _ITestingService.GetMidLineTestResults(machMapId);
            if (dbResults != null)
            {
                testModel.MidLineTestingRowId = dbResults.MidLineTestingRowId;

                testModel.PlatenTime = dbResults.PlatenTime;
                testModel.PlatenSeconds = dbResults.PlatenSeconds;
                testModel.PlatenSensor = dbResults.PlatenSensor;
                testModel.PlatenDelta = dbResults.PlatenDelta;

                testModel.InjectionTime = dbResults.InjectionTime;
                testModel.InjectionSeconds = dbResults.InjectionSeconds;
                testModel.InjectionSensor = dbResults.InjectionSensor;
                testModel.InjectionDelta = dbResults.InjectionDelta;

                testModel.VaccumTime = dbResults.VaccumTime;
                testModel.VaccumSeconds = dbResults.VaccumSeconds;
                testModel.VaccumSensor = dbResults.VaccumSensor;
                testModel.VaccumDelta = dbResults.VaccumDelta;

                testModel.USBInternalSensor = dbResults.USBInternalSensor;
                testModel.USBInternalDelta = dbResults.USBInternalDelta;
                testModel.USBExternalSensor = dbResults.USBExternalSensor;
                testModel.USBExternalDelta = dbResults.USBExternalDelta;

                testModel.RelativeSensor = dbResults.RelativeSensor;
                testModel.RelativeDelta = dbResults.RelativeDelta;
                testModel.DryingProcess = dbResults.DryingProcess;
                testModel.DryingCycles = dbResults.DryingCycles;

                testModel.Test1Name = dbResults.Test1Name;
                testModel.Test1Verified = dbResults.Test1Verified;
                testModel.Test2Name = dbResults.Test2Name;
                testModel.Test2Verified = dbResults.Test2Verified;
                testModel.Test3Name = dbResults.Test3Name;
                testModel.Test3Verified = dbResults.Test3Verified;

                // BindULResults
                //dbResults = null;
                dbResults = _ITestingService.GetULTestResults(machMapId);
                if (dbResults != null)
                {
                    testModel.ULTestingRowId = dbResults.ULTestingRowId;
                    testModel.DielectricTest = dbResults.DielectricTest;
                    testModel.DielectricVoltage = dbResults.DielectricVoltage;
                    testModel.DielectricCurrent = dbResults.DielectricCurrent;
                    testModel.DielectricTime = dbResults.DielectricTime;
                    testModel.GroundTest = dbResults.GroundTest;
                    testModel.GroundResistance = dbResults.GroundResistance;

                    // BindFinalResults
                    dbResults = _ITestingService.GetFinalTestResults(machMapId);
                    if (dbResults != null)
                    {
                        testModel.FinalTestingRowId = dbResults.FinalTestingRowId;
                        testModel.FinalAssembly = dbResults.FinalAssembly;
                        testModel.FinalProcessId = dbResults.FinalProcessId;
                    }
                }
            }
        }

        /// <summary>
        /// Update Machine testing results
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateMachineTesting(TestingModel model)
        {
            bool testPassed = false;
            if (Request.Form["hdnTestStageName"] != null && !string.IsNullOrEmpty(Request.Form["hdnTestStageName"]) &&
                Request.Form["hdnTestStageID"] != null && !string.IsNullOrEmpty(Request.Form["hdnTestStageID"]))
            {
                TestingModel testModel = new TestingModel();
                var currentUser = (CurrentUserDetail)Session["CurrentUser"];

                try
                {
                    testPassed = _ITestingService.UpdateMachineTesting(Request.Form["hdnTestStageName"], Convert.ToInt32(Request.Form["hdnTestStageID"]), model, currentUser.User_Id);
                    TempData["MachineTestingUpdated"] = true;

                    // If Test is passed and it is not the Final Test, then re-bind the page
                    if (testPassed && !Convert.ToString(Request.Form["hdnTestStageName"]).ToLower().Contains("final"))
                        BindPerformAction((int)model.MachineMappingId, testModel);
                    // If Test Failed, redirect to map machines page
                    else if (!testPassed && TempData["JobOrderHeaderIDEncoded"] != null)
                    {
                        string jobId = Convert.ToString(TempData["JobOrderHeaderIDEncoded"]);
                        return RedirectToAction("MapMachines", new RouteValueDictionary(
                            new { controller = "Machine", action = "MapMachines", Id = jobId }));
                    }
                    // If Test passed and it is Final Test, redirect to map machines page
                    else if (testPassed && Convert.ToString(Request.Form["hdnTestStageName"]).ToLower().Contains("final") && TempData["JobOrderHeaderIDEncoded"] != null)
                    {
                        string jobId = Convert.ToString(TempData["JobOrderHeaderIDEncoded"]);
                        return RedirectToAction("MapMachines", new RouteValueDictionary(
                            new { controller = "Machine", action = "MapMachines", Id = jobId }));
                    }
                    else
                    {
                        return View("Perform", testModel);
                    }
                }
                catch (Exception ex)
                {
                    testPassed = false;
                    throw;
                }
                // If final test also passed, redirect to Map Machines with jobOrder Id
                if ((testPassed && Convert.ToString(Request.Form["hdnTestStageName"]).ToLower().Contains("final")) && TempData["JobOrderHeaderIDEncoded"] != null)
                {
                    string jobId = Convert.ToString(TempData["JobOrderHeaderIDEncoded"]);
                    return RedirectToAction("MapMachines", new RouteValueDictionary(
                        new { controller = "Machine", action = "MapMachines", Id = jobId }));
                }
                else
                {
                    return View("Perform", testModel);
                }
            }
            else
            {
                throw new Exception("TestStageName and TestStageId not available in performing testing of machineMappingId:" + model.MachineMappingId);
            }
        }
    }
}