﻿using Revive.Redux.Entities;
using System;
using System.Collections.Generic;

namespace Revive.Redux.Repositories
{
    public interface IFAQRepository
    {
        IEnumerable<FAQModel> GetFAQs(string pageAccessCode);
        FAQModel GetFAQById(int rowId);
        void InsertFAQs(List<FAQModel> faqModel, Guid userId);
        void DeleteFAQs();
    }
}
