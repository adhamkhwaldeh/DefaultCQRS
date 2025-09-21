using System;

namespace AlJawad.DefaultCQRS.CQRS
{
    public class EntityHandlersConfiguration
    {
        public Type CreateCommandHandler { get; set; }
        public Type CreateCommandValidator { get; set; }
        public Type UpdateCommandHandler { get; set; }
        public Type UpdateCommandValidator { get; set; }
        public Type DeleteCommandHandler { get; set; }
        public Type AuthorizationHandler { get; set; }
        public Type IdentifierQueryHandler { get; set; }
        public Type ListQueryHandler { get; set; }
        public Type PagedQueryHandler { get; set; }
    }
}
