using AlJawad.DefaultCQRS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlJawad.DefaultCQRS.Entities
{
    public class EntityLocationModel<TKey> : IHaveIdentifier<TKey>
    {
        public TKey Id { get; set; }
        public dynamic Payload { get; set; }
        public PointModel Location { get; set; }
        public MultiPointModel Locations { get; set; }
    }
}