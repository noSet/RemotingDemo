using System;
using System.IO;
using System.Runtime.Serialization.Formatters;
using RremotingDemo.EventDemo;

namespace RremotingDemo.Client
{
    public class EventTest
    {
        public readonly TextWriter Out = Console.Out;

        private readonly CountingService _countingService = new CountingService();
        private readonly CountEventWrapper _countEventWrapper = new CountEventWrapper();


        public EventTest()
        {
        }

        public void Run()
        {
            // CountEventWrapper 类所在程序集是客户端服务端都能引用到的地方
            // CountEventWrapper 将客户端中的方法注册到包装类中包装的事件
            // CountingService   真正的服务端事件只要注册包装类的包装方法，该方法会真正执行客户端的事件
            _countEventWrapper.CountEvent += this.Counting_CountEvent;
            _countingService.CountEvent += _countEventWrapper.WrapperEvent;
            Console.WriteLine("register event...");

            Console.ReadKey();

            // 特别注意不要造成内存泄露
            _countingService.CountEvent -= _countEventWrapper.WrapperEvent;
            _countEventWrapper.CountEvent -= this.Counting_CountEvent;
            Console.WriteLine("unregister event...");
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
        private void Counting_CountEvent(object sender, CountEventArgs e)
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
