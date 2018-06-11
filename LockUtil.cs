using System;
using System.Threading;

namespace MyQueue
{
    public static class LockUtil
    {
        public struct ReaderLockDisposable : IDisposable
        {
            private readonly ReaderWriterLockSlim @lock;

            public ReaderLockDisposable(ReaderWriterLockSlim @lock)
            {
                @lock.EnterReadLock();
                this.@lock = @lock;
            }
           
            public void Dispose()
            {
                @lock.ExitReadLock();
            }
        }

        public struct WriterLockDisposable : IDisposable
        {
            private readonly ReaderWriterLockSlim @lock;

            public WriterLockDisposable(ReaderWriterLockSlim @lock)
            {
                @lock.EnterWriteLock();
                this.@lock = @lock;
            }
           
            public void Dispose()
            {
                @lock.ExitWriteLock();
            }
        }
        
        public static ReaderLockDisposable UsingWriterLock(this ReaderWriterLockSlim @lock)
        {
            return new ReaderLockDisposable(@lock);
        }

        public static WriterLockDisposable UsingReaderLock(this ReaderWriterLockSlim @lock)
        {
            return new WriterLockDisposable(@lock);
        }
    }
}