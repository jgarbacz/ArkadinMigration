using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

// A barebones non-blocking lock. User is responsible for releasing lock only when TryAcquireLock() returns true.
namespace MVM
{
    public class SimpleLock
    {
        public int isLocked=0; //0=unlocked, 1=locked
        public bool TryAcquireLock()
        {
            if (Interlocked.CompareExchange(ref isLocked, 1, 0) == 0) return true;
            return false;
        }
        public void ReleaseLock()
        {
            Interlocked.Decrement(ref isLocked);
        }
    }
}
