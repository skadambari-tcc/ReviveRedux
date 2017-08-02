using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revive.Redux.Entities;
using Revive.Redux.Repositories;
using Revive.Redux.Common;



namespace Revive.Redux.Repositories
{
    public class ManageMachineSpecsRepository : IManageMachineSpecsRepository
    {
        //function for get MacineSpecs
        public IEnumerable<MachineSpecsModel> GetMachineSpecs()
        {
            var result = new List<MachineSpecsModel>();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    result = (from lstspecs in dbContext.MachineSpecs

                              //select new model
                              select new MachineSpecsModel
                              {
                                  MachineSpec_ID = lstspecs.MachineSpec_ID,
                                  Generation = string.IsNullOrEmpty(lstspecs.Generation) ? string.Empty : lstspecs.Generation,
                                  Pump = string.IsNullOrEmpty(lstspecs.Pump) ? string.Empty : lstspecs.Pump,
                                  Debug_Parameter_default_value =
                                      string.IsNullOrEmpty(lstspecs.Debug_Parameter_default_value)
                                          ? string.Empty
                                          : lstspecs.Debug_Parameter_default_value,
                                  Customfield1 =
                                      string.IsNullOrEmpty(lstspecs.Customfield1) ? string.Empty : lstspecs.Customfield1,
                                  Customfield2 =
                                      string.IsNullOrEmpty(lstspecs.Customfield2) ? string.Empty : lstspecs.Customfield2,
                                  Customfield3 =
                                      string.IsNullOrEmpty(lstspecs.Customfield3) ? string.Empty : lstspecs.Customfield3,
                                  Customfield4 =
                                      string.IsNullOrEmpty(lstspecs.Customfield4) ? string.Empty : lstspecs.Customfield4,
                                  Customfield5 =
                                      string.IsNullOrEmpty(lstspecs.Customfield5) ? string.Empty : lstspecs.Customfield5,
                                  PCB_Version =
                                      string.IsNullOrEmpty(lstspecs.PCB_Version) ? string.Empty : lstspecs.PCB_Version,
                                  INjection_Heater =
                                      string.IsNullOrEmpty(lstspecs.INjection_Heater)
                                          ? string.Empty
                                          : lstspecs.INjection_Heater,
                                  PowerSupply_1 =
                                      string.IsNullOrEmpty(lstspecs.PowerSupply_1) ? string.Empty : lstspecs.PowerSupply_1,
                                  PowerSupply_2 =
                                      string.IsNullOrEmpty(lstspecs.PowerSupply_2) ? string.Empty : lstspecs.PowerSupply_2,
                                  Valve_1 = string.IsNullOrEmpty(lstspecs.Valve_1) ? string.Empty : lstspecs.Valve_1,
                                  Valve_2 = string.IsNullOrEmpty(lstspecs.Valve_2) ? string.Empty : lstspecs.Valve_2,
                                  Valve_3 = string.IsNullOrEmpty(lstspecs.Valve_3) ? string.Empty : lstspecs.Valve_3,
                                  Platen_heater =
                                      string.IsNullOrEmpty(lstspecs.Platen_heater) ? string.Empty : lstspecs.Platen_heater,
                                  Created_by = lstspecs.Created_by,
                                  Created_Date = lstspecs.Created_Date,
                                  Modified_by = lstspecs.Modified_by,
                                  Modified_Date = lstspecs.Modified_Date,
                                  software_version =
                                      string.IsNullOrEmpty(
                                          dbContext.MasterData_Config_definitions.Where(
                                              a =>
                                                  a.MasterData_Type == "Software" &&
                                                  a.MasterData_Type_ID == lstspecs.Firmware_Version)
                                              .Select(a => a.MasterData_Value)
                                              .FirstOrDefault())
                                          ? string.Empty
                                          : dbContext.MasterData_Config_definitions.Where(
                                              a =>
                                                  a.MasterData_Type == "Software" &&
                                                  a.MasterData_Type_ID == lstspecs.Firmware_Version)
                                              .Select(a => a.MasterData_Value)
                                              .FirstOrDefault()


                                  ,


                              }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }

        public bool AddEditMachineSpec(MachineSpecsModel mechspec)
        {
            var flag = false;
            var specs = new MachineSpec();
            var isNew = false;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {

                    if (mechspec.MachineSpec_ID == 0) // in case of new Machine
                    {
                        mechspec.Created_Date = DateTime.Now;

                        isNew = true;
                    }
                    else // in case of edit user
                    {
                        specs = dbContext.MachineSpecs.FirstOrDefault(us => us.MachineSpec_ID == mechspec.MachineSpec_ID);
                        specs.Created_Date = DateTime.Now;
                    }

                    specs = MapMachinespecs(mechspec, specs);



                    if (isNew) // in case of new user
                    {

                        // Restrict adding duplicate users
                        var objCustomer =
                            dbContext.MachineSpecs.FirstOrDefault(cond => cond.MachineSpec_ID == mechspec.MachineSpec_ID);
                        if (objCustomer == null)
                            dbContext.MachineSpecs.Add(specs);
                    }

                    dbContext.SaveChanges();
                    flag = true;

                }



            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    System.Diagnostics.Debug.WriteLine(
                        "Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
            return flag;
        }


        // Adding template with Parameter

        public bool AddTemplate(List<Debug_Templates_DefinitionModel> debugTemplatesDefinitions)
        {
            var flag = false;
            var isNew = false;

            var nTemplateId = -1;
            var sTemplateName = string.Empty;
            Guid? CreatedUser = new Guid();
            var templateNameExist = false;
            DateTime? DateCreated = new DateTime();

            if (debugTemplatesDefinitions.Count > 0)
            {
                nTemplateId = debugTemplatesDefinitions[0].Template_ID;
                sTemplateName = debugTemplatesDefinitions[0].Template_Name;

            }

            if (nTemplateId == 0)
            {
                isNew = true;
            }

            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    // Check duplicate template in EDIT Case
                    if (nTemplateId > 0)
                    {
                        var edittemplatName =
                            dbContext.Debug_Templates.FirstOrDefault(
                                p => p.Template_name == sTemplateName.Trim() && p.Template_ID != nTemplateId);


                        if (edittemplatName != null)
                        {
                            templateNameExist = true;
                        }

                    }

                    else
                    {
                        var edittemplatName =
                            dbContext.Debug_Templates.FirstOrDefault(p => p.Template_name == sTemplateName.Trim());
                        if (edittemplatName != null)
                        {
                            templateNameExist = true;
                        }
                    }
                    // Start code for inserting & Updating

                    if (templateNameExist == false)
                    {
                        Debug_Templates objdebutTemplates = new Debug_Templates();
                        if (isNew == true)
                        {
                            objdebutTemplates.Template_name = debugTemplatesDefinitions[0].Template_Name;


                            foreach (Debug_Templates_DefinitionModel newTempl in debugTemplatesDefinitions)
                            {
                                Debug_Templates_Definition objtemplateDefinitions = new Debug_Templates_Definition();
                                objtemplateDefinitions.DebugParameter_Id = Convert.ToInt32(newTempl.DebugParameter_Id);
                                objtemplateDefinitions.Template_ID = objdebutTemplates.Template_ID;

                                objtemplateDefinitions.Modified_by = newTempl.ModifiedBy;
                                objtemplateDefinitions.Modified_Date = Common.CommonMethods.GetCurrentDate();
                                objtemplateDefinitions.Created_by = newTempl.CreatedBy;
                                objtemplateDefinitions.Created_Date = Common.CommonMethods.GetCurrentDate();

                                objdebutTemplates.Modified_by = newTempl.ModifiedBy;
                                objdebutTemplates.Modified_Date = Common.CommonMethods.GetCurrentDate();
                                objdebutTemplates.Created_by = newTempl.CreatedBy;
                                objdebutTemplates.Created_Date = Common.CommonMethods.GetCurrentDate();

                                dbContext.Debug_Templates_Definition.Add(objtemplateDefinitions);


                            }

                            dbContext.Debug_Templates.Add(objdebutTemplates);

                            dbContext.SaveChanges();
                            flag = true;
                        }
                        else
                        {
                            var exittempTemplate =
                                dbContext.Debug_Templates.FirstOrDefault(td => td.Template_ID == nTemplateId);

                            if (exittempTemplate != null)
                            {
                                exittempTemplate.Template_name = sTemplateName;
                                var record =
                                    dbContext.Debug_Templates_Definition.Where(cond => cond.Template_ID == nTemplateId);
                                if (record != null)
                                {

                                    foreach (var newTempl in record)
                                    {
                                        CreatedUser = newTempl.Created_by;
                                        DateCreated = newTempl.Created_Date;
                                        dbContext.Debug_Templates_Definition.Remove(newTempl);
                                    }

                                    foreach (Debug_Templates_DefinitionModel newTempl in debugTemplatesDefinitions)
                                    {
                                        Debug_Templates_Definition objtemplateDefinitions =
                                            new Debug_Templates_Definition();
                                        objtemplateDefinitions.DebugParameter_Id =
                                            Convert.ToInt32(newTempl.DebugParameter_Id);
                                        objtemplateDefinitions.Template_ID = nTemplateId;

                                        objtemplateDefinitions.Modified_by = newTempl.ModifiedBy;
                                        objtemplateDefinitions.Modified_Date = Common.CommonMethods.GetCurrentDate();
                                        objtemplateDefinitions.Created_by = CreatedUser;
                                        objtemplateDefinitions.Created_Date = DateCreated;

                                        exittempTemplate.Modified_by = newTempl.ModifiedBy;
                                        exittempTemplate.Modified_Date = Common.CommonMethods.GetCurrentDate();

                                        dbContext.Debug_Templates_Definition.Add(objtemplateDefinitions);
                                    }
                                }

                                dbContext.SaveChanges();
                                flag = true;
                            }




                        }

                        // End Check duplicate template in EDIT Case

                    }
                    else
                    {
                        flag = false;
                    }

                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            catch (Exception EX)
            {
                throw;
            }
            return flag;
        }


        private MachineSpec MapMachinespecs(MachineSpecsModel mechspec, MachineSpec specs)
        {


            specs.Debug_Parameter_default_value = mechspec.Debug_Parameter_default_value;
            specs.Generation = mechspec.Generation;
            specs.Created_by = mechspec.Created_by;
            specs.Created_Date = mechspec.Created_Date;
            specs.Customfield1 = mechspec.Customfield1; // for edit mode

            specs.Customfield2 = mechspec.Customfield2;
            specs.Customfield3 = mechspec.Customfield3;
            specs.Customfield4 = mechspec.Customfield4;
            specs.Customfield5 = mechspec.Customfield5;
            specs.INjection_Heater = mechspec.INjection_Heater;
            specs.MachineSpec_ID = mechspec.MachineSpec_ID;
            specs.Modified_by = mechspec.Modified_by;
            specs.Modified_Date = mechspec.Modified_Date;
            specs.PCB_Version = mechspec.PCB_Version;
            specs.Platen_heater = mechspec.Platen_heater;
            specs.PowerSupply_1 = mechspec.PowerSupply_1;
            specs.PowerSupply_2 = mechspec.PowerSupply_2;
            specs.Pump = mechspec.Pump;
            specs.Firmware_Version = Convert.ToInt16(mechspec.Firmware_Version);
            specs.Valve_1 = mechspec.Valve_1;
            specs.Valve_2 = mechspec.Valve_2;
            specs.Valve_3 = mechspec.Valve_3;
            specs.Created_Date = DateTime.Now;
            specs.Created_by = mechspec.Created_by;
            specs.Modified_by = mechspec.Modified_by;
            specs.Modified_Date = mechspec.Modified_Date;



            return specs;
        }


        private MasterData_Config_definitions SoftwareVersionMapping(ManageSoftwareVersionModel softver,
            MasterData_Config_definitions vers)
        {

            vers.MasterData_Type_ID = softver.MasterData_Type_ID;
            vers.MasterData_Value = softver.MasterData_Value;
            vers.MasterData_Type = "Software";
            if (softver.Custom_Field1 != "")
            {
                vers.Custom_Field1 = softver.Custom_Field1;

            }
            vers.CustomField2 = softver.CustomField2;
            vers.CustomField3 = softver.CustomField3;
            vers.CustomField4 = softver.CheckSumKey;

            vers.status = true;





            return vers;
        }


        public MachineSpecsModel GetMachineDetailsById(int mechId)
        {

            var result = new MachineSpecsModel();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var specs = (from MachineSpec x in dbContext.MachineSpecs.ToList()
                                 where x.MachineSpec_ID == Convert.ToInt16(mechId)
                                 select new MachineSpecsModel
                                 {
                                     //string.IsNullOrEmpty(x.Platen_heater) ? string.Empty : x.Platen_heater,
                                     MachineSpec_ID = x.MachineSpec_ID,
                                     INjection_Heater =
                                         string.IsNullOrEmpty(x.INjection_Heater) ? string.Empty : x.INjection_Heater,

                                     Generation = string.IsNullOrEmpty(x.Generation) ? string.Empty : x.Generation,
                                     Pump = string.IsNullOrEmpty(x.Pump) ? string.Empty : x.Pump,
                                     Customfield1 = string.IsNullOrEmpty(x.Customfield1) ? string.Empty : x.Customfield1,
                                     Customfield2 = string.IsNullOrEmpty(x.Customfield2) ? string.Empty : x.Customfield2,
                                     Customfield3 = string.IsNullOrEmpty(x.Customfield3) ? string.Empty : x.Customfield3,
                                     Customfield4 = string.IsNullOrEmpty(x.Customfield4) ? string.Empty : x.Customfield4,
                                     Customfield5 = string.IsNullOrEmpty(x.Customfield5) ? string.Empty : x.Customfield5,
                                     PCB_Version = string.IsNullOrEmpty(x.PCB_Version) ? string.Empty : x.PCB_Version,
                                     PowerSupply_1 = string.IsNullOrEmpty(x.PowerSupply_1) ? string.Empty : x.PowerSupply_1,
                                     PowerSupply_2 = string.IsNullOrEmpty(x.PowerSupply_2) ? string.Empty : x.PowerSupply_2,
                                     Valve_1 = string.IsNullOrEmpty(x.Valve_1) ? string.Empty : x.Valve_1,
                                     Valve_2 = string.IsNullOrEmpty(x.Valve_2) ? string.Empty : x.Valve_2,
                                     Valve_3 = string.IsNullOrEmpty(x.Valve_3) ? string.Empty : x.Valve_3,
                                     Platen_heater = string.IsNullOrEmpty(x.Platen_heater) ? string.Empty : x.Platen_heater,
                                     Firmware_Version =
                                         string.IsNullOrEmpty(Convert.ToString(x.Firmware_Version))
                                             ? string.Empty
                                             : Convert.ToString(x.Firmware_Version),
                                     software_version =
                                         string.IsNullOrEmpty(
                                             dbContext.MasterData_Config_definitions.Where(
                                                 a =>
                                                     a.MasterData_Type == "Software" &&
                                                     a.MasterData_Type_ID == x.Firmware_Version)
                                                 .Select(a => a.MasterData_Value)
                                                 .FirstOrDefault())
                                             ? string.Empty
                                             : dbContext.MasterData_Config_definitions.Where(
                                                 a =>
                                                     a.MasterData_Type == "Software" &&
                                                     a.MasterData_Type_ID == x.Firmware_Version)
                                                 .Select(a => a.MasterData_Value)
                                                 .FirstOrDefault()

                                 }).FirstOrDefault();

                    result = specs;

                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }


        public ManageSoftwareVersionModel GetSoftwareDetailsById(int sfId)
        {

            var result = new ManageSoftwareVersionModel();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var specs =
                        (from MasterData_Config_definitions x in dbContext.MasterData_Config_definitions.ToList()
                         where x.MasterData_Type_ID == Convert.ToInt16(sfId)
                         select new ManageSoftwareVersionModel
                         {
                             MasterData_Type_ID = x.MasterData_Type_ID,
                             MasterData_Value = x.MasterData_Value,
                             Custom_Field1 = x.Custom_Field1,
                             CustomField2 = x.CustomField2,
                             CustomField3 = x.CustomField3,
                             CheckSumKey = x.CustomField4,
                             //Created_by = x.Created_by,
                             Created_Date = x.Created_Date,
                             //Modified_by = x.Modified_by,
                             Modified_Date = x.Modified_Date
                         }).FirstOrDefault();

                    result = specs;

                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }


        public IEnumerable<ManageSoftwareVersionModel> GetSoftwareDetails()
        {
            var result = new List<ManageSoftwareVersionModel>();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    result = (from verdetail in dbContext.MasterData_Config_definitions.ToList()
                              where verdetail.status == true
                                    && verdetail.MasterData_Type.ToLower() == "Software".ToLower()

                              select new ManageSoftwareVersionModel
                              {

                                  MasterData_Value = verdetail.MasterData_Value,
                                  MasterData_Type_ID = verdetail.MasterData_Type_ID,
                                  MasterData_Type = "Software",
                                  Custom_Field1 = verdetail.Custom_Field1,
                                  CustomField2 = verdetail.CustomField2,
                                  CustomField3 = verdetail.CustomField3,
                                  CheckSumKey = verdetail.CustomField4

                              }).OrderBy(c => c.Created_Date).ToList();
                }
            }

            catch (Exception ex)
            {
                throw;
            }

            return result;

        }


