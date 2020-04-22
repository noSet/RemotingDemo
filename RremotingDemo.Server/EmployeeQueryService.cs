using System;
using RremotingDemo.InterfaceDemo;

namespace RremotingDemo.Server
{
    public class EmployeeQueryService : MarshalByRefObject, IEmployeeQueryService
    {
        public string QueryName(int id) => "张伟";
    }
}
