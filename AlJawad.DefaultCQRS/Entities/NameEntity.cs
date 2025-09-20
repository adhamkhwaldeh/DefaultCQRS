using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlJawad.DefaultCQRS.Entities
{
    public class NameEntity
    {
        public int? Value { get; set; }
        public string Name { get; set; }
        
        public NameEntity()
        {

        }
        public NameEntity(int? value, string name)
        {
            Value = value;
            Name = name;
        }
        public NameEntity( string name)
        {
            Name = name;
        }


    }
}