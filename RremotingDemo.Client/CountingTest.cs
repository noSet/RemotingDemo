using System;
using System.IO;
using System.Runtime.Serialization.Formatters;

namespace RremotingDemo.Client
{
    public class CountingTest : MarshalByRefObject
    {
        public readonly TextWriter Out = Console.Out;

        private readonly CountingService _counting;

        public CountingTest()
        {
            _counting = new CountingService();
        }

        public void Run()
        {
            _counting.CountEvent += Counting_CountEvent;
        }

        /// <summary>
        /// 远程事件必须满足以下条件
        /// <para>
        /// 必须是 public 访问级别;
        /// </para>
        /// <para>
        /// 事件所在的程序集要被客户端和服务端同时引用
        /// </para>
        /// <para>
        /// 需要将序列化级别设置成<see cref="TypeFilterLevel.Full"/>
        /// </para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Counting_CountEvent(object sender, CountEventArgs e)
        {
            Out.WriteLine($"服务端触发：{e.Count}");
        }
    }

    /// <summary>
    /// 这个事件所在的程序集，需要服务端和客户端同时都引用
    /// </summary>
    public class Common
    {
        public static void Counting_CountEvent(object sender, CountEventArgs e)
        {
            Console.WriteLine($"服务端触发：{e.Count}");
        }
    }
}
