//using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;
using NetTopologySuite.Geometries;

namespace AlJawad.DefaultCQRS.Interfaces
{
    public interface IMultiPointEntity
    {
        public MultiPoint Locations { get; set; }
    }
}
