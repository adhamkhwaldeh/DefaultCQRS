using AlJawad.DefaultCQRS.Enums;
using AlJawad.DefaultCQRS.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DefaultCQRS.Entities
{
    public abstract class BaseEntity :IHaveIdentifier<long>, IBaseEntity, IGenCode
    {
        [Key]
        [Column(Order = 1)]
        public long Id { get; set; }

        public string? CreatedBy { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public string? Code { get; set; }
        public string? AdditonalInfo { get; set; }

        public string? DeletedBy { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
        public string? ActivatedBy { get; set; }
        public DateTimeOffset? ActivatedDate { get; set; }
        public EntityStatus Status { get; set; }

    }

}
