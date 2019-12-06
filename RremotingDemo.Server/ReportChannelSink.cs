using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace RremotingDemo.Server
{
    public class ReportChannelSink : BaseChannelObjectWithProperties, IServerChannelSink
    {
        public IServerChannelSink NextChannelSink { get; }

        public ReportChannelSink(IServerChannelSink nextChannelSink)
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

        public ServerProcessing ProcessMessage(
            IServerChannelSinkStack sinkStack,
            IMessage requestMsg,
            ITransportHeaders requestHeaders,
            Stream requestStream,
            out IMessage responseMsg,
            out ITransportHeaders responseHeaders,
            out Stream responseStream)
        {
            Console.WriteLine("ReportChannelSink开始执行");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var processing = NextChannelSink.ProcessMessage(sinkStack, requestMsg, requestHeaders, requestStream, out responseMsg, out responseHeaders, out responseStream);

            stopwatch.Stop();

            Console.WriteLine($"调用方法{requestMsg?.Properties?["__MethodName"]}, 耗时{stopwatch.ElapsedMilliseconds}, 请求大小requestStream.Length, 响应大小{responseStream?.Length}");
            Console.WriteLine("ReportChannelSink结束执行");

            return processing;
        }
    }
}
