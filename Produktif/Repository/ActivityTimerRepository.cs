using Produktif.Interfaces;
using Produktif.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Produktif
{
    public class ActivityTimerRepository : RepositoryBase<ActivityTimer>, IActivityTimerRepository
    {
        public ActivityTimerRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
        public UserActivity ByName(string name)
        {
            return base.Database.UserActivity.Single(x => x.Name == name);
        }

        public IEnumerable<ActivityTimer> ByUserActivityId(long id)
        {
            return (IEnumerable<ActivityTimer>)base.Database.ActivityTimer.Select(x => x.UserActivityId == id);
        }
    }
}
