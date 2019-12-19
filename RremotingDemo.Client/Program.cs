using RremotingDemo.LifetimeDemo;
using System.Runtime.Remoting;
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

            new LifetimeTest().Run();
        }
    }
}
