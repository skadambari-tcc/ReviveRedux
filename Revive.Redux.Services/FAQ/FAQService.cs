using Revive.Redux.Entities;
using Revive.Redux.Repositories;
using System;
using System.Collections.Generic;

namespace Revive.Redux.Services
{
    public class FAQService : IFAQService
    {
        private IFAQRepository _IFAQRepository = null;

        public FAQService()
        {
            _IFAQRepository= new FAQRepository();
        }

        public IEnumerable<FAQModel> GetFAQs(string pageAccessCode)
        {
            return _IFAQRepository.GetFAQs(pageAccessCode);
        }
        public FAQModel GetFAQById(int rowId)
        {
            return _IFAQRepository.GetFAQById(rowId);
        }
        public void InsertFAQs(List<FAQModel> faqModel, Guid userId)
        {
            _IFAQRepository.InsertFAQs(faqModel, userId);
        }
        public void DeleteFAQs()
        {
            _IFAQRepository.DeleteFAQs();
        }
    }
}
