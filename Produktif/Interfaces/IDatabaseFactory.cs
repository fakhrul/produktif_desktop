using System;
using System.Collections.Generic;
using System.Text;

namespace Produktif.Interfaces
{
    public interface IDatabaseFactory : IDisposable
    {
        Database Get();
    }
}
