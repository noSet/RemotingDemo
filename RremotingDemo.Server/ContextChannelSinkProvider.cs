using System.Runtime.Remoting.Channels;

namespace RremotingDemo.Server
{
    public class ContextChannelSinkProvider : IServerChannelSinkProvider
    {
        public IServerChannelSinkProvider Next { get; set; }

        public IServerChannelSink CreateSink(IChannelReceiver channel)
        {
            IServerChannelSink next = Next.CreateSink(channel);

            ContextChannelSink context = new ContextChannelSink(next);

            return context;
        }

        public void GetChannelData(IChannelDataStore channelData)
        {
        }
    }
}
