using System;
using Microsoft.SPOT;

namespace BlindPeople.DomainModel
{
    // This is basically a queue with a fixed maximum length.
    // It is implemented using a circular array.
    public class LimitedList<T>
    {
    	int latest;
    	int currentSize;
        int maxSize;
		
        // The list is from but not including latest - currentSize up to latest.
        // So when currentSize <= latest, the list is q(latest-currentSize..latest],
        // otherwise list is q((latest-currentSize)%maxSize..maxSize) ++ q[0..latest]
        T[] q;
		
        public LimitedList(int maxSize)
        {
        	latest = 0;
        	currentSize = 0;
            this.maxSize = maxSize;
            q = new T[maxSize];
        }
		
        // Adds a new element to the queue, if already full then
        // the oldest element is discarded
        public void add(T x)
        {
        	latest = (latest + 1) % maxSize;
        	q[latest] = x;
        	if (currentSize < maxSize) currentSize += 1;
        }
        
        // returns how long the list is currently
        public int size()
        {
        	return currentSize;
        }
		
		// Returns the nth most recently added element.
		// The most recent element is 0, then 1, etc.
		// If n > size, throws an IndexOutOfRangeException.
		public T at(int n)
		{
			if (n < 0 || n >= currentSize)
			{
				throw new IndexOutOfRangeException();
			}
			
			return q[(latest + maxSize - n) % maxSize];
		}
    }
}
