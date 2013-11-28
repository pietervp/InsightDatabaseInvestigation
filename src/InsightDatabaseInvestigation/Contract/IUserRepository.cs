using System;
using System.Collections.Generic;

namespace InsightDatabaseInvestigation
{
    public interface IUserRepository
    {
        IList<User> GetAllUsers();
        User GetUserById(int id);
        IList<User> GetUserByCriteria(Func<User, bool> whereClause);
    }
}