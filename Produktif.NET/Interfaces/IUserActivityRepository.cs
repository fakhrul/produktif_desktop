using Produktif.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Produktif.Interfaces
{
    public interface IUserActivityRepository : IRepository<UserActivity>
    {
        //Add custom methods here if needed
        UserActivity ByName(string name);
        IEnumerable<UserActivity> FindAllActiveStatus();
        IEnumerable<UserActivity> FindAllInProgressStatus();
        bool IsInProgress();
    }
}
