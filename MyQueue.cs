using System;
using System.Collections;
using System.Collections.Generic;

namespace MyQueue
{
    public sealed class MyQueue<T> : IEnumerable<T>
    {
        private const int DefaultCapacity = 4;
        
        public int Count { get; private set; }
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
            if (Count++ == capacity)
                RebaseArray();

            if (++lastElementPosition >= capacity)
                lastElementPosition = 0;

            array[lastElementPosition] = item;
        }

        public T Dequeue()
        {
            if (Count-- == 0)
                throw new InvalidOperationException();

            var result = array[firstElementPosition];

            if (++firstElementPosition == capacity)
                firstElementPosition = 0;

            return result;
        }

        public T Peek()
        {
            return array[firstElementPosition];
        }

        public void Clear()
        {
            Count = 0;
            firstElementPosition = 0;
            lastElementPosition = -1;
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
            return new MyQueueEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public struct MyQueueEnumerator : IEnumerator<T>
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
            }

            public bool MoveNext()
            {
                if (cur++ == queue.lastElementPosition)
                    return false;

                if (cur == queue.capacity)
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