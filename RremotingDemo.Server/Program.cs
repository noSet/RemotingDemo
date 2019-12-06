﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace RremotingDemo.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var hashtable = new Hashtable
            {
                ["port"] = 8226
            };

            IServerChannelSinkProvider sinkProvider = new TestChannelSinkProvider()
                .AddLastServerChannelSinkProvider(new ReportChannelSinkProvider())
                .AddLastServerChannelSinkProvider(new BinaryServerFormatterSinkProvider())
                //.AddLastServerChannelSinkProvider(new BinaryServerFormatterSinkProvider())
                .AddLastServerChannelSinkProvider(new TestChannelSinkProvider());

            TcpChannel channel = new TcpChannel(hashtable, null, sinkProvider);
            ChannelServices.RegisterChannel(channel, false);

            //RemotingConfiguration.Configure(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, false);

            RemotingConfiguration.RegisterActivatedServiceType(typeof(ClientActivatedService));
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(ServerActivatedSingleCallService), nameof(ServerActivatedSingleCallService), WellKnownObjectMode.SingleCall);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(ServerActivatedSingletonService), nameof(ServerActivatedSingletonService), WellKnownObjectMode.Singleton);

            Console.WriteLine("started ...");
            Console.Read();
        }
    }

    internal static class ServerChannelSinkProviderExtensions
    {
        internal static IServerChannelSinkProvider AddLastServerChannelSinkProvider(this IServerChannelSinkProvider serverChannelSinkProvider, IServerChannelSinkProvider nextServerChannelSinkProvider)
        {
            if (serverChannelSinkProvider is null)
            {
                throw new ArgumentNullException(nameof(serverChannelSinkProvider));
            }

            if (nextServerChannelSinkProvider is null)
            {
                throw new ArgumentNullException(nameof(nextServerChannelSinkProvider));
            }

            IServerChannelSinkProvider current = serverChannelSinkProvider;

            while (current.Next != null)
            {
                current = current.Next;
            }

            current.Next = nextServerChannelSinkProvider;

            return serverChannelSinkProvider;
        }
    }
}