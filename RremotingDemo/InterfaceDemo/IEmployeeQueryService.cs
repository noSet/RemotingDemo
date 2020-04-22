using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RremotingDemo.InterfaceDemo
{
    public interface IEmployeeQueryService
    {
        string QueryName(int id);
    }
}
