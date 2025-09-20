using System;

namespace AlJawad.DefaultCQRS.Interfaces
{
    public interface ITrackAudit
    {

        string CreatedBy { get; set; }
        DateTimeOffset? CreatedDate { get; set; }

        string ModifiedBy { get; set; }
        DateTimeOffset? ModifiedDate { get; set; }

    }

}
