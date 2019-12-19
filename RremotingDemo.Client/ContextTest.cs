using RremotingDemo.ContextDemo;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;

namespace RremotingDemo.Client
{
    public class ContextTest
    {
        public void Run()
        {
            ContextDemoService service = new ContextDemoService();
            int count = 700;
            ConcurrentDictionary<string, string> idmapping = new ConcurrentDictionary<string, string>();

            var result = Parallel.For(0, count, i =>
            {
                var id = new EventId();
                CallContext.HostContext = id;
                idmapping.TryAdd(id.Value.ToString(), id.Value.ToString());
                Console.WriteLine($"{id,-50}{id.Value.ToString()}");
                service.SomeTest(id.Value.ToString());
            });

            while (idmapping.Count != count)
            {
                Thread.Sleep(100);
            }

            var records = service.GetLogRecords();

            // 验证日志ID不能为空
            Debug.Assert(records.All(r => !string.IsNullOrEmpty(r.Id)));

            // 验证请求的次数和日志ID的数量一致
            Debug.Assert(records.Distinct(new RecodIDEqualityComparer()).Count() == count);

            // 验证日志ID和消息一致
            records.AsParallel().ForAll(r => Debug.Assert(r.Id == r.Msg));

            // 验证日志每次请求日志ID一致且每次请求有三条记录
            foreach (var group in records.GroupBy(r => r.Id))
            {
                Debug.Assert(group.Count() == 3);
                Debug.Assert(group.Distinct(new RecodIDEqualityComparer()).Count() == 1);
            }
        }

        public class RecodIDEqualityComparer : IEqualityComparer<Record>
        {
            public bool Equals(Record x, Record y)
            {
                return x?.Id == y?.Id;
            }

            public int GetHashCode(Record obj)
            {
                return obj.Id.GetHashCode();
            }
        }
    }
}
