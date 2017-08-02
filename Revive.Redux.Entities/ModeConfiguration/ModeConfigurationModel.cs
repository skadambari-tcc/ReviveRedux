using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public class ModeConfigurationModel
    {
        [Required(ErrorMessage = "Required")]
        public DateTime? From_Date { get; set; }

        [Required(ErrorMessage = "Required")]
        public DateTime? To_Date { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Number of Demos")]
        [Range(0, 90, ErrorMessage = "No. of Demos must be between 0 to 90")]
        public int? No_of_Activities { get; set; }

        [Required(ErrorMessage = "Required")]
        public int? customerID { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Required")]
        public int? locationID { get; set; }

        public int Config_Id { get; set; }
        public int ModeId { get; set; }
        public bool? Status { get; set; }
        public Guid Created_by { get; set; }
        public DateTime? Created_Date { get; set; }
        public Guid Modified_by { get; set; }
        public DateTime? Modified_Date { get; set; }
        public string CustomerName { get; set; }
        public string locationName { get; set; }
        public int No_of_Consumed { get; set; }

        //Subsidiary Changes
        [Display(Name = "Subsidiary")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Required")]
        public int SubsidiaryId { get; set; }

        [Display(Name = "Agent")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Required")]
        public int SubAgentId { get; set; }

        //public IEnumerable<DtoList> SubsidiaryNameList { get; set; }
        //public IEnumerable<DtoList> AgentNameList { get; set; }
        public bool isSelected { get; set; }


        private IEnumerable<DtoList> _CustomerNameList;
        public IEnumerable<DtoList> CustomerNameList
        {
            get { return _CustomerNameList; }
            set { _CustomerNameList = value; }
        }

        private IEnumerable<DtoList> _LocationList;
        public IEnumerable<DtoList> LocationList
        {
            get { return _LocationList; }
            set { _LocationList = value; }
        }

        private IEnumerable<DtoList> _SubsidiaryList;
        public IEnumerable<DtoList> SubsidiaryList
        {
            get { return _SubsidiaryList; }
            set { _SubsidiaryList = value; }
        }

        private IEnumerable<DtoList> _SubAgentList;
        public IEnumerable<DtoList> SubAgentList
        {
            get { return _SubAgentList; }
            set { _SubAgentList = value; }
        }

        public string SubAgentName { get; set; }
        public string StoreNumber { get; set; }
    }
}
