using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace MyQueue
{
    public sealed class MyQueue<T> 
    {
        public int Count { get; private set; }
        
        private const int DefaultCapacity = 4;
        
        private readonly ReaderWriterLockSlim @lock = new ReaderWriterLockSlim();
        
        private int capacity;
        private int firstElementPosition;
        private int lastElementPosition;
        private T[] array;

        
        public MyQueue()
        {
            capacity = DefaultCapacity;
            array = new T[capacity];
            Clear();
        }
        
        public MyQueue(int capacity)
        {
            this.capacity = capacity;
            array = new T[this.capacity];
            Clear();
        }
        
        public void Enqueue(T item)
        {
            using (@lock.UsingWriterLock())
            { 
                if (Count++ == capacity)
                    RebaseArray();

                if (++lastElementPosition >= capacity)
                    lastElementPosition = 0;

                array[lastElementPosition] = item;
            }
        }

        public T Dequeue()
        {
            T result;
            
            using (@lock.UsingWriterLock())
            {
                if (Count-- == 0)
                    throw new InvalidOperationException();

                result = array[firstElementPosition];

                if (++firstElementPosition == capacity)
                    firstElementPosition = 0;
            }

            return result;
        }

        public T Peek()
        {
            using (@lock.UsingReaderLock())
            {
                return array[firstElementPosition];
            }
        }

        public void Clear()
        {
            using (@lock.UsingWriterLock())
            {
                Count = 0;
                firstElementPosition = 0;
                lastElementPosition = -1;
            }
        }

        private void RebaseArray()
        {
            var newArray = new T[capacity * 2];

            Array.Copy(array, firstElementPosition, newArray, 0, capacity - firstElementPosition);
            Array.Copy(array, 0, newArray, capacity - firstElementPosition, firstElementPosition);

            array = newArray;
            lastElementPosition = capacity - 1;
            capacity *= 2;
        }

        public IEnumerator<T> GetEnumerator()
        {
            @lock.EnterReadLock();
            return new MyQueueEnumerator(this);
        }

        private struct MyQueueEnumerator : IEnumerator<T>
        {
            public T Current => queue.array[cur];
            private readonly MyQueue<T> queue;
            private int cur;
            
            public MyQueueEnumerator(MyQueue<T> queue)
            {
                this.queue = queue;
                cur = queue.firstElementPosition - 1;
            }
            
            public void Dispose()
            {
                queue.@lock.ExitReadLock();
            }
            
            public bool MoveNext()
            {
                if (cur == queue.lastElementPosition)
                    return false;

                if (++cur == queue.capacity)
                    cur = 0;

                return true;
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }
            
            object IEnumerator.Current => Current;
        }
    }
}