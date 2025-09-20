using System;
using System.Collections.Generic;
using System.Text;
using NetTopologySuite.Geometries;

namespace AlJawad.DefaultCQRS.Entities
{
    public class PointModel
    {
        public double longitude { get; set; }
        public double latitude { get; set; }
        public double radius { get; set; }

        public Point toPoint()
        {
            return new Point(longitude, latitude, radius) { SRID = 4326 };
            //return new Point(new Coordinate(longitude, latitude)) { SRID = 4326 };
        }

        public static PointModel fromPoint(Point point){
            return new PointModel()
            {
                longitude = point.X,
                latitude = point.Y,
                radius = point.Z,
            };
        }

        //public static PointModel fromCoordinate(Coordinate coordinate)
        //{
        //    return new PointModel()
        //    {
        //        longitude = coordinate.X,
        //        latitude = coordinate.Y,
        //        radius = coordinate.Z,
        //    };
        //}

        
    }   
}
