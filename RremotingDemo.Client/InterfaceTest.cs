using System;
using RremotingDemo.InterfaceDemo;

namespace RremotingDemo.Client
{
    public class Service
    {
        public static T GetRemotingProxy<T>()
        {
            // todo url
            return (T)Activator.GetObject(typeof(T), "tcp://localhost:8226/EmployeeQueryService");
        }
    }

    public class InterfaceTest
    {
        public void Run()
        {
            IEmployeeQueryService employeeQueryService = Service.GetRemotingProxy<IEmployeeQueryService>();
            Console.WriteLine(employeeQueryService.QueryName(1));
        }
    }
}
