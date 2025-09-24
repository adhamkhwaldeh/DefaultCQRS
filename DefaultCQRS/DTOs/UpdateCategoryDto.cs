using AlJawad.DefaultCQRS.Interfaces;

namespace DefaultCQRS.DTOs
{
    public class UpdateCategoryDto:IHaveIdentifier<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}