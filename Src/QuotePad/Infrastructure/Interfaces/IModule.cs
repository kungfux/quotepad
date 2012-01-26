using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuotePad.Infrastructure.Interfaces
{
    public interface IModule
    {
        string Header { get; }
        string HeaderExtended { get; }
    }
}
