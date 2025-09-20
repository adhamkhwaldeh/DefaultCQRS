using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NetTopologySuite.Geometries;
using AlJawad.DefaultCQRS.Extensions;

namespace AlJawad.DefaultCQRS.Entities
{
    public class MultiPointModel
    {
        public List<PointModel> Points { get; set; }

        public MultiPointModel()
        {

        }
        public MultiPointModel(List<PointModel> points)
        {
            Points = points;
            PointsConcatFromPoints();
        }

        public static MultiPointModel InitialFromMultiplePoint(MultiPoint multiplePoint)
        {
           var tmp =   new List<PointModel>();

            if (multiplePoint != null)
            {
                foreach (var itm in multiplePoint.Geometries)
                {
                    tmp.Add(PointModel.fromPoint((Point)itm));
                    //tmp.Add(PointModel.fromCoordinate(itm));
                }
               // return new MultiPointModel(tmp);
            }

            //foreach (var itm in multiplePoint.Coordinates)
            //{
            //    tmp.Add(PointModel.fromCoordinate(itm));
            //}

            //return null;
            return new MultiPointModel(tmp);

        }

        //[NonSerialized]
        [IgnoreDataMember]
        public Point[] geoPoints { get
            {
                Point[] tmp = new Point[Points.Count()];
                foreach (var item in Points.Select((value, i) => new { i, value }))
                {
                    tmp[item.i] = item.value.toPoint();
                }
                return tmp;
            } }

        public MultiPoint toMultiPoint()
        {
            return new MultiPoint(geoPoints) { SRID = 4326 };
        }
       
        public MultiPointModel addPoint(double lat, double lng)
        {

            Points.Add(new PointModel()
            {
                latitude = lat,
                longitude = lng,
            });
            return this;
        }

        public string PointsConcat { get; set; }

        public void PointsConcatFromPoints()
        {
            if (Points != null && Points != null && Points.Count() > 0)
            {
                PointsConcat = string.Join("=>", Points.Select(x => x.latitude + ";" + x.longitude).ToArray());
            }
            else
            {
                PointsConcat = "";
            }
        }

        public void PointsFromPointsConcat()
        {
            if (!PointsConcat.nullOrEmpty())
            {
                var tmp = PointsConcat.Split("=>").Select(x => new PointModel()
                {
                    latitude = Double.Parse(x.Split(";").First()),
                    longitude = Double.Parse(x.Split(";").Last()),
                }).ToList();
                Points = tmp;
            }
            else
            {
                Points = new List<PointModel>();
            }
        }

    }
}
