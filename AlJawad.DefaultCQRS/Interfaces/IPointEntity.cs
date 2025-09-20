using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlJawad.DefaultCQRS.Interfaces
{
    public interface IPointEntity
    {
        public Point Location { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}