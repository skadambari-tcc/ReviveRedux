
using System;
namespace Revive.Redux.Entities
{
    public class DtoList
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int? otherIntVal { get; set; }
        public string otherStrVal { get; set; }
        public Guid CompleteId { get; set; }
        public bool issel { get; set; }
    }
}