        public IEnumerable<ManageDebugParaModel> GetManageDebugPara(int CustomerId, int locId, int SubsidiaryId,
            int SubAgentId)
        {
            var result = new List<ManageDebugParaModel>();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    result =
                        dbContext.usp_GetMachineTemplate(CustomerId, locId, SubsidiaryId, SubAgentId)
                            .Select(d => new ManageDebugParaModel()
                            {

                                Customer_Name = d.Customer_Name,
                                Location_Name = d.Location_Name,
                                MachineId = (int)d.MachineId,
                                Template_Name = d.Template_name,
                                TempalteId = d.TempalteId,
                                CustomerId = d.CustomerId,
                                LocationId = (int)d.LocationId,
                                MachineId_Val = d.machineId_Val

                            }).ToList();


                    //if (CustomerId == 0 && locId == 0)
                    //{
                    //    result = (from met in dbContext.MachineTemplateMappings
                    //              join loc in dbContext.Customers_Locations on met.LocationId equals loc.Location_ID
                    //              join cus in dbContext.Customers on met.CustomerId equals cus.Customer_ID
                    //              join temp in dbContext.Debug_Templates on met.TempalteId equals temp.Template_ID


                    //              select new ManageDebugParaModel
                    //              {
                    //                  Customer_Name = cus.Customer_Name,
                    //                  Location_Name = loc.Location_Name,
                    //                  MachineId = met.MachineId,
                    //                  Template_Name = temp.Template_name,
                    //                  TempalteId = temp.Template_ID



                    //              }).ToList();

                    //}
                    //else if (CustomerId != 0 && locId == 0)
                    //{

                    //    result = (from met in dbContext.MachineTemplateMappings
                    //              join loc in dbContext.Customers_Locations on met.LocationId equals loc.Location_ID
                    //              join cus in dbContext.Customers on met.CustomerId equals cus.Customer_ID
                    //              join temp in dbContext.Debug_Templates on met.TempalteId equals temp.Template_ID
                    //              //where (Id == 0 ? met.CustomerId == null : met.CustomerId == Id && locId == 0 ? loc.Location_ID == null : loc.Location_ID == locId)
                    //              where met.CustomerId == CustomerId

                    //              select new ManageDebugParaModel
                    //              {
                    //                  Customer_Name = cus.Customer_Name,
                    //                  Location_Name = loc.Location_Name,
                    //                  MachineId = met.MachineId,
                    //                  Template_Name = temp.Template_name,
                    //                  TempalteId = temp.Template_ID
                    //              }).ToList();
                    //}

                    //else if (CustomerId != 0 && locId != 0)
                    //{

                    //    result = (from met in dbContext.MachineTemplateMappings
                    //              join loc in dbContext.Customers_Locations on met.LocationId equals loc.Location_ID
                    //              join cus in dbContext.Customers on met.CustomerId equals cus.Customer_ID
                    //              join temp in dbContext.Debug_Templates on met.TempalteId equals temp.Template_ID
                    //              //where (Id == 0 ? met.CustomerId == null : met.CustomerId == Id && locId == 0 ? loc.Location_ID == null : loc.Location_ID == locId)
                    //              where met.LocationId == locId && met.CustomerId == CustomerId

                    //              select new ManageDebugParaModel
                    //              {
                    //                  Customer_Name = cus.Customer_Name,
                    //                  Location_Name = loc.Location_Name,
                    //                  MachineId = met.MachineId,
                    //                  Template_Name = temp.Template_name
                    //              }).ToList();
                    //}




                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;

        }


