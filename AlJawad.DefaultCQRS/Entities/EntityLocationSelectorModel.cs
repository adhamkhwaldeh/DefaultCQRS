using NetTopologySuite.Geometries;
using AlJawad.DefaultCQRS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlJawad.DefaultCQRS.Entities
{
    public class EntityLocationSelectorModel<TKey> : IHaveIdentifier<TKey>
    {
        public TKey Id { get; set; }
        public dynamic Payload { get; set; }
        public Point Location { get; set; }
        public MultiPoint Locations { get; set; }
    }
}
