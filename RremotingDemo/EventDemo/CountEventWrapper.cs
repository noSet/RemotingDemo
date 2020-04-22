using System;

namespace RremotingDemo.EventDemo
{
    public class CountEventWrapper : MarshalByRefObject
    {
        public event EventHandler<CountEventArgs> CountEvent;

        public void WrapperEvent(object sender, CountEventArgs e)
        {
            CountEvent?.Invoke(sender, e);
        }
    }
}
