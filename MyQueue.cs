﻿using System;

namespace MyQueue
{
    public sealed class MyQueue<T>
    {
        private const int DefaultCapacity = 4;
        
        public int Count { get; private set; }
        private int myCapacity;
        private int myFirstElementPosition;
        private int myLastElementPosition;
        private T[] myArray;

        
        public MyQueue()
        {
            myCapacity = DefaultCapacity;
            myArray = new T[myCapacity];
            Clear();
        }
        
        public MyQueue(int capacity)
        {
            myCapacity = capacity;
            myArray = new T[myCapacity];
            Clear();
        }
        
        public void Enqueue(T item)
        {
            if (Count++ == myCapacity)
                RebaseArray();

            if (++myLastElementPosition >= myCapacity)
                myLastElementPosition = 0;

            myArray[myLastElementPosition] = item;
        }

        public T Dequeue()
        {
            if (Count-- == 0)
                throw new InvalidOperationException();

            var result = myArray[myFirstElementPosition];

            if (++myFirstElementPosition == myCapacity)
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

            Array.Copy(myArray, myFirstElementPosition, newArray, 0, myCapacity - myFirstElementPosition);
            Array.Copy(myArray, 0, newArray, myCapacity - myFirstElementPosition, myFirstElementPosition);

            myArray = newArray;
            myLastElementPosition = myCapacity;
            myCapacity *= 2;
        }
    }
}