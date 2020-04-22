using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using System.Threading;
using RremotingDemo.ContextDemo;
using RremotingDemo.EventDemo;
using RremotingDemo.LifetimeDemo;

namespace RremotingDemo.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(400);

            var hashtable = new Hashtable
            {
                ["port"] = 0
            };

            TcpChannel channel = new TcpChannel(hashtable, new BinaryClientFormatterSinkProvider(), new BinaryServerFormatterSinkProvider() { TypeFilterLevel = TypeFilterLevel.Full });
            ChannelServices.RegisterChannel(channel, false);
            RemotingConfiguration.RegisterWellKnownClientType(typeof(ServerActivatedSingleCallService), "tcp://localhost:8226/ServerActivatedSingleCallService");
            RemotingConfiguration.RegisterWellKnownClientType(typeof(ServerActivatedSingletonService), "tcp://localhost:8226/ServerActivatedSingletonService");
            RemotingConfiguration.RegisterWellKnownClientType(typeof(ContextDemoService), "tcp://localhost:8226/ContextDemoService");
            RemotingConfiguration.RegisterWellKnownClientType(typeof(CountingService), "tcp://localhost:8226/CountingService");

            //new ContextTest().Run();
            //new LifetimeTest().Run();
            new EventTest().Run();

            Console.ReadLine();
        }
    }
}
