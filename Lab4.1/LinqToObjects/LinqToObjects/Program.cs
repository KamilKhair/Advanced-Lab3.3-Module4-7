using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace LinqToObjects
{
    public class Program
    {
        public static void Main()
        {
            Exercise1A();
            Exercise1B();
            Exercise1C();
            Exercise1D();
            Exercise2();
        }

        private static void Exercise1A()
        {
            Console.WriteLine("Exercise 1.a:");
            var interfaces = GetInterfacesOrderedByName();

            foreach (var @interface in interfaces)
            {
                Console.WriteLine(@interface);
            }
            Console.WriteLine();
        }

        private static IEnumerable<object> GetInterfacesOrderedByName()
        {

            var interfaces = Assembly.GetAssembly(typeof(int))
                .GetTypes()
                .Where(@interface => @interface.IsInterface && @interface.IsPublic)
                .OrderBy(@interface => @interface.Name)
                .Select(@interface => new { @interface.Name, NumOfMethods = @interface.GetMethods().Length});
            return interfaces;
        }

        private static void Exercise1B()
        {
            Console.WriteLine("Exercise 1.b:");
            var processes = Process.GetProcesses()
                .Where(process => process.Threads.Count < 5 && CanAccessProcess(process))
                .OrderBy(process => process.Id)
                .Select(process => new { process.ProcessName, process.Id, process.StartTime });
            foreach (var process in processes)
            {
                Console.WriteLine(process);
            }
            Console.WriteLine();
        }

        private static bool CanAccessProcess(Process process)
        {
            try
            {
                return process?.Handle != IntPtr.Zero;
            }
            catch
            {
                return false;
            }
        }

        private static void Exercise1C()
        {
            Console.WriteLine("Exercise 1.c:");
            var processes = Process.GetProcesses()
                .Where(process => process.Threads.Count < 5 && CanAccessProcess(process))
                .OrderBy(process => process.Id)
                .GroupBy(process => process.BasePriority, process => new { process.ProcessName, process.Id, process.StartTime })
                .Select(group => group);
            foreach (var group in processes)
            {
                Console.WriteLine($"Base priority: {group.Key}");
                foreach (var process in group)
                {
                    Console.WriteLine(process);
                }
            }
            Console.WriteLine();
        }

        private static void Exercise1D()
        {
            Console.WriteLine("Exercise 1.d:");
            Console.WriteLine($"The total number of threads in the system is: {Process.GetProcesses().Sum(p => p.Threads.Count)}");
            Console.WriteLine();
        }

        private static void Exercise2()
        {
            Console.WriteLine("Exercise 2:");
            var person = new Person
            {
                Address = "Haifa",
                Age = 35,
                Wieght = 75
            };
            var worker = new Worker
            {
                Id = "225455687",
                Age = 28,
                Role = "Developer",
                Address = "Tel-Aviv"
            };
            Console.WriteLine($"Before copy: {worker}");
            person.CopyTo(worker);
            Console.WriteLine($"After copy: {worker}");
        }
    }
}
