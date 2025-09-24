using AlJawad.DefaultCQRS.Interfaces;

namespace DefaultCQRS.DTOs
{
    public class CategoryDto:IHaveIdentifier<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}