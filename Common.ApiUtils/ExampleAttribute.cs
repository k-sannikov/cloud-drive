using System;

namespace Common.ApiUtils
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class ExampleAttribute : Attribute
    {
        public virtual object ExampleValue { get; }

        protected ExampleAttribute()
        {
        }

        public ExampleAttribute(object exampleValue)
        {
            ExampleValue = exampleValue;
        }
    }
}