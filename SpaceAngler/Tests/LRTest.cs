using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Sql;

namespace Tests
{
    public class LRTest
    {
        private string _expected;
        private readonly SqlConnection _connection;

        public string Table { get; }

        public LRTest(string tableName, string expected, SqlConnection connection)
        {
            Table = tableName;
            _expected = expected;
            _connection = connection;
        }

        internal async Task ExecuteAsync(string v)
        {
            await _connection.ExecuteAsync(v);
        }

        internal async Task AssertAll(string expected = null)
        {
            var nodes = (await _connection.QueryAsync<Node>($"SELECT * FROM {SqlStuff.Escape(Table)}")).ToDictionary(x => x.Id, x => x);
            await DropTable().ExecuteAsync(_connection);
            var asserter =  new Asserter(nodes, expected ?? _expected);
            asserter.AssertAll();
        }

        private Script DropTable()
        {
            return TestsBase.DropTable.New("[Node]", SqlStuff.Escape(Table));
        }
    }
}
