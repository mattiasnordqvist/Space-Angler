using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Core;
using Dapper;
using NUnit.Framework;

namespace Tests
{
    public abstract class TestsBase
    {
        private readonly string DBName = "SpaceAnglerTestDb";
        private string _connectionString = $@"
                Server=.; 
                Integrated Security=true;
                Initial Catalog=master;
            ";


        public static Script CreateTable = TestScripts.Read("create-table");
        public static Script DropTable = TestScripts.Read("drop-table");
      
        protected SqlConnection _connection;

        protected string CreateTableName() { return $"{Guid.NewGuid().ToString()}"; }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _connection = new SqlConnection(_connectionString);
            _connection.Open();
            _connection.Execute($@"
                IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = N'{DBName}')
                BEGIN
                    CREATE DATABASE [{DBName}]
                END");
            _connection.ChangeDatabase(DBName);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _connection.Close();
            _connection.Dispose();
        }



        protected async Task<LRTest> Test(string tree)
        {
            var tableName = CreateTableName();
            await CreateTable.New("[%_Node_%]", SqlStuff.Escape(tableName)).ExecuteAsync(_connection);
            await Arrange(tree, SqlStuff.Escape(tableName), _connection);
            await MigrationFactory.CreateScript(tableName, "Id", "Parent_Id").ExecuteAsync(_connection);
            return new LRTest(tableName, tree, _connection);
        }

        private async Task Arrange(string data, string tableName, SqlConnection connection)
        {
            var nodes = new Tree(data).Nodes;

            var captain = new CaptainData.Captain();
            foreach (var node in nodes.Values)
            {
                captain.Insert(tableName, node);
            }

            await captain.Go(connection);
        }
    }
}
