using System;
using Microsoft.SPOT;

namespace BlindPeople.DomainModel
{
    //this class describes a list that is implemented as a circular array.
    class LimitedList<T>
    {
        int maxSize;
        int start;
        int stop;

        //list is q[(start mod maxSize)..(stop mod maxSize))
        T[] q;


        public LimitedList(int maxSize)
        {
            this.maxSize = maxSize;
            q = new T[maxSize];
        }

        //adds to the top of the list
        //if array is full then the first value of the list is replaced.
        public void add(T x)
        {
            q[stop] = x;
            stop = (stop + 1) % maxSize;
            if (start == stop) start = (start + 1) % maxSize;
        }

        //peeks at the value at the nth-from-top position in the list
        //if list is empty throws IndexOutOfRangeException
        // if n is out of bounds throws ArgumentOutOfRangeException
        public T peek(int n)
        {
            int size = (stop - start + maxSize) % maxSize;
            int pos = (start + n) % maxSize;
            if (size == 0)
            {
                throw new IndexOutOfRangeException();
            }
            else if (n > size)
            {
                throw new ArgumentOutOfRangeException();
            }
            return q[pos];
        }


    }
}
