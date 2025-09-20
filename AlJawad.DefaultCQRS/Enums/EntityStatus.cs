using NetTopologySuite.Index.HPRtree;
using Newtonsoft.Json.Converters;
using AlJawad.DefaultCQRS.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
//using static QRCoder.PayloadGenerator;

namespace AlJawad.DefaultCQRS.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EntityStatus { Active, InActive, Deleted }

    public static class EntityStatusExtensions
    {
        public static String GetString(this EntityStatus theme)
        {

            //TODO need to be checked
            //switch (theme)
            //{
            //    case EntityStatus.Active:
            //        return InfraResource.Active;
            //    case EntityStatus.InActive:
            //        return InfraResource.InActive;
            //    default:
            //        return InfraResource.Deleted;
            //}
            return "";
        }
        public static List<NameEntity> OptionsList(string title)
        {
            var results = new List<NameEntity>();
            results.Add(new NameEntity(name: title));
            foreach (var item in Enum.GetValues<EntityStatus>())
            {
                results.Add(new NameEntity(value: (int?)item, name: item.GetString()));
            }
            return results;
        }
    }
}