using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using InsightDatabaseInvestigation.Contract;

namespace InsightDatabaseInvestigation
{
    public class SqlDatabaseFactory : IDatabaseFactory
    {
        public IDbConnection GetOpenConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["SQLExpress"].ToString());
        }
    }
}