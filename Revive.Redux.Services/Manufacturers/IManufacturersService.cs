using Revive.Redux.Entities;
using Revive.Redux.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Services
{
    public interface IManufacturersService
    {
        IEnumerable<ManufacturersModel> GetAllMFs(string pageAccessCode);

        ManufacturersModel GetMFById(int manufacId);

        bool CreateMF(ManufacturersModel model);

        bool UpdateMFDetails(ManufacturersModel model);

        bool UpdateMFStatus(int manufacId, bool currentStatus, Guid currentUserId);
        Boolean CheckManufacturerRefCodeExists(String Manufacturer_Ref_Code, int Manufacturer_Id);
    }
}
