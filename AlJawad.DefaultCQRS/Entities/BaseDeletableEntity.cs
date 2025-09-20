using AlJawad.DefaultCQRS.Enums;
using AlJawad.DefaultCQRS.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlJawad.DefaultCQRS.Entities
{
    public abstract class BaseDeletableEntity : BaseEntity, IHardDelete
    {
 

    }
}
