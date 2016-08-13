using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace SyncDemo
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine($"Process {Process.GetCurrentProcess().Id} is starting:");
            using (var syncFileMutex = new Mutex(false, "MyMutex"))
            {
                for (var i = 0; i < 10000; i++)
                {
                    try
                    {
                        syncFileMutex.WaitOne();
                        using (var fileWriter = new StreamWriter(@"c:\temp\data.txt", true))
                        {
                            fileWriter.WriteLine($"Process {Process.GetCurrentProcess().Id} is writing to the file");
                        }
                    }
                    finally
                    {
                        using (var fileWriter = new StreamWriter(@"c:\temp\data2.txt", true))
                        {
                            fileWriter.WriteLine($"Process {Process.GetCurrentProcess().Id} is about to release the mutex MyMutex");
                        }
                        syncFileMutex.ReleaseMutex();
                    }
                }
                Console.WriteLine("Done !");
            }
        }
    }
}