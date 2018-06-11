using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MyQueue
{
    public class PerformanceTests
    {
        private const int NumberOfElements = 100000;
        private const int NumberOfAttempts = 1000;

        public static void TestMyQueue()
        {
            TestEnqueueDequeue<int>();
            GeneralTest<int>();
        }

        private static void TestEnqueueDequeue<T>()
        {
            var systemQueue = new Queue<T>();
            var myQueue = new MyQueue<T>();
            var item = default(T);

            long systemEnqueueResult = 0;
            long systemDequeueResult = 0;
            long myEnqueueResult = 0;
            long myDequeueResult = 0;

            for (var attempt = 0; attempt < NumberOfAttempts; attempt++)
            {
                Refresh(systemQueue, myQueue, item);
                
                var watch = Stopwatch.StartNew();
                for (var i = 0; i < NumberOfElements; i++)
                {
                    systemQueue.Enqueue(item);
                }

                systemEnqueueResult += watch.ElapsedTicks;

                watch = Stopwatch.StartNew();
                for (var i = 0; i < NumberOfElements; i++)
                {
                    systemQueue.Dequeue();
                }

                watch.Stop();
                systemDequeueResult += watch.ElapsedTicks;

                watch = Stopwatch.StartNew();
                for (var i = 0; i < NumberOfElements; i++)
                {
                    myQueue.Enqueue(item);
                }

                watch.Stop();
                myEnqueueResult += watch.ElapsedTicks;

                watch = Stopwatch.StartNew();
                for (var i = 0; i < NumberOfElements; i++)
                {
                    myQueue.Dequeue();
                }

                watch.Stop();
                myDequeueResult += watch.ElapsedTicks;
            }

            Console.WriteLine($"[Enqueue] System Queue:  {systemEnqueueResult / 1000}");
            Console.WriteLine($"[Enqueue] My Queue:      {myEnqueueResult / 1000}");
            Console.WriteLine($"[Dequeue] System Queue:  {systemDequeueResult / 1000}");
            Console.WriteLine($"[Dequeue] My Queue:      {myDequeueResult / 1000}");
        }

        private static void GeneralTest<T>()
        {
            var systemQueue = new Queue<T>();
            var myQueue = new MyQueue<T>();
            var item = default(T);

            long systemResult = 0;
            long myResult = 0;

            for (var attempt = 0; attempt < NumberOfAttempts; attempt++)
            {
                Refresh(systemQueue, myQueue, item);
                systemResult += MeasureSystemQueue(systemQueue, item);
                myResult += MeasureMyQueue(myQueue, item);
            }

            Console.WriteLine($"[Random] System Queue:  {systemResult / 1000}");
            Console.WriteLine($"[Random] My Queue:      {myResult / 1000}");
        }

        private static void Refresh<T>(Queue<T> systemQueue, MyQueue<T> myQueue, T item)
        {
            systemQueue.Clear();
            myQueue.Clear();
            for (var i = 0; i < 100; i++)
            {
                systemQueue.Enqueue(item);
                myQueue.Enqueue(item);
            }
        }

        private static long MeasureSystemQueue<T>(Queue<T> systemQueue, T item)
        {
            var watch = Stopwatch.StartNew();
            for (var i = 0; i < NumberOfElements; i++)
            {
                if (i % 3 == 0)
                    systemQueue.Dequeue();
                else
                    systemQueue.Enqueue(item);
            }

            watch.Stop();
            return watch.ElapsedTicks;
        }

        private static long MeasureMyQueue<T>(MyQueue<T> myQueue, T item)
        {
            var watch = Stopwatch.StartNew();
            for (var i = 0; i < NumberOfElements; i++)
            {
                if (i % 3 == 0)
                    myQueue.Dequeue();
                else
                    myQueue.Enqueue(item);
            }

            watch.Stop();
            return watch.ElapsedTicks;
        }
    }
}