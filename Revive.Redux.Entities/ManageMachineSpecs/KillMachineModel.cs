using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public class KillMachineModel : PageBasic
    {
        private int _nCustomerId;

        [Display(Name = "CustomerName")]
        [Required(ErrorMessage = "Required")]
        public int CustomerId
        {
            get { return _nCustomerId; }
            set { _nCustomerId = value; }
        }

        private IEnumerable<DtoList> _CustomerNameList;

        public IEnumerable<DtoList> CustomerNameList
        {
            get { return _CustomerNameList; }
            set { _CustomerNameList = value; }
        }

        private int _nLocationId;

        [Display(Name = "Location Id")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Required")]
        public int LocationId
        {
            get { return _nLocationId; }
            set { _nLocationId = value; }
        }

        private IEnumerable<DtoList> _CustomerLocationDetails;

        public IEnumerable<DtoList> CustomerLocationDetails
        {
            get { return _CustomerLocationDetails; }
            set { _CustomerLocationDetails = value; }
        }

        private List<MachineModel> _MachineLocationDetails;

        public List<MachineModel> MachineLocationDetails  
        {
            get { return _MachineLocationDetails; }
            set { _MachineLocationDetails = value; }
        }

        //Subsidiary Changes
        private int _nSubsidiaryId;

        [Display(Name = "Subsidiary")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Required")]
        public int SubsidiaryId
        {
            get { return _nSubsidiaryId; }
            set { _nSubsidiaryId = value; }
        }

        private int _nSubAgentId;

        [Display(Name = "Agent")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Required")]
        public int SubAgentId
        {
            get { return _nSubAgentId; }
            set { _nSubAgentId = value; }
        }

        private IEnumerable<DtoList> _SubsidiaryNameList;

        public IEnumerable<DtoList> SubsidiaryNameList
        {
            get { return _SubsidiaryNameList; }
            set { _SubsidiaryNameList = value; }
        }

        private IEnumerable<DtoList> _AgentNameList;

        public IEnumerable<DtoList> AgentNameList
        {
            get { return _AgentNameList; }
            set { _AgentNameList = value; }
        }
        
    }
}
