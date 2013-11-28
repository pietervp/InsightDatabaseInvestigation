using System;
using System.Collections.Generic;
using System.Linq;
using Insight.Database;
using InsightDatabaseInvestigation.Contract;

namespace InsightDatabaseInvestigation
{
    public class UserRepository : IUserRepository
    {
        public IDatabaseFactory DatabaseFactory { get; set; }

        public UserRepository(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
        }

        public IList<User> GetAllUsers()
        {
            var multipleResultSetsCommand = @"GetUsersGraph";

            var res = DatabaseFactory.GetOpenConnection().QueryResults<User, UserGroup, Membership>(multipleResultSetsCommand);

            foreach (var membership in res.Set3)
            {
                membership.User = res.Set1.First(x => x.UserID == membership.UserID);
                membership.UserGroup = res.Set2.First(x => x.GroupID == membership.GroupID);
                membership.UserGroup.Users.Add(membership.User);
                membership.User.UserGroups.Add(membership.UserGroup);
            }

            return res.Set1;
        }

        public User GetUserById(int id)
        {
            throw new NotImplementedException();
        }

        public IList<User> GetUserByCriteria(Func<User, bool> whereClause)
        {
            throw new NotImplementedException();
        }
    }
}