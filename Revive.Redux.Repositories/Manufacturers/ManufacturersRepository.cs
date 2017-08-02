using System;
using System.Collections.Generic;
using System.Linq;
using Revive.Redux.Entities;
using Revive.Redux.Common;
using System.Web;

namespace Revive.Redux.Repositories
{
    public class ManufacturersRepository : IManufacturersRepository
    {
        public ManufacturersRepository()
        {
        }

        /// <summary>
        /// Get list of All Manufacturers
        /// </summary>
        /// <param name="pageAccessCode"></param>
        /// <returns></returns>
        public IEnumerable<ManufacturersModel> GetAllMFs(string pageAccessCode)
        {
            try
            {
                IEnumerable<ManufacturersModel> manufacturers = null;
                using (var dbContext = new ReviveDBEntities())
                {
                    manufacturers = dbContext.Manufacturers
                        .ToList()
                        .Select(d => new ManufacturersModel()
                    {
                        Manufacturer_Id = d.Manufacturer_Id,
                        Manufacturer_Name = d.Manufacturer_Name,
                        Address = HttpUtility.HtmlDecode(d.Address),
                        Phone_Number = d.Phone_Number,
                        Company_Website = d.Company_Website,
                        PM_Name = d.PM_Name,
                        PM_Email = d.PM_Email,
                        PM_Office_Phone = d.PM_Office_Phone,
                        PM_Mobile = d.PM_Mobile,
                        Status = d.Status,
                        Created_by = d.Created_by,
                        Created_Date = d.Created_Date,
                        Modified_by = d.Modified_by,
                        Modified_Date = d.Modified_Date,
                        PageAccessCode = pageAccessCode
                    });
                }
                return manufacturers;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Get single Manufacturer by MF ID
        /// </summary>
        /// <param name="manufacId"></param>
        /// <returns></returns>
        public ManufacturersModel GetMFById(int manufacId)
        {
            try
            {
                ManufacturersModel manufacturer = new ManufacturersModel();
                using (var dbContext = new ReviveDBEntities())
                {
                    var result = (from Manufacturer dbResult in dbContext.Manufacturers
                                  where dbResult.Manufacturer_Id == manufacId
                                  select new ManufacturersModel
                                  {
                                      Manufacturer_Id = dbResult.Manufacturer_Id,
                                      Manufacturer_Name = dbResult.Manufacturer_Name,
                                      Address = dbResult.Address,
                                      Phone_Number = dbResult.Phone_Number,
                                      Company_Website = dbResult.Company_Website,
                                      PM_Name = dbResult.PM_Name,
                                      PM_Email = dbResult.PM_Email,
                                      PM_Office_Phone = dbResult.PM_Office_Phone,
                                      PM_Mobile = dbResult.PM_Mobile,
                                      Status = dbResult.Status,
                                      Created_by = dbResult.Created_by,
                                      Created_Date = dbResult.Created_Date,
                                      Modified_by = dbResult.Modified_by,
                                      Modified_Date = dbResult.Modified_Date,
                                      Manufacturer_Ref_Code = dbResult.Manufacturer_Ref_Code
                                  }).FirstOrDefault();
                    manufacturer = result;
                }
                return manufacturer;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Create new Manufacturer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool CreateMF(ManufacturersModel model)
        {
            bool isInserted = false;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    Manufacturer insert = new Manufacturer();
                    insert.Manufacturer_Name = model.Manufacturer_Name;
                    insert.Address = model.Address;
                    insert.Phone_Number = model.Phone_Number;
                    insert.Company_Website = model.Company_Website;
                    insert.PM_Name = model.PM_Name;
                    insert.PM_Email = model.PM_Email;
                    insert.PM_Office_Phone = model.PM_Office_Phone;
                    insert.PM_Mobile = model.PM_Mobile;
                    insert.Status = model.Status;
                    insert.Created_by = model.Created_by;
                    insert.Created_Date = DateTime.Now;
                    insert.Manufacturer_Ref_Code = model.Manufacturer_Ref_Code;
                    dbContext.Manufacturers.Add(insert);
                    dbContext.SaveChanges();
                    isInserted = true;
                }
            }
            catch (Exception ex)
            {
                isInserted = false;
                throw;
            }
            return isInserted;
        }

        /// <summary>
        /// Update details of a single Manufacturer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateMFDetails(ManufacturersModel model)
        {
            bool isUpdated = false;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var recordToUpdate = dbContext.Manufacturers.FirstOrDefault(c => c.Manufacturer_Id == model.Manufacturer_Id);
                    if (recordToUpdate != null)
                    {
                        recordToUpdate.Manufacturer_Name = model.Manufacturer_Name;
                        recordToUpdate.Address = model.Address;
                        recordToUpdate.Phone_Number = model.Phone_Number;
                        recordToUpdate.Company_Website = model.Company_Website;
                        recordToUpdate.PM_Name = model.PM_Name;
                        recordToUpdate.PM_Email = model.PM_Email;
                        recordToUpdate.PM_Office_Phone = model.PM_Office_Phone;
                        recordToUpdate.PM_Mobile = model.PM_Mobile;
                        recordToUpdate.Modified_by = model.Modified_by;
                        recordToUpdate.Modified_Date = DateTime.Now;
                        recordToUpdate.Manufacturer_Ref_Code = model.Manufacturer_Ref_Code;
                        dbContext.SaveChanges();
                        isUpdated = true;
                    }
                }
            }
            catch (Exception ex)
            {
                isUpdated = false;
                throw;
            }
            return isUpdated;
        }

