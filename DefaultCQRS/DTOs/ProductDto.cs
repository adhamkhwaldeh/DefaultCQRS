using AlJawad.DefaultCQRS.Interfaces;

namespace DefaultCQRS.DTOs
{
    public class ProductDto:IHaveIdentifier<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
