using System;

namespace AlJawad.DefaultCQRS.Models
{
    public class CreateResult
    {
        public Guid Stream_id { get; set; }
        public string Path { get; set; }
        public string PullPath { get; set; }
        public string Name { get; set; }
    }
}
