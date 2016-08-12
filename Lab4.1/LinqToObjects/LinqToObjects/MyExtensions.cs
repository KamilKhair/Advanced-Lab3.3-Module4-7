using System.Linq;

namespace LinqToObjects
{
    public static class MyExtensions
    {
        public static void CopyTo(this object source, object destination)
        {
            var properties = source.GetType().GetProperties()
                .SelectMany(property1 => destination.GetType().GetProperties(), 
                            (property1, property2) => new { property1, property2 })
                .Where(type => type.property1.Name == type.property2.Name &&
                       type.property1.PropertyType == type.property2.PropertyType &&
                       type.property1.CanRead && type.property2.CanWrite)
                .Select(type => new { SourceProperty = type.property1, DestinationProperty = type.property2 });
            foreach (var property in properties)
            {
                property.DestinationProperty.SetValue(destination, property.SourceProperty.GetValue(source, null), null);
            }
        }
    }
}