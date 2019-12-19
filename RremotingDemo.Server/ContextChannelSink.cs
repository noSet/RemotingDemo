using RremotingDemo.ContextDemo;
using System;
using System.IO;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace RremotingDemo.Server
{
    public class ContextChannelSink : BaseChannelObjectWithProperties, IServerChannelSink
    {
        public IServerChannelSink NextChannelSink { get; }

        public ContextChannelSink(IServerChannelSink nextChannelSink)
        {
            NextChannelSink = nextChannelSink;
        }

        public void AsyncProcessResponse(IServerResponseChannelSinkStack sinkStack, object state, IMessage msg, ITransportHeaders headers, Stream stream)
        {
            throw new NotImplementedException();
        }

        public Stream GetResponseStream(IServerResponseChannelSinkStack sinkStack, object state, IMessage msg, ITransportHeaders headers)
        {
            throw new NotImplementedException();
        }

        public ServerProcessing ProcessMessage(IServerChannelSinkStack sinkStack, IMessage requestMsg, ITransportHeaders requestHeaders, Stream requestStream, out IMessage responseMsg, out ITransportHeaders responseHeaders, out Stream responseStream)
        {
            var processing = NextChannelSink.ProcessMessage(sinkStack, requestMsg, requestHeaders, requestStream, out responseMsg, out responseHeaders, out responseStream);
            return processing;
        }
    }
}
