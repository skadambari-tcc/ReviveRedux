using Revive.Redux.Entities;
using Revive.Redux.Repositories;
using Revive.Redux.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Services
{
    public class ManufacturersService : IManufacturersService
    {
        private IManufacturersRepository _IManufacturersRepository = null;

        public ManufacturersService()
        {
            _IManufacturersRepository = new ManufacturersRepository();
        }

        public IEnumerable<ManufacturersModel> GetAllMFs(string pageAccessCode)
        {
            return _IManufacturersRepository.GetAllMFs(pageAccessCode);
        }

        public ManufacturersModel GetMFById(int manufacId)
        {
            return _IManufacturersRepository.GetMFById(manufacId);
        }

        public bool CreateMF(ManufacturersModel model)
        {
            return _IManufacturersRepository.CreateMF(model);
        }

        public bool UpdateMFDetails(ManufacturersModel model)
        {
            return _IManufacturersRepository.UpdateMFDetails(model);
        }

        public bool UpdateMFStatus(int manufacId, bool currentStatus, Guid currentUserId)
        {
            return _IManufacturersRepository.UpdateMFStatus(manufacId, currentStatus, currentUserId);
        }

        public Boolean CheckManufacturerRefCodeExists(String Manufacturer_Ref_Code, int Manufacturer_Id)
        {
            return _IManufacturersRepository.CheckManufacturerRefCodeExists(Manufacturer_Ref_Code, Manufacturer_Id);
        }
    }
}
