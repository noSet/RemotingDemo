using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;

namespace RremotingDemo.Server
{
    class TestChannelSinkProvider : IServerChannelSinkProvider
    {
        public IServerChannelSinkProvider Next { get; set; }

        public IServerChannelSink CreateSink(IChannelReceiver channel)
        {
            IServerChannelSink next = Next.CreateSink(channel);

            TestChannelSink report = new TestChannelSink(next);

            return report;
        }

        public void GetChannelData(IChannelDataStore channelData)
        {
            //throw new NotImplementedException();
        }
    }
}
