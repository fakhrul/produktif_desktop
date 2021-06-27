using Produktif.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Produktif.Interfaces
{
    public interface IActiveAppRepository : IRepository<ActiveApp>
    {
        //Add custom methods here if needed
        ActiveApp ByLatestDateTime();
        IEnumerable<ActiveApp> SinceInHour(int hour);
    }
}
