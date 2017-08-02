using Revive.Redux.Entities;
using System;
using System.Collections.Generic;

namespace Revive.Redux.Repositories
{
    public interface IManufacturersRepository
    {
        IEnumerable<ManufacturersModel> GetAllMFs(string pageAccessCode);

        ManufacturersModel GetMFById(int manufacId);

        bool CreateMF(ManufacturersModel model);

        bool UpdateMFDetails(ManufacturersModel model);

        bool UpdateMFStatus(int manufacId, bool currentStatus, Guid currentUserId);
        Boolean CheckManufacturerRefCodeExists(String Manufacturer_Ref_Code, int Manufacturer_Id);
    }
}