        public bool UpdtTempl(IEnumerable<ManageDebugParaModel> da, int templId)
        {
            var flag = false;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (templId != 0)
                    {



                        var toUpdate = da.Where(cond => cond.isSelected == true).ToList();

                        foreach (var tempToUpdate in toUpdate)
                        {
                            var exittempDebug =
                                dbContext.MachineTemplateMappings.FirstOrDefault(
                                    td => td.MachineId == tempToUpdate.MachineId);

                            if (exittempDebug != null)
                            {
                                exittempDebug.TempalteId = templId;
                            }
                            else
                            {
                                MachineTemplateMapping objnewmachineTemp = new MachineTemplateMapping();
                                objnewmachineTemp.CustomerId = tempToUpdate.CustomerId;
                                objnewmachineTemp.LocationId = tempToUpdate.LocationId;
                                objnewmachineTemp.MachineId = tempToUpdate.MachineId;
                                objnewmachineTemp.TempalteId = templId;
                                dbContext.MachineTemplateMappings.Add(objnewmachineTemp);

                            }



                            dbContext.SaveChanges();
                        }

                    }

                    flag = true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return flag;
        }



        public bool UpdSoftware(IEnumerable<MachineModel> data, int softversn)
        {
            var flag = false;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (softversn != 0)
                    {
                        var toUpdate = data;
                        foreach (var softToUpdate in toUpdate)
                        {
                            var currentver = dbContext.Machines.FirstOrDefault(td => td.Machine_Id == softToUpdate.MachineId);
                            currentver.Proposedversion_id = softversn;
                        }
                        dbContext.SaveChanges();
                    }

                    flag = true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return flag;
        }

        #region Deleting Machine Id from database table in case of Machine not shipped.

        // Delete from MachineTemplateMapping
        private void DeleteMappingIdFromMachineReplacedHistory(ReviveDBEntities dbContext, int MappingId)
        {
            var record =
                dbContext.MachineReplacedHistories.Where(td => td.JobOrder_Machine_mapping_ID == MappingId).ToList();
            if (record != null)
            {

                foreach (var RecordId in record)
                {

                    dbContext.MachineReplacedHistories.Remove(RecordId);
                }
            }





        }



        // Delete from MachineTemplateMapping
        private void DeleteMappingIdFromMachineTemplateMapping(ReviveDBEntities dbContext, int machineId)
        {
            var record = dbContext.MachineTemplateMappings.FirstOrDefault(td => td.MachineId == machineId);
            if (record != null)
            {
                dbContext.MachineTemplateMappings.Remove(record);
                // dbContext.SaveChanges();
            }

        }



        // Delete from tabletMachineActivity table
        private void DeleteMappingIdFromTabletMachineActivity(ReviveDBEntities dbContext, int machineId)
        {
            var record = dbContext.Tablet_App_Machine_Activity_Details.Where(td => td.Machine_Id == machineId).ToList();
            if (record != null)
            {
                foreach (var RecordId in record)
                {

                    dbContext.Tablet_App_Machine_Activity_Details.Remove(RecordId);
                }
            }

        }


        // Deleting Mapping id from JobOrder_Machine_TestResultsUL Table
        private void DeleteMappingIdFromTestResultsUL(ReviveDBEntities dbContext, int MappingId)
        {
            var record =
                dbContext.JobOrder_Machine_TestResultsUL.FirstOrDefault(
                    td => td.JobOrder_Machine_mapping_ID == MappingId);
            if (record != null)
            {
                dbContext.JobOrder_Machine_TestResultsUL.Remove(record);
                // dbContext.SaveChanges();
            }

        }

