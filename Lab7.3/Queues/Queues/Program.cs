using System;
using System.Collections.Generic;
using System.Threading;

namespace Queues
{
    public class Program
    {
        private readonly LimitedQueue<int> _limitedQueue = new LimitedQueue<int>(new Queue<int>(), 20);
        public static void Main(string[] args)
        {
            for (var i = 0; i < 1000000; ++i)
            {
                var program = new Program();
                ThreadPool.QueueUserWorkItem(program.ThreadEnqueue, i);
                ThreadPool.QueueUserWorkItem(program.ThreadDequeue, i);
            }
            for (var i = 0; i < 1000000; ++i)
            {
                var program = new Program();
            }
            Console.ReadLine();
        }

        public void ThreadEnqueue(object threadContext)
        {
            for (var i = 0; i < 10000; ++i)
            {
                _limitedQueue.Enqueue((int)threadContext);
            }
        }

        public void ThreadDequeue(object threadContext)
        {
            for (var i = 0; i < 10000; ++i)
            {
                try
                {
                    _limitedQueue.Dequeue();
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
            }
        }
    }
}
