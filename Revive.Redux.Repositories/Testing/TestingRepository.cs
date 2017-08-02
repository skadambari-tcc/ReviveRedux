using Revive.Redux.Common;
using Revive.Redux.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Repositories
{
    public class TestingRepository : ITestingRepository
    {
        private ILogService logService = null;

        public TestingRepository()
        {
            logService = new LogService();
        }

        /// <summary>
        /// Get all values of Mid Line testing from JobOrder_Machine_TestResultsMidLine table
        /// based on MachineMappingId
        /// </summary>
        /// <param name="machMapId"></param>
        /// <returns></returns>
        public TestingModel GetMidLineTestResults(int machMapId)
        {
            var result = new TestingModel();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var record = (from JobOrder_Machine_TestResultsMidLine dbResult in dbContext.JobOrder_Machine_TestResultsMidLine
                                  where dbResult.JobOrder_Machine_mapping_ID == machMapId
                                  select new TestingModel
                                  {
                                      MidLineTestingRowId = dbResult.JobOrder_Machine_Testing_id,

                                      PlatenTime = dbResult.Platen_time,
                                      PlatenSeconds = dbResult.Platen_seconds,
                                      PlatenSensor = dbResult.Platen_sensor,
                                      PlatenDelta = dbResult.Platen_delta,

                                      InjectionTime = dbResult.Injection_time,
                                      InjectionSeconds = dbResult.Injection_seconds,
                                      InjectionSensor = dbResult.Injection_sensor,
                                      InjectionDelta = dbResult.Injection_delta,

                                      VaccumTime = dbResult.Vaccum_time,
                                      VaccumSeconds = dbResult.Vaccum_seconds,
                                      VaccumSensor = dbResult.Vaccum_sensor,
                                      VaccumDelta = dbResult.Vaccum_delta,

                                      USBInternalSensor = dbResult.USB_Internal_sensor,
                                      USBInternalDelta = dbResult.USB_Internal_delta,
                                      USBExternalSensor = dbResult.USB_External_sensor,
                                      USBExternalDelta = dbResult.USB_External_delta,

                                      RelativeSensor = dbResult.Relative_Sensor,
                                      RelativeDelta = dbResult.Relative_Delta,
                                      DryingProcess = dbResult.Drying_Process,
                                      DryingCycles = dbResult.Drying_Cycles,

                                      Test1Name = dbResult.Test1_Name,
                                      Test1Verified = dbResult.Test1_Verified,
                                      Test2Name = dbResult.Test2_Name,
                                      Test2Verified = dbResult.Test2_Verified,
                                      Test3Name = dbResult.Test3_Name,
                                      Test3Verified = dbResult.Test3_Verified

                                  }).FirstOrDefault();
                    result = record;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }
        /// <summary>
        /// Get all values of UL testing from JobOrder_Machine_TestResultsUL table
        /// based on MachineMappingId
        /// </summary>
        /// <param name="machMapId">MachineMappingId</param>
        /// <returns></returns>
        public TestingModel GetULTestResults(int machMapId)
        {
            var result = new TestingModel();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var record = (from JobOrder_Machine_TestResultsUL dbResult in dbContext.JobOrder_Machine_TestResultsUL
                                  where dbResult.JobOrder_Machine_mapping_ID == machMapId
                                  select new TestingModel
                                  {
                                      ULTestingRowId = dbResult.JobOrder_Machine_Testing_id,

                                      DielectricTest = dbResult.Dielectric_test,
                                      DielectricVoltage = dbResult.Dielectric_Voltage,
                                      DielectricCurrent = dbResult.Dielectric_Current,
                                      DielectricTime = dbResult.Dielectric_Time,

                                      GroundTest = dbResult.Ground_Test,
                                      GroundResistance = dbResult.Ground_Resistance

                                  }).FirstOrDefault();
                    result = record;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }
        /// <summary>
        /// Get all values of Final testing from JobOrder_Machine_TestResultsFinal table
        /// based on MachineMappingId
        /// </summary>
        /// <param name="machMapId">MachineMappingId</param>
        /// <returns></returns>
        public TestingModel GetFinalTestResults(int machMapId)
        {
            var result = new TestingModel();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var record = (from JobOrder_Machine_TestResultsFinal dbResult in dbContext.JobOrder_Machine_TestResultsFinal
                                  where dbResult.JobOrder_Machine_mapping_ID == machMapId
                                  select new TestingModel
                                  {
                                      FinalTestingRowId = dbResult.JobOrder_Machine_Testing_id,

                                      FinalAssembly = dbResult.Final_Assembly,
                                      FinalProcessId = dbResult.Final_Process_Id

                                  }).FirstOrDefault();
                    result = record;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// Update machine testing results to their corresponding tables.
        /// </summary>
        /// <param name="testStageName">Name of test stage</param>
        /// <param name="testStageId">id of test stage</param>
        /// <param name="model">Testing model</param>
        /// <param name="currentUserId">Guid of current user</param>
        public bool UpdateMachineTesting(string testStageName, int testStageId, TestingModel model, Guid currentUserId)
        {
            bool ifTestPassed = false;
            try
            {
                if (testStageName.Trim().ToLower().Contains("Mid-Line".ToLower()))
                {
                    ifTestPassed = UpdateMidLineTest(testStageId, model, currentUserId);
                }
                else if (testStageName.Trim().ToLower().Contains("UL Requirements".ToLower()))
                {
                    ifTestPassed = UpdateULTest(testStageId, model, currentUserId);
                }
                else
                {
                    ifTestPassed = UpdateFinalTest(testStageId, model, currentUserId);
                }
            }
            catch (Exception ex)
            {
                ifTestPassed = false;
                throw;
            }
            return ifTestPassed;
        }

        private bool UpdateMidLineTest(int testStageId, TestingModel model, Guid currentUserId)
        {
            bool isMachinePass = false;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    bool recordInserted = false;
                    int machineMappingId = (int)model.MachineMappingId;
                    JobOrder_Machine_TestResultsMidLine dbRecord = dbContext.JobOrder_Machine_TestResultsMidLine.FirstOrDefault(c => c.JobOrder_Machine_mapping_ID == machineMappingId);
                    // if record exist, will use object to update.
                    // if record doesn't exist, create insert object.
                    if (dbRecord == null)
                    {
                        dbRecord = new JobOrder_Machine_TestResultsMidLine();
                        recordInserted = true;
                    }
                    if (dbRecord != null)
                    {
                        dbRecord.JobOrder_Machine_mapping_ID = machineMappingId;
                        dbRecord.Platen_time = model.PlatenTime;
                        dbRecord.Platen_seconds = model.PlatenSeconds;
                        dbRecord.Platen_sensor = model.PlatenSensor;
                        dbRecord.Platen_delta = model.PlatenDelta;
                        dbRecord.Injection_time = model.InjectionTime;
                        dbRecord.Injection_seconds = model.InjectionSeconds;
                        dbRecord.Injection_sensor = model.InjectionSensor;
                        dbRecord.Injection_delta = model.InjectionDelta;
                        dbRecord.Vaccum_time = model.VaccumTime;
                        dbRecord.Vaccum_seconds = model.VaccumSeconds;
                        dbRecord.Vaccum_sensor = model.VaccumSensor;
                        dbRecord.Vaccum_delta = model.VaccumDelta;
                        dbRecord.USB_Internal_sensor = model.USBInternalSensor;
                        dbRecord.USB_Internal_delta = model.USBInternalDelta;
                        dbRecord.USB_External_sensor = model.USBExternalSensor;
                        dbRecord.USB_External_delta = model.USBExternalDelta;
                        dbRecord.Relative_Sensor = model.RelativeSensor;
                        dbRecord.Relative_Delta = model.RelativeDelta;
                        dbRecord.Drying_Process = model.DryingProcess;
                        dbRecord.Drying_Cycles = model.DryingCycles;
                        dbRecord.Test1_Name = string.IsNullOrEmpty(model.Test1Name) ? string.Empty : model.Test1Name.Trim();
                        dbRecord.Test1_Verified = model.Test1Verified;
                        dbRecord.Test2_Name = string.IsNullOrEmpty(model.Test2Name) ? string.Empty : model.Test2Name.Trim();
                        dbRecord.Test2_Verified = model.Test2Verified;
                        dbRecord.Test3_Name = string.IsNullOrEmpty(model.Test3Name) ? string.Empty : model.Test3Name.Trim();
                        dbRecord.Test3_Verified = model.Test3Verified;

                        if (recordInserted)
                        {
                            dbRecord.Created_by = currentUserId;
                            dbRecord.Created_Date = DateTime.Now;
                            dbContext.JobOrder_Machine_TestResultsMidLine.Add(dbRecord);
                        }
                        else
                        {
                            dbRecord.Modified_by = currentUserId;
                            dbRecord.Modified_Date = DateTime.Now;
                        }
                        dbContext.SaveChanges();

                        // If all are radio values true, update MachineMapping table with Passed
                        if (MidLineVerifyAllTrue(dbRecord))
                        {
                            isMachinePass = true;
                        }
                        var machineMapTbl = dbContext.JobOrder_Machine_Mapping.FirstOrDefault(c => c.JobOrder_Machine_mapping_ID == machineMappingId);
                        if (machineMapTbl != null)
                        {
                            machineMapTbl.MidlineTesting_Status = isMachinePass;
                            // As Mid-line test FAILED, if UL test status is not PENDING (NULL) & not PASSED. Mark it as FAILED too.
                            if (!isMachinePass && machineMapTbl.ULLineTesting_Status != null && machineMapTbl.ULLineTesting_Status == true)
                            {
                                machineMapTbl.ULLineTesting_Status = false;
                                // As Mid-line test FAILED, if Final test status is not PENDING (NULL) & not PASSED. Mark it as FAILED too.
                                if (machineMapTbl.FinalTesting_Status != null && machineMapTbl.FinalTesting_Status == true)
                                {
                                    machineMapTbl.FinalTesting_Status = false;
                                }
                            }

                            // Set In-Testing, once the testing started for the machine. Ir-respective of Pass/Fail, amrk it as In-Testing.
                            machineMapTbl.Machine_Status_Id = dbContext.JobOrder_Status.FirstOrDefault(c => c.Status_Name == OrderStatusType.InTesting).JobOrder_Status_Id;

                            machineMapTbl.Modified_by = currentUserId;
                            machineMapTbl.Modified_Date = DateTime.Now;
                            dbContext.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isMachinePass = false;
                throw;
            }
            return isMachinePass;
        }
        private static bool MidLineVerifyAllTrue(JobOrder_Machine_TestResultsMidLine dbRecord)
        {
            bool machinePassed = false;

            if (dbRecord.Platen_time != null && dbRecord.Platen_sensor != null && dbRecord.Injection_time != null && dbRecord.Injection_sensor != null &&
            dbRecord.Vaccum_time != null && dbRecord.Vaccum_sensor != null && dbRecord.USB_Internal_sensor != null && dbRecord.USB_External_sensor != null &&
            dbRecord.Relative_Sensor != null && dbRecord.Drying_Process != null &&
            (bool)dbRecord.Platen_time && (bool)dbRecord.Platen_sensor && (bool)dbRecord.Injection_time && (bool)dbRecord.Injection_sensor &&
            (bool)dbRecord.Vaccum_time && (bool)dbRecord.Vaccum_sensor && (bool)dbRecord.USB_Internal_sensor && (bool)dbRecord.USB_External_sensor &&
            (bool)dbRecord.Relative_Sensor && (bool)dbRecord.Drying_Process)
            {
                machinePassed = true;
            }
            // dbRecord.Test1_Verified != null && dbRecord.Test2_Verified != null && dbRecord.Test3_Verified != null &&
            // && (bool)dbRecord.Test1_Verified && (bool)dbRecord.Test2_Verified && (bool)dbRecord.Test3_Verified
            if (machinePassed && !string.IsNullOrEmpty(dbRecord.Test1_Name))
            {
                machinePassed = (bool)dbRecord.Test1_Verified;
            }
            if (machinePassed && !string.IsNullOrEmpty(dbRecord.Test2_Name))
            {
                machinePassed = (bool)dbRecord.Test2_Verified;
            }
            if (machinePassed && !string.IsNullOrEmpty(dbRecord.Test3_Name))
            {
                machinePassed = (bool)dbRecord.Test3_Verified;
            }
            return machinePassed;
        }
        private bool UpdateULTest(int testStageId, TestingModel model, Guid currentUserId)
        {
            bool isMachinePass = false;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    bool recordInserted = false;
                    int machineMappingId = (int)model.MachineMappingId;
                    JobOrder_Machine_TestResultsUL dbRecord = dbContext.JobOrder_Machine_TestResultsUL.FirstOrDefault(c => c.JobOrder_Machine_mapping_ID == machineMappingId);
                    // if record exist, will use object to update.
                    // if record doesn't exist, create insert object.
                    if (dbRecord == null)
                    {
                        dbRecord = new JobOrder_Machine_TestResultsUL();
                        recordInserted = true;
                    }
                    if (dbRecord != null)
                    {
                        dbRecord.JobOrder_Machine_mapping_ID = machineMappingId;
                        dbRecord.Dielectric_test = model.DielectricTest;
                        dbRecord.Dielectric_Voltage = model.DielectricVoltage;
                        dbRecord.Dielectric_Current = model.DielectricCurrent;
                        dbRecord.Dielectric_Time = model.DielectricTime;
                        dbRecord.Ground_Test = model.GroundTest;
                        dbRecord.Ground_Resistance = model.GroundResistance;
                        if (recordInserted)
                        {
                            dbRecord.Created_by = currentUserId;
                            dbRecord.Created_Date = DateTime.Now;
                            dbContext.JobOrder_Machine_TestResultsUL.Add(dbRecord);
                        }
                        else
                        {
                            dbRecord.Modified_by = currentUserId;
                            dbRecord.Modified_Date = DateTime.Now;
                        }
                        dbContext.SaveChanges();

                        // If all radio values are true, update MachineMapping table with PASSED
                        if (dbRecord.Dielectric_test != null && dbRecord.Ground_Test != null &&
                            (bool)dbRecord.Dielectric_test && (bool)dbRecord.Ground_Test)
                        {
                            isMachinePass = true;
                        }
                        var machineMapTbl = dbContext.JobOrder_Machine_Mapping.FirstOrDefault(c => c.JobOrder_Machine_mapping_ID == machineMappingId);
                        if (machineMapTbl != null)
                        {
                            machineMapTbl.ULLineTesting_Status = isMachinePass;

                            // As UL test FAILED, if Final test status is not PENDING (NULL) & not PASSED. Mark it as Failed too.
                            if (machineMapTbl.FinalTesting_Status != null && machineMapTbl.FinalTesting_Status == true)
                            {
                                machineMapTbl.FinalTesting_Status = false;
                            }
                            machineMapTbl.Modified_by = currentUserId;
                            machineMapTbl.Modified_Date = DateTime.Now;

                            dbContext.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isMachinePass = false;
                throw;
            }
            return isMachinePass;
        }
        private bool UpdateFinalTest(int testStageId, TestingModel model, Guid currentUserId)
        {
            bool isMachinePass = false;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    bool recordInserted = false;
                    int machineMappingId = (int)model.MachineMappingId;
                    JobOrder_Machine_TestResultsFinal dbRecord = dbContext.JobOrder_Machine_TestResultsFinal.FirstOrDefault(c => c.JobOrder_Machine_mapping_ID == machineMappingId);
                    // if record exist, will use object to update.
                    // if record doesn't exist, create insert object.
                    if (dbRecord == null)
                    {
                        dbRecord = new JobOrder_Machine_TestResultsFinal();
                        recordInserted = true;
                    }
                    if (dbRecord != null)
                    {
                        dbRecord.JobOrder_Machine_mapping_ID = machineMappingId;
                        dbRecord.Final_Assembly = model.FinalAssembly;
                        dbRecord.Final_Process_Id = model.FinalProcessId.Trim();
                        if (recordInserted)
                        {
                            dbRecord.Created_by = currentUserId;
                            dbRecord.Created_Date = DateTime.Now;
                            dbContext.JobOrder_Machine_TestResultsFinal.Add(dbRecord);
                        }
                        else
                        {
                            dbRecord.Modified_by = currentUserId;
                            dbRecord.Modified_Date = DateTime.Now;
                        }
                        dbContext.SaveChanges();

                        
                        // If all radio values are true, update MachineMapping table with PASSED
                        if (dbRecord.Final_Assembly != null && (bool)dbRecord.Final_Assembly)
                        {
                            isMachinePass = true;
                        }
                        var machineMapTbl = dbContext.JobOrder_Machine_Mapping.FirstOrDefault(c => c.JobOrder_Machine_mapping_ID == machineMappingId);
                        if (machineMapTbl != null)
                        {
                            machineMapTbl.FinalTesting_Status = isMachinePass;
                            machineMapTbl.Modified_by = currentUserId;
                            machineMapTbl.Modified_Date = DateTime.Now;

                            // If Final test also passed and machine location is not empty/null,
                            // then change machine status from In-Testing to In-Shipping.
                            if (isMachinePass && machineMapTbl.JobOrder_Location_id != null)
                            {
                                machineMapTbl.Machine_Status_Id = dbContext.JobOrder_Status.FirstOrDefault(c => c.Status_Name == OrderStatusType.InShipping).JobOrder_Status_Id;
                            }

                            dbContext.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isMachinePass = false;
                throw;
            }
            return isMachinePass;
        }

    }
}
