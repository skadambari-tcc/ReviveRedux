using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public class LoggingModel
    {
        public int LocationId { get; set; }
        public string FilePath { get; set; }
        public DateTime? LogFileTimeStamp { get; set; }
        public Nullable<System.Guid> Created_by { get; set; }

        public string LogFileDate { get; set; }
        public string LocationName { get; set; }
        public string UserName { get; set; }

        
        public int CustomerId { get; set; }
        public int SubsidiaryID { get; set; }
        public int SubAgentID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

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

        public int LoggTypeId { get; set; }
        public List<LoggingTextFile> objLoggingTextFile { get; set; }
        public filedata FileDetails { get; set; }

        public bool IsFileExists { get; set; }
    }

    public class LoggingParameter
    {
        public int LocationId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int CustomerId { get; set; }
        public int SubsidiaryId { get; set; }
        public int SubAgentId { get; set; }
    }

    public class LoggingTextFile
    {
        public string Type { get; set; }
        public string Text { get; set; }
        public string TimeStamp { get; set; }
    }

    public class filedata
    {
        public IList<string> data { get; set; }
        public IList<string> Exceptiondata { get; set; }
        public IList<string> Errordata { get; set; }
        public IList<string> Infodata { get; set; }
        public string FileName { get; set; }

        public filedata()
        {
            data = new List<string>();
            Infodata = new List<string>();
            Exceptiondata = new List<string>();
            Errordata = new List<string>();
        }
    }
}
