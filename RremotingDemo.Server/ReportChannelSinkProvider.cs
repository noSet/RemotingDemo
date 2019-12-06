using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;

namespace RremotingDemo.Server
{
    public class ReportChannelSinkProvider : IServerChannelSinkProvider
    {
        public IServerChannelSinkProvider Next { get; set; }

        public ReportChannelSinkProvider()
        {

        }

        public IServerChannelSink CreateSink(IChannelReceiver channel)
        {
            IServerChannelSink next = Next.CreateSink(channel);

            ReportChannelSink report = new ReportChannelSink(next);

            return report;
        }

        public void GetChannelData(IChannelDataStore channelData)
        {
            //throw new NotImplementedException();
        }
    }
}
