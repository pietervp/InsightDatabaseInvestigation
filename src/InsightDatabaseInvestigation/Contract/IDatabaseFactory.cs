using System.Data;

namespace InsightDatabaseInvestigation.Contract
{
    public interface IDatabaseFactory
    {
        IDbConnection GetOpenConnection();
    }
}