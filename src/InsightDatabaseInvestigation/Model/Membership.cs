namespace InsightDatabaseInvestigation
{
    public class Membership
    {
        public int MembershipID { set; get; }
        
        public int GroupID { set; get; }
        public int UserID { set; get; }

        public User User { get; set; }
        public UserGroup UserGroup { get; set; }
    }
}