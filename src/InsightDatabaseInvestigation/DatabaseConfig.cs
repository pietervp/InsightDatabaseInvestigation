using System;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.SQLite;
using System.IO;

namespace InsightDatabaseInvestigation
{
    public class DatabaseConfig
    {
        private const string InsertDummyDataSql = @".\Scripts\insertDummyData.sql";
        private const string CreateTablesCommandSql = @".\Scripts\createCommand.sql";

        public static IDbConnection CreateConnectionAndInitDatabase()
        {
            var dbConnection = GetDbConnection();
            InitDatabase(dbConnection);
            return dbConnection;
        }

        public static void CreateSampleData(IDbConnection connection)
        {
            //setup database tables
            var dbCommand = connection.CreateCommand();

            //split on [NEXT] as we cannot execute multiple commands
            var commands = File.ReadAllText(InsertDummyDataSql).Split(new []{"[NEXT]"}, StringSplitOptions.RemoveEmptyEntries);

            foreach (string command in commands)
            {
                dbCommand.CommandText = command;

                if (connection.State != ConnectionState.Open)
                    connection.Open();

                dbCommand.ExecuteNonQuery();
            }
        }

        private static void InitDatabase(IDbConnection connection)
        {
            //setup database tables
            var dbCommand = connection.CreateCommand();
            dbCommand.CommandText = File.ReadAllText(CreateTablesCommandSql);

            if(connection.State != ConnectionState.Open)
                connection.Open();

            dbCommand.ExecuteNonQuery();
        }

        private static IDbConnection GetDbConnection()
        {
            //reset existing db
            if(File.Exists(@".\data.db"))
                File.Delete(@".\data.db");

            //create fresh db
            SQLiteConnection.CreateFile(@".\data.db");

            return new OdbcConnection(ConfigurationManager.ConnectionStrings["SQLite"].ConnectionString);
        }
    }
}