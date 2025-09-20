using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlJawad.DefaultCQRS.CQRS.Permissions
{
    public static class BaseRequirementActions
    {
        public const String ViewListAction = "ViewList";
        public const String ViewDetailsAction = "ViewDetails";
        public const String CreateAction = "Create";
        public const String UpdateAction = "Update";
        public const String DeleteAction = "Delete";
        public const String DisableAction = "Disable";
    }
}