        // Deleting Mapping id from TestResultsMidLine Table
        private void DeleteMappingIdFromTestResultsMidLine(ReviveDBEntities dbContext, int MappingId)
        {
            var record =
                dbContext.JobOrder_Machine_TestResultsMidLine.FirstOrDefault(
                    td => td.JobOrder_Machine_mapping_ID == MappingId);
            if (record != null)
            {
                dbContext.JobOrder_Machine_TestResultsMidLine.Remove(record);
                // dbContext.SaveChanges();
            }

        }

        // Deleting Mapping id from JobOrder_Machine_TestResultsFinal Table
        private void DeleteMappingIdFromTestResultsFinal(ReviveDBEntities dbContext, int MappingId)
        {
            var record =
                dbContext.JobOrder_Machine_TestResultsFinal.FirstOrDefault(
                    td => td.JobOrder_Machine_mapping_ID == MappingId);
            if (record != null)
            {
                dbContext.JobOrder_Machine_TestResultsFinal.Remove(record);
                // dbContext.SaveChanges();
            }

        }

        // Deleting Mapping id from JobOrderActivity Table
        private void DeleteMappingIdJobOrderActivity(ReviveDBEntities dbContext, int MappingId)
        {
            var ActivityDetails =
                dbContext.Machine_activity_detail.Where(
                    td => td.Machine_Id == MappingId).ToList();
            if (ActivityDetails != null)
            {
                foreach (var RecordId in ActivityDetails)
                {

                    dbContext.Machine_activity_detail.Remove(RecordId);
                }
            }




        }




        // delete from JobOrder_Machine_Mapping
        private void DeleteMachineIdFromMapTable(ReviveDBEntities dbContext, int MachineId)
        {
            var MapDetails = dbContext.Machines.Where(td => td.Machine_Id == MachineId).ToList();
            if (MapDetails != null)
            {
                foreach (var mapdtl in MapDetails)
                {

                    DeleteMappingIdJobOrderActivity(dbContext, mapdtl.Machine_Id);
                    DeleteMappingIdFromMachineReplacedHistory(dbContext, mapdtl.Machine_Id);

                    dbContext.Machines.Remove(mapdtl);
                }
            }

        }

        // Delete from Main Table JobOrder_Machine_Id
        public bool DeleteMachine(IEnumerable<MachineModel> DeletedMachineData)
        {
            var flag = false;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    foreach (var Machine in DeletedMachineData)
                    {
                        DeleteMachineIdFromMapTable(dbContext, (int)Machine.MachineId);
                        //DeleteMappingIdFromMachineTemplateMapping(dbContext, (int)Machine.MachineId);
                        //DeleteMappingIdFromTabletMachineActivity(dbContext, (int)Machine.MachineId);
                        var JobOrderMachine =
                            dbContext.Machines.FirstOrDefault(c => c.Machine_Id == Machine.MachineId);
                        dbContext.Machines.Remove(JobOrderMachine);

                        var MachineLocation = dbContext.LocationMapMachines.FirstOrDefault(c => c.Machine_Id == Machine.MachineId);
                        if (MachineLocation != null)
                        {
                            dbContext.LocationMapMachines.Remove(MachineLocation);
                        }
                    }

                    dbContext.SaveChanges();

                    flag = true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return flag;
        }

        #endregion




        public bool AddEditSoftware(ManageSoftwareVersionModel softver)
        {
            var flag = false;
            var vers = new MasterData_Config_definitions();
            var isNew = false;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {

                    if (softver.MasterData_Type_ID == 0) // in case of new user
                    {
                        isNew = true;
                    }
                    else // in case of edit user
                    {
                        vers =
                            dbContext.MasterData_Config_definitions.FirstOrDefault(
                                us => us.MasterData_Type_ID == softver.MasterData_Type_ID);
                        vers.Modified_Date = DateTime.Now;
                        vers.Modified_by = softver.Modified_by;
                    }
                    vers = SoftwareVersionMapping(softver, vers);
                    if (isNew) // in case of new user
                    {
                        vers.Created_by = softver.Created_by;
                        vers.Created_Date = DateTime.Now;
                        // Restrict adding duplicate users
                        var objCustomer =
                            dbContext.MasterData_Config_definitions.FirstOrDefault(
                                cond => cond.MasterData_Type_ID == softver.MasterData_Type_ID);
                        if (objCustomer == null)
                            dbContext.MasterData_Config_definitions.Add(vers);
                    }
                    dbContext.SaveChanges();
                    flag = true;

                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return flag;
        }

        public IEnumerable<MachineModel> GetMappedMachinesByCustomer(int customer_Id, int location_Id, int SubsidiaryId,
            int SubAgentId)
        {
            try
            {


                IEnumerable<MachineModel> objMachineMapDetails;
                using (var dbContext = new ReviveDBEntities())
                {
                    objMachineMapDetails =
                        dbContext.usp_GetMachineMapByCustomer(customer_Id, location_Id, SubsidiaryId, SubAgentId)
                            .Select(d => new MachineModel()
                            {

                                MachineMappingId = d.JobOrder_Machine_mapping_ID,
                                MachineId = d.machineId,
                                CustomerName = d.customerName,
                                Location = d.locationName,
                                CurrentVersion_Id = d.currentVersionId,
                                SoftwareVersion = d.Softwareversoion,
                                ShippingId = "0000000000000000",
                                ProposedVersion = d.Proposedversoion,
                                MachineId_Val = d.machineId_Val


                            }).ToList();
                }



                return objMachineMapDetails;
            }
            catch (Exception ex)
            {
                throw;
            }


        }

        public IEnumerable<Debug_Templates_DefinitionModel> GetTemplatesPara()
        {
            var result = new List<Debug_Templates_DefinitionModel>();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    result = (from tmpldel in dbContext.Debug_Templates.ToList()


                              select new Debug_Templates_DefinitionModel
                              {

                                  Template_ID = tmpldel.Template_ID,
                                  Template_Name = tmpldel.Template_name,
                                  CreatedDate = tmpldel.Created_Date,
                                  TemplateCreatedBy =
                                      dbContext.Users.Where(a => a.User_ID == tmpldel.Created_by)
                                          .Select(a => a.FirstName + " " + (a.LastName == null ? "" : a.LastName))
                                          .FirstOrDefault()

                              }).ToList();


                }
            }

            catch (Exception ex)
            {
                throw;
            }

            return result;

        }


        public List<MachineModel> GetKillMachineByLocationAndCustomer(int nLocationId, int nCustomerId,
            int nSubsidiaryId, int nSubAgentId)
        {
            try
            {
                List<MachineModel> objKillMachineModel = new List<MachineModel>();
                using (var dbContext = new ReviveDBEntities())
                {
                    objKillMachineModel =
                        dbContext.usp_GetKillMachineByLocationAndCustomer(nCustomerId, nLocationId, nSubsidiaryId,
                            nSubAgentId).Select(d => new MachineModel()
                            {
                                MachineId = d.MachineId,
                                CustomerName = d.Customer_Name,
                                Location = d.Location_Name,
                                ActivityType = d.Action,
                                JobOrderMachineMappingID =
                                    (d.JobOrder_Summary_Act_Detail_ID != null)
                                        ? (int)d.JobOrder_Summary_Act_Detail_ID
                                        : 0,
                                Status = d.Status,
                                ModifiedDate = d.Modified_Date,
                                MachineId_Val = d.machineId_Val


                            }).ToList();
                }

                return objKillMachineModel;
            }
            catch (Exception ex)
            {
                throw;
            }
        }





