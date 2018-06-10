using System;
using System.Collections.Generic;

namespace MyQueue
{
    public class PerformanceTests
    {
        private static readonly Random myRandom = new Random();

        public static void TestMyQueue()
        {
            Console.WriteLine("[Int]:");
            TestEnqueueDequeue<int>();
            TestRandomTrash<int>();
            Console.WriteLine("[double]:");
            TestEnqueueDequeue<double>();
            TestRandomTrash<double>();
            Console.WriteLine("[string]:");
            TestEnqueueDequeue<string>();
            TestRandomTrash<string>();
            Console.WriteLine("[TestClass]:");
            TestEnqueueDequeue<TestClass>();
            TestRandomTrash<TestClass>();
            Console.WriteLine("[TestStruct]:");
            TestEnqueueDequeue<TestStruct>();
            TestRandomTrash<TestStruct>();
        }

        private static void TestEnqueueDequeue<T>()
        {
            var systemQueue = new Queue<T>();
            var myQueue = new MyQueue<T>();

            var item = default(T);

            double systemEnqueueResult = 0;
            double systemDequeueResult = 0;
            for (var attempt = 0; attempt < 1000; attempt++)
            {
                {
                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    for (var i = 0; i < 100000; i++)
                    {
                        systemQueue.Enqueue(item);
                    }

                    systemEnqueueResult += watch.ElapsedTicks;
                }
                {
                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    for (var i = 0; i < 100000; i++)
                    {
                        systemQueue.Dequeue();
                    }

                    watch.Stop();
                    systemDequeueResult += watch.ElapsedTicks;
                }
            }

            double myEnqueueResult = 0;
            double myDequeueResult = 0;
            for (var attempt = 0; attempt < 1000; attempt++)
            {
                {
                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    for (var i = 0; i < 100000; i++)
                    {
                        myQueue.Enqueue(item);
                    }

                    watch.Stop();
                    myEnqueueResult += watch.ElapsedTicks;
                }
                {
                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    for (var i = 0; i < 100000; i++)
                    {
                        myQueue.Dequeue();
                    }

                    watch.Stop();
                    myDequeueResult += watch.ElapsedTicks;
                }
            }
            
            Console.WriteLine($"[Enqueue] system Queue:  {systemEnqueueResult / 1000}");
            Console.WriteLine($"[Enqueue] my Queue:      {myEnqueueResult / 1000}");
            Console.WriteLine($"[Dequeue] system Queue:  {systemDequeueResult / 1000}");
            Console.WriteLine($"[Dequeue] my Queue:      {myDequeueResult / 1000}");
        }

        private static void TestRandomTrash<T>()
        {
            var systemQueue = new Queue<T>();
            var myQueue = new MyQueue<T>();

            var item = default(T);


            double systemResult = 0;
            double myResult = 0;
            for (var attempt = 0; attempt < 1000; attempt++)
            {
                systemQueue.Clear();
                myQueue.Clear();
                for (var i = 0; i < 100; i++)
                {
                    systemQueue.Enqueue(item);
                    myQueue.Enqueue(item);
                }

                lock (myRandom)
                {
                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    for (var i = 0; i < 100000; i++)
                    {
                        var op = myRandom.Next(0, 3);
                        if (op == 0)
                            systemQueue.Dequeue();
                        else
                            systemQueue.Enqueue(item);
                    }

                    watch.Stop();
                    systemResult += watch.ElapsedTicks;
                }

                lock (myRandom)
                {
                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    for (var i = 0; i < 100000; i++)
                    {
                        var op = myRandom.Next(0, 3);
                        if (op == 0)
                            myQueue.Dequeue();
                        else
                            myQueue.Enqueue(item);
                    }

                    watch.Stop();
                    myResult += watch.ElapsedTicks;
                }
            }

            Console.WriteLine($"[Random] dystem Queue:  {systemResult / 1000}");
            Console.WriteLine($"[Random] my Queue:      {myResult / 1000}");
        }
    }
}