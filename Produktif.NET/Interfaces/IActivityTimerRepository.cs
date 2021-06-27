using Produktif.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Produktif.Interfaces
{
    public interface IActivityTimerRepository : IRepository<ActivityTimer>
    {
        //Add custom methods here if needed
        IEnumerable<ActivityTimer> ByUserActivityId(long id);
        
    }
}
