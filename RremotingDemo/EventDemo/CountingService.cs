using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RremotingDemo.EventDemo
{
    [Serializable]
    public class CountEventArgs : EventArgs
    {
        public int Count { get; }

        public CountEventArgs(int count)
        {
            Count = count;
        }
    }

    public class CountingService : MarshalByRefObject
    {
        private int _count = 0;

        public event EventHandler<CountEventArgs> CountEvent;

        public CountingService()
        {
            Thread thread = new Thread(Counting);
            thread.Start();
        }

        public void Counting()
        {
            while (true)
            {
                Thread.Sleep(500);

                CountEvent?.Invoke(this, new CountEventArgs(++_count));
                Console.WriteLine($"计数-{_count}");
            }
        }
    }
}
