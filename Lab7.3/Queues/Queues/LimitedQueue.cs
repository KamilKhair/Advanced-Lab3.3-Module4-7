using System;
using System.Collections.Generic;
using System.Threading;

namespace Queues
{
    //No IDisposable to release the semaphore?
    public class LimitedQueue<T>
    {
        private readonly Queue<T> _queue;
        private readonly SemaphoreSlim _semaphore;
        private readonly object _syncLock = new object();

        public LimitedQueue(Queue<T> queue, int maxQueueSize)
        {
            MaxSize = maxQueueSize;
            _queue = queue;
            _semaphore = new SemaphoreSlim(MaxSize);
        }

        public int MaxSize { get; private set; }

        public void Enqueue(T element)
        {
            _semaphore.Wait();
            lock (_syncLock)
            {
                _queue.Enqueue(element);
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} Enqueue");
                Console.WriteLine($"Current queue size: {_queue.Count}");
            }
        }

        public T Dequeue()
        {
            if (_queue.Count == 0)
            {
                throw new InvalidOperationException("The queue is empty");
            }

            //You should insert it into a finaly clause when working with acquire-release pattern
            _semaphore.Release();
            T element;
            lock (_syncLock)
            {
                element = _queue.Dequeue();
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} Dequeue");
                Console.WriteLine($"Current queue size: {_queue.Count}");
            }
            return element;
        }
    }
}
