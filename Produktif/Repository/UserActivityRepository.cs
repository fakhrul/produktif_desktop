using Produktif.Interfaces;
using Produktif.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq; 

namespace Produktif
{
    public class UserActivityRepository : RepositoryBase<UserActivity>, IUserActivityRepository
    {
        public UserActivityRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
        public UserActivity ByName(string name)
        {
            return base.Database.UserActivity.Single(x => x.Name == name);
        }

        public IEnumerable<UserActivity> FindAllActiveStatus()
        {
            return base.Database.UserActivity.Where(c => c.LatestStatus !=  ActivityStatusType.Stop);
        }

        public IEnumerable<UserActivity> FindAllInProgressStatus()
        {
            return base.Database.UserActivity.Where(c => c.LatestStatus == ActivityStatusType.InProgress);
        }

        public bool IsInProgress()
        {
            var status = base.Database.UserActivity.Where(c => c.LatestStatus == ActivityStatusType.InProgress).FirstOrDefault();
            if (status != null)
                return true;
            return false;
        }

        public IEnumerable<UserActivity> SinceInHour(int hour)
        {
            return base.Database.UserActivity.Where(c => c.CreatedDateTime > DateTime.Now.AddHours(hour * -1));
        }
    }

}
