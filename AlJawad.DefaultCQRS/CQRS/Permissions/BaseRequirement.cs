using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlJawad.DefaultCQRS.CQRS.Permissions
{
    public class BaseRequirement<TEntity,Tkey> : IAuthorizationRequirement
    {
        public string ActionName { get; }
        public Tkey EntityId { get; set; }
        public TEntity Entity { get; set; }
        
        public BaseRequirement(string actionName)
        {
            ActionName = actionName;
        }
        public BaseRequirement(string actionName,Tkey entityId)
        {
            ActionName = actionName;
            EntityId = entityId;
        }
        public BaseRequirement(string actionName, TEntity entity)
        {
            ActionName = actionName;
            Entity = entity;
        }
        public BaseRequirement(string actionName, TEntity entity, Tkey entityId)
        {
            ActionName = actionName;
            Entity = entity;
            EntityId = entityId;
        }
    }
}
