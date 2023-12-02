using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Common.ApiUtils
{
    public class ExampleSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            IEnumerable<ExampleAttribute> exampleAttributes =
                context?.MemberInfo?.GetCustomAttributes<ExampleAttribute>();

            exampleAttributes = exampleAttributes ?? context.ParameterInfo?.GetCustomAttributes<ExampleAttribute>();

            if (IsNullOrEmpty(exampleAttributes) && context.Type != null)
            {
                exampleAttributes = context.Type.GetCustomAttributes<ExampleAttribute>();
            }

            if (IsNullOrEmpty(exampleAttributes))
                return;

            if (schema.Type == "array")
            {
                OpenApiArray array = new OpenApiArray();

                foreach (var exampleAttribute in exampleAttributes)
                {
                    array.Add(Convert(exampleAttribute.ExampleValue));
                }

                schema.Example = array;
            }
            else if (exampleAttributes.Count() == 1)
            {
                schema.Example = Convert(exampleAttributes.First().ExampleValue);
            }
        }

        private static IOpenApiAny Convert(object value) =>
            value switch
            {
                null => new OpenApiNull(),
                string stringValue => new OpenApiString(stringValue),
                int intValue => new OpenApiInteger(intValue),
                bool boolValue => new OpenApiBoolean(boolValue),
                Enum => new OpenApiString(value.ToString()),
                Type => ((IOpenApiExample)Activator.CreateInstance((Type)value)).Example,
                _ => null,
            };

        private static bool IsNullOrEmpty<ItemType>(IEnumerable<ItemType> source)
        {
            return source == null || !source.Any();
        }
    }

    public interface IOpenApiExample
    {
        IOpenApiAny Example { get; }
    }
}