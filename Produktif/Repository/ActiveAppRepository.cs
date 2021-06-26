using Produktif.Interfaces;
using Produktif.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Produktif.Repository
{

    public class ActiveAppRepository : RepositoryBase<ActiveApp>, IActiveAppRepository
    {
        public ActiveAppRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
        public ActiveApp ByLatestDateTime()
        {
            return base.Database.ActiveApp.OrderByDescending(t => t.StartDateTime).FirstOrDefault();
        }

        public IEnumerable<ActiveApp> Pass8hour()
        {
            return base.Database.ActiveApp.Where(c => c.StartDateTime > DateTime.Now.AddHours(-24));
        }
    }
}
