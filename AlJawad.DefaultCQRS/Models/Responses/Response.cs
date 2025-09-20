using AlJawad.DefaultCQRS.Extensions;
using MassTransit;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace AlJawad.DefaultCQRS.Models.Responses
{
    public class Response<T>
    {


        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public T Data { get; set; }

        [JsonProperty("status")]
        [DataMember]
        public bool Status { get => ReturnStatus ?? !(Data is null) && ErrorCode.IsEmpty(); set => ReturnStatus = value; }

        public bool? ReturnStatus;

        public int StatusCode { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "errorCode")]
        public string ErrorCode { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "message")]
        public string Message { get; set; }


        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "messageAr")]
        public string MessageAr { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "messageDev")]
        public string MessageDev { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "stackTrace")]
        public string StackTrace { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "serverTimeLocal")]
        public DateTime ServerTimeLocal { get { return DateTime.Now; } }

        [DataMember]
        [JsonProperty(PropertyName = "serverTimeUTC")]
        public DateTime ServerTimeUTC { get { return DateTime.UtcNow; } }

        [JsonProperty("returnMessage")]
        [DataMember]
        public List<string> ReturnMessage { get; set; }

        [DataMember]
        public Hashtable Errors { get; set; }

        public Response()
        {
            ReturnMessage = new List<string>();
            Errors = new Hashtable();
        }

    }
}