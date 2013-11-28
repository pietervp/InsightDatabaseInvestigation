using System;
using System.Data;
using System.IO;
using InsightDatabaseInvestigation.Contract;

namespace InsightDatabaseInvestigation
{
    public class SQliteDatabaseInitializer : IDatabaseInitializer
    {
        private const string InsertDummyDataSql = @".\Scripts\SQlite\insertDummyData.sql";
        private const string CreateTablesCommandSql = @".\Scripts\SQlite\createCommand.sql";

        private readonly IDatabaseFactory _databaseFactory;

        public SQliteDatabaseInitializer(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public void Create()
        {
            //setup database tables
            var connection = _databaseFactory.GetOpenConnection();
            var dbCommand = connection.CreateCommand();
            dbCommand.CommandText = File.ReadAllText(CreateTablesCommandSql);

            if (connection.State != ConnectionState.Open)
                connection.Open();

            dbCommand.ExecuteNonQuery();
        }

        public void Seed()
        {
            //setup database tables
            var connection = _databaseFactory.GetOpenConnection();
            var dbCommand = connection.CreateCommand();

            //split on [NEXT] as we cannot execute multiple commands
            var commands = File.ReadAllText(InsertDummyDataSql).Split(new[] { "[NEXT]" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string command in commands)
            {
                dbCommand.CommandText = command;

                if (connection.State != ConnectionState.Open)
                    connection.Open();

                dbCommand.ExecuteNonQuery();
            }   
        }
    }
}