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
    public class TestingService : ITestingService
    {
        private ITestingRepository _ITestingRepository = null;

        public TestingService()
        {
            _ITestingRepository = new TestingRepository();
        }
        public TestingModel GetMidLineTestResults(int machMapId)
        {
            return _ITestingRepository.GetMidLineTestResults(machMapId);
        }
        public TestingModel GetULTestResults(int machMapId)
        {
            return _ITestingRepository.GetULTestResults(machMapId);
        }
        public TestingModel GetFinalTestResults(int machMapId)
        {
            return _ITestingRepository.GetFinalTestResults(machMapId);
        }
        public bool UpdateMachineTesting(string testStageName, int testStageId, TestingModel model, Guid currentUserId)
        {
            return _ITestingRepository.UpdateMachineTesting(testStageName, testStageId, model, currentUserId);
        }
    }
}
