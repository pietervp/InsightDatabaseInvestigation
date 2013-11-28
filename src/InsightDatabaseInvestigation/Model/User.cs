using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace InsightDatabaseInvestigation
{
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