using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using Insight.Database;

namespace InsightDatabaseInvestigation
{
    class Program
    {
        static void Main()
        {
            // recreate database and get connection
            var connection = DatabaseConfig.CreateConnectionAndInitDatabase();

            //fill database with sample data
            DatabaseConfig.CreateSampleData(connection);

            //different implementations
            var usingProjection = UsingProjection(connection);
            var usingMultipleResultSets = UsingMultipleResultSets(connection);
            var usingMultipleCommands = UsingMultipleCommands(connection);

            //check results
            foreach (IList<User> users in new[]{usingProjection, /*usingMultipleResultSets,*/ usingMultipleCommands})
            {
                // Get the first user
                var firstUser = users.First();

                // Get all the userGroups they belong to
                var userGroups = firstUser.UserGroups;

                // Get the first user group and find out all the users in that user group.
                var usersInFirstUserGroup = userGroups.First().Users;
            }
        }

        private static IList<User> UsingMultipleResultSets(IDbConnection connection)
        {
            //does not work with odbc driver ('only one SQL statement allowed') would be best solution
            return new List<User>();

            var multipleResultSetsCommand = @"  Select * from [User];
                                                Select * from [UserGroup];
                                                Select * from [Membership];";

            var res = connection.QueryResults<User, UserGroup, Membership>(multipleResultSetsCommand, commandType: CommandType.Text);

            foreach (var membership in res.Set3)
            {
                membership.User = res.Set1.First(x => x.UserID == membership.UserID);
                membership.UserGroup = res.Set2.First(x => x.GroupID == membership.GroupID);
                membership.UserGroup.Users.Add(membership.User);
                membership.User.UserGroups.Add(membership.UserGroup);
            }

            return res.Set1;
        }

        private static IList<User> UsingMultipleCommands(IDbConnection connection)
        {
            var userQuery = "Select * from [User]";
            var userGroupQuery = "Select * from [UserGroup]";
            var membershipQuery = "Select * from [Membership]";

            var users = connection.Query<User>(userQuery, commandType: CommandType.Text);
            var userGroups = connection.Query<UserGroup>(userGroupQuery, commandType: CommandType.Text);
            var memberships = connection.Query<Membership>(membershipQuery, commandType: CommandType.Text);

            foreach (var membership in memberships)
            {
                membership.User = users.First(x => x.UserID == membership.UserID);
                membership.UserGroup = userGroups.First(x => x.GroupID == membership.GroupID);
                membership.UserGroup.Users.Add(membership.User);
                membership.User.UserGroups.Add(membership.UserGroup);
            }

            return users;
        }

        private static List<User> UsingProjection(IDbConnection connection)
        {
            var users = new List<User>();

            var projectCommandSql = @"  Select  mem.GroupID, mem.UserId, us.*, ug.* 
	                                    from [user] us
		                                    left join membership mem
			                                    on us.UserID = mem.UserID
		                                    inner join usergroup ug
			                                    on mem.GroupID = ug.GroupID";

            connection.ForEach<Membership>(projectCommandSql,
            null, membership =>
            {
                membership.UserGroup.Users.Add(membership.User);
                membership.User.UserGroups.Add(membership.UserGroup);
                users.Add(membership.User);
            }, typeof (Graph<Membership, User, UserGroup>), CommandType.Text);

            var distinctUsers = new List<User>();

            foreach (var user in users)
            {
                User existingUser;

                if ((existingUser = distinctUsers.FirstOrDefault(x => x.UserID == user.UserID)) == null)
                {
                    distinctUsers.Add(user);
                    existingUser = user;
                }

                foreach (var userGroup in user.UserGroups.ToList())
                {
                    if (existingUser.UserGroups.All(x => x.GroupID != userGroup.GroupID))
                        existingUser.UserGroups.Add(userGroup);
                }
            }

            return distinctUsers;
        }
    }

    public class Membership
    {
        public int MembershipID { set; get; }
        
        public int GroupID { set; get; }
        public int UserID { set; get; }

        public User User { get; set; }
        public UserGroup UserGroup { get; set; }
    }

    public class UserGroup
    {
        public UserGroup()
        {
            Users = new Collection<User>();
        }

        public int GroupID { set; get; }
        public string Name { set; get; }
        public string Comment { set; get; }

        public virtual ICollection<User> Users { set; get; }
    }

    public class User
    {
        public User()
        {
            UserGroups = new Collection<UserGroup>();
        }

        public int UserID { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string Email { set; get; }
        public string Phone { set; get; }
        public string Comment { set; get; }

        public virtual ICollection<UserGroup> UserGroups { set; get; }
    }

}