using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace RremotingDemo.ContextDemo
{
    [Serializable]
    public class EventId : ILogicalThreadAffinative
    {
        public Guid Value { get; } = Guid.NewGuid();

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    [Serializable]
    public class Record
    {
        public string Id { get; }

        public string ThreadId { get; }

        public string Owner { get; }

        public string Msg { get; }

        public Record(string owner, string msg)
        {
            if (CallContext.HostContext is EventId id)
            {
                Id = id.ToString();
            }

            ThreadId = Thread.CurrentThread.ManagedThreadId.ToString();

            Owner = owner;
            Msg = msg;
        }

        public override string ToString()
        {
            return $"{(string.IsNullOrEmpty(Id) ? "no id" : Id),-40}{Owner,-20}{ThreadId,-5}{Msg}";
        }
    }

    public class ContextDemoService : MarshalByRefObject
    {
        ConcurrentQueue<Record> _printQueue = new ConcurrentQueue<Record>();
        ConcurrentBag<Record> _logRecords = new ConcurrentBag<Record>();

        public ContextDemoService()
        {
            new Thread(() =>
            {
                while (true)
                {
                    if (!_printQueue.TryDequeue(out var record))
                    {
                        Thread.Sleep(100);
                        continue;
                    }

                    Console.WriteLine(record);
                }
            }).Start();
        }

        private void PrintLog(Record record)
        {
            _printQueue.Enqueue(record);
            _logRecords.Add(record);
        }

        public void SomeTest(string str)
        {
            PrintLog(new Record("Response Thread", str));

            new Thread(() =>
            {
                PrintLog(new Record("New Thread", str));
            }).Start();

            ThreadPool.QueueUserWorkItem(s =>
            {
                PrintLog(new Record("ThreadPool Thread", str));
            });
        }

        public IEnumerable<Record> GetLogRecords()
        {
            var records = _logRecords.ToArray();

            _logRecords = new ConcurrentBag<Record>();

            Console.WriteLine();

            foreach (var recordGroup in records.GroupBy(r => r.Id))
            {
                Console.WriteLine();
                foreach (var record in recordGroup.OrderBy(r => r.Owner))
                {
                    Console.WriteLine(record);
                }
            }

            return records;
        }
    }
}
