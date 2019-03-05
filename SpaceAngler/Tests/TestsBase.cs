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
        protected SqlConnection _connection;
        protected CaptainData.Captain Captain = new CaptainData.Captain();

        protected string CreateTableName() { return $"[{Guid.NewGuid().ToString()}]"; }

        protected Script AlterTable(string tableName)
        {
            return _alterTable.Replace("[Node]", tableName);
        }

        protected Script Filler(string tableName)
        {
            return _filler.Replace("[Node]", tableName);
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
            await CreateTable.Replace("[Node]", tableName).ExecuteAsync(_connection);
            var expected = await Arrange(tree, tableName, _connection);
            await AlterTable(tableName).ExecuteAsync(_connection);
            await Filler(tableName).ExecuteAsync(_connection);
            return new LRTest(tableName, expected, _connection);
        }

        private async Task<List<Node>> Arrange(string data, string tableName, SqlConnection connection)
        {
            var nodes = new List<Node>();
            var list = data.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            var parent = new Stack<Node>();
            var lastLevel = 0;
            foreach (var e in list)
            {
                var splits = e.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                var level = splits.Count();
                var number = int.Parse(splits.Last());
                while (lastLevel >= level)
                {
                    lastLevel--;
                    parent.Pop();

                }
                if (lastLevel == 0)
                {
                    var node = new Node() { Id = number };
                    nodes.Add(node);
                    parent.Push(node);
                }

                else if (lastLevel + 1 == level)
                {
                    var node = new Node() { Id = number, Parent_Id = parent.Peek().Id, Parent = parent.Peek() };
                    parent.Peek().Children.Add(node);
                    nodes.Add(node);
                    parent.Push(node);
                }

                lastLevel = level;
            }

            foreach (var node in nodes)
            {
                Captain.Insert(tableName, node);
            }

            await Captain.Go(connection);
            return nodes;
        }
    }

    public class LRTest
    {
        private string _tableName;
        private List<Node> _expected;
        private readonly SqlConnection _connection;

        public LRTest(string tableName, List<Node> expected, SqlConnection connection)
        {
            _tableName = tableName;
            _expected = expected;
            _connection = connection;
        }
        private async Task<Asserter> GetAsserter(string tableName, List<Node> expected)
        {
            var nodes = _connection.Query<Node>($"SELECT * FROM {tableName}").ToDictionary(x => x.Id, x => x);
            await DropTable(tableName).ExecuteAsync(_connection);
            return new Asserter(nodes, expected);
        }
        internal async Task AssertAll()
        {
            // ASSERT
            var asserter = await GetAsserter(_tableName, _expected);
            asserter.AssertAll();
        }

        private Script DropTable(string tableName)
        {
            return TestsBase.DropTable.Replace("[Node]", tableName);
        }
    }
}
