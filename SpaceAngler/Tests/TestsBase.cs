using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using NUnit.Framework;
using Sql;

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
        public static Script DropTable = TestScripts.Read("create-table");
        private Script _alterTable = Script.Read<Script>("alter-table");
        private Script _filler = Script.Read<Script>("filler");
        private Script _iTrigger = Script.Read<Script>("triggers.insert");
        private Script _uTrigger = Script.Read<Script>("triggers.update");
        private Script _dTrigger = Script.Read<Script>("triggers.delete");
        protected SqlConnection _connection;

        protected string CreateTableName() { return $"{Guid.NewGuid().ToString()}"; }

        protected Script NewAlterTable(string tableName)
        {
            return _alterTable.New("[Node]", SqlStuff.Escape(tableName));
        }

        protected Script Filler(string tableName)
        {
            return _filler.New("[Node]", SqlStuff.Escape(tableName));
        }

        protected Script UTrigger(string tableName)
        {
            return _uTrigger.New("[Node]", SqlStuff.Escape(tableName))
            .New("Update" + SqlStuff.UnEscape("Node") + "Trigger", SqlStuff.Escape("Update" + SqlStuff.UnEscape(tableName) + "Trigger"));
        }

        protected Script ITrigger(string tableName)
        {
            return _iTrigger.New("[Node]", SqlStuff.Escape(tableName))
                .New("Insert"+SqlStuff.UnEscape("Node") + "Trigger", SqlStuff.Escape("Insert" + SqlStuff.UnEscape(tableName) + "Trigger"));
        }
        protected Script DTrigger(string tableName)
        {
            return _dTrigger.New("[Node]", SqlStuff.Escape(tableName))
                .New("Delete" + SqlStuff.UnEscape("Node") + "Trigger", SqlStuff.Escape("Delete" + SqlStuff.UnEscape(tableName) + "Trigger"));
        }



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
            await CreateTable.New("[Node]", SqlStuff.Escape(tableName)).ExecuteAsync(_connection);
            await Arrange(tree, SqlStuff.Escape(tableName), _connection);
            await NewAlterTable(tableName).ExecuteAsync(_connection);
            await Filler(tableName).ExecuteAsync(_connection);
            await ITrigger(tableName).ExecuteAsync(_connection);
            await UTrigger(tableName).ExecuteAsync(_connection);
            await DTrigger(tableName).ExecuteAsync(_connection);
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