        public List<Debug_Templates_DefinitionModel> GetTemplatesParaById(int Id)
        {

            var result = new List<Debug_Templates_DefinitionModel>();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {

                    if (Id != 0)
                    {



                        result = (
                            from met in dbContext.Debug_Templates_Definition
                            join loc in dbContext.Debug_Templates on met.Template_ID equals loc.Template_ID
                            where met.Template_ID == Id
                            join cus in dbContext.MasterData_Config_definitions on met.DebugParameter_Id equals
                                cus.MasterData_Type_ID

                            select new Debug_Templates_DefinitionModel
                            {
                                Template_ID = loc.Template_ID,
                                Template_Name = loc.Template_name,
                                DebugParameter_Id = cus.MasterData_Type_ID,
                                DebugParameterName = cus.MasterData_Value


                            }).ToList();

                    }





                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;


        }


        public MachineModel KillMachine(MachineModel objMachineModel)
        {
            try
            {
                objMachineModel.Result = false;
                //var objJobOrderMachineActivityDetail = new Machine_activity_detail();
                var objMachineActivityDetail = new Machine_activity_detail();
                //dbContext.MasterData_Config_definitions.Where(a => a.MasterData_Type == TabletMachineActivityStages.ReviveBoardfaults).Select(a => a.MasterData_Type_ID).FirstOrDefault();



                using (var dbContext = new ReviveDBEntities())
                {
                    int Machine_Kill_Id =
                        dbContext.MasterData_Config_definitions.Where(a => a.MasterData_Type == "MachineKilled")
                            .Select(a => a.MasterData_Type_ID)
                            .FirstOrDefault();
                    int Machine_Active_id =
                        dbContext.MasterData_Config_definitions.Where(a => a.MasterData_Type == "MachineActive")
                            .Select(a => a.MasterData_Type_ID)
                            .FirstOrDefault();


                    objMachineActivityDetail =
                     dbContext.Machine_activity_detail.FirstOrDefault(
                         cond => cond.Machine_Id == objMachineModel.MachineId);

                    //objJobOrderMachineActivityDetail =
                    //    dbContext.Machine_activity_detail.FirstOrDefault(
                    //        cond => cond.Machine_Id == objMachineModel.JobOrderMachineMappingID);
                    if (objMachineActivityDetail == null)
                    {
                        objMachineActivityDetail = new Machine_activity_detail();
                        objMachineActivityDetail.Machine_Id =
                            objMachineModel.MachineId;
                        objMachineActivityDetail.Activity_type_id = Machine_Kill_Id;
                        objMachineActivityDetail.Created_by = objMachineModel.CreatedBy;
                        objMachineActivityDetail.Created_Date = objMachineModel.CreatedDate;
                        objMachineActivityDetail.Modified_by = objMachineModel.ModifiedBy;
                        objMachineActivityDetail.Modified_Date = objMachineModel.ModifiedDate;
                        objMachineActivityDetail.Activity_Date = objMachineModel.ModifiedDate;
                        objMachineActivityDetail.Activity_result = "Kill";
                        dbContext.Machine_activity_detail.Add(objMachineActivityDetail);

                    }
                    else
                    {
                        if (objMachineActivityDetail.Activity_type_id == Machine_Active_id)
                        {
                            objMachineActivityDetail.Activity_result = "Kill";
                            objMachineActivityDetail.Activity_type_id = Machine_Kill_Id;
                        }
                        else
                        {
                            objMachineActivityDetail.Activity_result = "Active";
                            objMachineActivityDetail.Activity_type_id = Machine_Active_id;
                        }
                        objMachineActivityDetail.Modified_by = objMachineModel.ModifiedBy;
                        objMachineActivityDetail.Modified_Date = objMachineModel.ModifiedDate;
                        objMachineActivityDetail.Activity_Date = objMachineModel.ModifiedDate;
                    }

                    objMachineModel.Status = objMachineActivityDetail.Activity_result;
                    objMachineModel.ActivityTypeId = (objMachineActivityDetail.Activity_type_id != null)
                        ? (int)objMachineActivityDetail.Activity_type_id
                        : 0;
                    objMachineModel.MachineId_Val =
                        dbContext.Machines.Where(a => a.Machine_Id == objMachineModel.MachineId)
                            .Select(a => a.Machine_Id_Val)
                            .FirstOrDefault();
                    int statusId = GetStatuesID(ConstantEntities.StatuesKilled, dbContext);
                    UpdateMachine((int)objMachineModel.MachineId, statusId, dbContext);
                    SaveMachineHistoryData(dbContext, MapkillMachineToMachineHistory(objMachineModel));

                    dbContext.SaveChanges();
                    objMachineModel.Result = true;
                }
                return objMachineModel;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            catch (Exception ex)
            {


                throw;

            }

        }
        private EditMachineHistory MapkillMachineToMachineHistory(MachineModel MoveMachineObj)
        {
            EditMachineHistory MachineHistoryObj = new EditMachineHistory();
            MachineHistoryObj.MachineId = (int)MoveMachineObj.MachineId;
            if (MoveMachineObj.Status == "Active")
            {
                MachineHistoryObj.Notes = MoveMachineObj.MachineId_Val.ToString() + " Machine Recovered " + DateTime.Now.ToLongDateString();
            }
            else
            {
                MachineHistoryObj.Notes = MoveMachineObj.MachineId_Val.ToString() + " Machine Killed " + DateTime.Now.ToLongDateString();
            }

            MachineHistoryObj.TransactionTypeId = 0;
            MachineHistoryObj.CustomerId = (int)MoveMachineObj.Customer_Id;
            MachineHistoryObj.SubsidiaryId = MoveMachineObj.SubsidiaryId;
            MachineHistoryObj.SubAgentId = MoveMachineObj.SubAgentId;
            MachineHistoryObj.LocationId = (int)MoveMachineObj.Location_Id;

            return MachineHistoryObj;
        }
        public string GetActivityName(int nActivityTypeId)
        {
            try
            {
                string sGetActivityName = string.Empty;
                var objMasterData = new MasterData_Config_definitions();
                using (var dbContext = new ReviveDBEntities())
                {
                    objMasterData =
                        dbContext.MasterData_Config_definitions.FirstOrDefault(
                            cond => cond.MasterData_Type_ID == nActivityTypeId);

                    if (objMasterData != null)
                    {
                        sGetActivityName = objMasterData.MasterData_Value;


                    }
                }
                return sGetActivityName;
            }
            catch (Exception ex)
            {
                throw;

            }
        }

        public void UpdateArchiveStatus(int MasterData_Type_ID)
        {
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var existingRecord =
                        (from m in dbContext.MasterData_Config_definitions
                         where m.MasterData_Type_ID == MasterData_Type_ID
                         select m).FirstOrDefault();
                    if (existingRecord != null)
                    {
                        existingRecord.status = false;
                        dbContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public IEnumerable<DtoList> GetShippedMachIds(int customer_Id, int location_Id)
        {
            IEnumerable<DtoList> result = new List<DtoList>();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    result = dbContext.usp_GetShippedMachIds(customer_Id, location_Id).Select(m => new DtoList()
                    {
                        Id = (int)m.MachineId,
                        Text = m.Machine_Id_Val
                    }).ToList();

                }
            }
            catch (Exception ex)
            {
                throw;

            }
            return result;
        }

        public bool ReplaceMachine(int shippedMachId, int unassignedMachId, Guid currentUser, int OldMachineLocationId)
        {
            bool isSuccess = false;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    Machine jobOrdMachMapng =
                        dbContext.Machines.FirstOrDefault(m => m.Machine_Id == shippedMachId);

                    // Add record in History table                     
                    MachineReplacedHistory history = new MachineReplacedHistory();
                    history.OldMachineId = shippedMachId;
                    history.NewMachineId = unassignedMachId;
                    //  history.JobOrder_Machine_mapping_ID = jobOrdMachMapng.JobOrder_Machine_mapping_ID;
                    history.CreatedBy = currentUser;
                    history.CreatedDate = DateTime.Now;
                    dbContext.MachineReplacedHistories.Add(history);

                    // Set shipped machine in un-assigned list
                    //Machine shipped =
                    //    dbContext.Machines.FirstOrDefault(m => m.Machine_Id == shippedMachId);
                    jobOrdMachMapng.IsAssigned = false;
                    jobOrdMachMapng.IsShipped = false;
                    // shipped.Location_Id = OldMachineLocationId;
                    // Add by KD For Updating location unUsed machine as per client point on dated 30-SEP-2015

                    // Set unassigned machine in Assigned list
                    Machine unassigned =
                        dbContext.Machines.FirstOrDefault(m => m.Machine_Id == unassignedMachId);
                    unassigned.IsAssigned = true;
                    unassigned.IsShipped = true;

                    // Update Machine Id of Existing to replaced Machine Id
                    LocationMapMachine machineLocation = dbContext.LocationMapMachines.FirstOrDefault(m => m.Machine_Id == shippedMachId);
                    machineLocation.Machine_Id = unassignedMachId;




                    jobOrdMachMapng.Modified_by = currentUser;
                    jobOrdMachMapng.Modified_Date = DateTime.Now;
                    if (jobOrdMachMapng.IsMove != null && (bool)jobOrdMachMapng.IsMove)
                    {
                        var movedHistoryTbl = dbContext.MachineMovedHistories.Where(h => h.MachineId == shippedMachId);
                        foreach (var current in movedHistoryTbl)
                        {
                            current.MachineId = unassignedMachId;
                            current.ModifiedBy = currentUser;
                            current.ModifiedDate = DateTime.Now;
                        }
                    }
                    //for New Machine
                    int replacestatusId = GetStatuesID(ConstantEntities.StatuesReplaced, dbContext);
                    UpdateMachine(shippedMachId, replacestatusId, dbContext);
                    EditMachineHistory shippedmodel = new EditMachineHistory();
                    shippedmodel.MachineId = shippedMachId;
                    shippedmodel.Notes = "Machine Replaced Old one ";
                    SaveMachineHistoryData(dbContext, shippedmodel);

                    //For Old Machine
                    int stockstatusId = GetStatuesID(ConstantEntities.StatuesStock, dbContext);
                    UpdateMachine(unassignedMachId, stockstatusId, dbContext);
                    EditMachineHistory unassignedmodel = new EditMachineHistory();
                    unassignedmodel.MachineId = unassignedMachId;
                    unassignedmodel.Notes = "Machine Replaced New one ";
                    SaveMachineHistoryData(dbContext, unassignedmodel);

                    dbContext.SaveChanges();
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                throw;
            }
            return isSuccess;
        }




        public bool MoveMachine(MoveMachineModel model, Guid currentUser)
        {
            bool isSuccess = false;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    Machine jomm =
                        dbContext.Machines.FirstOrDefault(m => m.Machine_Id == model.ShippedMachId);
                    if (jomm != null)
                    {
                        MachineMovedHistory machine = new MachineMovedHistory();
                        machine.JobOrderHeaderId = 0;
                        machine.MachineId = model.ShippedMachId;
                        machine.JobOrderMachineMappingId = 0;
                        machine.CustomerIdFrom = model.CustomerIdFrom;
                        machine.SubsidiaryIdFrom = model.SubsidiaryIdFrom;
                        machine.AgentIdFrom = model.SubAgentIdFrom;
                        machine.LocationIdFrom = model.LocationIdFrom;
                        machine.CustomerIdTo = model.CustomerIdTo;
                        machine.SubsidiaryIdTo = model.SubsidiaryIdTo;
                        machine.AgentIdTo = model.SubAgentIdTo;
                        machine.LocationIdTo = model.LocationIdTo;
                        machine.CreatedBy = currentUser;
                        machine.CreatedDate = DateTime.Now;
                        machine.transactionTypeId = model.TransactionType;

                        // Update IsMoved to true
                        jomm.IsMove = true;
                        dbContext.MachineMovedHistories.Add(machine);
                        // Calling method to update mapped_location_id & mapped_location_date in table jobOrder_machine_id
                        // UpdateMachineMapDetails((int)model.ShippedMachId, (int)model.LocationIdTo, dbContext);
                        int statusId = GetStatuesID(ConstantEntities.StatuesMoved, dbContext);
                        UpdateLocationMapMachines((int)model.ShippedMachId, (int)model.LocationIdTo, currentUser, dbContext);
                        UpdateMachine((int)model.ShippedMachId, statusId, dbContext);
                        SaveMachineHistoryData(dbContext, MapMoveMachineToMachineHistory(model));
                        dbContext.SaveChanges();

                        isSuccess = true;
                    }
                }
            }
            catch (Exception)
            {
                isSuccess = false;
            }
            return isSuccess;
        }
        private int GetStatuesID(string status, ReviveDBEntities dbContext)
        {
            try
            {
                var RecordmachineId = dbContext.MasterData_Config_definitions.Where(c => c.MasterData_Type == ConstantEntities.MachineStatus && c.MasterData_Value == status).FirstOrDefault();
                if (RecordmachineId != null)
                {
                    return RecordmachineId.MasterData_Type_ID;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return 0;

        }
        private void UpdateMachine(int MachineId, int StatusId, ReviveDBEntities dbContext)
        {
            try
            {
                var RecordmachineId = dbContext.Machines.Where(c => c.Machine_Id == MachineId).FirstOrDefault();
                if (RecordmachineId != null)
                {
                    RecordmachineId.StatusId = StatusId;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private EditMachineHistory MapMoveMachineToMachineHistory(MoveMachineModel MoveMachineObj)
        {
            EditMachineHistory MachineHistoryObj = new EditMachineHistory();
            MachineHistoryObj.MachineId = (int)MoveMachineObj.ShippedMachId;
            MachineHistoryObj.Notes = "Move machine details updated on Dated " + DateTime.Now.ToLongDateString();
            MachineHistoryObj.TransactionTypeId = (int)MoveMachineObj.TransactionType;
            MachineHistoryObj.CustomerId = (int)MoveMachineObj.CustomerIdTo;
            MachineHistoryObj.SubsidiaryId = MoveMachineObj.SubsidiaryIdTo;
            MachineHistoryObj.SubAgentId = MoveMachineObj.SubAgentIdTo;
            MachineHistoryObj.LocationId = (int)MoveMachineObj.LocationIdTo;

            return MachineHistoryObj;
        }
        public void SaveMachineHistoryData(ReviveDBEntities dbContext, EditMachineHistory model)
        {

            MachineHistory MachineNewHistory = new MachineHistory();
            MachineNewHistory.Machine_ID = model.MachineId;
            MachineNewHistory.ReasonTypeId = 0;
            MachineNewHistory.StatusTypeId = 0;
            MachineNewHistory.Notes = model.Notes;
            MachineNewHistory.EventDate = DateTime.Now;
            MachineNewHistory.TransactoionTypeId = model.TransactionTypeId;
            MachineNewHistory.Customer_Id = model.CustomerId;
            MachineNewHistory.Subsidiary_Id = model.SubsidiaryId;
            MachineNewHistory.SubAgent_Id = model.SubAgentId;
            MachineNewHistory.Location_Id = model.LocationId;
            MachineNewHistory.ShippingLabelData = model.ShippinglabelData;
            dbContext.MachineHistories.Add(MachineNewHistory);


        }

        private void UpdateLocationMapMachines(int machineId, int locationId, Guid currentUser, ReviveDBEntities dbContext)
        {

            try
            {
                var RecordmachineId = dbContext.LocationMapMachines.Where(c => c.Machine_Id == machineId).FirstOrDefault();

                if (RecordmachineId != null)
                {
                    RecordmachineId.Modified_Date = System.DateTime.Now;
                    RecordmachineId.Modified_by = currentUser;
                    RecordmachineId.Location_id = locationId;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private void UpdateMachineMapDetails(int machineId, int locationId, ReviveDBEntities dbContext)
        {

            try
            {
                var RecordmachineId = dbContext.JobOrder_Machine_Id.Where(c => c.Machine_Id == machineId).FirstOrDefault();

                if (RecordmachineId != null)
                {
                    RecordmachineId.Mapped_Location_Date = System.DateTime.Now;
                    RecordmachineId.Mapped_Location_Id = locationId;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public Boolean CheckSoftwareVersion(String Name, int MasterData_Type_ID)
        {
            var Duplicateflag = true;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (MasterData_Type_ID != 0)
                    {
                        var record = dbContext.MasterData_Config_definitions.FirstOrDefault(cond => cond.MasterData_Value.ToLower() == Name.ToLower() && cond.MasterData_Type_ID != MasterData_Type_ID && cond.status == true);
                        if (record != null)
                        {
                            Duplicateflag = false;
                        }

                    }
                    else
                    {
                        var record = dbContext.MasterData_Config_definitions.FirstOrDefault(cond => cond.MasterData_Value.ToLower() == Name.ToLower() && cond.status == true);
                        if (record != null)
                        {
                            Duplicateflag = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return Duplicateflag;
        }
        #region Manage machine History Item no 267,268 & 269
        /// <summary>
        /// Get MachineHistory list for data binding 
        /// </summary>
        /// <param name="customer_Id"></param>
        /// <param name="location_Id"></param>
        /// <param name="SubsidiaryId"></param>
        /// <param name="SubAgentId"></param>
        /// <returns></returns>
        public IEnumerable<MachineHistoryLst> GetMachineHistoryLst(int customer_Id, int location_Id, int SubsidiaryId, int SubAgentId)
        {
            try
            {
                IEnumerable<MachineHistoryLst> objMachineMapDetails;
                using (var dbContext = new ReviveDBEntities())
                {
                    objMachineMapDetails =
                        dbContext.usp_GetMachineHistory(customer_Id, SubsidiaryId, SubAgentId, location_Id)
                            .Select(d => new MachineHistoryLst()
                            {
                                MachineId = (int)d.MachineId,
                                Location = d.Location,
                                MachineId_Val = d.MachineId_Val,
                                Status = "Active",
                                IsHistoryView = d.HistoryView > 0 ? true : false
                            }).ToList();
                }
                return objMachineMapDetails;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// get Machine History details  by machine Id
        /// </summary>
        /// <param name="MachineId"></param>
        /// <returns></returns>
        public EditMachineHistory GetMachineHistoryByMachineId(int MachineId)
        {
            try
            {

                EditMachineHistory EditMachineHistoryObj = new EditMachineHistory();
                using (var dbContext = new ReviveDBEntities())
                {
                    var MachineId_Val = dbContext.Machines.Where(x => x.Machine_Id == MachineId).Select(x => x.Machine_Id_Val).FirstOrDefault();
                    string ManufacturerCode = "";
                    string ManufacturerName = "";
                    string SoftwareVersionName = "";
                    if (MachineId_Val != null)
                    {
                        ManufacturerCode = MachineId_Val.Substring(3, 1).ToString();
                        ManufacturerName = dbContext.Manufacturers.Where(x => x.Manufacturer_Ref_Code == ManufacturerCode).Select(x => x.Manufacturer_Name).FirstOrDefault();

                    }
                    var softwarerVersionId = dbContext.JobOrder_Machine_Mapping.Where(x => x.MachineId == MachineId).Select(c => c.Softwareversion_Id).FirstOrDefault();

                    if (softwarerVersionId != null)
                    {
                        SoftwareVersionName = dbContext.MasterData_Config_definitions.Where(x => x.MasterData_Type_ID == softwarerVersionId).Select(c => c.MasterData_Value).FirstOrDefault();

                    }

                    var objMachine = dbContext.usp_GetMachineHistoryCustomerDetailsByMachineId(MachineId).FirstOrDefault();
                    if (objMachine != null)
                    {
                        EditMachineHistoryObj.CustomerId = Convert.ToInt32(objMachine.CustomerId);
                        EditMachineHistoryObj.SubsidiaryId = Convert.ToInt32(objMachine.Subsidiary_ID);
                        EditMachineHistoryObj.SubAgentId = Convert.ToInt32(objMachine.SubAgent_ID);
                        EditMachineHistoryObj.LocationId = Convert.ToInt32(objMachine.LocationId);
                        EditMachineHistoryObj.TransactionTypeId = Convert.ToInt32(objMachine.TransactionTypeId);
                    }

                    // Assinging value in return object.
                    EditMachineHistoryObj.MachineId_Val = MachineId_Val;
                    EditMachineHistoryObj.LastReportedSoftwareVersion = SoftwareVersionName;
                    EditMachineHistoryObj.Manufacturer = ManufacturerName;

                }
                return EditMachineHistoryObj;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IEnumerable<DtoList> GetMachineHistoryConfigTypeByValue(string ConfigType, string ConfigHistoryType)
        {
            try
            {
                var collection = new List<DtoList>();
                using (var dbContext = new ReviveDBEntities())
                {
                    var lstUserTypes = (from d in dbContext.MachineHistory_Configuration
                                        join mast in dbContext.MasterData_Config_definitions on d.ConfigureTypeId equals mast.MasterData_Type_ID
                                        where mast.MasterData_Value == ConfigType && mast.MasterData_Type == ConfigHistoryType
                                        && d.Status == true
                                        select new DtoList
                                        {
                                            Id = d.Id,
                                            Text = d.ConfiguredValue
                                        }).OrderBy(z => z.Text).ToList();
                    collection.AddRange(lstUserTypes);
                }
                return collection;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // <summary>
        /// Get MachineHistory list for data binding for EditMachineHistory
        /// </summary>
        /// <param name="machineId"></param>       
        /// <returns></returns>
        public IEnumerable<EditMachineHistoryLst> GetEditMachineHistoryDetailsByMachineId(int machineId)
        {
            try
            {
                IEnumerable<EditMachineHistoryLst> objEditMachineHistDetails;
                using (var dbContext = new ReviveDBEntities())
                {
                    objEditMachineHistDetails =
                        dbContext.usp_GetMachineHistoryByMachineId(machineId)
                            .Select(d => new EditMachineHistoryLst()
                            {
                                Location = d.Location_Name,
                                EventDate = (DateTime)d.EventDate,
                                Customer = d.Customer_Name,
                                Subsidiary = d.SubsidiaryName,
                                SubAgent = d.SubAgentName,
                                Transaction_Type = d.TransactionType,
                                Notes_Lst = d.Notes,
                                Attachment_Path_Lst = d.Attachments_Path,
                                MachineHistoryId = d.MachineHistory_ID,
                                ShippingLabelData = d.ShippingLabelData
                            }).ToList();
                }
                return objEditMachineHistDetails;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string DeleteMachineHistoryById(int MachineHistoryId)
        {
            string strMsg = string.Empty;
            try
            {
                using (var dbcontext = new ReviveDBEntities())
                {
                    var MachineHistory = dbcontext.MachineHistories.Where(m => m.MachineHistory_ID == MachineHistoryId).FirstOrDefault();
                    if (MachineHistory != null)
                    {
                        if (MachineHistory.Machine_Documents.Count() > 0)
                        {
                            dbcontext.Machine_Documents.RemoveRange(MachineHistory.Machine_Documents);
                        }
                        dbcontext.MachineHistories.Remove(MachineHistory);
                        dbcontext.SaveChanges();
                        strMsg = "Success";
                    }
                    else
                    {
                        strMsg = "Fail";
                    }
                }
            }
            catch (Exception ex)
            {
                strMsg = "Fail";
                throw;
            }
            return strMsg;
        }

        public string DeleteConfigureMachineHistoryById(int ConfigureMachineHistoryID)
        {
            string strMsg = string.Empty;
            try
            {
                using (var dbcontext = new ReviveDBEntities())
                {
                    var MachineHistory = dbcontext.MachineHistory_Configuration.Where(m => m.Id == ConfigureMachineHistoryID).FirstOrDefault();
                    if (MachineHistory != null)
                    {
                        MachineHistory.Status = false;
                        dbcontext.SaveChanges();
                        strMsg = "Success";
                    }
                    else
                    {
                        strMsg = "Fail";
                    }
                }
            }
            catch (Exception ex)
            {
                strMsg = "Fail";
                throw;
            }
            return strMsg;
        }


        public bool AddMachineHistory(EditMachineHistory MachineHistModel)
        {
            var flag = false;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (MachineHistModel.MachineId != 0)
                    {
                        MachineHistory toUpdateMachineHistory = new MachineHistory();
                        toUpdateMachineHistory.Machine_ID = MachineHistModel.MachineId;
                        toUpdateMachineHistory.ReasonTypeId = MachineHistModel.ReasonType;
                        toUpdateMachineHistory.StatusTypeId = MachineHistModel.Status_MachineHistory;
                        toUpdateMachineHistory.Attachments_Path = MachineHistModel.Attachment_Path;
                        toUpdateMachineHistory.Notes = MachineHistModel.Notes;
                        toUpdateMachineHistory.EventDate = DateTime.Now;
                        toUpdateMachineHistory.TransactoionTypeId = MachineHistModel.TransactionTypeId;
                        toUpdateMachineHistory.Customer_Id = MachineHistModel.CustomerId;
                        toUpdateMachineHistory.Subsidiary_Id = MachineHistModel.SubsidiaryId;
                        toUpdateMachineHistory.SubAgent_Id = MachineHistModel.SubAgentId;
                        toUpdateMachineHistory.Location_Id = MachineHistModel.LocationId;



                        dbContext.MachineHistories.Add(toUpdateMachineHistory);

                        foreach (var item in MachineHistModel.MachineDocs)
                        {
                            var MachineDocs = new Machine_Documents();
                            MachineDocs.Machine_Doc_Name = item.Machine_Doc_Name;
                            MachineDocs.Machine_Doc_type = item.Machine_Doc_type;
                            MachineDocs.Doc_Path = item.Doc_Path;
                            MachineDocs.Created_Date = item.Created_Date;
                            MachineDocs.MachineHistory_ID = toUpdateMachineHistory.MachineHistory_ID;
                            MachineDocs.Machine_ID = MachineHistModel.MachineId;
                            dbContext.Machine_Documents.Add(MachineDocs);

                        }
                        dbContext.SaveChanges();
                    }
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return flag;
        }

        /// <summary>
        /// Get Machine Documents Records On the Basis of MachineHistoryId
        /// </summary>
        /// <param name="CustID"></param>
        /// <returns></returns>
        public IEnumerable<MachineDocumentsModel> GetMachineDocsList(int MachineId, int MachineHistoryId)
        {
            try
            {
                IEnumerable<MachineDocumentsModel> objMachineDocLst;
                using (var dbContext = new ReviveDBEntities())
                {
                    //var ObjMachineDocs = dbContext.MachineHistories.Where(m => m.MachineHistory_ID == MachineHistoryId).FirstOrDefault();
                    objMachineDocLst = dbContext.Machine_Documents.Where(m => m.MachineHistory_ID == MachineHistoryId && m.Machine_ID == MachineId).Select(m => new MachineDocumentsModel
                    {
                        Machine_Doc_Name = m.Machine_Doc_Name,
                        Machine_ID = m.Machine_ID,
                        Machine_Doc_type = m.Machine_Doc_type,
                        Created_Date = m.Created_Date
                    }).ToList();

                }

                return objMachineDocLst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Get Details of Machine Documents
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public MachineDocumentsModel GetMachineDocumentDetailById(MachineDocumentsModel Obj)
        {
            MachineDocumentsModel returnRecord = new MachineDocumentsModel();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if ((Obj.Machine_ID) != 0)
                    {

                        var record = dbContext.Machine_Documents.FirstOrDefault(cond => cond.Machine_ID == Obj.Machine_ID && cond.Machine_Doc_Name == Obj.Machine_Doc_Name);
                        if (record != null)
                        {
                            returnRecord.Machine_ID = record.Machine_ID;
                            returnRecord.Machine_Doc_Name = record.Machine_Doc_Name;
                            returnRecord.Machine_Doc_type = record.Machine_Doc_type;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return returnRecord;

        }


        #endregion


        #region ConfigureMachineHistory

        public List<ConfigureMachineHistoryModel> GetConfigureMachineHistory()
        {

            var result = new List<ConfigureMachineHistoryModel>();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var ConfigMacHist = (from ConfigMachineHist in dbContext.MachineHistory_Configuration
                                         join masterConfigdata in dbContext.MasterData_Config_definitions
                                         on ConfigMachineHist.ConfigureTypeId equals masterConfigdata.MasterData_Type_ID
                                         where ConfigMachineHist.Status == true
                                         select new ConfigureMachineHistoryModel
                                         {
                                             ConfigureMachineHistoryId = ConfigMachineHist.Id,
                                             ConfiguredValue = ConfigMachineHist.ConfiguredValue,
                                             ConfigureTypeId = (int)ConfigMachineHist.ConfigureTypeId,
                                             ConfigureTypeValue = masterConfigdata.MasterData_Value,
                                             Status = ConfigMachineHist.Status != null ? (bool)ConfigMachineHist.Status : false
                                         }).ToList();

                    result = ConfigMacHist;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        public ConfigureMachineHistoryModel GetConfigureMachineHistoryById(int Id)
        {

            var result = new ConfigureMachineHistoryModel();
            try
            {

                using (var dbContext = new ReviveDBEntities())
                {
                    if (Id > 0)
                    {
                        var ConfigMacHist = (from ConfigMachineHist in dbContext.MachineHistory_Configuration
                                             where ConfigMachineHist.Id == Id
                                             select new ConfigureMachineHistoryModel
                                             {
                                                 ConfigureMachineHistoryId = ConfigMachineHist.Id,
                                                 ConfiguredValue = ConfigMachineHist.ConfiguredValue,
                                                 ConfigureTypeId = (int)ConfigMachineHist.ConfigureTypeId,
                                                 Status = ConfigMachineHist.Status != null ? (bool)ConfigMachineHist.Status : false
                                             }).FirstOrDefault();

                        result = ConfigMacHist;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// Add and update ConfigureMachineHistory
        /// </summary>
        /// <param name="ConfigureMachineHistory"></param>
        /// <returns></returns>
        public bool AddEditConfigureMachineHistory(ConfigureMachineHistoryModel ConfigMachineHisModel, Guid CurrentUser)
        {
            var flag = false;
            var ConfigMachineHis = new MachineHistory_Configuration();
            var isNew = false;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (ConfigMachineHisModel.ConfigureMachineHistoryId == 0)
                    {
                        isNew = true;
                    }
                    else // in case of edit user
                    {
                        ConfigMachineHis = dbContext.MachineHistory_Configuration.FirstOrDefault(us => us.Id == ConfigMachineHisModel.ConfigureMachineHistoryId);
                    }

                    ConfigMachineHis = MapConfigureMachine(ConfigMachineHisModel, ConfigMachineHis);

                    if (isNew) // in case of new user
                    {
                        // Restrict adding duplicate ConfigureHistory
                        var objConfigureHistory =
                            dbContext.MachineHistory_Configuration.FirstOrDefault(cond => cond.Id == ConfigMachineHisModel.ConfigureMachineHistoryId);
                        if (objConfigureHistory == null)
                            dbContext.MachineHistory_Configuration.Add(ConfigMachineHis);
                        ConfigMachineHis.Created_Date = DateTime.Now;
                    }

                    dbContext.SaveChanges();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return true;
        }


        private MachineHistory_Configuration MapConfigureMachine(ConfigureMachineHistoryModel ConfigMachineHisModel, MachineHistory_Configuration MachineHistory_Congfig)
        {
            MachineHistory_Congfig.ConfiguredValue = ConfigMachineHisModel.ConfiguredValue;
            MachineHistory_Congfig.ConfigureTypeId = ConfigMachineHisModel.ConfigureTypeId;
            MachineHistory_Congfig.Status = true;
            return MachineHistory_Congfig;
        }

        public bool CheckDuplicateConfigureHistoryName(string ConfiguredValue, int ConfigureTypeId, int ConfigureHistoryId)
        {
            try
            {
                bool isExists = false;
                using (var dbContext = new ReviveDBEntities())
                {

                    var allCustName = new List<int>();
                    if (ConfigureTypeId != 0)
                    {
                        allCustName = (from mhc in dbContext.MachineHistory_Configuration where mhc.ConfiguredValue.ToLower() == ConfiguredValue.ToLower() && mhc.Id != ConfigureHistoryId && mhc.ConfigureTypeId == ConfigureTypeId select (int)mhc.ConfigureTypeId).ToList();
                    }
                    else
                        allCustName = (from mhc in dbContext.MachineHistory_Configuration where mhc.ConfiguredValue.ToLower() == ConfiguredValue.ToLower() select (int)mhc.ConfigureTypeId).ToList();
                    if (allCustName.Any())
                        isExists = true;
                }
                return isExists;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion



    }
}
