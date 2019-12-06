using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace RremotingDemo.Server
{
    public class TestChannelSink : BaseChannelObjectWithProperties, IServerChannelSink
    {
        public IServerChannelSink NextChannelSink { get; }

        public TestChannelSink(IServerChannelSink nextChannelSink)
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
            Console.WriteLine("TestChannelSink开始执行");
            var processing = NextChannelSink.ProcessMessage(sinkStack, requestMsg, requestHeaders, requestStream, out responseMsg, out responseHeaders, out responseStream);
            Console.WriteLine("TestChannelSink结束执行");

            return processing;
        }
    }
}