        /// <summary>
        /// Update status of single Manufacturer and his corresponding associated Users
        /// </summary>
        /// <param name="manufacId"></param>
        /// <param name="currentStatus"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public bool UpdateMFStatus(int manufacId, bool currentStatus, Guid currentUserId)
        {
            bool isUpdated = false;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var recordToUpdate = dbContext.Manufacturers.FirstOrDefault(c => c.Manufacturer_Id == manufacId);

                    if (recordToUpdate != null)
                    {
                        bool statusToUpdate = false;
                        // If current status is Activated, then Deactivate, else reverse.
                        if (currentStatus == true)
                            statusToUpdate = false;
                        else
                            statusToUpdate = true;

                        recordToUpdate.Status = statusToUpdate;
                        recordToUpdate.Modified_by = currentUserId;
                        recordToUpdate.Modified_Date = DateTime.Now;
                        UpdateMFUsersByManufacId(dbContext, manufacId, currentUserId, statusToUpdate);  // All users under this MF shud be activated/deactivated
                        dbContext.SaveChanges();
                        isUpdated = true;
                    }
                }
            }
            catch (Exception ex)
            {
                isUpdated = false;
                throw;
            }
            return isUpdated;
        }
        /// <summary>
        /// All users under MF shud be activated/deactivated
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="manufacId"></param>
        /// <param name="currentUserId"></param>
        /// <param name="statusToUpdate"></param>
        private void UpdateMFUsersByManufacId(ReviveDBEntities dbContext, int manufacId, Guid currentUserId, bool statusToUpdate)
        {

            var manufacUsers = (from d in dbContext.Users
                                      where d.Manufacturer_Id == manufacId
                                      select d).ToList();
            foreach (User user in manufacUsers)
            {
                user.status = statusToUpdate;
                user.Modified_by = currentUserId;
                user.Modified_Date = DateTime.Now;

            }
        }

        public Boolean CheckManufacturerRefCodeExists(String Manufacturer_Ref_Code, int Manufacturer_Id)
        {
            var Duplicateflag = true;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (Manufacturer_Id != 0)
                    {
                        var record = dbContext.Manufacturers.FirstOrDefault(cond => cond.Manufacturer_Ref_Code.ToLower() == Manufacturer_Ref_Code.ToLower() && cond.Manufacturer_Id != Manufacturer_Id);
                        if (record != null)
                        {
                            Duplicateflag = false;
                        }
                    }
                    else
                    {
                        var record = dbContext.Manufacturers.FirstOrDefault(cond => cond.Manufacturer_Ref_Code.ToLower() == Manufacturer_Ref_Code.ToLower());
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

    }
}
