using AlJawad.DefaultCQRS.Enums;
using System;

namespace AlJawad.DefaultCQRS.Interfaces
{
    public interface IActivation
    {
        public string DeletedBy { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
        public string ActivatedBy { get; set; }
        public DateTimeOffset? ActivatedDate { get; set; }
        public EntityStatus Status { get; set; }

    }
}
