using System;
namespace Modules.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class InjectAttribute : Attribute { }
}