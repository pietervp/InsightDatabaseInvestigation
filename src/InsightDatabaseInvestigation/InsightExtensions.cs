using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace InsightDatabaseInvestigation
{
    public static class InsightExtensions
    {
        public static IList<T> QueryOneToMany<T>(this IDbConnection connection, string sql)
        {
            var foreignKeys = typeof (T).GetProperties().Where(x => x.CanWrite && x.GetGetMethod().IsVirtual && x.PropertyType.GetInterface(typeof (ICollection).Name) != null);
            return null;
        }
    }
}