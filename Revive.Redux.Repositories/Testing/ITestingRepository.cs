using Revive.Redux.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Repositories
{
    public interface ITestingRepository
    {
        TestingModel GetMidLineTestResults(int machMapId);
        TestingModel GetULTestResults(int machMapId);
        TestingModel GetFinalTestResults(int machMapId);
        bool UpdateMachineTesting(string testStageName, int testStageId, TestingModel model, Guid currentUserId);
    }
}
