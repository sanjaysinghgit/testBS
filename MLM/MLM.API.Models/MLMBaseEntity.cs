using System;

namespace MLM.Models
{
    public class MLMBaseEntity : IMLMBaseEntity
    {
        public virtual long      Id          { get; set; }
        public virtual bool     IsDeleted   { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime UpdateDate  { get; set; }

    }

    public enum SortOrder
    {
        NONE = 0,
        ASC = 1,
        DESC = 2
    }

}
