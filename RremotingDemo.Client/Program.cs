using System;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;

namespace RremotingDemo.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(400);

            TcpChannel channel = new TcpChannel();
            ChannelServices.RegisterChannel(channel, false);
            RemotingConfiguration.RegisterWellKnownClientType(typeof(ServerActivatedSingleCallService), "tcp://localhost:8226/ServerActivatedSingleCallService");
            RemotingConfiguration.RegisterWellKnownClientType(typeof(ServerActivatedSingletonService), "tcp://localhost:8226/ServerActivatedSingletonService");

            while (true)
            {
                // “生命周期”可以由客户端自己管理（客户端持有实例的引用，若客户端不释放则一直存在，但是Remoting真正的生命周期还待研究）
                CallClientActivatedService();
                CallServerActivatedSingleCallService();
                CallServerActivatedSingletonService();

                Console.ReadKey();
                Console.WriteLine();
            }
        }

        private static void CallClientActivatedService()
        {
            var clientActivatedService = (ClientActivatedService)Activator.CreateInstance(
                typeof(ClientActivatedService),
                null,
                new[] { new UrlAttribute("tcp://localhost:8226") });

            var clientActivatedService2 = (ClientActivatedService)Activator.CreateInstance(
                typeof(ClientActivatedService),
                null,
                new[] { new UrlAttribute("tcp://localhost:8226") });

            var str3 = clientActivatedService2.DemoMethod("a");
            var str4 = clientActivatedService2.DemoMethod("a");
            var str1 = clientActivatedService.DemoMethod("a");
            var str2 = clientActivatedService.DemoMethod("a");

            Debug.Assert(str1 == str2);
            Debug.Assert(str3 == str4);
            Debug.Assert(str1 != str3);
        }

        private static void CallServerActivatedSingletonService()
        {
            var serverActivatedSingletonService = new ServerActivatedSingletonService();
            var serverActivatedSingletonService2 = new ServerActivatedSingletonService();

            var str1 = serverActivatedSingletonService.DemoMethod("a");
            var str2 = serverActivatedSingletonService.DemoMethod("a");
            var str3 = serverActivatedSingletonService2.DemoMethod("a");
            var str4 = serverActivatedSingletonService2.DemoMethod("a");

            Debug.Assert(str1 == str2);
            Debug.Assert(str1 == str4);
            Debug.Assert(str1 == str3);
        }

        private static void CallServerActivatedSingleCallService()
        {
            var serverActivatedSingleCallService = new ServerActivatedSingleCallService();
            var serverActivatedSingleCallService2 = new ServerActivatedSingleCallService();

            var str1 = serverActivatedSingleCallService.DemoMethod("a");
            var str2 = serverActivatedSingleCallService.DemoMethod("a");
            var str3 = serverActivatedSingleCallService2.DemoMethod("a");
            var str4 = serverActivatedSingleCallService2.DemoMethod("a");

            Debug.Assert(str1 != str2);
            Debug.Assert(str1 != str4);
            Debug.Assert(str1 != str3);
        }
    }
}
