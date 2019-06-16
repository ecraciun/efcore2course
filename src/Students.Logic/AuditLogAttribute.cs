using System;
using System.Collections.Generic;
using System.Text;

namespace Students.Logic
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class AuditLogAttribute : Attribute
    {
    }
}
