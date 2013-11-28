using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace InsightDatabaseInvestigation
{
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
}