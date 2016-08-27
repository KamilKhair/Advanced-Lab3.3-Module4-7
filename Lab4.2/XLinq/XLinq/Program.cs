using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace XLinq
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var classes = GetXElementOfClasses();
            var xElements = classes as IList<XElement> ?? classes.ToList();
            Exercise3A(xElements);
            Exercise3B(xElements);
            Exercise3C(xElements);
            Exercise3D(xElements);
            Exercise3E(xElements);
        }

        private static IEnumerable<XElement> GetXElementOfClasses()
        {

            // Why did't you use: From .... in ...... syntex ???
            var classes = Assembly.GetAssembly(typeof(string)).GetTypes()
                .Where(type => type.IsClass && type.IsPublic)
                .Select(@class => new XElement("Type", new XAttribute("FullName", @class.FullName),
                    new XElement("Properties", @class.GetProperties().Select(prop => //should be @class.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        new XElement("Property", new XAttribute("Name", prop.Name),
                            new XAttribute("Type", prop.PropertyType)))),
                    new XElement("Methods",
                        @class.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                            .Select(method =>
                                new XElement("Method", new XAttribute("Name", method.Name),
                                    new XAttribute("ReturnType", method.ReturnType),
                                    new XElement("Parameters", method.GetParameters().Select(parameter =>
                                        new XElement("Parameter", new XAttribute("Name", parameter.Name),
                                            new XAttribute("Type", parameter.ParameterType)))))))));
            return classes;
        }

        private static void Exercise3A(IEnumerable<XElement> classes)
        {
            Console.WriteLine();
            Console.WriteLine("Exercise 3.a: ");
            Console.WriteLine();

            var noProperties = classes.Where(@class =>
            {
                //use: From .... in ...... syntex next time
                var xElement = @class.Element("Properties");
                return xElement != null && !xElement.Descendants().Any();
            })
            // you can skip first "select" and go straight to "order"
                .Select(@class => new {@class, name = (string) @class.Attribute("FullName")})
                .OrderBy(type => type.name)
                .Select(type => type.name);
            var noPropertiesList = noProperties as IList<string> ?? noProperties.ToList();
            Console.WriteLine("The classes with no properties are: ");
            foreach (var property in noPropertiesList)
            {
                Console.WriteLine($"{property} ");
            }
            Console.WriteLine($"Number of classes with no properties: {noPropertiesList.Count}");
        }

        private static void Exercise3B(IEnumerable<XElement> classes)
        {
            Console.WriteLine();
            Console.WriteLine("Exercise 3.b: ");
            Console.WriteLine();

            Console.WriteLine($"Total number of methods: {classes.Sum(@class => @class.Descendants("Method").Count())}");
        }

        private static void Exercise3C(IEnumerable<XElement> classes)
        {
            Console.WriteLine();
            Console.WriteLine("Exercise 3.c: ");
            Console.WriteLine();

            var xElements = classes as IList<XElement> ?? classes.ToList();
            // next time make your calculations out of prints
            Console.WriteLine($"Total number of properties: {xElements.Sum(e => e.Descendants("Property").Count())}");
            var parameters = xElements.Descendants("Parameter")
                .GroupBy(parameter => (string) parameter.Attribute("Type"))
                .OrderByDescending(parameter => parameter.Count())
                .Select(parameter => new {parameter.Key, Count = parameter.Count()});
            Console.WriteLine(
                $"Most common parameter type: {parameters.First().Key}, used {parameters.First().Count} times.");
        }

        private static void Exercise3D(IEnumerable<XElement> xElements)
        {
            Console.WriteLine();
            Console.WriteLine("Exercise 3.d: Check out \"Exercise3D.xml\" file in bin\\debug directory.");
            Console.WriteLine();
            // "select" should be the last one, first of all you should filter your query and in the end select your choices
            var myTypes = xElements.Select( type =>
                                    new
                                    {
                                        Name = type.FirstAttribute.Value,
                                        propsCount = type.Descendants("Property").Count(),
                                        methodsCount = type.Descendants("Method").Count()
                                    })
                                    .OrderByDescending(type => type.methodsCount);

            var newXElement = myTypes.Select(type => new XElement("Type", new XAttribute("FullName", type.Name),
                                                        new XElement("Methods", new XAttribute("MethodsCount", type.methodsCount)),
                                                        new XElement("Properties", new XAttribute("PropertiesCount", type.propsCount))));
            var xelement = new XElement("Types", newXElement);
            xelement.Save("Exercise3D.xml");
        }


        private static void Exercise3E(IEnumerable<XElement> xElements)
        {
            Console.WriteLine();
            Console.WriteLine("Exercise 3.e: Check out \"Exercise3E.txt\" file in bin\\debug directory.");
            Console.WriteLine();
            // "select" should be the last one, first of all you should filter your query and in the end select your choices
            var typesByNumOfMethods = xElements.Select(type => new { type, methods = type.Descendants("Method").Count()})
                .OrderBy(t => (string) t.type.Attribute("FullName"))
                .GroupBy(group => group.methods, group => new
                {
                    Methods = group.methods,
                    Name = group.type.FirstAttribute.Value
                }).OrderByDescending(group => group.Key);

            using (var file = new StreamWriter("Excercise3E.txt"))
            {
                foreach (var group in typesByNumOfMethods)
                {
                    foreach (var type in group)
                    {
                        file.WriteLine(type);
                    }
                }
            }
        }
    }
}
