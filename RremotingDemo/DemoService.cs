using System;

namespace RremotingDemo
{
    public class DemoService : MarshalByRefObject
    {
        public string Id { get; }

        public DemoService()
            : this(null)
        {
        }

        public DemoService(string id)
        {
            Id = id ?? Guid.NewGuid().ToString();
        }

        public virtual string DemoMethod(string demoStr)
        {
            demoStr = $"{Id} | {demoStr}";

            Console.WriteLine($"{this.GetType().Name}结果：{demoStr}");

            return demoStr;
        }
    }
}
