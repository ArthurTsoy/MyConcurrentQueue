using System;

namespace MyQueue
{
    public sealed class MyQueue<T>
    {
        public int Count { get; private set; }
        private int myCapacity;
        private int myFirstElementPosition;
        private int myLastElementPosition;
        private T[] myArray;

        public MyQueue()
        {
            Count = 0;
            myCapacity = 4;
            myFirstElementPosition = 0;
            myLastElementPosition = -1;
            myArray = new T[myCapacity];
        }

        public void Enqueue(T item)
        {
            if (++Count >= myCapacity)
                RebaseArray();

            if (++myLastElementPosition >= myCapacity)
                myLastElementPosition = 0;

            myArray[myLastElementPosition] = item;
        }

        public T Dequeue()
        {
            if (--Count < 0)
                throw new InvalidOperationException();

            var result = myArray[myFirstElementPosition];

            if (++myFirstElementPosition >= myCapacity)
                myFirstElementPosition = 0;

            return result;
        }

        public T Peek()
        {
            return myArray[myFirstElementPosition];
        }


        public void Clear()
        {
            Count = 0;
            myFirstElementPosition = 0;
            myLastElementPosition = -1;
        }

        private void RebaseArray()
        {
            var newArray = new T[myCapacity * 2];
            var pos = 0;

            for (var i = myFirstElementPosition; i < myCapacity; i++)
            {
                newArray[pos++] = myArray[i];
            }

            for (var i = 0; i < myFirstElementPosition; i++)
            {
                newArray[pos++] = myArray[i];
            }

            myArray = newArray;
            myCapacity *= 2;
            myLastElementPosition = Count;
        }
    }
}