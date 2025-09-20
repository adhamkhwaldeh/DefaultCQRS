namespace AlJawad.DefaultCQRS.Interfaces
{
    public interface IHaveIdentifier<TKey>
    {
        public TKey Id { get; set; }
    }
}
