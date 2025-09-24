using NetTopologySuite.Index.HPRtree;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
//using static QRCoder.PayloadGenerator;

namespace AlJawad.DefaultCQRS.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EntityStatus { Active, InActive, Deleted }

}